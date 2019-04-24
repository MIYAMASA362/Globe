Shader "Custom/ScannerEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_ScanDistance("Scan Distance", float) = 0
		_ScanWidth("Scan Width", float) = 10
		_ScanColor("Scan Color", Color) = (1, 1, 1, 0)
		_CenterPosition("Center Position", Vector) = (0, 0, 0, 0)
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

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_depth : TEXCOORD1;
			};

			float4 _CameraWS;
			float4 _MainTex_TexelSize;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;
				o.uv_depth = v.uv.xy;

				return o;
			}

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			float _ScanDistance;
			float _ScanWidth;
			float4 _ScanColor;
			float3 _CenterPosition;

			half4 frag (v2f i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);

				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv_depth);
				float linear01Depth = Linear01Depth(depth);


				if (linear01Depth < _ScanDistance && linear01Depth > _ScanDistance - _ScanWidth && linear01Depth < 1)
				{
					float diff = 1 - (_ScanDistance - linear01Depth) / (_ScanWidth);
					_ScanColor *= diff;
					return col + _ScanColor;
				}

				return col;
			}
			ENDCG
		}
	}
}
