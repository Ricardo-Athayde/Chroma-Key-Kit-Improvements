Shader "MSK/Filter/Despill" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_Despill("DespillStrength", Range(0, 1)) = 1
        _DespillLuminanceAdd("DespillLuminanceAdd", Range(0, 1)) = 0.2
	}
	CGINCLUDE
	#include "UnityCG.cginc"
	struct VS_OUT {
		half4 position:POSITION;
		fixed2 texcoord0:TEXCOORD0;
	};

	sampler2D _MainTex;
	fixed4 _MainTex_ST;

    float _Despill;
    float _DespillLuminanceAdd;

	VS_OUT vert(appdata_base input) {
		VS_OUT o;
		o.position = UnityObjectToClipPos(input.vertex);
		o.texcoord0 = TRANSFORM_TEX(input.texcoord, _MainTex);
		return o;
	}
	
	float rgb2y(float3 c) 
    {
        return (0.299*c.r + 0.587*c.g + 0.114*c.b);
    }

	fixed4 frag(VS_OUT input) : SV_Target {
		fixed4 c = tex2D(_MainTex, input.texcoord0);
		fixed4 color = tex2D(_MainTex, input.texcoord0);
		// Despill
                float v = (2*c.b+c.r)/4;
                if(c.g > v) c.g = lerp(c.g, v, _Despill);
                float4 dif = (color - c);
                float desaturatedDif = rgb2y(dif.xyz);
                c += lerp(0, desaturatedDif, _DespillLuminanceAdd);

		return float4(c.xyz, color.a);
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