// PBR ground/road/sidewalk shader. Preserves the stencil hole illusion
// (Ref 1, Comp NotEqual) while adding specular highlights from the moon
// and point lights so roads read as wet pavement at night.
Shader "DowntownDevour/GroundMasked"
{
    Properties
    {
        _BaseColor      ("Color",           Color)  = (0.35, 0.35, 0.35, 1)
        _Smoothness     ("Smoothness",      Float)  = 0.05
        [HDR] _EmissionColor ("Emission Color", Color) = (0, 0, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "RenderType"     = "Opaque"
            "Queue"          = "Geometry"
            "RenderPipeline" = "UniversalPipeline"
        }

        // ── Forward Lit pass ────────────────────────────────────────────────
        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            Blend  Off
            ZWrite On
            ZTest  LEqual
            Cull   Back

            // Hole stencil: skip pixels inside the hole (stencil == 1)
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
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float  _Smoothness;
                float4 _EmissionColor;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                float  fogFactor   : TEXCOORD1;
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
                OUT.fogFactor   = ComputeFogFactor(vpi.positionCS.z);
                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                // Ground is always flat — world normal is straight up.
                const half3 N = half3(0.0h, 1.0h, 0.0h);
                half3 V = normalize(GetCameraPositionWS() - IN.positionWS);

                half3 albedo  = _BaseColor.rgb;
                // Blinn-Phong exponent: higher smoothness → tighter, brighter specular
                float specPow = exp2(_Smoothness * 10.0 + 2.0);

                // ── Main directional light (moon) ───────────────────────────
                Light mainLight  = GetMainLight();
                half  NdotLMain  = saturate(dot(N, mainLight.direction));
                half3 H_main     = normalize(mainLight.direction + V);
                half  spec_main  = pow(saturate(dot(N, H_main)), specPow) * _Smoothness;
                half3 color = albedo * (mainLight.color * NdotLMain * 0.30h)
                            + mainLight.color * spec_main * 0.8h;

                // ── Additional point lights (lamp posts, car lights, hole) ──
                #ifdef _ADDITIONAL_LIGHTS
                uint lightCount = GetAdditionalLightsCount();
                UNITY_LOOP
                for (uint i = 0u; i < lightCount; ++i)
                {
                    Light light  = GetAdditionalLight(i, IN.positionWS);
                    half  NdotL  = saturate(dot(N, light.direction));
                    half3 H_add  = normalize(light.direction + V);
                    half  spec   = pow(saturate(dot(N, H_add)), specPow) * _Smoothness;
                    half  att    = light.distanceAttenuation;
                    // Diffuse very subtle (road is dark); specular carries the wet look
                    color += albedo * light.color * att * NdotL * 0.18h
                           + light.color          * att * spec  * 2.2h;
                }
                #endif

                // ── Ambient ─────────────────────────────────────────────────
                color += albedo * SampleSH(N) * 0.85h;

                // ── Emission (lamp pools, emissive decals) ───────────────────
                color += half3(_EmissionColor.r, _EmissionColor.g, _EmissionColor.b);

                // ── Fog ─────────────────────────────────────────────────────
                color = MixFog(color, IN.fogFactor);

                return half4(color, 1.0h);
            }
            ENDHLSL
        }

        // Shadow caster and depth passes fall back to the URP Lit shader
    }
    FallBack "Universal Render Pipeline/Lit"
}
