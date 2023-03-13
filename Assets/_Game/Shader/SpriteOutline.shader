Shader "Wonnasmith/SpriteOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		[HDR]_Color ("Color", Color) =  (1, 1, 1, 1)
		_Rate ("Rate", Float) = 0.5
		_Speed ("Speed", Float) = 0.5

		[PerRendererData] _isOutlineActive("Outline Active", Float) = 1
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 _Color;
            fixed4 _MainTex_TexelSize;
		    half _Rate;
		    half _Speed;
		    half _isOutlineActive;
            sampler2D _MainTex;

            fixed4 frag (v2f i) : COLOR
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

                return lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha));
            }
            ENDCG
        }
    }
}