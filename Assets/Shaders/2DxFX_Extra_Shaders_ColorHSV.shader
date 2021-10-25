Shader "2DxFX_Extra_Shaders/ColorHSV"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _ColorHSV_Hue_1 ("_ColorHSV_Hue_1", Range(0, 360)) = 180
    _ColorHSV_Saturation_1 ("_ColorHSV_Saturation_1", Range(0, 2)) = 1
    _ColorHSV_Brightness_1 ("_ColorHSV_Brightness_1", Range(0, 2)) = 1
    _SpriteFade ("SpriteFade", Range(0, 1)) = 1
    _Color ("Color", Color) = (1,1,1,1)
    [HideInInspector] _StencilComp ("Stencil Comparison", float) = 8
    [HideInInspector] _Stencil ("Stencil ID", float) = 0
    [HideInInspector] _StencilOp ("Stencil Operation", float) = 0
    [HideInInspector] _StencilWriteMask ("Stencil Write Mask", float) = 255
    [HideInInspector] _StencilReadMask ("Stencil Read Mask", float) = 255
    [HideInInspector] _ColorMask ("Color Mask", float) = 15
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float _SpriteFade;
      uniform float4 _Color;
      uniform float _ColorHSV_Hue_1;
      uniform float _ColorHSV_Saturation_1;
      uniform float _ColorHSV_Brightness_1;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float4 color :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.texcoord.xy = in_v.texcoord.xy;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.color = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat0_d;
      float4 u_xlat1_d;
      float4 u_xlat2;
      float4 u_xlat10_3;
      float2 u_xlat4;
      float u_xlat8;
      float u_xlat12;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d = (_ColorHSV_Hue_1 * 0.0174532942);
          u_xlat1_d.x = cos(u_xlat0_d);
          u_xlat0_d = sin(u_xlat0_d);
          u_xlat4.x = (_ColorHSV_Saturation_1 * _ColorHSV_Brightness_1);
          u_xlat8 = (u_xlat1_d.x * u_xlat4.x);
          u_xlat0_d = (u_xlat0_d * u_xlat4.x);
          u_xlat1_d = (float4(u_xlat8, u_xlat8, u_xlat8, u_xlat8) * float4(0.412999988, 0.300000012, 0.588, 0.885999978));
          u_xlat2 = (float4(u_xlat8, u_xlat8, u_xlat8, u_xlat8) * float4(0.700999975, 0.587000012, 0.114, 0.298999995));
          u_xlat4.xy = ((float2(float2(_ColorHSV_Brightness_1, _ColorHSV_Brightness_1)) * float2(0.587000012, 0.114)) + u_xlat1_d.xw);
          u_xlat1_d.xy = ((float2(float2(_ColorHSV_Brightness_1, _ColorHSV_Brightness_1)) * float2(0.298999995, 0.587000012)) + (-u_xlat1_d.yz));
          u_xlat8 = (((-u_xlat0_d) * 0.202999994) + u_xlat4.y);
          u_xlat4.x = ((u_xlat0_d * 0.0350000001) + u_xlat4.x);
          u_xlat12 = ((u_xlat0_d * 1.25) + u_xlat1_d.x);
          u_xlat1_d.x = (((-u_xlat0_d) * 1.04999995) + u_xlat1_d.y);
          u_xlat10_3 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat1_d.x = (u_xlat1_d.x * u_xlat10_3.y);
          u_xlat12 = ((u_xlat12 * u_xlat10_3.x) + u_xlat1_d.x);
          u_xlat1_d.z = ((u_xlat8 * u_xlat10_3.z) + u_xlat12);
          u_xlat8 = ((_ColorHSV_Brightness_1 * 0.298999995) + u_xlat2.x);
          u_xlat2.xyz = ((float3(float3(_ColorHSV_Brightness_1, _ColorHSV_Brightness_1, _ColorHSV_Brightness_1)) * float3(0.587000012, 0.114, 0.298999995)) + (-u_xlat2.yzw));
          u_xlat8 = ((u_xlat0_d * 0.167999998) + u_xlat8);
          u_xlat12 = ((u_xlat0_d * 0.330000013) + u_xlat2.x);
          u_xlat12 = (u_xlat10_3.y * u_xlat12);
          u_xlat8 = ((u_xlat8 * u_xlat10_3.x) + u_xlat12);
          u_xlat2.xz = (((-float2(u_xlat0_d, u_xlat0_d)) * float2(0.497000009, 0.328000009)) + u_xlat2.yz);
          u_xlat0_d = ((u_xlat0_d * 0.291999996) + u_xlat2.y);
          u_xlat1_d.x = ((u_xlat2.x * u_xlat10_3.z) + u_xlat8);
          u_xlat8 = (u_xlat10_3.x * u_xlat2.z);
          u_xlat4.x = ((u_xlat4.x * u_xlat10_3.y) + u_xlat8);
          u_xlat1_d.y = ((u_xlat0_d * u_xlat10_3.z) + u_xlat4.x);
          u_xlat0_d = (u_xlat10_3.w * _SpriteFade);
          u_xlat0_d = (u_xlat0_d * in_f.color.w);
          out_f.color.w = (u_xlat0_d * _Color.w);
          out_f.color.xyz = (u_xlat1_d.xyz * in_f.color.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
