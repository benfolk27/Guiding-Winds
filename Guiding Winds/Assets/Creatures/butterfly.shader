Shader "Custom/ButterflyTextureFlapURP"
{
    Properties
    {
        _MainTex ("main Texture", 2D) = "white" {}
        _PartMask ("Part Mask", 2D) = "black" {}
        //_Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
        _FlapSpeed ("Flap Speed", Float) = 5.0

        // not exactly at 0.5 though but can try moving it around a bit or edge of the righ will will not be captured
        // very jank approach
        //keep this low or it will lgit look insane
        _FlapAmount ("Flap Amount", Float) = 0.002

        // this needs fixing, on to:do
        // part of right tip of wing, is still not caught,  mayb poor color job
        _CenterU ("Hinge Center U", Range(0, 1)) = 0.68
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "TransparentCutout"
            "Queue" = "AlphaTest"
            "RenderPipeline" = "UniversalPipeline"
        }

        Cull Off

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv0        : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 texUV       : TEXCOORD0;
                float2 maskUV      : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_PartMask);
            SAMPLER(sampler_PartMask);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _PartMask_ST;
                float  _Cutoff;
                float  _FlapSpeed;
                float  _FlapAmount;
                float  _CenterU;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.texUV  = TRANSFORM_TEX(IN.uv0, _MainTex);
                OUT.maskUV = TRANSFORM_TEX(IN.uv0, _PartMask);

                float3 mask = SAMPLE_TEXTURE2D_LOD(_PartMask, sampler_PartMask, OUT.maskUV, 0).rgb;

                // #FF00D5 color on uv sheet translate to this here -> (1.0, 0.0, 213.0 / 255.0)
                const float3 targetMagenta = float3(1.0, 0.0, 213.0 / 255.0);
                float isAnchor = step(length(mask - targetMagenta), 0.25);
                float anchorMask = 1.0 - isAnchor;

                // to-do: clean comments later
                // ok so now it's -0.5 to 0.5 * 2 it to get it to -1 to 1
                // the abs is the trick hree which maaks it curbves in from both directions
                // so it goes from -1 to 1 into 1 0.5 0 0.5 1, boom now wheen we multiply this with da sin function wave it looks lik a flap thats
                // symmtric
                float spanMask = abs(OUT.maskUV.x - _CenterU) * 2.0 * anchorMask;

                float wave = sin(_Time.y * _FlapSpeed * TWO_PI);
                float3 normalOS = normalize(IN.normalOS);

                float3 displacedOS = IN.positionOS.xyz + normalOS * (wave * spanMask * _FlapAmount);

                OUT.positionHCS = TransformObjectToHClip(displacedOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.texUV);
                //clip(col.a - _Cutoff);
                return col;
            }
            ENDHLSL
        }
    }
}