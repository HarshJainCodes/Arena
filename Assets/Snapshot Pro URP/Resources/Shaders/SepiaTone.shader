﻿Shader "SnapshotProURP/SepiaTone"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
		Tags
		{
			"RenderType" = "Opaque"
			"RenderPipeline" = "UniversalPipeline"
		}

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "SnapshotUtility.hlsl"

            sampler2D _MainTex;

			CBUFFER_START(UnityPerMaterial)
				float _Strength;
			CBUFFER_END

            float4 frag (v2f i) : SV_Target
            {
				float4 col = tex2D(_MainTex, i.uv);
				
				float3x3 sepia = float3x3
				(
					0.393f, 0.349f, 0.272f,		// Red.
					0.769f, 0.686f, 0.534f,		// Green.
					0.189f, 0.168f, 0.131f		// Blue.
				);

				float3 sepiaTint = mul(col.rgb, sepia);

				col.rgb = lerp(col.rgb, sepiaTint, _Strength);

                return col;
            }
            ENDHLSL
        }
    }
}
