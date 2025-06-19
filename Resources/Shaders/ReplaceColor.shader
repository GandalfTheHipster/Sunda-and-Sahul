Shader "Custom/ReplaceColorMulti"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        // Color replacement pairs
        _ColorToReplace0 ("Color To Replace 0", Color) = (1,0,0,1)
        _ReplacementColor0 ("Replacement Color 0", Color) = (0,1,0,1)

        _ColorToReplace1 ("Color To Replace 1", Color) = (0,0,1,1)
        _ReplacementColor1 ("Replacement Color 1", Color) = (1,1,0,1)

        _ColorToReplace2 ("Color To Replace 2", Color) = (1,1,1,1)
        _ReplacementColor2 ("Replacement Color 2", Color) = (0,0,0,1)

        _ColorToReplace3 ("Color To Replace 3", Color) = (0,1,1,1)
        _ReplacementColor3 ("Replacement Color 3", Color) = (1,0,1,1)

        _Threshold ("Threshold", Range(0,1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;

            float4 _ColorToReplace0;
            float4 _ReplacementColor0;

            float4 _ColorToReplace1;
            float4 _ReplacementColor1;

            float4 _ColorToReplace2;
            float4 _ReplacementColor2;

            float4 _ColorToReplace3;
            float4 _ReplacementColor3;

            float _Threshold;

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
                fixed4 col = tex2D(_MainTex, i.uv);

                if (distance(col.rgb, _ColorToReplace0.rgb) < _Threshold)
                    col.rgb = _ReplacementColor0.rgb;
                else if (distance(col.rgb, _ColorToReplace1.rgb) < _Threshold)
                    col.rgb = _ReplacementColor1.rgb;
                else if (distance(col.rgb, _ColorToReplace2.rgb) < _Threshold)
                    col.rgb = _ReplacementColor2.rgb;
                else if (distance(col.rgb, _ColorToReplace3.rgb) < _Threshold)
                    col.rgb = _ReplacementColor3.rgb;

                return col;
            }
            ENDCG
        }
    }
}