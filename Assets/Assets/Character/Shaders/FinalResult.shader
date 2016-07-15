Shader "Hidden/FinalResult"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		UsePass "shader/Outline_Base"
		UsePass "shader/Outline_Add"
	}
}
