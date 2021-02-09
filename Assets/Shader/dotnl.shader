Shader "Unlit/dotnl"
{
    Properties
    {
        _Color ("Color", Color) = (1.0,1.0,1.0,0)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
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
                float3 normal : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            uniform float3 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float nl = dot(i.normal,lightDir);
                float3 c = i.normal + float3(1,1,1);
                c /= 2;
                return fixed4(_Color * (nl/2 + 0.5),1);
            }
            ENDCG
        }
    }
}
