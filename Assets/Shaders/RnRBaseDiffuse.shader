Shader "RnRBaseDiffuse"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
    	
    	_MainTex ("Albedo (RGB)", 2D) = "white" {}
    	
		[PowerSlider(5.0)] _Shininess ("Shininess", Range (0.01, 1)) = 0.078125
    	
	    _Power ("Power", Range (0, 1)) = 0
        _MoveX ("Move X", Range(0,1)) = 0
        _MoveY ("Move Y", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        #pragma surface surf BlinnPhong

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        half _MoveX;
        half _MoveY;
        half _Shininess;
        half _Power;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
        	IN.uv_MainTex.x += _Time.z * _MoveX;
        	IN.uv_MainTex.y += _Time.z * _MoveY;
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
			o.Gloss = _Power;
        	o.Alpha = c.a;
        	o.Specular = _Shininess;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
