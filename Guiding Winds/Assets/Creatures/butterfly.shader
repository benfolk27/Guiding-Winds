Shader "Custom/ButterflyTextureFlapURP"
{
    // fix the color of main tex
    Properties
    {
        _MainTex ("Wing Texture", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
        _UVScale ("Mesh-to-UV Scale", Float) = 1.0
        _FlapSpeed ("Flap Speed", Float) = 2.0
        _FlapAmount ("Flap Angle (Radians)", Float) = 0.2
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
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #define TAU 6.28318530718

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uvCenter    : TEXCOORD0;
                float2 texUV       : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float  _Cutoff;
                float  _UVScale;
                float  _FlapSpeed;
                float  _FlapAmount;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float2 uvCenter = IN.positionOS.xy * _UVScale;

                float side = sign(uvCenter.x);                
                float hinge = saturate(abs(uvCenter.x));       
                float flapping = sin(_Time.y * _FlapSpeed * TAU);
                float angle = flapping * _FlapAmount * hinge * side;

                float s, c;
                sincos(angle, s, c);

                float3 pos = IN.positionOS.xyz;
                float xNew = pos.x * c + pos.z * s;
                float zNew = -pos.x * s + pos.z * c;
                pos.xz = float2(xNew, zNew);

                OUT.positionHCS = TransformObjectToHClip(pos);
                OUT.uvCenter = uvCenter;

                float2 unitUV = saturate(uvCenter * 0.5 + 0.5);
                OUT.texUV = TRANSFORM_TEX(unitUV, _MainTex);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.texUV);
                clip(col.a - _Cutoff);
                return col;
            }
            ENDHLSL
        }
    }
}