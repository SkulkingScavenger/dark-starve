
Shader "Custom/lightmapSprite"
{
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_MainTex2 ("Texture", 2D) = "white" {}
	}
	SubShader {
		Pass {
			Tags { 
				"LightMode"="ForwardBase" 
			}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			#pragma vertex vert  
			#pragma fragment frag 

			#include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 posWorld : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _MainTex2;
			float4 _MainTex2_ST;
			float4x4 Unity_WorldToCamera;
			
			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex2);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target {
				// sample the texture
				fixed4 sprite = tex2D(_MainTex, i.uv);

				float2 pixelcoords;
				//convert world coords to pixels: -8=0 8=1024
				float pixelsPerUnit = 64;
				float screenWidth = 1024;
				float screenHeight = 768;
				
				// _WorldSpaceCameraPos.y, its still cool 
				pixelcoords.x = ((i.posWorld.x + 8)*pixelsPerUnit)/screenWidth;
				pixelcoords.y = ((i.posWorld.y + 6)*pixelsPerUnit)/screenHeight;

				fixed4 lightmap = tex2D(_MainTex2, pixelcoords);

				float4 color = (sprite*6 + lightmap) / 7;
				color = color * lightmap;
				color.a = sprite.a;

				return color;
				//return float4(lightmap.b,lightmap.b,0,sprite.a);
			}
			ENDCG
		}
	}
}
