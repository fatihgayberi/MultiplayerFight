Shader "Wonnasmith/SpecialBall"
{
    Properties
    {
        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (1,1,1,1)
        _ColorStart ("Color Start", Range(0,1)) = 0
        _ColorEnd ("Color End", Range(0,1)) = 1
        _WaveSpeed ("Wave Speed ", Float) = 0.01
    }
    SubShader
    {
        Tags{ "RenderType"="Transparent" "Queue"="Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define TAU 6.28318530718

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            float GetWave(float2 uv)
            {
                float2 uvsCentered = uv * 2 - 1;
                float radialDistance = length(uvsCentered);
                float wave = cos((radialDistance - _Time.y * 0.1f) * TAU * 5) * 0.5 + 0.5;
                wave *= 1 - radialDistance;

                return wave;
            }

            float InverseLerp(float a, float b, float v)
            {
                return (v-a)/(b-a);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv;
                return o;
            }

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;
            float _WaveSpeed;

            
            fixed4 frag (v2f i) : SV_Target
            {
                float s = saturate(InverseLerp(_ColorStart, _ColorEnd,  i.uv.y));

                float xOffset = cos(i.uv.x * TAU * 8) * 0.01;
                float t = cos((i.uv.y + xOffset - _Time.y * _WaveSpeed ) * TAU * 10) * 0.5 + 0.5;

                float topBottomRemover = (abs(i.normal.y) < 0.999);
                float waves = t * topBottomRemover*3;

                float4 gradiedent = lerp(_ColorA, _ColorB, s);

                return (gradiedent * waves);
            }
            ENDCG
        }
    }
}
