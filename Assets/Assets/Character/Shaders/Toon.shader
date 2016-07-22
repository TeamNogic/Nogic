Shader "Custom/Toon"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ToonShade("ToonShader Cubemap(RGB)", CUBE) = "" { }
		_OutlineSize("Outline Size", Float) = 0.01
		_OutlineColor("Outline Color", color) = (0,0,0,1)
		_AlphaValue("Alpha Value", Float) = 1.0
	}

		CGINCLUDE
#include "UnityCG.cginc"
#include "AutoLight.cginc"

		struct v2
		{
			float4 vertex : SV_POSITION;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float3 normal : TEXCOORD1;
			LIGHTING_COORDS(2, 3)
		};

		float _OutlineSize;

		//アウトライン頂点シェーダー
		v2 vertOutLine(appdata_base v)
		{
			v2 o;
			float3 normal = normalize(mul(UNITY_MATRIX_MV, float4(v.normal, 0)));
			float2 offset = TransformViewToProjection(normal.xy);

			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.vertex.xy += offset * _OutlineSize;

			return o;
		}

		uniform float4 _OutlineColor;
		float _AlphaValue;

		//アウトラインピクセルシェーダー
		float4 fragOutLine(v2 i) : SV_Target
		{
			float4 color = _OutlineColor;
			color.a = _AlphaValue;
			return color;
		}

		struct ToonOutput
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float3 cubenormal : TEXCOORD1;
			LIGHTING_COORDS(2, 3)
		};

		//トゥーンピクセルシェーダー
		v2f vert(appdata_tan v)
		{
			ToonOutput o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			//o.normal = UnityObjectToWorldNormal(v.normal);
			o.uv = v.texcoord;
			o.cubenormal = mul(UNITY_MATRIX_MV, float4(v.normal, 0));
			TRANSFER_VERTEX_TO_FRAGMENT(o);

			return o;
		}

		sampler2D _MainTex;
		samplerCUBE _ToonShade;

		//トゥーンピクセルシェーダー
		float4 fragToon(ToonOutput i) : COLOR
		{
			float4 color = tex2D(_MainTex, i.uv);
			float4 cube = texCUBE(_ToonShade, i.cubenormal);

			color.rgb *= cube.rgb * LIGHT_ATTENUATION(i) + 0.2f;
			color.a = _AlphaValue;
			return color;
		}

			//トゥーンピクセルシェーダー（複数ライト）
			float4 fragToons(ToonOutput i) : COLOR
		{
			float4 color = tex2D(_MainTex, i.uv);

			//float NL = max(dot(_WorldSpaceLightPos0.xyz, i.normal), 0.0f);
			//float3 Shade = tex2D(_ShadowAdjustmentTexture, float2(NL, 0)).rgb;

			float4 cube = texCUBE(_ToonShade, i.cubenormal);

			color.rgb *= cube.rgb * LIGHT_ATTENUATION(i) + 0.2f;
			return color;
		}
			ENDCG

			SubShader
		{
			Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
				//アウトラインをレンダリング
				Pass
			{
				Cull Front
				ZWrite On
				ZTest LEqual
				Blend SrcAlpha OneMinusSrcAlpha

				Tags{ "LightMode" = "ForwardBase" }

				CGPROGRAM
				#pragma vertex vertOutLine
				#pragma fragment fragOutLine
				#pragma target 3.0
				ENDCG
			}

				//Toonレンダリング
				Pass
			{
				Tags{ "LightMode" = "ForwardBase" }
				Cull Back
				ZWrite On
				ZTest LEqual
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment fragToon
				#pragma multi_compile_fwdbase
				#pragma target 3.0
				ENDCG
			}

				//複数ライト用
				Pass
			{
				Tags{ "LightMode" = "ForwardAdd" }
				Cull Back
				ZWrite On
				ZTest LEqual
				Blend One SrcColor

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment fragToons
				#pragma multi_compile_fwdbase
				ENDCG
			}
		}
		Fallback "VertexLit"
}