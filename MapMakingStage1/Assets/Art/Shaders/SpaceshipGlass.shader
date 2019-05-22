// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "My/SpaceshipGlass"
{
	Properties
	{
		_SpecColor("Specular Color",Color)=(1,1,1,1)
		_Color("Color", Color) = (0,0,0,0)
		_GlossScale("GlossScale", Float) = 0
		_SpecScale("SpecScale", Float) = 0
		_FreshnelScale("FreshnelScale", Float) = 0
		_FreshnelPower("FreshnelPower", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#pragma target 4.0
		#pragma surface surf BlinnPhong alpha:fade keepalpha noshadow noforwardadd 
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _GrabTexture;
		uniform float4 _Color;
		uniform float _FreshnelScale;
		uniform float _FreshnelPower;
		uniform float _SpecScale;
		uniform float _GlossScale;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 screenColor11 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ase_grabScreenPos ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV5 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode5 = ( 0.0 + _FreshnelScale * pow( 1.0 - fresnelNdotV5, _FreshnelPower ) );
			float clampResult2 = clamp( fresnelNode5 , 0.0 , 1.0 );
			float4 lerpResult14 = lerp( screenColor11 , _Color , clampResult2);
			o.Albedo = lerpResult14.rgb;
			o.Specular = _SpecScale;
			o.Gloss = _GlossScale;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
640.8;146.4;1071;800;1296.044;424.9893;1.782437;True;False
Node;AmplifyShaderEditor.RangedFloatNode;9;-1033.318,342.3385;Float;False;Property;_FreshnelPower;FreshnelPower;6;0;Create;True;0;0;False;0;0;3.22;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-802.9178,420.7386;Float;False;Property;_FreshnelScale;FreshnelScale;5;0;Create;True;0;0;False;0;0;6.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;5;-660.0899,93.20269;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;11;-686.1196,-368.0622;Float;False;Global;_GrabScreen0;Grab Screen 0;6;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-673.3195,-140.8615;Float;False;Property;_Color;Color;1;0;Create;True;0;0;False;0;0,0,0,0;0.9150943,0.4074507,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;2;-426.9201,305.5382;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;13;-326.1197,-230.4617;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-321.3188,19.13834;Float;False;Property;_SpecScale;SpecScale;4;0;Create;True;0;0;False;0;0;1.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;14;-81.3197,-214.4617;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-209.3201,535.9384;Float;False;Property;_Opacity;Opacity;2;0;Create;True;0;0;False;0;0;0.31;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-302.1184,139.1383;Float;False;Property;_GlossScale;GlossScale;3;0;Create;True;0;0;False;0;0;1.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3;-132.52,382.3384;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;4;Float;ASEMaterialInspector;0;0;BlinnPhong;My/SpaceshipGlass;False;False;False;False;False;False;False;False;False;False;False;True;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;0;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;2;10;0
WireConnection;5;3;9;0
WireConnection;2;0;5;0
WireConnection;13;0;11;0
WireConnection;13;1;1;0
WireConnection;14;0;11;0
WireConnection;14;1;1;0
WireConnection;14;2;2;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;0;0;14;0
WireConnection;0;3;7;0
WireConnection;0;4;6;0
ASEEND*/
//CHKSM=87ADA48F09899582831787D14DFC14CEBCB43A43