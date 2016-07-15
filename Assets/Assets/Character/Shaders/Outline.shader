Shader "Hidden/Outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			float _OutlineSize;

			v2f vert(appdata v)
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
			Name "Outline_Add"

			Cull Front
			ZWrite On
			ZTest LEqual

			Tags{ "LightMode" = "ForwardAdd" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			float _OutlineSize;

			v2f vert(appdata v)
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
	}
}
