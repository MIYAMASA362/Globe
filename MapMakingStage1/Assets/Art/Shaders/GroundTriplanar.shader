// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Igori/GroundTriplanar"
{
	Properties
	{
		_ASEOutlineColor( "Outline Color", Color ) = (0,0,0,0)
		_ASEOutlineWidth( "Outline Width", Float ) = 0.003
		_Diffuse("Diffuse", 2D) = "white" {}
		_Emmision("Emmision", 2D) = "white" {}
		[HDR]_EmmisionColor("EmmisionColor", Color) = (0,0,0,0)
		_DiffuseColor("DiffuseColor", Color) = (0,0,0,0)
		_Normal("Normal", 2D) = "bump" {}
		_DiffuseSaturation("DiffuseSaturation", Float) = 0
		_NormalScale("NormalScale", Float) = 0
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
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Lambert keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _NormalScale;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform float _DiffuseSaturation;
		uniform float4 _DiffuseColor;
		uniform sampler2D _Emmision;
		uniform float4 _Emmision_ST;
		uniform float4 _EmmisionColor;

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackScaleNormal( tex2D( _Normal, uv_Normal ), _NormalScale );
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float3 desaturateInitialColor86 = tex2D( _Diffuse, uv_Diffuse ).rgb;
			float desaturateDot86 = dot( desaturateInitialColor86, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar86 = lerp( desaturateInitialColor86, desaturateDot86.xxx, _DiffuseSaturation );
			o.Albedo = ( float4( desaturateVar86 , 0.0 ) * _DiffuseColor ).rgb;
			float2 uv_Emmision = i.uv_texcoord * _Emmision_ST.xy + _Emmision_ST.zw;
			o.Emission = ( tex2D( _Emmision, uv_Emmision ) * _EmmisionColor ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
6.4;176;1460;660;-312.7964;651.2698;1.690629;True;False
Node;AmplifyShaderEditor.SamplerNode;78;1413.344,-890.3759;Float;True;Property;_Diffuse;Diffuse;12;0;Create;True;0;0;False;0;None;31c49e613c685a646a29136bcf6f021c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;87;2064.288,-459.6448;Float;False;Property;_DiffuseSaturation;DiffuseSaturation;17;0;Create;True;0;0;False;0;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;81;2394.788,-188.2616;Float;False;Property;_EmmisionColor;EmmisionColor;14;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;80;2438.752,-316.269;Float;True;Property;_Emmision;Emmision;13;0;Create;True;0;0;False;0;None;1a6ed33dbea520a48a7e60391c3a6955;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;88;1180.089,43.57874;Float;False;Property;_NormalScale;NormalScale;18;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;84;1525.822,-468.0972;Float;False;Property;_DiffuseColor;DiffuseColor;15;0;Create;True;0;0;False;0;0,0,0,0;0.9542113,1,0.5330188,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;86;1655.155,-593.2046;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;644.2809,333.6362;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;77;1199.515,275.1053;Float;False;Property;_FreshnelColor;FreshnelColor;11;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;66;727.2206,37.32735;Float;False;Property;_VertexPower;VertexPower;10;0;Create;True;0;0;False;0;0.45;0.45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;1841.97,-454.5728;Float;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;30;384.8045,129.6385;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-221.0854,-644.7664;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;902.5748,-190.1123;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;85;1430.302,-50.51223;Float;True;Property;_Normal;Normal;16;0;Create;True;0;0;False;0;None;f53512d44b91e954dae7bf028209df1a;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;2267.984,-431.8433;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TriplanarNode;69;-52.10404,193.6712;Float;True;Spherical;World;False;Mask;_Mask;white;7;None;Mid Texture 4;_MidTexture4;white;-1;None;Bot Texture 4;_BotTexture4;white;-1;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-944.8453,-128.7274;Float;False;Property;_GroundTiling;GroundTiling;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;67;1064.04,-87.67773;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-394.5252,5.677025;Float;False;Property;_GrassColor;GrassColor;5;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;202.7067,-109.6027;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-185.4517,277.4123;Float;False;Property;_MaskTiling;MaskTiling;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;32;475.1854,-112.5911;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;587.6028,-667.059;Float;False;Property;_RocksTiling;RocksTiling;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;35;-419.2241,-415.022;Float;False;Property;_DirtColor;DirtColor;6;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;8;-349.7554,-236.3735;Float;True;Spherical;Object;False;Grass;_Grass;white;1;None;Mid Texture 2;_MidTexture2;white;-1;None;Bot Texture 2;_BotTexture2;white;-1;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;55;663.7074,-220.8377;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;11;417.155,-382.4689;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;1302.215,152.9053;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;31;285.3148,348.6408;Float;False;Property;_MaskPower;MaskPower;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;75;856.3151,167.2053;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;72;950.655,-693.2406;Float;True;Spherical;World;False;Rocks;_Rocks;white;0;None;Mid Texture 5;_MidTexture5;white;-1;None;Bot Texture 5;_BotTexture5;white;-1;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;62;1384.148,-281.9305;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TriplanarNode;1;-921.7391,-703.4963;Float;True;Spherical;Object;False;Dirt;_Dirt;white;2;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;False;9;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1938.437,-77.75018;Float;False;True;2;Float;ASEMaterialInspector;0;0;Lambert;Igori/GroundTriplanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;True;0.003;0,0,0,0;VertexScale;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;86;0;78;0
WireConnection;86;1;87;0
WireConnection;74;0;69;1
WireConnection;74;1;31;0
WireConnection;83;0;86;0
WireConnection;83;1;84;0
WireConnection;30;0;69;1
WireConnection;30;1;31;0
WireConnection;34;0;1;0
WireConnection;34;1;35;0
WireConnection;65;0;55;3
WireConnection;65;1;66;0
WireConnection;85;5;88;0
WireConnection;82;0;80;0
WireConnection;82;1;81;0
WireConnection;69;3;48;0
WireConnection;67;0;65;0
WireConnection;10;0;8;0
WireConnection;10;1;14;0
WireConnection;32;0;74;0
WireConnection;8;3;2;0
WireConnection;11;0;34;0
WireConnection;11;1;10;0
WireConnection;11;2;32;0
WireConnection;76;0;75;0
WireConnection;76;1;77;0
WireConnection;72;3;73;0
WireConnection;62;0;72;0
WireConnection;62;1;11;0
WireConnection;62;2;67;0
WireConnection;1;3;2;0
WireConnection;0;0;83;0
WireConnection;0;1;85;0
WireConnection;0;2;82;0
ASEEND*/
//CHKSM=7256409BD3A3A4C4E609F396507D2092C93B4F55