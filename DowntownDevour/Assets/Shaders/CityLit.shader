// Replacement for Universal Render Pipeline/Lit for all city objects: buildings,
// props, cars, windows, lamp globes.  Uses the same include chain and LIGHT_LOOP
// macros as GroundMasked (which is proven to render on iOS Safari WebGL2).
//
// Key difference from URP Lit: _EmissionColor is ALWAYS added to the output —
// no _EMISSION shader keyword.  URP's build-time keyword stripping silently
// removes the _EMISSION variant when no pre-baked materials use it, making all
// runtime EnableKeyword("_EMISSION") calls no-ops on mobile.  Eliminating the
// keyword means emission is unconditional and always visible.
Shader "DowntownDevour/CityLit"
{
    Properties
    {
        _BaseColor     ("Base Color", Color)  = (1, 1, 1, 1)
        [HDR]
        _EmissionColor ("Emission",   Color)  = (0, 0, 0, 0)
        _Smoothness    ("Smoothness", Float)  = 0.15
        _Metallic      ("Metallic",   Float)  = 0.0
    }

    SubShader
    {
        Tags
        {
            "RenderType"     = "Opaque"
            "Queue"          = "Geometry"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            Cull   Back
            ZWrite On
            ZTest  LEqual

            HLSLPROGRAM
            #pragma vertex   Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            // Forward+ clustered keywords — same pair as GroundMasked to handle
            // the rename across URP versions (_FORWARD_PLUS → _CLUSTER_LIGHT_LOOP).
            #pragma multi_compile _ _FORWARD_PLUS
            #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _EmissionColor;
                float  _Smoothness;
                float  _Metallic;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float  fogFactor   : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                VertexPositionInputs vpi = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = vpi.positionCS;
                OUT.positionWS  = vpi.positionWS;
                OUT.normalWS    = TransformObjectToWorldNormal(IN.normalOS);
                OUT.fogFactor   = ComputeFogFactor(vpi.positionCS.z);
                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                half3 N = normalize(half3(IN.normalWS));
                half3 V = normalize(half3(GetCameraPositionWS() - IN.positionWS));

                half3 albedo  = _BaseColor.rgb;
                float specPow = exp2(_Smoothness * 10.0 + 2.0);

                // Main directional light (moon at night, sun during day)
                Light mainLight  = GetMainLight();
                half  NdotLMain  = saturate(dot(N, half3(mainLight.direction)));
                half3 H_main     = normalize(half3(mainLight.direction) + V);
                half  spec_main  = pow(saturate(dot(N, H_main)), specPow) * _Smoothness * 0.4h;
                half3 color      = albedo * (mainLight.color * NdotLMain * 0.8h)
                                 + mainLight.color * spec_main;

                // Additional lights: player lantern, lamp posts, car headlights/taillights.
                // Uses the same LIGHT_LOOP macros as GroundMasked so Forward+ clustered
                // lighting works without the per-object cap of classic Forward.
                #if defined(_ADDITIONAL_LIGHTS) || defined(_FORWARD_PLUS) || defined(_CLUSTER_LIGHT_LOOP)
                InputData inputData = (InputData)0;
                inputData.positionWS              = IN.positionWS;
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionHCS);

                uint pixelLightCount = GetAdditionalLightsCount();
                LIGHT_LOOP_BEGIN(pixelLightCount)
                    Light  light = GetAdditionalLight(lightIndex, IN.positionWS);
                    half   NdotL = saturate(dot(N, half3(light.direction)));
                    half3  H_add = normalize(half3(light.direction) + V);
                    half   spec  = pow(saturate(dot(N, H_add)), specPow) * _Smoothness * 0.3h;
                    half   att   = light.distanceAttenuation;
                    color += albedo * light.color * att * NdotL
                           + light.color          * att * spec;
                LIGHT_LOOP_END
                #endif

                // Ambient (spherical harmonics — collapses to flat ambient in Flat mode)
                color += albedo * SampleSH(N) * 0.85h;

                // Emission — unconditional, no keyword required
                color += _EmissionColor.rgb;

                color = MixFog(color, IN.fogFactor);
                return half4(color, 1.0h);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
