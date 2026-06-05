// Renders the hole disc: writes stencil=1 to the disc region, writes depth,
// outputs NO color.  Any ground material with Stencil { Ref 1, Comp NotEqual }
// will skip pixels inside the hole, letting the camera background (dark void)
// show through.
Shader "DowntownDevour/HoleMask"
{
    SubShader
    {
        Tags
        {
            "RenderType"  = "Opaque"
            "Queue"       = "Geometry-10"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "HoleMask"
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Blend Off
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Off

            Stencil
            {
                Ref   1
                Comp  Always
                Pass  Replace
                Fail  Keep
                ZFail Keep
            }

            HLSLPROGRAM
            #pragma vertex   Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
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

            half4 Frag(Varyings IN) : SV_Target { return 0; }
            ENDHLSL
        }
    }
    FallBack Off
}
