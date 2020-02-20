Shader "FromTheShadows/LightPlatformShader"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1) //note: required but not used
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

	SubShader
	{
		Cull off
		
		// FIRST RENDER PASS, CLIPPING UNLIGHTED PARTS
		Pass
		{

			Tags{ "LightMode" = "ForwardAdd" "RenderType" = "TransparentCutout" }
			Blend One Zero
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			#pragma multi_compile_fwdadd
			#pragma multi_compile_fwadd_fullshadows

			#include "AutoLight.cginc"


			struct v2f
			{
				float4 pos : SV_POSITION;

				LIGHTING_COORDS(0,1)
			};


			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}

			uniform fixed _Cutoff;

			fixed4 frag(v2f i) : COLOR{

			float attenuation = LIGHT_ATTENUATION(i);
			clip(attenuation - _Cutoff);  // make pixels transparent if sufficient light reaches them.  _Cutoff set in material properties.

			return fixed4(1.0,1.0,0.0,1.0);
		}
		ENDCG
	}

		// SECOND RENDER PASS, RENDERING OTHER PARTS
		Pass
		{

			Tags { "LightMode" = "ForwardBase" "RenderType" = "TransparentCutout" }
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			#pragma multi_compile_fwdbase

			#include "AutoLight.cginc"


			struct v2f
			{
				float4 pos : SV_POSITION;

				LIGHTING_COORDS(0,1)
			};


			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}

			uniform fixed _Cutoff;

			fixed4 frag(v2f i) : COLOR
			{
				float attenuation = LIGHT_ATTENUATION(i);

				return fixed4(1.0,1.0,0.0,0.02);
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}