Shader "Unlit/selected"
{
    Properties
    {
        _Color("Color", Color) = (1,0,1,1)
        _Alpha ("Alpha"  , Range(0, 1)) = 0.3
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "LightMode"="ForwardBase" 
        }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
             #include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float3 normal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                // 必要なさげだけど少し前に出す
                v.vertex += float4(v.normal * 0.01f,0);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _Color;
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
