Shader "Alex/ToonLighting"
{
	
	Properties
	{	[HDR]
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4, 0.4, 0.4, 1)
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9, 0.9, 0.9, 1)
		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)	
		_MainTex("Main Texture", 2D) = "white" {}		
		_Glossines("Glossiness", Float) = 32		
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
		_OutlineColor("Outline Color", Color) = (0.0, 0.0, 0.0, 1)
		_Outline("Outline", Range(0.002,0.03)) = 0.005
	    
	}

	CGINCLUDE
	#include "UnityCG.cginc"	
	#include "Lighting.cginc"
	#include "AutoLight.cginc"
	
	struct appdata
	{
		float4 vertex : POSITION;
		float4 uv : TEXCOORD0;
		float3 normal : NORMAL;
		
	
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float3 worldNormal : NORMAL;
		float3 viewDir : TEXCOORD1;
		SHADOW_COORDS(2)

	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	
	v2f vert(appdata v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		o.worldNormal = UnityObjectToWorldNormal(v.normal);
		o.viewDir = WorldSpaceViewDir(v.vertex);
		TRANSFER_SHADOW(o);
		return o;
	}

	ENDCG   
	SubShader
	{	
		UsePass "Alex/ToonOutline/OUTLINE"
		Pass
		{
			Name "BASE"
			Tags
			{
			"LightMode" = "ForwardBase"
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			
			float4 _Color;
			float4 _AmbientColor;
			float  _Glossines;
			float4 _SpecularColor;
			float4 _RimColor;
			float  _RimAmount;
			float  _RimThreshold;

			float4 frag (v2f i) : SV_Target
			{
				float4 sample = tex2D(_MainTex, i.uv);
				// the normal vertex from the input
				float3 normal = normalize(i.worldNormal);
				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float  NdotH = dot(normal, halfVector);
				float  shadow = SHADOW_ATTENUATION(i);
				
				// theh normal dot product with the lights position to add shadow
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				// the seperation between the two colors smoothed  with added shadow
				float lightIntensity = smoothstep(0, 0.01, NdotL);

				// the color of the model is affected by the lighting
				float4 light = lightIntensity * _LightColor0 * shadow;

				// Calcualted the specular light
				float  specularIntensity = pow(NdotH * lightIntensity, _Glossines * _Glossines);
				float  specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;
				float4 rimDot = 1 - dot(viewDir, normal);
				float  rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;						
				
				// the model is added to the color of the Ambient color and the light and the specular lighting
				return _Color * sample * (_AmbientColor + light + specularIntensity + specular + rim);
			}
			ENDCG
		
		}

		// All Other Lights
		Pass
		{
			Name "FORWARD"
			Tags
			{
			"LightMode" = "ForwardAdd"
			}
			Blend One One
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"	
			#include "Lighting.cginc"
			#include "AutoLight.cginc"				

			float4 _Color;
			float4 _AmbientColor;
			float  _Glossines;
			float4 _SpecularColor;
			float4 _RimColor;
			float  _RimAmount;
			float  _RimThreshold;
		

			float4 frag(v2f i) : SV_Target
			{
				float4 sample = tex2D(_MainTex, i.uv);
				// the normal vertex from the input
				float3 normal = normalize(i.worldNormal);
				float3 viewDir = normalize(i.viewDir);
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float  NdotH = dot(normal, halfVector);
				float  shadow = SHADOW_ATTENUATION(i);

				// theh normal dot product with the lights position to add shadow
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				// the seperation between the two colors smoothed  with added shadow
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);

				// the color of the model is affected by the lighting
				float4 light = lightIntensity * _LightColor0;

				// Calcualted the specular light
				float  specularIntensity = pow(NdotH * lightIntensity, _Glossines * _Glossines);
				float  specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;
				float4 rimDot = 1 - dot(viewDir, normal);
				float  rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity * shadow);
				float4 rim = rimIntensity * _RimColor;

			
				// the model is added to the color of the Ambient color and the light and the specular lighting
				return _Color * sample *  (light + specularIntensity + specular + rim);
			}
				ENDCG
		}	
	
		
		// Shadow casting support.
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		
	}
}
