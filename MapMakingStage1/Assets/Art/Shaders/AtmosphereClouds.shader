// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Igori/AtmosphereClouds"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
		_Clouds001("Clouds001", 2D) = "white" {}
		_CloudsPower("CloudsPower", Float) = 0
		_Transparancy("Transparancy", Float) = 0
		_Mask("Mask", 2D) = "white" {}
		_CameraLength("CameraLength", Float) = 0
		_CameraOffset("CameraOffset", Float) = 0
		_CloudsTiling("CloudsTiling", Vector) = (0,0,0,0)
		_TimeScale("TimeScale", Float) = 0
		_PannerDirection("PannerDirection", Vector) = (0,0,0,0)
		_depthFade("depthFade", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Front
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float eyeDepth;
			float4 screenPos;
		};

		struct SurfaceOutputStandardSpecularCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Specular;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Transmission;
		};

		uniform float4 _Color;
		uniform float _Transparancy;
		uniform sampler2D _Clouds001;
		uniform float _TimeScale;
		uniform float2 _PannerDirection;
		uniform float2 _CloudsTiling;
		uniform float _CloudsPower;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float _CameraLength;
		uniform float _CameraOffset;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _depthFade;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		inline half4 LightingStandardSpecularCustom(SurfaceOutputStandardSpecularCustom s, half3 viewDir, UnityGI gi )
		{
			half3 transmission = max(0 , -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
			half4 d = half4(s.Albedo * transmission , 0);

			SurfaceOutputStandardSpecular r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Specular = s.Specular;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandardSpecular (r, viewDir, gi) + d;
		}

		inline void LightingStandardSpecularCustom_GI(SurfaceOutputStandardSpecularCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardSpecularCustom o )
		{
			float4 temp_output_1_0 = _Color;
			o.Albedo = temp_output_1_0.rgb;
			o.Emission = temp_output_1_0.rgb;
			o.Transmission = temp_output_1_0.rgb;
			float mulTime17 = _Time.y * _TimeScale;
			float2 uv_TexCoord15 = i.uv_texcoord * _CloudsTiling;
			float2 panner14 = ( mulTime17 * _PannerDirection + uv_TexCoord15);
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float cameraDepthFade9 = (( i.eyeDepth -_ProjectionParams.y - _CameraOffset ) / _CameraLength);
			float clampResult10 = clamp( cameraDepthFade9 , 0.0 , 1.0 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth23 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth23 = abs( ( screenDepth23 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _depthFade ) );
			float clampResult25 = clamp( distanceDepth23 , 0.0 , 1.0 );
			float temp_output_13_0 = ( ( _Transparancy * pow( tex2D( _Clouds001, panner14 ).r , _CloudsPower ) * tex2D( _Mask, uv_Mask ).r ) * clampResult10 * clampResult25 );
			o.Alpha = temp_output_13_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecularCustom alpha:fade keepalpha fullforwardshadows exclude_path:deferred noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack1.z = customInputData.eyeDepth;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.eyeDepth = IN.customPack1.z;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandardSpecularCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecularCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
923.2;206.4;1460;642;926.2238;222.3755;1.535391;True;False
Node;AmplifyShaderEditor.RangedFloatNode;18;-1221.718,512.621;Float;False;Property;_TimeScale;TimeScale;8;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;16;-1554.518,373.4208;Float;False;Property;_CloudsTiling;CloudsTiling;7;0;Create;True;0;0;False;0;0,0;1,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;17;-1044.118,399.0209;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;19;-1332.117,341.4211;Float;False;Property;_PannerDirection;PannerDirection;9;0;Create;True;0;0;False;0;0,0;0.03,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1452.118,128.6208;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;14;-1087.32,247.0209;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-618.3505,707.2166;Float;False;Property;_CameraLength;CameraLength;5;0;Create;True;0;0;False;0;0;14.69;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;123.885,770.2213;Float;False;Property;_depthFade;depthFade;10;0;Create;True;0;0;False;0;0;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-712.0885,456.4954;Float;False;Property;_CloudsPower;CloudsPower;2;0;Create;True;0;0;False;0;0;1.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-905.2999,111.2;Float;True;Property;_Clouds001;Clouds001;1;0;Create;True;0;0;False;0;None;783b1cc18a6bc4345a1dd80b44043e54;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-606.6501,813.8165;Float;False;Property;_CameraOffset;CameraOffset;6;0;Create;True;0;0;False;0;0;6.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;4;-537.9,184.5999;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;23;130.5399,557.6646;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-404.1885,559.0955;Float;False;Property;_Transparancy;Transparancy;3;0;Create;True;0;0;False;0;0;0.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-694.288,-122.6046;Float;True;Property;_Mask;Mask;4;0;Create;True;0;0;False;0;None;783b1cc18a6bc4345a1dd80b44043e54;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CameraDepthFade;9;-364.8489,639.6164;Float;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;25;351.085,639.0213;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;10;-95.74898,596.7164;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-160.9186,310.875;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;27;185.4848,191.0214;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;2.284781,234.8212;Float;False;Property;_FreshnelScale;FreshnelScale;11;0;Create;True;0;0;False;0;0;-0.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;104.5537,387.0099;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;507.7499,422.6035;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-66.86707,-356.1049;Float;False;Property;_fs;fs;12;0;Create;True;0;0;False;0;0;1.81;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-194.6165,-201.4609;Float;False;Property;_fp;fp;13;0;Create;True;0;0;False;0;0;5.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-242.1153,-53.80948;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;0,0,0,0;0.4622641,0.4622641,0.4622641,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;263.6566,290.5634;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;26;143.6849,-258.2787;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;34;774.9893,554.5323;Float;False;Constant;_Color0;Color 0;14;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;22;542.9,21.9;Float;False;True;2;Float;ASEMaterialInspector;0;0;StandardSpecular;Igori/AtmosphereClouds;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;Front;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;18;0
WireConnection;15;0;16;0
WireConnection;14;0;15;0
WireConnection;14;2;19;0
WireConnection;14;1;17;0
WireConnection;2;1;14;0
WireConnection;4;0;2;1
WireConnection;4;1;5;0
WireConnection;23;0;24;0
WireConnection;9;0;11;0
WireConnection;9;1;12;0
WireConnection;25;0;23;0
WireConnection;10;0;9;0
WireConnection;3;0;6;0
WireConnection;3;1;4;0
WireConnection;3;2;8;1
WireConnection;27;0;26;0
WireConnection;13;0;3;0
WireConnection;13;1;10;0
WireConnection;13;2;25;0
WireConnection;31;0;28;0
WireConnection;31;1;13;0
WireConnection;28;0;27;0
WireConnection;28;1;30;0
WireConnection;26;2;32;0
WireConnection;26;3;33;0
WireConnection;22;0;1;0
WireConnection;22;2;1;0
WireConnection;22;6;1;0
WireConnection;22;9;13;0
ASEEND*/
//CHKSM=4A899569B451B10E0F6CC7CBF2DB1670A08F33A1