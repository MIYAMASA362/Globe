// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Igori/BackGroundStars"
{
	Properties
	{
		_Stars("Stars", 2D) = "white" {}
		[HDR]_StarsColor("StarsColor", Color) = (0,0,0,0)
		_StarsTiling("StarsTiling", Vector) = (0,0,0,0)
		_TimeScale("TimeScale", Float) = 1
		_Sky1("Sky1", Color) = (0.4938145,0.5966114,0.764151,0)
		_sky("sky", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Front
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting keepalpha addshadow fullforwardshadows exclude_path:deferred noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _Stars;
		uniform float _TimeScale;
		uniform float2 _StarsTiling;
		uniform float4 _StarsColor;
		uniform float4 _Sky1;
		uniform float _sky;

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			c.rgb = 0;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			float2 temp_cast_0 = (_TimeScale).xx;
			float2 uv_TexCoord11 = i.uv_texcoord * _StarsTiling;
			float2 panner13 = ( _Time.y * temp_cast_0 + uv_TexCoord11);
			float4 lerpResult25 = lerp( ( tex2D( _Stars, panner13 ) * _StarsColor ) , _Sky1 , _sky);
			o.Emission = lerpResult25.rgb;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
574.4;197.6;1355;610;916.253;275.3914;1.674935;True;False
Node;AmplifyShaderEditor.Vector2Node;12;-1207.212,4.357607;Float;False;Property;_StarsTiling;StarsTiling;2;0;Create;True;0;0;False;0;0,0;6,6;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;16;-810.3902,217.1616;Float;False;Property;_TimeScale;TimeScale;3;0;Create;True;0;0;False;0;1;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-992.3104,-55.16766;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;15;-811.8783,309.4261;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;13;-642.2305,157.636;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-437.5,-94;Float;True;Property;_Stars;Stars;0;0;Create;True;0;0;False;0;None;6e9b9f5978796b24ea4e8302d13c59be;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-236.5,197;Float;False;Property;_StarsColor;StarsColor;1;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.1886792,0.1877892,0.1877892,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;98.47506,91.65707;Float;False;Property;_sky;sky;5;0;Create;True;0;0;False;0;0;0.4999997;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-82.5,-122;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;19;71.9431,-736.9634;Float;False;Property;_Sky1;Sky1;4;0;Create;True;0;0;False;0;0.4938145,0.5966114,0.764151,0;0.4938145,0.5966114,0.764151,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;25;425.2645,-290.0383;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;297,-15;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Igori/BackGroundStars;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;Front;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;0;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;12;0
WireConnection;13;0;11;0
WireConnection;13;2;16;0
WireConnection;13;1;15;0
WireConnection;1;1;13;0
WireConnection;2;0;1;0
WireConnection;2;1;5;0
WireConnection;25;0;2;0
WireConnection;25;1;19;0
WireConnection;25;2;24;0
WireConnection;0;2;25;0
ASEEND*/
//CHKSM=9FBB99C76BD774C1E82D8C65531FF6F7CC0072CB