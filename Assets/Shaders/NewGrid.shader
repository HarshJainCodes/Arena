Shader "Unlit/NewGrid"
{
    Properties
    {
        _distance("distance", Float) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}

        Pass
        {
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _distance;

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
                if (i.uv.x <= 0.006 || i.uv.y <= 0.006){
                        return float4(0, 0, 0, 1);
                    }
                if ((i.uv.x % _distance <= _distance - 0.005) & (i.uv.y % _distance <= _distance - 0.005)){
                    return float4(1, 0, 0, 0);
                }else{
                    return float4(0, 0, 0, 1);
                }
            }
            ENDCG
        }
    }
}
