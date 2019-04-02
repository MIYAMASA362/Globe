// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Igori/Water"
{
	Properties
	{
		_DepthColor("DepthColor", Color) = (0.4926471,0.8740366,1,1)
		_WaterColor("Water Color", Color) = (0.4926471,0.8740366,1,1)
		_WaveGuide("Wave Guide", 2D) = "white" {}
		_WaveSpeed("Wave Speed", Range( 0 , 5)) = 0
		_WaveHeight("Wave Height", Range( 0 , 5)) = 0
		_FoamColor("Foam Color", Color) = (1,1,1,0)
		_Foam("Foam", 2D) = "white" {}
		_FoamDistortion("Foam Distortion", 2D) = "white" {}
		_FoamDist("Foam Dist", Range( 0 , 1)) = 0.1
		[Toggle]_LowPoly("Low Poly", Float) = 0
		_NormalOnlyNoPolyMode("Normal (Only No Poly Mode)", 2D) = "bump" {}
		_WaveTiling("WaveTiling", Vector) = (0,0,0,0)
		_GlossScale("GlossScale", Float) = 0
		_SpecScale("SpecScale", Float) = 0
		_Tesselation("Tesselation", Float) = 0
		_depthScale("depthScale", Float) = 0
		_depthFadeScale("depthFadeScale", Float) = 0
		_Diffuse("Diffuse", 2D) = "white" {}
		[HDR]_Color1("Color 1", Color) = (0,0,0,0)
		_FreshnelColor("FreshnelColor", Color) = (0,0,0,0)
		_FreshnelPower("FreshnelPower", Float) = 0
		_FreshnelScale("FreshnelScale", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf StandardSpecular keepalpha noshadow vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			float4 screenPos;
			INTERNAL_DATA
		};

		uniform sampler2D _WaveGuide;
		uniform float2 _WaveTiling;
		uniform float _WaveSpeed;
		uniform float _WaveHeight;
		uniform float _LowPoly;
		uniform sampler2D _NormalOnlyNoPolyMode;
		uniform float4 _WaterColor;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform float4 _DepthColor;
		uniform sampler2D _CameraDepthTexture;
		uniform float _depthScale;
		uniform float4 _FoamColor;
		uniform sampler2D _Foam;
		uniform float4 _Foam_ST;
		uniform sampler2D _FoamDistortion;
		uniform float _FoamDist;
		uniform float _FreshnelScale;
		uniform float _FreshnelPower;
		uniform float4 _FreshnelColor;
		uniform float _SpecScale;
		uniform float4 _Color1;
		uniform float _GlossScale;
		uniform float _depthFadeScale;
		uniform float _Tesselation;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float4 temp_cast_1 = (_Tesselation).xxxx;
			return temp_cast_1;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float4 speed183 = ( _Time * _WaveSpeed );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float2 uv_TexCoord96 = v.texcoord.xy * _WaveTiling + ( speed183 + (ase_vertex3Pos).y ).xy;
			float3 ase_vertexNormal = v.normal.xyz;
			float3 VertexAnimation127 = ( ( tex2Dlod( _WaveGuide, float4( uv_TexCoord96, 0, 1.0) ).r - 0.5 ) * ( ase_vertexNormal * _WaveHeight ) );
			v.vertex.xyz += VertexAnimation127;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float4 speed183 = ( _Time * _WaveSpeed );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float2 uv_TexCoord96 = i.uv_texcoord * _WaveTiling + ( speed183 + (ase_vertex3Pos).y ).xy;
			float3 tex2DNode202 = UnpackNormal( tex2D( _NormalOnlyNoPolyMode, uv_TexCoord96 ) );
			float3 ase_worldPos = i.worldPos;
			float3 normalizeResult123 = normalize( ( cross( ddx( ase_worldPos ) , ddy( ase_worldPos ) ) + float3( 1E-09,0,0 ) ) );
			float3 Normal124 = lerp(tex2DNode202,normalizeResult123,_LowPoly);
			o.Normal = Normal124;
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float4 tex2DNode223 = tex2D( _Diffuse, uv_Diffuse );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth213 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth213 = abs( ( screenDepth213 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _depthScale ) );
			float clampResult217 = clamp( distanceDepth213 , 0.0 , 1.0 );
			float4 lerpResult216 = lerp( ( _WaterColor * tex2DNode223 ) , ( _DepthColor * tex2DNode223 ) , clampResult217);
			float2 uv_Foam = i.uv_texcoord * _Foam_ST.xy + _Foam_ST.zw;
			float2 panner177 = ( speed183.x * float2( 0.5,0.5 ) + uv_Foam);
			float cos182 = cos( speed183.x );
			float sin182 = sin( speed183.x );
			float2 rotator182 = mul( panner177 - float2( 0,0 ) , float2x2( cos182 , -sin182 , sin182 , cos182 )) + float2( 0,0 );
			float4 tex2DNode169 = tex2D( _Foam, rotator182 );
			float clampResult181 = clamp( tex2D( _FoamDistortion, rotator182 ).r , 0.0 , 1.0 );
			float screenDepth164 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth164 = abs( ( screenDepth164 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _FoamDist ) );
			float clampResult191 = clamp( ( clampResult181 * distanceDepth164 ) , 0.0 , 1.0 );
			float4 lerpResult157 = lerp( ( _FoamColor * tex2DNode169 ) , float4(0,0,0,0) , clampResult191);
			o.Albedo = ( lerpResult216 + lerpResult157 ).rgb;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV228 = dot( tex2DNode202, ase_worldViewDir );
			float fresnelNode228 = ( 0.0 + _FreshnelScale * pow( 1.0 - fresnelNdotV228, _FreshnelPower ) );
			float4 clampResult234 = clamp( ( fresnelNode228 * _FreshnelColor ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			o.Emission = clampResult234.rgb;
			float4 temp_output_211_0 = ( tex2DNode169 * _SpecScale * _Color1 );
			o.Specular = temp_output_211_0.rgb;
			float temp_output_208_0 = _GlossScale;
			o.Smoothness = temp_output_208_0;
			float screenDepth219 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth219 = abs( ( screenDepth219 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _depthFadeScale ) );
			float clampResult221 = clamp( distanceDepth219 , 0.0 , 1.0 );
			o.Alpha = clampResult221;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
132.8;140.8;1355;622;2316.692;1089.779;3.200226;True;False
Node;AmplifyShaderEditor.CommentaryNode;199;-2827.374,-925.0059;Float;False;914.394;362.5317;Comment;4;89;15;88;183;Wave Speed;1,1,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;89;-2706.477,-875.0057;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-2777.374,-677.473;Float;False;Property;_WaveSpeed;Wave Speed;3;0;Create;True;0;0;False;0;0;0.3;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;192;-2804.44,147.8661;Float;False;2009.663;867.9782;Comment;15;176;177;182;179;181;161;174;169;191;159;170;164;167;184;134;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-2377.44,-739.9845;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;184;-2755.296,577.5368;Float;False;183;0;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;183;-2155.98,-832.3298;Float;False;speed;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;197;-2751.606,-436.2369;Float;False;2321.461;426.9865;Comment;13;53;118;47;96;86;43;54;44;36;29;127;195;206;Vertex Animation;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;176;-2786.44,327.7948;Float;False;0;169;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;177;-2454.748,382.3567;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;204;-1883.495,-920.8318;Float;False;1244.412;443.4576;Comment;9;119;121;120;122;202;123;200;124;205;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.PosVertexDataNode;53;-2701.606,-286.774;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;182;-2377.307,563.6425;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;119;-1872,-656;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;195;-2377.985,-347.0552;Float;False;183;0;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;118;-2462.842,-267.5019;Float;False;False;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;167;-2382.478,814.4856;Float;False;Property;_FoamDist;Foam Dist;8;0;Create;True;0;0;False;0;0.1;0.003;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;179;-2143.537,609.8837;Float;True;Property;_FoamDistortion;Foam Distortion;7;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-2177.163,-350.3377;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DdyOpNode;121;-1664,-576;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;206;-2202.116,-121.5259;Float;False;Property;_WaveTiling;WaveTiling;12;0;Create;True;0;0;False;0;0,0;5,5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DdxOpNode;120;-1664,-672;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;214;-132.4715,-61.2556;Float;False;Property;_depthScale;depthScale;16;0;Create;True;0;0;False;0;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;96;-1808.334,-78.4807;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;181;-1851.366,695.3312;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CrossProductOpNode;122;-1536,-640;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DepthFade;164;-2020.088,829.655;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;205;-1367.996,-605.7512;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;1E-09,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;2;-196.5804,-716.6601;Float;False;Property;_WaterColor;Water Color;1;0;Create;True;0;0;False;0;0.4926471,0.8740366,1,1;0.2876466,0.6037736,0.5657212,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;213;20.48918,-13.55817;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;215;-435.5428,-550.935;Float;False;Property;_DepthColor;DepthColor;0;0;Create;True;0;0;False;0;0.4926471,0.8740366,1,1;0.05517956,0.2829264,0.3773582,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;202;-1509.38,-870.8318;Float;True;Property;_NormalOnlyNoPolyMode;Normal (Only No Poly Mode);11;0;Create;True;0;0;False;0;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;232;919.0092,-284.0449;Float;False;Property;_FreshnelScale;FreshnelScale;23;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;233;-388.4442,1779.233;Float;False;Property;_FreshnelPower;FreshnelPower;22;0;Create;True;0;0;False;0;0;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;161;-1881.812,197.8662;Float;False;Property;_FoamColor;Foam Color;5;0;Create;True;0;0;False;0;1,1,1,0;0.6132076,0.6132076,0.6132076,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;54;-1353.867,-280.0608;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-1398.183,-124.2504;Float;False;Property;_WaveHeight;Wave Height;4;0;Create;True;0;0;False;0;0;0.03;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;223;-382.4372,-301.9023;Float;True;Property;_Diffuse;Diffuse;18;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;174;-1683.11,819.9097;Float;False;2;2;0;FLOAT;0.075;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;169;-1928.3,373.9261;Float;True;Property;_Foam;Foam;6;0;Create;True;0;0;False;0;None;a34a33a1f9ea0154db9d1f4de30ae782;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;86;-1709.089,-380.2153;Float;True;Property;_WaveGuide;Wave Guide;2;0;Create;True;0;0;False;0;None;44680e884f8253348bb26dc35c9c8ddd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;191;-1486.683,783.1451;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;217;194.8315,-18.49234;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;222;157.948,-649.8676;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;231;1111.258,178.0262;Float;False;Property;_FreshnelColor;FreshnelColor;21;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;228;1283.27,-265.4946;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;220;-534.3792,774.7862;Float;False;Property;_depthFadeScale;depthFadeScale;17;0;Create;True;0;0;False;0;0;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;36;-1021.508,-352.6445;Float;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1026.813,-198.9792;Float;False;2;2;0;FLOAT3;1,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;224;272.6876,-467.0623;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;159;-1656.591,619.5612;Float;False;Constant;_Color0;Color 0;9;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;123;-1232,-576;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;170;-1513.108,437.6683;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;157;-160.3349,375.1265;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DepthFade;219;-312.679,710.6099;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;229;1470.459,81.90195;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;227;392.8555,-228.394;Float;False;Property;_Color1;Color 1;20;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.6675394,0.4613361,0.4193964,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;207;-709.13,218.2064;Float;False;Property;_SpecScale;SpecScale;14;0;Create;True;0;0;False;0;0;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;200;-1084.856,-668.6083;Float;False;Property;_LowPoly;Low Poly;10;1;[Toggle];Create;True;0;0;False;0;0;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;216;452.6741,-562.1086;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-859.5037,-220.1143;Float;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;-8.505981,255.6002;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;-706.1451,-228.0923;Float;False;VertexAnimation;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;221;-112.371,665.8811;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;210;-473.4297,390.1059;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;234;1739.068,111.9281;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;208;-760.6367,298.1274;Float;False;Property;_GlossScale;GlossScale;13;0;Create;True;0;0;False;0;0;1.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-641.8879,48.64041;Float;False;124;0;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;212;110.9508,358.1531;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;134;-1044.976,419.6484;Float;False;-1;0;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;225;-431.7903,260.6593;Float;False;Property;_SpecColor;SpecColor;19;0;Create;True;0;0;False;0;0,0,0,0;0.764151,0.5809035,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;124;-882.0831,-657.6036;Float;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;163;-666.4636,118.8066;Float;False;-1;0;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;209;-247.7086,909.2958;Float;False;Property;_Tesselation;Tesselation;15;0;Create;True;0;0;False;0;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-613.7995,614.4047;Float;False;Property;_Opacity;Opacity;9;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;211;431.2685,77.8698;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;-355.355,866.3677;Float;False;127;0;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;754.504,44.22191;Float;False;True;6;Float;ASEMaterialInspector;0;0;StandardSpecular;Igori/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;False;0;False;Opaque;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;4;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;88;0;89;0
WireConnection;88;1;15;0
WireConnection;183;0;88;0
WireConnection;177;0;176;0
WireConnection;177;1;184;0
WireConnection;182;0;177;0
WireConnection;182;2;184;0
WireConnection;118;0;53;0
WireConnection;179;1;182;0
WireConnection;47;0;195;0
WireConnection;47;1;118;0
WireConnection;121;0;119;0
WireConnection;120;0;119;0
WireConnection;96;0;206;0
WireConnection;96;1;47;0
WireConnection;181;0;179;1
WireConnection;122;0;120;0
WireConnection;122;1;121;0
WireConnection;164;0;167;0
WireConnection;205;0;122;0
WireConnection;213;0;214;0
WireConnection;202;1;96;0
WireConnection;174;0;181;0
WireConnection;174;1;164;0
WireConnection;169;1;182;0
WireConnection;86;1;96;0
WireConnection;191;0;174;0
WireConnection;217;0;213;0
WireConnection;222;0;2;0
WireConnection;222;1;223;0
WireConnection;228;0;202;0
WireConnection;228;2;232;0
WireConnection;228;3;233;0
WireConnection;36;0;86;1
WireConnection;44;0;54;0
WireConnection;44;1;43;0
WireConnection;224;0;215;0
WireConnection;224;1;223;0
WireConnection;123;0;205;0
WireConnection;170;0;161;0
WireConnection;170;1;169;0
WireConnection;157;0;170;0
WireConnection;157;1;159;0
WireConnection;157;2;191;0
WireConnection;219;0;220;0
WireConnection;229;0;228;0
WireConnection;229;1;231;0
WireConnection;200;0;202;0
WireConnection;200;1;123;0
WireConnection;216;0;222;0
WireConnection;216;1;224;0
WireConnection;216;2;217;0
WireConnection;29;0;36;0
WireConnection;29;1;44;0
WireConnection;226;0;211;0
WireConnection;127;0;29;0
WireConnection;221;0;219;0
WireConnection;210;1;208;0
WireConnection;234;0;229;0
WireConnection;212;0;216;0
WireConnection;212;1;157;0
WireConnection;124;0;200;0
WireConnection;211;0;169;0
WireConnection;211;1;207;0
WireConnection;211;2;227;0
WireConnection;0;0;212;0
WireConnection;0;1;125;0
WireConnection;0;2;234;0
WireConnection;0;3;211;0
WireConnection;0;4;208;0
WireConnection;0;9;221;0
WireConnection;0;11;128;0
WireConnection;0;14;209;0
ASEEND*/
//CHKSM=EA48CCDC5E9CDDD043B6BFB46429687730FD0E6F