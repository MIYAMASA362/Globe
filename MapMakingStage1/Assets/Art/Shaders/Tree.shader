// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Igori/Tree"
{
	Properties
	{
		_Alpha("Alpha", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.01
		_Diffuse("Diffuse", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
		_WindPower("WindPower", Float) = 0
		_Wind("Wind", 2D) = "white" {}
		_Tiling("Tiling", Float) = 0
		_TimeScale("TimeScale", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:forward vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _WindPower;
		uniform sampler2D _Wind;
		uniform float _TimeScale;
		uniform float _Tiling;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform float4 _Color;
		uniform sampler2D _Alpha;
		uniform float4 _Alpha_ST;
		uniform float _Cutoff = 0.01;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime13 = _Time.y * _TimeScale;
			float2 temp_cast_0 = (_Tiling).xx;
			float2 uv_TexCoord10 = v.texcoord.xy * temp_cast_0;
			float2 panner9 = ( mulTime13 * float2( 1,1 ) + uv_TexCoord10);
			float3 ase_vertexNormal = v.normal.xyz;
			v.vertex.xyz += ( _WindPower * tex2Dlod( _Wind, float4( panner9, 0, 0.0) ) * float4( ase_vertexNormal , 0.0 ) ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			o.Albedo = ( tex2D( _Diffuse, uv_Diffuse ) * _Color ).rgb;
			o.Alpha = 1;
			float2 uv_Alpha = i.uv_texcoord * _Alpha_ST.xy + _Alpha_ST.zw;
			float4 tex2DNode2 = tex2D( _Alpha, uv_Alpha );
			clip( tex2DNode2.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
645.6;88.8;1071;800;1198.445;241.7778;1.626177;True;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-1240.906,824.8787;Float;False;Property;_Tiling;Tiling;6;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-964.8124,956.2294;Float;False;Property;_TimeScale;TimeScale;7;0;Create;True;0;0;False;0;0;1.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1004.854,713.0255;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;12;-993.5451,791.015;Float;False;Constant;_WindDirection;WindDirection;7;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;13;-725.713,946.9938;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;9;-762.519,711.7779;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;4;-752.4318,87.57261;Float;False;Property;_Color;Color;3;0;Create;True;0;0;False;0;0,0,0,0;0.905741,1,0.6745283,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-703.5,-115;Float;True;Property;_Diffuse;Diffuse;2;0;Create;True;0;0;False;0;None;dda2b7f4d155ce74ea0137bf06931a33;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-359.3889,368.2301;Float;False;Property;_WindPower;WindPower;4;0;Create;True;0;0;False;0;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-300.3868,857.8672;Float;True;Property;_Wind;Wind;5;0;Create;True;0;0;False;0;None;44680e884f8253348bb26dc35c9c8ddd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;5;-500.9828,530.554;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-100.2205,559.9849;Float;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-247.5,-77;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-770.7006,283.2403;Float;True;Property;_Alpha;Alpha;0;0;Create;True;0;0;False;0;None;5196caf1cecefa34e9635d621e92c281;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;18;-372.3472,221.6826;Float;False;Property;_Transmission;Transmission;8;0;Create;True;0;0;False;0;0,0,0,0;0.5566037,0.5566037,0.5566037,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-1.626177,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Igori/Tree;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.01;True;True;0;False;TransparentCutout;;AlphaTest;DeferredOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;-34;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;11;0
WireConnection;13;0;14;0
WireConnection;9;0;10;0
WireConnection;9;2;12;0
WireConnection;9;1;13;0
WireConnection;8;1;9;0
WireConnection;6;0;7;0
WireConnection;6;1;8;0
WireConnection;6;2;5;0
WireConnection;3;0;1;0
WireConnection;3;1;4;0
WireConnection;0;0;3;0
WireConnection;0;9;2;1
WireConnection;0;10;2;1
WireConnection;0;11;6;0
ASEEND*/
//CHKSM=CEFC8FCB34CBD788E45B2546BBE8A0C933A812C0