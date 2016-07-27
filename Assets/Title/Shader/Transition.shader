Shader "Sprites/Transition"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaskTex ("MaskTexture", 2D) = "white" {}
		_AlphaValue ("Alpha Value", Float) = 0.0
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
			#pragma target 3.0

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

			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _MaskTex;
			float _AlphaValue;

			float4 frag (v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);

				//return float4(1.0f, 0.0f, 0.0f, 1.0f);

				if (tex2D(_MaskTex, i.uv).r < _AlphaValue)
				{
					return float4(0.0f, 0.0f, 0.0f, 1.0f);
					color.a = 0.0f;
				}
				
				return color;
			}
			ENDCG
		}
	}
}