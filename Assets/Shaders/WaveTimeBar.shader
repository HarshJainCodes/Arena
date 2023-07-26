Shader "Unlit/WaveTimeBar"
{
    Properties
    {
        _WaveTime ("WaveTime", Range(0, 1)) = 1
        _HighTimeColor ("More Time", Color) = (0, 1, 0)
        _LowTimeColor ("Low Time", Color) = (1, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}

        Pass
        {
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
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float _WaveTime;
            float3 _HighTimeColor;
            float3 _LowTimehColor;

            fixed4 frag (v2f i) : SV_Target
            {
                float WaveTimeMask = i.uv.x < _WaveTime;

                float3 outColor = lerp(_LowTimehColor, _HighTimeColor, _WaveTime);

                float3 finalColor = outColor * WaveTimeMask;
                return float4(finalColor, 0.6);
            }
            ENDCG
        }
    }
}
