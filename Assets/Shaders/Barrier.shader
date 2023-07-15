Shader "Unlit/Barrier"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}

        Pass
        {
            Cull Off
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #include "unitycg.cginc"

            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD1;
                float testing : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.testing = length(o.viewDir);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //if (_Time.y < 5){
                //    return float4(0, 0, 0, 0);
                //}

                float3 col = float3(0.7, 0, 0);

                if (i.uv.x < 0.02 || i.uv.x > 0.98 || i.uv.y < 0.02 || i.uv.y > 0.98){
                    col *= 0;
                }

                return float4(col, 0.5);

                //float distanceToMesh = length(i.viewDir);
                //float distanceToMesh = i.testing;
                //float SeeDistance = step(distanceToMesh, 10);
                //return float4(col, min((_Time.y - 5) / 2, min(0.7, saturate(0.7 * (1 - i.uv.y) * SeeDistance/(distanceToMesh/ 10)))));
            }
            ENDCG
        }
    }
}
