Shader "Custom/RadiationBlur"
{
	Properties
	{
		_MainTex("", any) = "" {}
		_Width("BulrWidth", Float) = 0.00052083333
		_Height("BulrHeight", Float) = 0.00092592592
		_BlurPower("BlurPower", Float) = 5.0
		_RadiationCenter("RadiationCenter", Vector) = (0.0, 0.0, 0.0, 1.0)
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

			v2f vert(appdata_img v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			sampler2D _MainTex;
			float _Width;
			float _Height;
			float _BlurPower;
			float4 _RadiationCenter;

			float4 frag(v2f i) : SV_Target
			{
				float2 dir = _RadiationCenter.xy - i.uv;
				float len = length(dir);
				dir = normalize(dir) * float2(_Width, _Height);
				dir /= length(_RadiationCenter);
				dir *= len * _BlurPower;

				float4 color = tex2D(_MainTex, i.uv) * 0.1f;

				color += tex2D(_MainTex, i.uv + dir) * 0.1f;
				color += tex2D(_MainTex, i.uv + dir * 2.0f) * 0.1f;
				color += tex2D(_MainTex, i.uv + dir * 3.0f) * 0.1f;
				color += tex2D(_MainTex, i.uv + dir * 4.0f) * 0.1f;
				color += tex2D(_MainTex, i.uv + dir * 5.0f) * 0.1f;
				color += tex2D(_MainTex, i.uv + dir * 6.0f) * 0.1f;
				color += tex2D(_MainTex, i.uv + dir * 7.0f) * 0.1f;
				color += tex2D(_MainTex, i.uv + dir * 8.0f) * 0.1f;
				color += tex2D(_MainTex, i.uv + dir * 9.0f) * 0.1f;

				return color;
			}
			ENDCG
		}
	}
	Fallback off
}
