Shader "Unlit/Dissolve"
{
    Properties
    {
        [Header(Base Texture)]
        [Space(10)]
        [NoScaleOffset]_Albedo("Albedo",2D)="bump"{}
        [NoScaleOffset]_Specular("Specular_Smoothness",2D)="black"{}
        [NoScaleOffset]_Normal("Normal",2D)="bump"{}
        [NoScaleOffset]_AO("AO",2D)="white"{}
        [Header(Dissolve Properties)][Space(10)]
        _Noise ("Dissolve Noise" ,2D)="white"{}
        _Dissolve("Dissolve" ,Range(0,1))=0
        [NoScaleOffset]_Gradient("Edge Gradient" ,2D)="black"{}
        _Range( "Edge Range" ,Range(2,100))=6
        _Brightness( "Brightness" ,Range(0,10))=1
    }
    SubShader
    {
        Tags { 
                "RenderType"="TransparentCutout" 
                "Queue"="AlphaTest"
             }
        LOD 100


            CGPROGRAM
            #pragma surface surf StandardSpecular addshadow fullforwardshadows
            #include "UnityCG.cginc"

            struct Input{
                float2 uv_Albedo;
                float2 uv_Noise;
            };

            sampler2D _Albedo;
            sampler2D _Specular;
            sampler2D _Normal;
            sampler2D _AO;

            sampler2D _Noise;
            fixed _Dissolve;
            sampler2D _Gradient;
            float _Range;
            float _Brightness;

            //代码提示: Input就是上面声明的Input结构体,SurfaceOutputStandardSpecular表示自带的高光工作流结构体
            void surf(Input IN, inout SurfaceOutputStandardSpecular o)
            {
                //根据噪声图剪裁
                //代码提示: tex2D(纹理,纹理坐标)用于获取纹理的颜色信息等，.r表示颜色ngba中的r值(用xyzw也是一样的道理)
                fixed noise = tex2D(_Noise,IN.uv_Noise).r;
                fixed dissolve = _Dissolve * 2 - 1;
                //代码提示: saturate(x)表示将x的范围截取到(0,1)之间。
                fixed mask = saturate(noise - dissolve);
                //代码提示:clip(x)如果x小于0,则在最终输出中剔除这个像素。
                clip(mask - 0.5);
                //边缘燃烧
                fixed texcoord = saturate(mask *_Range - 0.5 *_Range);
                o.Emission = tex2D(_Gradient,fixed2(texcoord,0.5)) * _Brightness;
                //正常的纹理
                fixed4 c = tex2D(_Albedo,IN.uv_Albedo);
                o.Albedo = c.rgb;
                //高光
                fixed4 specular = tex2D(_Specular, IN.uv_Albedo);
                o.Specular = specular.rgb;
                o.Smoothness = specular.a;
                //法线与环境光遮蔽
                o.Normal = UnpackNormal(tex2D(_Normal,IN.uv_Albedo));
                o.Occlusion = tex2D(_AO,IN.uv_Albedo);
            }
            ENDCG
    }
}
