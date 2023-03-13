Shader "Wonnasmith/Player"
{
    Properties
    {
        [PerRendererData]_isSpecialActive ("EffectStop", Float) = 1
	    [PerRendererData]_isOutlineActive("Outline Active", Float) = 0
        
        _MainTex ("Texture", 2D) = "white" {}

        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (1,1,1,1)
        _ColorStart ("Color Start", Range(0,1)) = 0
        _ColorEnd ("Color End", Range(0,1)) = 1
        _WaveSpeed ("Wave Speed ", Float) = 0.01

		_Rate ("Rate", Float) = 0.5
		_Speed ("Speed", Float) = 0.5
        [HDR]_Color ("Color", Color) =  (1, 0, 0, 1)

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

            sampler2D _MainTex;

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;
            float _WaveSpeed;
            half _isSpecialActive;

            
            fixed4 _Color;
            fixed4 _MainTex_TexelSize;
		    half _Rate;
		    half _Speed;
		    half _isOutlineActive;

            fixed4 frag (v2f i) : SV_Target
            {
                half4 c = tex2D(_MainTex, i.uv);
                c.rgb *= c.a;

                half4 outlineC = _Color;
                outlineC.a *= ceil(c.a);

                outlineC.rgb *= outlineC.a;

                half _rateSin = _isOutlineActive * (_Rate + (sin(_Time.y * _Speed) * 0.5 + 0.5));

                fixed upAlpha = tex2D(_MainTex, i.uv + fixed2(0, _rateSin * _MainTex_TexelSize.y)).a;
                fixed downAlpha = tex2D(_MainTex, i.uv - fixed2(0, _rateSin * _MainTex_TexelSize.y)).a;
                fixed rightAlpha = tex2D(_MainTex, i.uv + fixed2(_rateSin *_MainTex_TexelSize.x, 0)).a;
                fixed leftAlpha = tex2D(_MainTex, i.uv - fixed2(_rateSin *_MainTex_TexelSize.x, 0)).a;


                float s = saturate(InverseLerp(_ColorStart, _ColorEnd,  i.uv.y));

                float xOffset = cos(i.uv.x * TAU * 8) * 0.01;
                float t = cos((i.uv.y + xOffset - _Time.y * _WaveSpeed ) * TAU * 10) * 0.5 + 0.5;

                float topBottomRemover = (abs(i.normal.y) < 0.999);
                float waves = t * topBottomRemover;

                float4 gradiedent = lerp(_ColorA, _ColorB, s);

                return lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha)) + ((gradiedent * waves) * _isSpecialActive);
            }
            ENDCG
        }
    }
}
