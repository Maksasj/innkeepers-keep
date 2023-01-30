Shader "ItemGlitteringShader"
{
    Properties
    {
        _ItemTintColorOverlay("Item tint color overlay", Color) = (1, 1, 1, 1)
        _ItemTintStrength("Item tint strength", Range(0.0, 1.0)) = 0.7

        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo", 2D) = "white" {}

        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        _Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
        _GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
        [Enum(Metallic Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel("Smoothness texture channel", Float) = 0

        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0

        _BumpScale("Scale", Float) = 1.0
        [Normal] _BumpMap("Normal Map", 2D) = "bump" {}

        _Parallax("Height Scale", Range(0.005, 0.08)) = 0.02
        _ParallaxMap("Height Map", 2D) = "black" {}

        _OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
        _OcclusionMap("Occlusion", 2D) = "white" {}

        _EmissionColor("Color", Color) = (0,0,0)
        _EmissionMap("Emission", 2D) = "white" {}

        _DetailMask("Detail Mask", 2D) = "white" {}

        _DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
        _DetailNormalMapScale("Scale", Float) = 1.0
        [Normal] _DetailNormalMap("Normal Map", 2D) = "bump" {}

        [Enum(UV0,0,UV1,1)] _UVSec("UV Set for secondary textures", Float) = 0

        // Blending state
        [HideInInspector] _Mode("__mode", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
    }

        CGINCLUDE
#define UNITY_SETUP_BRDF_INPUT MetallicSetup
            ENDCG

        SubShader
        {
            Tags { "RenderType" = "Opaque" "PerformanceChecks" = "False" }
            LOD 300


            // ------------------------------------------------------------------
            //  Base forward pass (directional light, emission, lightmaps, ...)
            Pass
            {
                Name "FORWARD"
                Tags { "LightMode" = "ForwardBase" }

                Blend[_SrcBlend][_DstBlend]
                ZWrite[_ZWrite]

                CGPROGRAM
                #pragma target 3.0

            // -------------------------------------

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature_local _METALLICGLOSSMAP
            #pragma shader_feature_local _DETAIL_MULX2
            #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local _GLOSSYREFLECTIONS_OFF
            #pragma shader_feature_local _PARALLAXMAP

            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            // Uncomment the following line to enable dithering LOD crossfade. Note: there are more in the file to uncomment for other passes.
            //#pragma multi_compile _ LOD_FADE_CROSSFADE

            #pragma vertex vertBase
            #pragma fragment fragBase
            #include "UnityStandardCoreForward.cginc"

            ENDCG
        }
            // ------------------------------------------------------------------
            //  Additive forward pass (one light per pass)
            Pass
            {
                Name "FORWARD_DELTA"
                Tags { "LightMode" = "ForwardAdd" }
                Blend[_SrcBlend] One
                Fog { Color(0,0,0,0) } // in additive pass fog should be black
                ZWrite Off
                ZTest LEqual

                CGPROGRAM
                #pragma target 3.0

            // -------------------------------------


            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local _METALLICGLOSSMAP
            #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local _DETAIL_MULX2
            #pragma shader_feature_local _PARALLAXMAP

            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            // Uncomment the following line to enable dithering LOD crossfade. Note: there are more in the file to uncomment for other passes.
            //#pragma multi_compile _ LOD_FADE_CROSSFADE

            #pragma vertex vertAdd
            #pragma fragment fragAdd
            #include "UnityStandardCoreForward.cginc"

            ENDCG
        }
            // ------------------------------------------------------------------
            //  Shadow rendering pass
            Pass {
                Name "ShadowCaster"
                Tags { "LightMode" = "ShadowCaster" }

                ZWrite On ZTest LEqual

                CGPROGRAM
                #pragma target 3.0

            // -------------------------------------


            #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature_local _METALLICGLOSSMAP
            #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local _PARALLAXMAP
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing
            // Uncomment the following line to enable dithering LOD crossfade. Note: there are more in the file to uncomment for other passes.
            //#pragma multi_compile _ LOD_FADE_CROSSFADE

            #pragma vertex vertShadowCaster
            #pragma fragment fragShadowCaster

            #include "UnityStandardShadow.cginc"

            ENDCG
        }
            // ------------------------------------------------------------------
            //  Deferred pass
            Pass
            {
                Name "DEFERRED"
                Tags { "LightMode" = "Deferred" }

                CGPROGRAM
                #pragma target 3.0
                #pragma exclude_renderers nomrt


            // -------------------------------------

            #pragma shader_feature_local _NORMALMAP
            #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _EMISSION
            #pragma shader_feature_local _METALLICGLOSSMAP
            #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature_local _DETAIL_MULX2
            #pragma shader_feature_local _PARALLAXMAP

            #pragma multi_compile_prepassfinal
            #pragma multi_compile_instancing
            // Uncomment the following line to enable dithering LOD crossfade. Note: there are more in the file to uncomment for other passes.
            //#pragma multi_compile _ LOD_FADE_CROSSFADE

            #pragma vertex vertDeferred
            #pragma fragment fragDeferred

            #include "UnityStandardCore.cginc"

            ENDCG
        }

            // ------------------------------------------------------------------
            // Extracts information for lightmapping, GI (emission, albedo, ...)
            // This pass it not used during regular rendering.
            Pass
            {
                Name "META"
                Tags { "LightMode" = "Meta" }

                Cull Off

                CGPROGRAM
                #pragma vertex vert_meta
                #pragma fragment frag_meta

                #pragma shader_feature _EMISSION
                #pragma shader_feature_local _METALLICGLOSSMAP
                #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                #pragma shader_feature_local _DETAIL_MULX2
                #pragma shader_feature EDITOR_VISUALIZATION

                #include "UnityStandardMeta.cginc"
                ENDCG
            }

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                float4 _ItemTintColorOverlay;
                float _ItemTintStrength;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv);

                    // apply item color tint
                    col += _ItemTintColorOverlay*_ItemTintStrength;

                    // apply fog
                    return col;
                }
                ENDCG
            }
        }

            SubShader
        {
            Tags { "RenderType" = "Opaque" "PerformanceChecks" = "False" }
            LOD 150

            // ------------------------------------------------------------------
            //  Base forward pass (directional light, emission, lightmaps, ...)
            Pass
            {
                Name "FORWARD"
                Tags { "LightMode" = "ForwardBase" }

                Blend[_SrcBlend][_DstBlend]
                ZWrite[_ZWrite]

                CGPROGRAM
                #pragma target 2.0

                #pragma shader_feature_local _NORMALMAP
                #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
                #pragma shader_feature _EMISSION
                #pragma shader_feature_local _METALLICGLOSSMAP
                #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                #pragma shader_feature_local _SPECULARHIGHLIGHTS_OFF
                #pragma shader_feature_local _GLOSSYREFLECTIONS_OFF
            // SM2.0: NOT SUPPORTED shader_feature_local _DETAIL_MULX2
            // SM2.0: NOT SUPPORTED shader_feature_local _PARALLAXMAP

            #pragma skip_variants SHADOWS_SOFT DIRLIGHTMAP_COMBINED

            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog

            #pragma vertex vertBase
            #pragma fragment fragBase
            #include "UnityStandardCoreForward.cginc"

            ENDCG
            }
            
            // ------------------------------------------------------------------
            //  Additive forward pass (one light per pass)
            Pass
            {
                Name "FORWARD_DELTA"
                Tags { "LightMode" = "ForwardAdd" }
                Blend[_SrcBlend] One
                Fog { Color(0,0,0,0) } // in additive pass fog should be black
                ZWrite Off
                ZTest LEqual

                CGPROGRAM
                #pragma target 2.0

                #pragma shader_feature_local _NORMALMAP
                #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
                #pragma shader_feature_local _METALLICGLOSSMAP
                #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                #pragma shader_feature_local _SPECULARHIGHLIGHTS_OFF
                #pragma shader_feature_local _DETAIL_MULX2

                // SM2.0: NOT SUPPORTED shader_feature_local _PARALLAXMAP
                #pragma skip_variants SHADOWS_SOFT

                #pragma multi_compile_fwdadd_fullshadows
                #pragma multi_compile_fog

                #pragma vertex vertAdd
                #pragma fragment fragAdd
                #include "UnityStandardCoreForward.cginc"

            ENDCG
            }
            // ------------------------------------------------------------------
            //  Shadow rendering pass
            Pass {
                Name "ShadowCaster"
                Tags { "LightMode" = "ShadowCaster" }

                ZWrite On ZTest LEqual

                CGPROGRAM
                #pragma target 2.0

                #pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
                #pragma shader_feature_local _METALLICGLOSSMAP
                #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                #pragma skip_variants SHADOWS_SOFT
                #pragma multi_compile_shadowcaster

                #pragma vertex vertShadowCaster
                #pragma fragment fragShadowCaster

                #include "UnityStandardShadow.cginc"

                ENDCG
            }

            // ------------------------------------------------------------------
            // Extracts information for lightmapping, GI (emission, albedo, ...)
            // This pass it not used during regular rendering.
            Pass
            {
                Name "META"
                Tags { "LightMode" = "Meta" }

                Cull Off

                CGPROGRAM
                #pragma vertex vert_meta
                #pragma fragment frag_meta

                #pragma shader_feature _EMISSION
                #pragma shader_feature_local _METALLICGLOSSMAP
                #pragma shader_feature_local _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                #pragma shader_feature_local _DETAIL_MULX2
                #pragma shader_feature EDITOR_VISUALIZATION

                #include "UnityStandardMeta.cginc"
                ENDCG
            }
        }
}