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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (_Time.y < 5){
                    return float4(0, 0, 0, 0);
                }

                float3 col = float3(0.7, 0, 0);

                if (i.uv.x < 0.02 || i.uv.x > 0.98 || i.uv.y < 0.02 || i.uv.y > 0.98){
                    col *= 0;
                }
                return float4(col, min((_Time.y - 5) / 2, 0.7) * (1 - i.uv.y));
            }
            ENDCG
        }
    }
}
