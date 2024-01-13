Shader "Custom/field"
{
    Properties
    {
        _Color ("Main Color", Color) = (.5,.5,.5,1)
        _Alpha ("Alpha", Range (0.0, 1.0)) = 1.0
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }

    SubShader
    {
        Tags {"Queue" = "Overlay" }
        LOD 100
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha // Настройка блендинга для прозрачности

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        float _Alpha;

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a * _Alpha;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
