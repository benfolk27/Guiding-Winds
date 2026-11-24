Shader "Custom/Fire"
{
    Properties
    {
        //[MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _MainTex("Main Texture", 2D) = "white" {}
        _BaseMap("Noise", 2D) = "white" {}

        [Header(Flame Motion)]

        // movement of flame
        _ScrollSpeed("Flame Speed (X, Y)", Vector) = (0.15, -0.5, 0, 0)
        _DistortionStrength("Distortion intensity", Range(0, 1)) = 0.25
        _FlamePower("Flame sharpness", Range(0.2, 5)) = 1.6

        [Header(Flame Gradient)]
        [HDR] _CoreColor("Core Color", Color) = (3.0, 2.2, 0.5, 1)
        [HDR] _MidColor("Mid Color", Color) = (2.0, 0.5, 0.05, 1)
        _TipColor("Tip Color", Color) = (0.4, 0.03, 0.01, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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
                float2 mainTexUV : TEXCOORD1;
                float2 rawUV : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            TEXTURE2D(_MainTex);

            SAMPLER(sampler_MainTex);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _MainTex_ST;
                float4 _BaseMap_ST;
                float4 _ScrollSpeed;
                half _DistortionStrength;
                half _FlamePower;
                half4 _CoreColor;
                half4 _MidColor;
                half4 _TipColor;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.rawUV = IN.uv;
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.mainTexUV = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {

                half4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.mainTexUV);

                float2 scrolledUV = IN.uv + _ScrollSpeed.xy * _Time.y;

                half noiseA = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, scrolledUV).r;
                half noiseB = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, scrolledUV * 1.65 - float2(0.0, _Time.y * 0.8)).r;
                half noise = lerp(noiseA, noiseB, 0.5);

                float2 distortedRawUV = IN.rawUV;
                distortedRawUV.x += (noise - 0.5) * _DistortionStrength;

                float horizontalMask = 1.0 - abs(distortedRawUV.x * 2.0 - 1.0);
                horizontalMask = saturate(horizontalMask * lerp(1.6, 0.4, saturate(IN.rawUV.y)));
                float verticalFade = saturate(1.0 - smoothstep(0.15, 1.0, IN.rawUV.y));

                half flameShape = noise * horizontalMask * verticalFade;

                
                half intensity = pow(saturate(flameShape * 2.2), _FlamePower);
                half3 midBlend = lerp(_TipColor.rgb, _MidColor.rgb, smoothstep(0.1, 0.55, intensity));
                half3 finalRGB = lerp(midBlend, _CoreColor.rgb, smoothstep(0.55, 1.0, intensity));
                finalRGB *= mainTex.rgb;

                half alpha = saturate(intensity * 1.4) * mainTex.a;

                //todo: add some nvironment distoritons

                return half4(finalRGB, alpha);
            }
            ENDHLSL
        }
    }
}