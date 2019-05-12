Shader "Custom/ScratchNormal"
{
	Properties
	{
		_MainTex ("Diffuse Texture", 2D) = "white" {}
		_Normal ("Normal", 2D) = "bump" {}
		_Depth ("Depth", 2D) = "gray" {}
		_SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
		_Shininess ("Shininess", Float) = 10
		_AmplifyDepth ("Amplify Depth", Float) = 1
		_CelShadingLevels ("Cel Shading Levels", Float) = 0
	}

	SubShader {
		AlphaTest NotEqual 0.0
		Pass{	
			Tags { 
				"LightMode"="ForwardBase" 
			}
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			#pragma vertex vert  
			#pragma fragment frag 

			#include "UnityCG.cginc"

			// User-specified properties
			uniform sampler2D _MainTex;

			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 uv : TEXCOORD0;	
			};

			struct VertexOutput
			{
				float4 pos : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			VertexOutput vert(VertexInput input){
				VertexOutput output;

				output.pos = UnityObjectToClipPos(input.vertex);
				output.color = input.color;
				output.uv = float2(input.uv.xy);
				return output;
			}

			float4 frag(VertexOutput input) : COLOR
			{
				float4 diffuseColor = tex2D(_MainTex, input.uv);

				float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.xyz * diffuseColor.rgb * input.color.rgb;
				return float4(ambientLighting.rgb,diffuseColor.a);
			}

			ENDCG
		} Pass {
			Tags { "LightMode" = "ForwardAdd"}
			ZWrite Off
			Blend One One

			CGPROGRAM

			#pragma vertex vert  
			#pragma fragment frag 
			#pragma multi_compile_fwdadd

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			// User-specified properties
			uniform sampler2D _MainTex;
			uniform sampler2D _Normal;
			uniform sampler2D _Depth;
			uniform float4 _SpecColor; 
			uniform float4 _LightColor0;
			uniform float _Shininess;
			uniform float _AmplifyDepth;
			uniform float _CelShadingLevels;

			uniform float4 _Normal_ST;

			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 uv : TEXCOORD0;

				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct VertexOutput
			{
				float4 pos : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
				float4 posWorld : TEXCOORD1;

				float3 tangentWorld : TEXCOORD2;  
				float3 normalWorld : TEXCOORD3;
				float3 binormalWorld : TEXCOORD4;

				float3 lightDir : TEXCOORD5;
				LIGHTING_COORDS(6,7)
			};

			VertexOutput vert(VertexInput v){
				VertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.color = v.color;
				o.uv = float2(v.uv.xy);

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;
				o.tangentWorld = normalize(mul(modelMatrix, float4(v.tangent.xyz, 0.0)).xyz);
				o.normalWorld = normalize(mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
				o.binormalWorld = normalize(cross(o.normalWorld, o.tangentWorld) * v.tangent.w); // tangent.w is specific to Unity

				o.lightDir = ObjSpaceLightDir(v.vertex).xyz;
				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}

			float4 frag(VertexOutput input) : COLOR {
				float4 diffuseColor = tex2D(_MainTex, input.uv);

				//normal map stuff
				float4 encodedNormal = tex2D(_Normal, _Normal_ST.xy * input.uv + _Normal_ST.zw);
				float3 localCoords = float3(2.0 * encodedNormal.a - 1.0, 2.0 * encodedNormal.g - 1.0, 0.0);
				localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));
				float3x3 local2WorldTranspose = float3x3(input.tangentWorld, input.binormalWorld, input.normalWorld);
				float3 normalDirection = normalize(mul(localCoords, local2WorldTranspose));

				// float3 normalDirection = (tex2D(_Normal, input.uv).xyz - 0.5f) * 2.0f;
				// normalDirection = float3(mul(float4(normalDirection.xyz, 1.0f), unity_WorldToObject).xyz);
				// normalDirection.z *= -1;
				// normalDirection = normalize(normalDirection);
				// normalDirection = float3(0.0f, 0.0f, -1.0f);AAS

				//compute dynamic lighting
				float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
				float distance = length(vertexToLightSource);
				//float attenuation = 1.0f-((1/10.0f)*distance/*_LightPositionRange.w*/) ;//1.0f / distance;
				fixed atten = LIGHT_ATTENUATION(input)*2;
	
				float3 lightDirection = normalize(vertexToLightSource);

				//compute diffuse lighting
				float normalDotLight = dot(normalDirection, lightDirection);
				float3 diffuseLevel = diffuseColor.rgb * _LightColor0.rgb * input.color.rgb * atten * max(0.0f, normalDotLight);
			
				//compute specular lighting
				float specularLevel;
				if(normalDotLight < 0.0){
					specularLevel = 0.0f;
				}else{
					float3 viewDirection = float3(0.0f, 0.0f, -1.0f);
					specularLevel = atten * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
				}



				// Add cel-shading if enough levels were specified
				// if (_CelShadingLevels >= 2){
				// 	diffuseLevel = floor(diffuseLevel * _CelShadingLevels) / (_CelShadingLevels - 0.5f);
				// 	specularLevel = floor(specularLevel * _CelShadingLevels) / (_CelShadingLevels - 0.5f);
				// }

				//diffuseLevel = diffuseColor.rgb * atten * 2;

				float3 diffuseReflection = diffuseColor.rgb * input.color.rgb *
					_LightColor0.rgb * diffuseLevel;
				float3 specularReflection = float3(_LightColor0.rgb) * float3(_SpecColor.rgb) *
					input.color.rgb * specularLevel;

				diffuseReflection = diffuseColor * _LightColor0 * atten;

				return float4(diffuseReflection.rgb, diffuseColor.a);
			}
			ENDCG
		}
	}
	// The definition of a fallback shader should be commented out 
	// during development:
	// Fallback "Transparent/Diffuse"
}
