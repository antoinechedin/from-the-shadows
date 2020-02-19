// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SolidColor" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1) //note: required but not used
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
		SubShader{
			Pass{

		// 1.) Skipping the base forward rendering pass in which ambient, vertex, and
		// main directional light will be applied. Spot/Points (Additional) lights require the "ForwardAdd" lightmode.
		// see: http://docs.unity3d.com/Manual/SL-PassTags.html
		Tags{ "LightMode" = "ForwardAdd" "RenderType" = "Opaque" }
		Cull off //material is two sided

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		// 2.) This matches the LightMode tag to ensure the shader compiles
		// properly for the forward right pass (forward base vs fowrward add)
		// Use either "multi_compile_" + "_fwdbase" or "_fwdadd".  For more/shadows on forward add mode, use multi_compile_fwdadd_fullshadows too.
#pragma multi_compile_fwdadd
#pragma multi_compile_fwdadd_fullshadows

		// 3.) Reference the Unity library that includes all the lighting shadow macros
#include "AutoLight.cginc"


	struct v2f
	{
		float4 pos : SV_POSITION;

		// 4.) The LIGHTING_COORDS macro (defined in AutoLight.cginc) defines the parameters needed to sample
		// the shadow map. The (0,1) specifies which unused TEXCOORD semantics to hold the sampled values -
		// As I'm not using any texcoords in this shader, I can use TEXCOORD0 and TEXCOORD1 for the shadow
		// sampling. If I was already using TEXCOORD for UV coordinates, say, I could specify
		// LIGHTING_COORDS(1,2) instead to use TEXCOORD1 and TEXCOORD2.
		LIGHTING_COORDS(0,1)
	};


	v2f vert(appdata_base v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		// 5.) The TRANSFER_VERTEX_TO_FRAGMENT macro populates the chosen LIGHTING_COORDS in the v2f structure
		// with appropriate values to sample from the shadow/lighting map
		TRANSFER_VERTEX_TO_FRAGMENT(o);

		return o;
	}

	uniform fixed _Cutoff;

	fixed4 frag(v2f i) : COLOR{

		// 6.) The LIGHT_ATTENUATION samples the shadowmap (using the coordinates calculated by TRANSFER_VERTEX_TO_FRAGMENT
		// and stored in the structure defined by LIGHTING_COORDS), and returns the value as a float.
		float attenuation = LIGHT_ATTENUATION(i);
		clip(attenuation - _Cutoff);  // make pixels transparent if sufficient light reaches them.  _Cutoff set in material properties.

	return fixed4(1.0,0.0,0.0,1.0) * attenuation;
	}

		ENDCG
	}
	}

		// 7.) To receive or cast a shadow, shaders must implement the appropriate "Shadow Collector" or "Shadow Caster" pass.
		// Although we haven't explicitly done so in this shader, if these passes are missing they will be read from a fallback
		// shader instead, so specify one here to import the collector/caster passes used in that fallback.
		Fallback "VertexLit"
}
