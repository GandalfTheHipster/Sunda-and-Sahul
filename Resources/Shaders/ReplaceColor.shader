Shader "Custom/ReplaceColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorToReplace ("Color To Replace", Color) = (1,0,0,1) // Red
        _ReplacementColor ("Replacement Color", Color) = (0,1,0,1) // Green
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
            float4 _ColorToReplace;
            float4 _ReplacementColor;
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
                float4 col = tex2D(_MainTex, i.uv);
                float diff = distance(col.rgb, _ColorToReplace.rgb);
                if (diff < _Threshold)
                {
                    col.rgb = _ReplacementColor.rgb;
                }
                return col;
            }
            ENDCG
        }
    }
}