Shader "Custom/LineEffect" {

	Properties
	{
		_MainColor("MainColor", Color) = (1,1,1,1)
		_LineColor("LineColor", Color) = (1,1,1,1)
		[HDR] _LineEmissionColor("EmissionColor", Color) = (1,1,1,1)
		_LineDistance("LineDistance", float) = 1.0
		_LineWidth("LineWith", float) = 1.0
		_CenterPosition("Center Position", Vector) = (0, 0, 0)
	}

	SubShader
	{
			Tags { "RenderType" = "Transparent" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard alpha:fade
			#pragma target 3.0

			struct Input {
				float3 worldPos;
			};

		float3 _CenterPosition;
		half4 _MainColor;
		half4 _LineColor;
		half3 _LineEmissionColor;
		float _LineEmisionValue;
		float _LineDistance;
		float _LineWidth;
		float _LineTime;

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float dist = distance(_CenterPosition, IN.worldPos) - (_LineTime);

			if ((dist > 0 && dist < _LineWidth) || 
				(dist > _LineDistance && dist < _LineDistance + _LineWidth) || 
				(dist > _LineDistance*2 && dist < _LineDistance*2 + _LineWidth)) 
			{
				o.Albedo = _LineColor;
				o.Emission = _LineEmissionColor * _LineEmisionValue;
				o.Alpha = _LineColor.a;
			}
			else 
			{
				o.Albedo = _MainColor;
				o.Alpha = _MainColor.a;
			}
		}
	ENDCG
	}
	FallBack "Diffuse"
}