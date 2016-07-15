Shader "Custom/Blur"
{
	Properties
	{
		_MainTex ("", any) = "" {}
		_Width ("BulrWidth", Float) = 0.00052083333
		_Height("BulrHeight", Float) = 0.00092592592
	}

	SubShader
	{
		Cull Off 
		ZWrite Off 
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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
			float _Width;
			float _Height;

			float4 frag (v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv) * 0.2f;

				color += tex2D(_MainTex, i.uv + float2(-_Width, -_Height)) * 0.1f;
				color += tex2D(_MainTex, i.uv + float2(0.0f, _Height)) * 0.1f;
				color += tex2D(_MainTex, i.uv + float2(_Width, -_Height)) * 0.1f;

				color += tex2D(_MainTex, i.uv + float2(-_Width,0.0f)) * 0.1f;
				color += tex2D(_MainTex, i.uv + float2(_Width, 0.0f)) * 0.1f;
				
				color += tex2D(_MainTex, i.uv + float2(-_Width, _Height)) * 0.1f;
				color += tex2D(_MainTex, i.uv + float2(0.0f, _Height)) * 0.1f;
				color += tex2D(_MainTex, i.uv + float2(_Width, _Height)) * 0.1f;

				return color;
			}
			ENDCG
		}
	}

	Fallback off
}
