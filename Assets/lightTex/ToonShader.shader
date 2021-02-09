Shader "Unlit/ToonShader"
{
    Properties
    {
        _LightTex("LightTexture", 2D) = "white" {}
        _eLight("environmentalLight",Range(0.0,1.0)) = 0.7 
        _OutLine("OutLine", Color) = (0,0,0,0)
        _OutLineTickness("OutLineTickness", float) = 0.1
        _Color("Color", Color) = (1,1,1,1)
        _Roughness("Roughness", Range(0.0, 1.0)) = 0.5
        _ColorBasis("ColorBasis",float) = 255
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass
        {
            Cull front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
            };

            uniform float3 _OutLine;
            uniform float _OutLineTickness;
            v2f vert (appdata v)
            {
                v2f o;
                v.vertex += float4(v.normal * 0.01f * _OutLineTickness,0);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(_OutLine,1);
                return col;
            }
            ENDCG
        }
        Pass
        {
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                half vdoth : TEXCOORD2;
                half spequer : TEXCOORD3;
            };

            uniform float3 _Color;
            uniform float _Roughness;

            uniform sampler2D _LightTex;
            uniform float _eLight;
            uniform float _ColorBasis;

            v2f vert (appdata v)
            {
               v2f o;
               v.vertex -= float4(v.normal * 0.01f,0);
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 worldPos = mul(unity_ObjectToWorld,v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                half3 viewDir    = normalize(ObjSpaceViewDir(v.vertex));
                float3 lightDir  = _WorldSpaceLightPos0.xyz;
                float3 halfDir = normalize(viewDir + lightDir);
                o.spequer        = dot(halfDir,o.normal.xyz);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float nl = dot(i.normal,lightDir);
                nl = max(nl,0.01);

                i.spequer=max(0,i.spequer);
                
                fixed3 lightCol = tex2D(_LightTex,float2(nl,0.5)) * (1-_eLight) * 255 / _ColorBasis;
                lightCol.r *= _Color.r;
                lightCol.g *= _Color.g;
                lightCol.b *= _Color.b;

                fixed4 col = fixed4((_Color * _eLight + lightCol) * (1 + i.spequer * _Roughness),1);

                return col;
            }
            ENDCG
        }
    }
}
