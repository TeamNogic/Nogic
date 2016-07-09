Shader "Custom/DepthofField"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;

			float4 frag (v2f i) : SV_Target
			{
				//float4 col = tex2D(_CameraDepthTexture, i.uv);
				//return float4(col.r, col.r, col.r, 1.0f);

				i.uv.y = i.uv.y * -0.5 + 0.5;
				float d = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
				//d = Linear01Depth(d);
				return float4(d, d, d, 1.0f);
			}
			ENDCG
		}
	}
}
