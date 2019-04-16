Shader "Alex/s_ToonLitShader"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_OutlineColor("Outline Color", Color) = (0.0, 0.0, 0.0, 1)
		_Outline("Outline", Range(0.002,0.03)) = 0.005
	}

	CGINCLUDE
	#include "UnityCG.cginc"
	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};
	struct v2f
	{
		float4 pos : SV_POSITION;
		fixed4 color : COLOR;
	};

	uniform float  _Outline;
	uniform float4 _OutlineColor;
	v2f vert(appdata v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		float3 outNorm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
		float2 offset = TransformViewToProjection(outNorm.xy);

		o.pos.xy += offset * _Outline;
		o.color = _OutlineColor;
		return o;

	}
	ENDCG

		SubShader
	{
		//Tags{"RenderType" = "Opaque"}
		//UsePass "Alex/ToonLighting/FORWARD"
		Pass
		{
			Tags{"LightMode" = "Always"}
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend Zero Zero
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4 _Color;
			fixed4 frag(v2f i) : SV_Target
			{
				
				return i.color * _Color;
			}

			ENDCG

		}
	}
		Fallback "Alex/ToonLighting"

}
