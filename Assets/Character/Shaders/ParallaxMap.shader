Shader "Custom/ParallaxMap"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NormalMap("NormalMap", 2D) = "bump" {}
		_HeightMap("HeightMap", 2D) = "white" {}
		_Height("Height", Float) = 0.02
	}

	SubShader
	{

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

			#include "UnityCG.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 light : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _NormalMap;
			sampler2D _HeightMap;
			float _Height;

			v2f vert(appdata_tan v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;

				float3 n = UnityObjectToWorldNormal(v.normal);
				float3 t = normalize(v.tangent * 0.5f + 0.5f);
				float3 b = cross(n, t);

				float4 light = _WorldSpaceLightPos0;

				o.light.x = dot(t, light);
				o.light.y = dot(b, light);
				o.light.z = dot(n, light);
				o.light = normalize(o.light);

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);

				//float3 normalMap = (tex2D(_NormalMap, i.uv) * 2.0 - 1.0).xyz;
				float heightScale = tex2D(_HeightMap, i.uv).r * _Height;
				float2 texCood = i.uv - (heightScale * _WorldSpaceCameraPos.xy);
				float3 normalMap = UnpackNormal(tex2D(_NormalMap, texCood));

				float NL = max(dot(normalize(i.light), normalMap), 0.0f);
				//return float4(NL, NL, NL, 1.0f);

				col.xyz *= NL;
				col.a = 1.0f;
				return col;
			}
			ENDCG
		}
	}
}
