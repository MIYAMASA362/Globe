// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "OceanLava"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (1,0.4768409,0,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.007
		_LavaDiffuse("LavaDiffuse", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,0,0,0)
		_DistortionNormal2("DistortionNormal2", 2D) = "bump" {}
		_DistortionNormal("DistortionNormal", 2D) = "bump" {}
		_DistortionDirection("DistortionDirection", Vector) = (2,2,0,0)
		_DistortionDirection2("DistortionDirection2", Vector) = (2,2,0,0)
		_DistortionTiling("DistortionTiling", Vector) = (2,2,0,0)
		_Tiling("Tiling", Vector) = (2,2,0,0)
		_DistortionPower("DistortionPower", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		
		
		
		struct Input {
			half filler;
		};
		uniform half4 _ASEOutlineColor;
		uniform half _ASEOutlineWidth;
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz *= ( 1 + _ASEOutlineWidth);
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _ASEOutlineColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		ZTest LEqual
		Blend One Zero , Zero Zero
		BlendOp Add , Add
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _DistortionNormal;
		uniform float2 _DistortionDirection;
		uniform float2 _DistortionTiling;
		uniform sampler2D _LavaDiffuse;
		uniform float2 _Tiling;
		uniform sampler2D _DistortionNormal2;
		uniform float2 _DistortionDirection2;
		uniform float _DistortionPower;
		uniform float4 _Color0;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_TexCoord214 = i.uv_texcoord * _DistortionTiling;
			float2 panner207 = ( _Time.y * _DistortionDirection + uv_TexCoord214);
			float3 tex2DNode205 = UnpackNormal( tex2D( _DistortionNormal, panner207 ) );
			o.Normal = tex2DNode205;
			float2 uv_TexCoord220 = i.uv_texcoord * _Tiling;
			float2 panner229 = ( _Time.y * _DistortionDirection2 + uv_TexCoord214);
			float3 lerpResult215 = lerp( ( tex2DNode205 * UnpackNormal( tex2D( _DistortionNormal2, panner229 ) ) ) , float3(0,0,1) , _DistortionPower);
			o.Albedo = ( tex2D( _LavaDiffuse, ( float3( uv_TexCoord220 ,  0.0 ) * lerpResult215 ).xy ) * _Color0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
6.4;311.2;1461;526;158.8797;387.348;1.699628;True;False
Node;AmplifyShaderEditor.Vector2Node;208;388.8786,-192.1399;Float;False;Property;_DistortionTiling;DistortionTiling;6;0;Create;True;0;0;False;0;2,2;3,3.49;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;230;689.2344,360.4883;Float;False;Property;_DistortionDirection2;DistortionDirection2;5;0;Create;True;0;0;False;0;2,2;0.04,0.04;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;214;677.0349,-233.0139;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;212;484.8774,47.06;Float;False;Property;_DistortionDirection;DistortionDirection;4;0;Create;True;0;0;False;0;2,2;0.04,0.04;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;209;627.0265,232.7568;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;207;945.6788,-137.74;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;229;1003.666,216.0199;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;205;1307.133,-158.1156;Float;True;Property;_DistortionNormal;DistortionNormal;3;0;Create;True;0;0;False;0;None;dd2fd2df93418444c8e280f1d34deeb5;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;227;1279.005,95.34628;Float;True;Property;_DistortionNormal2;DistortionNormal2;2;0;Create;True;0;0;False;0;None;dd2fd2df93418444c8e280f1d34deeb5;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;1783.795,27.36118;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;216;1845.078,189.5751;Float;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector2Node;206;1462.183,-377.6597;Float;False;Property;_Tiling;Tiling;7;0;Create;True;0;0;False;0;2,2;8,8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;210;1632.163,272.2187;Float;False;Property;_DistortionPower;DistortionPower;8;0;Create;True;0;0;False;0;0;-0.66;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;215;2021.002,-15.01907;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;220;1665.948,-602.259;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;1914.983,-291.2599;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;196;2163.847,-411.4614;Float;True;Property;_LavaDiffuse;LavaDiffuse;0;0;Create;True;0;0;False;0;None;c789cf7efb34db945887c2bf80d51866;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;199;2587.623,-487.4474;Float;False;Property;_Color0;Color 0;1;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;2900.273,-536.3795;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;223;2759.381,158.9064;Float;False;Property;_DepthFadePosition;DepthFadePosition;9;0;Create;True;0;0;False;0;0;2.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;222;2482.34,24.63572;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;221;2834.164,-2.558334;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3142.558,-384.7502;Float;False;True;6;Float;ASEMaterialInspector;0;0;StandardSpecular;OceanLava;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;1.76;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;1;9.9;10;25;False;0.5;True;0;5;False;-1;10;False;-1;1;0;False;-1;0;False;-1;2;False;-1;1;False;-1;39;True;0.007;1,0.4768409,0,0;VertexScale;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;214;0;208;0
WireConnection;207;0;214;0
WireConnection;207;2;212;0
WireConnection;207;1;209;0
WireConnection;229;0;214;0
WireConnection;229;2;230;0
WireConnection;229;1;209;0
WireConnection;205;1;207;0
WireConnection;227;1;229;0
WireConnection;226;0;205;0
WireConnection;226;1;227;0
WireConnection;215;0;226;0
WireConnection;215;1;216;0
WireConnection;215;2;210;0
WireConnection;220;0;206;0
WireConnection;204;0;220;0
WireConnection;204;1;215;0
WireConnection;196;1;204;0
WireConnection;198;0;196;0
WireConnection;198;1;199;0
WireConnection;221;1;222;0
WireConnection;221;0;223;0
WireConnection;0;0;198;0
WireConnection;0;1;205;0
ASEEND*/
//CHKSM=7DA159E445DA15886E2EBE7C2E5B7EBF33189D1B