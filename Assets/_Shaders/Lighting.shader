Shader "Hidden/Lighting"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightingTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.uv2 = v.uv;

#if UNITY_UV_STARTS_AT_TOP
					o.uv2.y = 1 - o.uv2.y;
#endif

				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _LightingTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 lighting = tex2D(_LightingTex, i.uv2);

				float greyscaleValue = col.r * 0.2126 + col.g * 0.7152 + col.b * 0.0622;
				fixed4 greyscale = fixed4(greyscaleValue, greyscaleValue, greyscaleValue, greyscaleValue);
				
				fixed4 minLighting = clamp(lighting, 0.1, 1);

				//return col * lighting;
				return lerp(greyscale, col, pow(lighting.a, 2)) * minLighting;
			}
			ENDCG
		}
	}
}
