Shader "Unlit/HealthBar"
{
    Properties
    {
        _PlayerHealth ("PlayerHealth", Range(0, 1)) = 1
        _FullHealthColor ("Full Health", Color) = (0, 1, 0)
        _LowHealthColor ("Low Health", Color) = (1, 0, 0)
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

            float _PlayerHealth;
            float3 _FullHealthColor;
            float3 _LowHealthColor;

            fixed4 frag (v2f i) : SV_Target
            {
                float healthMask = i.uv.x < _PlayerHealth;

                float3 outColor = lerp(_LowHealthColor, _FullHealthColor, _PlayerHealth);

                float3 finalColor = outColor * healthMask;
                return float4(finalColor, 0.6);
            }
            ENDCG
        }
    }
}
