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
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float eyeDepth;
			float4 screenPos;
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
		uniform sampler2D _CameraDepthTexture;
		uniform float _depthFade;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Albedo = _Color.rgb;
			float mulTime17 = _Time.y * _TimeScale;
			float2 uv_TexCoord15 = i.uv_texcoord * _CloudsTiling;
			float2 panner14 = ( mulTime17 * _PannerDirection + uv_TexCoord15);
			float clampResult7 = clamp( pow( tex2D( _Clouds001, panner14 ).r , _CloudsPower ) , 0.0 , 1.0 );
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float cameraDepthFade9 = (( i.eyeDepth -_ProjectionParams.y - _CameraOffset ) / _CameraLength);
			float clampResult10 = clamp( cameraDepthFade9 , 0.0 , 1.0 );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth23 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth23 = abs( ( screenDepth23 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _depthFade ) );
			float clampResult25 = clamp( distanceDepth23 , 0.0 , 1.0 );
			o.Alpha = ( ( _Transparancy * clampResult7 * tex2D( _Mask, uv_Mask ).r ) * clampResult10 * clampResult25 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Lambert alpha:fade keepalpha fullforwardshadows noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 

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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
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
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
Version=15401
768.8;242.4;1355;610;1250.515;125.7787;1.6;True;False
Node;AmplifyShaderEditor.Vector2Node;16;-1554.518,373.4208;Float;False;Property;_CloudsTiling;CloudsTiling;7;0;Create;True;0;0;False;0;0,0;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;18;-1221.718,512.621;Float;False;Property;_TimeScale;TimeScale;8;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-1452.118,128.6208;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;19;-1332.117,341.4211;Float;False;Property;_PannerDirection;PannerDirection;9;0;Create;True;0;0;False;0;0,0;0.02,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;17;-1044.118,399.0209;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;14;-1087.32,247.0209;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-852.5,188;Float;True;Property;_Clouds001;Clouds001;1;0;Create;True;0;0;False;0;None;26200319cd04b1d40bc34448e1fff1eb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-712.0885,456.4954;Float;False;Property;_CloudsPower;CloudsPower;2;0;Create;True;0;0;False;0;0;2.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-606.6501,813.8165;Float;False;Property;_CameraOffset;CameraOffset;6;0;Create;True;0;0;False;0;0;6.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;4;-475.5,271;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-618.3505,707.2166;Float;False;Property;_CameraLength;CameraLength;5;0;Create;True;0;0;False;0;0;1.61;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;123.885,770.2213;Float;False;Property;_depthFade;depthFade;10;0;Create;True;0;0;False;0;0;0.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-541.7886,458.2955;Float;False;Property;_Transparancy;Transparancy;3;0;Create;True;0;0;False;0;0;0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-539.0885,94.99539;Float;True;Property;_Mask;Mask;4;0;Create;True;0;0;False;0;None;b12e60730bd3ac74a8a2da800a58d9bd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;7;-290.0885,291.4954;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CameraDepthFade;9;-364.8489,639.6164;Float;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;23;288.6851,525.4213;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-147.1001,329.2996;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;10;-95.74898,596.7164;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;25;351.085,639.0213;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-280.5,3;Float;False;Property;_Color;Color;0;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;78.45203,491.4165;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;22;112.5,9.099999;Float;False;True;2;Float;ASEMaterialInspector;0;0;Lambert;Igori/AtmosphereClouds;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;16;0
WireConnection;17;0;18;0
WireConnection;14;0;15;0
WireConnection;14;2;19;0
WireConnection;14;1;17;0
WireConnection;2;1;14;0
WireConnection;4;0;2;1
WireConnection;4;1;5;0
WireConnection;7;0;4;0
WireConnection;9;0;11;0
WireConnection;9;1;12;0
WireConnection;23;0;24;0
WireConnection;3;0;6;0
WireConnection;3;1;7;0
WireConnection;3;2;8;1
WireConnection;10;0;9;0
WireConnection;25;0;23;0
WireConnection;13;0;3;0
WireConnection;13;1;10;0
WireConnection;13;2;25;0
WireConnection;22;0;1;0
WireConnection;22;9;13;0
ASEEND*/
//CHKSM=F9053E5EF15CA4DB00EF8960E970F8A4A137384D