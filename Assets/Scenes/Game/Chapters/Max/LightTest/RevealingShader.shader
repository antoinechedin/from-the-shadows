﻿Shader "Custom/GlobalDissolveSurface" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_BorderDistance("Border distance", Range(0,1)) = 0.5
		_Radius("Distance", Float) = 1 //distance where we start to reveal the objects

	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200
			Cull off //material is two sided

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos; //Built-in world position
		};

		half _Glossiness;
		half _Metallic;

		half _BorderDistance;
		fixed4 _Color;

		float3 _PlayerPos; //"Global Shader Variable", contains the Player Position
		float _Radius;



		void surf(Input IN, inout SurfaceOutputStandard o) {

			float dist = distance(_PlayerPos, IN.worldPos);

			float clip_value = 1 - (dist - _Radius);
			clip(clip_value); // what is clipped

			o.Emission = float3(1, 1, 1) * step(clip_value, _BorderDistance/10) * step(_Radius, dist);


			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}

		ENDCG
		}
			FallBack "Diffuse"
}