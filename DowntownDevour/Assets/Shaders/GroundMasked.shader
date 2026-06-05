// Flat-color unlit shader for ground, roads, and sidewalks.
// Uses stencil test NotEqual 1 so it skips pixels where HoleMask
// has already written stencil=1, creating the hole-in-ground illusion.
Shader "DowntownDevour/GroundMasked"
{
    Properties
    {
        _BaseColor ("Color", Color) = (0.35, 0.35, 0.35, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderType"  = "Opaque"
            "Queue"       = "Geometry"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "GroundMasked"
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Blend Off
            ZWrite On
            ZTest LEqual
            Cull Back

            Stencil
            {
                Ref   1
                Comp  NotEqual
                Pass  Keep
                Fail  Keep
                ZFail Keep
            }

            HLSLPROGRAM
            #pragma vertex   Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                return half4(_BaseColor.rgb, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
