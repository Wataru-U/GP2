﻿Shader "points"
{
    Properties
    {
        _color("Color", Color) = (0,0,0,1)
    }
    // https://light11.hatenadiary.com/entry/2018/02/02/001515
    SubShader
    {
        
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}

        Pass
        {
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #pragma target 3.0

           #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half size: PSIZE;
            };

            half _PointSize;
            half _Alpha;
            float4x4 _TransformMatrix;
            half4 _color;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex = mul(_TransformMatrix, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                half3 viewDir = ObjSpaceViewDir(v.vertex);
                o.size = _PointSize;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                half4 color = _color;
                return color;
            }
            ENDCG
        }
    }
}
