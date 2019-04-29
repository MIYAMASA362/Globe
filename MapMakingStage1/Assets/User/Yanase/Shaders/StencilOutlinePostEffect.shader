Shader "Custom/StencilOutlinePostEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		// ステンシル
	Stencil{
		Ref 1
		Comp Equal
	}
	
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
	
		sampler2D _MainTex;
		float4 _MainTex_ST;
	
		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		}
	
		fixed4 frag(v2f i) : SV_Target
		{
			// 描画部分を緑色に
			return fixed4(0, 1, 0, 1);
		}
		ENDCG
		}
	}
}
