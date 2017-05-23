// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/TerrainWorldSpace"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TexSize("Texture Size", Float) = 64
		_Scale("Scale", Float) = 1.0
	}
	SubShader
	{
		//Tags { "RenderType"="Opaque" }
		//LOD 100

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 texPos : TEXCOORD0;
			};

			sampler2D _MainTex;
			float _TexSize;
			float _Scale;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.texPos = mul(unity_ObjectToWorld,v.vertex)*_Scale;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture

				float2 p;
			p.x = 0.2f;
			p.y = 0.2f;

				fixed4 col = tex2D(_MainTex, i.texPos );

				return col;
			}
			ENDCG
		}
	}
}
