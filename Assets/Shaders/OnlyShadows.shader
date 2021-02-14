Shader "OnlyShadows"  {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Base (RGB)", 2D) = "white" {}
    }
        SubShader{
            Tags {"Queue" = "AlphaTest" "RenderType" = "TransparentCutout"}
            // Pass to render object as a shadow caster
            Pass {
                Name "Caster"
                Tags { "LightMode" = "ShadowCaster" }
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                #pragma multi_compile_shadowcaster
                #include "UnityCG.cginc"
                struct v2f {
                    V2F_SHADOW_CASTER;
                    float2  uv : TEXCOORD1;
                };
                uniform float4 _MainTex_ST;
                v2f vert(appdata_base v) {
                    v2f o;
                    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                    return o;
                }
                uniform sampler2D _MainTex;
                uniform fixed4 _Color;
                float4 frag(v2f i) : SV_Target{
                    fixed4 texcol = tex2D(_MainTex, i.uv);
                    clip(texcol.a* _Color.a - 0.9);
                    SHADOW_CASTER_FRAGMENT(i)
                }
                ENDCG
            }
    }
}