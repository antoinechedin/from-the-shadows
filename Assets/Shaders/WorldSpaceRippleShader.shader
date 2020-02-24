Shader "Custom/WorldSpaceRippleShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _RippleOrigin("Ripple Origin", Vector) = (0, 0, 0, 0)
        _RippleDistance("Ripple Distance", Float) = 0
        _RippleWidth("Ripple Width", Float) = 0.1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _RippleOrigin;
        float _RippleDistance;
        float _RippleWidth;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float Abs(float r)
        {
            if (r < 0)
            {
                return -r;
            }

            return r;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 localPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
            // Albedo comes from a texture tinted by color
            float distance = length(localPos - _RippleOrigin.xyz);

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            if ((distance + _RippleWidth) > _RippleDistance) o.Alpha = 0;
            else o.Alpha = ((distance + _RippleWidth) / _RippleDistance) * c.a;

            o.Albedo = ((distance + _RippleWidth) / _RippleDistance) * c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
