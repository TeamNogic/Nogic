Shader "Sprites/Transition2D"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_MaskTex ("MaskTexture", 2D) = "white" {}
		_AlphaValue ("Alpha Value", Float) = 0.0
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

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
			sampler2D _MaskTex;
			float _AlphaValue;
			float4 _Color;

			float4 frag (v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv) * _Color;

				if (tex2D(_MaskTex, i.uv).a >= _AlphaValue)
				{
					color.a = 0.0f;
				}
				
				return color;
			}
			ENDCG
		}
	}
}
