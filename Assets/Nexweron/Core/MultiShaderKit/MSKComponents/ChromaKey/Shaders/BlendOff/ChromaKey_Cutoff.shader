Shader "MSK/ChromaKey/BlendOff/ChromaKey_Cutoff" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
	    _CropLeft("Crop Left", Range(0,1)) = 0.0
		_CropRight("Crop Right", Range(0,1)) = 0.0
		_CropUp("Crop Up", Range(0,1)) = 0.0
		_CropDown("Crop Down", Range(0,1)) = 0.0

	}
	CGINCLUDE
	#include "UnityCG.cginc"
	struct VS_OUT {
		half4 position:POSITION;
		fixed2 texcoord0:TEXCOORD0;
	};

	sampler2D _MainTex;
	fixed4 _MainTex_ST;
	
	float _CropLeft;
    float _CropRight;
    float _CropUp;
    float _CropDown;

	VS_OUT vert(appdata_base input) {
		VS_OUT o;
		o.position = UnityObjectToClipPos(input.vertex);
		o.texcoord0 = TRANSFORM_TEX(input.texcoord, _MainTex);
		return o;
	}

	float cropMask(float2 uv, float a)
	{
		if(uv.x < _CropLeft) return (0);

		if(uv.x > 1 - _CropRight) return (0);

		if(uv.y > 1 - _CropUp) return (0);

		if(uv.y < _CropDown) return (0);

		return a;
	}
	fixed4 frag(VS_OUT input) : SV_Target {
		fixed4 c = tex2D(_MainTex, input.texcoord0);		
		c.a = cropMask(input.texcoord0, c.a);
		return c;
	}
	ENDCG
	
	SubShader {
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		Lighting Off
		ZWrite Off
		AlphaTest Off
		Blend Off

		Pass {
			CGPROGRAM
			  #pragma vertex vert
			  #pragma fragment frag
			ENDCG
		}
	}
	Fallback Off
}