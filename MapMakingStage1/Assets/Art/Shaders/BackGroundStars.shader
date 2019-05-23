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
		[Toggle]_gradientInvert("gradientInvert", Float) = 0
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_color2("color2", Color) = (0.1372549,0.1490196,0.2627451,1)
		[Toggle]_Texture("Texture", Float) = 0
		_color1("color1", Color) = (0.4196079,0.2431373,0.2588235,1)
		[HDR]_sttt("sttt", Color) = (0,0,0,0)
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

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float4 _sttt;
		uniform float _Texture;
		uniform float4 _color1;
		uniform float4 _color2;
		uniform float _gradientInvert;
		uniform sampler2D _Stars;
		uniform float _TimeScale;
		uniform float2 _StarsTiling;
		uniform float4 _StarsColor;

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
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode37 = tex2D( _TextureSample0, uv_TextureSample0 );
			float clampResult33 = clamp( ( lerp(( 1.0 - i.uv_texcoord.y ),i.uv_texcoord.y,_gradientInvert) * 1.4 ) , 0.0 , 1.0 );
			float4 lerpResult28 = lerp( _color1 , _color2 , clampResult33);
			float2 temp_cast_0 = (_TimeScale).xx;
			float2 uv_TexCoord11 = i.uv_texcoord * _StarsTiling;
			float2 panner13 = ( _Time.y * temp_cast_0 + uv_TexCoord11);
			o.Emission = ( ( tex2DNode37.r * _sttt ) + lerp(lerpResult28,( tex2D( _Stars, panner13 ) * _StarsColor ),_Texture) ).rgb;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
6.4;170.4;1460;666;1537.205;978.5565;1.974935;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-772.6211,-200.0195;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;34;-581.6282,-330.6644;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;12;-1207.212,4.357607;Float;False;Property;_StarsTiling;StarsTiling;2;0;Create;True;0;0;False;0;0,0;13,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ToggleSwitchNode;35;-496.2068,-215.0939;Float;False;Property;_gradientInvert;gradientInvert;4;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-287.3147,-52.62505;Float;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;False;0;1.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;15;-811.8783,309.4261;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-992.3104,-55.16766;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-810.3902,217.1616;Float;False;Property;_TimeScale;TimeScale;3;0;Create;True;0;0;False;0;1;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;13;-642.2305,157.636;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-163.3695,-154.7962;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-236.5,197;Float;False;Property;_StarsColor;StarsColor;1;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.754717,0.754717,0.754717,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-417.4008,33.29505;Float;True;Property;_Stars;Stars;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;33;-0.9006836,-193.3197;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;27;-345.9373,-375.8875;Float;False;Property;_color2;color2;6;0;Create;True;0;0;False;0;0.1372549,0.1490196,0.2627451,1;0.1339444,0.3320619,0.6603774,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;26;-324.1633,-548.4058;Float;False;Property;_color1;color1;8;0;Create;True;0;0;False;0;0.4196079,0.2431373,0.2588235,1;0.4196078,0.2431372,0.3318338,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;37;467.3535,-682.3165;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;False;0;None;3376aa4164c8aba47ad7d73f9d99921d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;41;453.5288,-427.5498;Float;False;Property;_sttt;sttt;9;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;28;87.8708,-416.0859;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-65.75066,-6.429508;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;828.7666,-524.3216;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;36;392.2311,-166.2453;Float;False;Property;_Texture;Texture;7;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;923.5639,-336.7028;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;919.6134,2.985891;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;671.8109,-18.34987;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Igori/BackGroundStars;False;False;False;False;True;True;True;True;True;True;True;True;False;False;True;False;False;False;False;False;Front;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;34;0;29;2
WireConnection;35;0;34;0
WireConnection;35;1;29;2
WireConnection;11;0;12;0
WireConnection;13;0;11;0
WireConnection;13;2;16;0
WireConnection;13;1;15;0
WireConnection;31;0;35;0
WireConnection;31;1;32;0
WireConnection;1;1;13;0
WireConnection;33;0;31;0
WireConnection;28;0;26;0
WireConnection;28;1;27;0
WireConnection;28;2;33;0
WireConnection;2;0;1;0
WireConnection;2;1;5;0
WireConnection;40;0;37;1
WireConnection;40;1;41;0
WireConnection;36;0;28;0
WireConnection;36;1;2;0
WireConnection;39;0;40;0
WireConnection;39;1;36;0
WireConnection;38;0;37;1
WireConnection;38;1;36;0
WireConnection;0;2;39;0
ASEEND*/
//CHKSM=CB2246DAB8ABA3B45DF9CAFF492BA8CB06424FA9