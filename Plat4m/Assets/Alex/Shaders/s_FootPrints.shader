Shader "Alex/Footprints"
{
	Properties
	{
		// How many verts the plane has
		_Tess ("Tessellation", Range(1,32)) = 4
		// the Main Texture applied
		_MainTex("Main (RGB)", 2D) = "white" {}
		_MainColor("Main Color", color) = (1,1,1,1)
		// The Texture for the depth of the texture
		_DepthTex("Depth (RGB)", 2D) = "white" {}
		_DepthColor("Depth Color", color) = (1,1,1,1)
		_Splat("Splat Map", 2D) = "black" {}
		_NormalMap("Normalmap", 2D) = "bump" {}
		_Displacement("Displacement", Range(0, 1.0)) = 0.3
		_Color("Color", color) = (1,1,1,0)
		_SpecColor("Spec color", color) = (0.5,0.5,0.5,0.5)
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessDistance nolightmap
		#pragma target 4.6
		#include "Tessellation.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		float _Tess;

		float4 tessDistance(appdata v0, appdata v1, appdata v2) {
			float minDist = 10.0;
			float maxDist = 25.0;
			return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
		}

		sampler2D _Splat;
		float _Displacement;

		void disp(inout appdata v)
		{
			float d = tex2Dlod(_Splat, float4(v.texcoord.xy,0,0)).r *  _Displacement;
			v.vertex.xyz -= v.normal * d;
			v.vertex.xyz += v.normal * _Displacement;
		}

		sampler2D _DepthTex;
		fixed4 _DepthColor;
		sampler2D _MainTex;
		fixed4 _MainColor;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_DepthTex;
			float2 uv_Splat;
		};		

		void surf(Input IN, inout SurfaceOutput o) {

			half amount = tex2Dlod(_Splat, float4(IN.uv_Splat, 0, 0)).r;
		
			fixed4 c = lerp(tex2D(_MainTex, IN.uv_MainTex) * _MainColor, tex2D(_DepthTex, IN.uv_DepthTex) * _DepthColor, amount);
			
			o.Albedo = c.rgb;
			o.Specular = 0.2;
			o.Gloss = 1.0;
			
		}
		ENDCG
	}
		FallBack "Diffuse"
}
