Shader "PBR Silhouette" {
	Properties{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor1("Outline Color 1", Color) = (0,0,0,1)
		_OutlineColor2("Outline Color 2", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0.0, 0.03)) = .005
		_Opacity("Outline Opacity", Range(0.0, 1.0)) = 1.0
		_MainTex("Base (RGB)", 2D) = "white" { }
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_MetalTex("Metallic", 2D) = "white" {}
		_SmoothTex("Smoothness", 2D) = "white" {}
		_Roughness("Roughness", Range(0.03, 10.0)) = 1.0
	}

		CGINCLUDE
#include "UnityCG.cginc"

	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : POSITION;
		float4 color : COLOR;
	};

	uniform float _Outline;
	uniform float _Roughness;
	uniform float _Opacity;
	uniform float4 _OutlineColor1;
	uniform float4 _OutlineColor2;

	v2f vert(appdata v) {
		// just make a copy of incoming vertex data but scaled according to normal direction
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		float2 offset = TransformViewToProjection(norm.xy);

		o.pos.xy += offset * o.pos.z * _Outline;

		float gradient = v.vertex.y / 0.5f;
		float4 col;
		col = _OutlineColor1 * gradient + _OutlineColor2 * (1 - gradient);
		col.w *= _Opacity;
		o.color = col;

		return o;
	}
	ENDCG

		SubShader{
		Tags{ "Queue" = "Geometry+1" }

		// note that a vertex shader is specified here but its using the one above
		Pass{
		Name "OUTLINE"
		Tags{ "LightMode" = "Always" }
		Cull Off
		ZWrite Off
		ZTest Always

		// you can choose what kind of blending mode you want for the outline
		Blend SrcAlpha OneMinusSrcAlpha // Normal
										//Blend One One // Additive
										//Blend One OneMinusDstColor // Soft Additive
										//Blend DstColor Zero // Multiplicative
										//Blend DstColor SrcColor // 2x Multiplicative

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		half4 frag(v2f i) : COLOR{
		return i.color;
	}
		ENDCG
	}


		CGPROGRAM
#pragma surface surf Standard fullforwardshadows

	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float2 uv_MetalTex;
		float2 uv_SmoothTex;
	};

	sampler2D _MainTex;
	sampler2D _BumpMap;
	sampler2D _MetalTex;
	sampler2D _SmoothTex;

	uniform float3 _Color;
	void surf(Input IN, inout SurfaceOutputStandard o) {
		fixed4 m = tex2D(_MetalTex, IN.uv_MetalTex);
		fixed4 s = tex2D(_SmoothTex, IN.uv_SmoothTex);
		o.Metallic = m.r;
		o.Smoothness = pow(s.r, _Roughness);

		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}
	ENDCG

	}

		SubShader{
		Tags{ "Queue" = "Geometry+1" }

		Pass{
		Name "OUTLINE"
		Tags{ "LightMode" = "Always" }
		Cull Front
		ZWrite Off
		ZTest Always
		Offset 15,15

		// you can choose what kind of blending mode you want for the outline
		Blend SrcAlpha OneMinusSrcAlpha // Normal
										//Blend One One // Additive
										//Blend One OneMinusDstColor // Soft Additive
										//Blend DstColor Zero // Multiplicative
										//Blend DstColor SrcColor // 2x Multiplicative

		CGPROGRAM
#pragma vertex vert
#pragma exclude_renderers gles xbox360 ps3
		ENDCG
		SetTexture[_MainTex]{ combine primary }
	}

		CGPROGRAM
#pragma surface surf Lambert
	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
	};
	sampler2D _MainTex;
	sampler2D _BumpMap;
	uniform float3 _Color;
	void surf(Input IN, inout SurfaceOutput o) {
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}
	ENDCG

	}

		Fallback "Diffuse"
}