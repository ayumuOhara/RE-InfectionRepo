Shader "Custom/UI_Gradation"
{
    Properties
    {
        [MainColor] _ColorA("Color A", Color) = (1, 0, 0, 1)
        [MainColor] _ColorB("Color B", Color) = (0, 0, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.01
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        AlphaToMask On

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _ColorA;
                half4 _ColorB;
                float4 _BaseMap_ST;
                half _Cutoff;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half uvY = saturate(IN.uv.y);
                half4 gradColor = lerp(_ColorA, _ColorB, uvY);

                half alpha = texColor.a * gradColor.a;
                clip(alpha - _Cutoff); // アルファテストで透明部分を除去

                half3 rgb = texColor.rgb * gradColor.rgb;
                return half4(rgb, alpha);
            }

            ENDHLSL
        }
    }
}