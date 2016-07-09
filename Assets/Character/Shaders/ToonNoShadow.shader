Shader "Custom/ToonNoShadow"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ShadowAdjustmentTexture("ShadowAdjustment", 2D) = "gray" {}
		_OutlineSize("Outline Size", Float) = 0.01
		_OutlineColor("Outline Color", color) = (0,0,0,1)
	}

	SubShader
	{
		//アウトラインをレンダリング
		Pass
		{
			Name "Outline_Base"

			Cull Front
			ZWrite On
			ZTest LEqual

			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			float _OutlineSize;

			v2f vert(appdata_base v)
			{
				v2f o;
				float3 normal = normalize(mul(UNITY_MATRIX_MV, float4(v.normal, 0)));
				float2 offset = TransformViewToProjection(normal.xy);

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.vertex.xy += offset * _OutlineSize;

				return o;
			}

			uniform float4 _OutlineColor;

			fixed4 frag(v2f i) : SV_Target
			{
				return _OutlineColor;
			}

			ENDCG	
		}

		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			Cull Back
			ZWrite On
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				LIGHTING_COORDS(2, 3)
			};

			v2f vert(appdata_tan v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = v.texcoord;
				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}

			sampler2D _MainTex;
			sampler2D _ShadowAdjustmentTexture;

			float4 frag(v2f i) : COLOR
			{
				float4 color = tex2D(_MainTex, i.uv);

				float NL = max(dot(_WorldSpaceLightPos0.xyz, i.normal), 0.0f);

				float3 Shade = tex2D(_ShadowAdjustmentTexture, float2(NL, 0));

				//color.rgb *= Shade + 0.2f;
				return color;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}