Shader "2DxFX/Standard/HSV"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Alpha ("Alpha", Range(0, 1)) = 1
    _Color ("_Color", Color) = (1,1,1,1)
    _HueShift ("HueShift", Range(0, 360)) = 0
    _Sat ("Saturation", float) = 1
    _Val ("Value", float) = 1
    _StencilComp ("Stencil Comparison", float) = 8
    _Stencil ("Stencil ID", float) = 0
    _StencilOp ("Stencil Operation", float) = 0
    _StencilWriteMask ("Stencil Write Mask", float) = 255
    _StencilReadMask ("Stencil Read Mask", float) = 255
    _ColorMask ("Color Mask", float) = 15
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
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
      uniform float _HueShift;
      uniform float _Sat;
      uniform float _Val;
      uniform float _Alpha;
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
      float3 u_xlat0_d;
      float4 u_xlat10_0;
      float4 u_xlat1_d;
      float4 u_xlat2;
      float3 u_xlat4;
      float2 u_xlat5;
      float u_xlat8;
      float u_xlat9;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          out_f.color.w = ((u_xlat10_0.w * in_f.color.w) + (-_Alpha));
          u_xlat0_d.xyz = (u_xlat10_0.xyz * in_f.color.xyz);
          u_xlat9 = (_HueShift * 0.0174532942);
          u_xlat1_d.x = sin(u_xlat9);
          u_xlat2.x = cos(u_xlat9);
          u_xlat9 = (_Sat * _Val);
          u_xlat4.x = (u_xlat2.x * u_xlat9);
          u_xlat9 = (u_xlat1_d.x * u_xlat9);
          u_xlat2 = (u_xlat4.xxxx * float4(0.412999988, 0.300000012, 0.588, 0.885999978));
          u_xlat1_d = (u_xlat4.xxxx * float4(0.700999975, 0.587000012, 0.114, 0.298999995));
          u_xlat5.xy = ((float2(float2(_Val, _Val)) * float2(0.298999995, 0.587000012)) + (-u_xlat2.yz));
          u_xlat2.xw = ((float2(float2(_Val, _Val)) * float2(0.587000012, 0.114)) + u_xlat2.xw);
          u_xlat5.x = ((u_xlat9 * 1.25) + u_xlat5.x);
          u_xlat8 = (((-u_xlat9) * 1.04999995) + u_xlat5.y);
          u_xlat8 = (u_xlat0_d.y * u_xlat8);
          u_xlat5.x = ((u_xlat5.x * u_xlat0_d.x) + u_xlat8);
          u_xlat8 = (((-u_xlat9) * 0.202999994) + u_xlat2.w);
          u_xlat2.x = ((u_xlat9 * 0.0350000001) + u_xlat2.x);
          out_f.color.z = ((u_xlat8 * u_xlat0_d.z) + u_xlat5.x);
          u_xlat1_d.x = ((_Val * 0.298999995) + u_xlat1_d.x);
          u_xlat4.xyz = ((float3(float3(_Val, _Val, _Val)) * float3(0.587000012, 0.114, 0.298999995)) + (-u_xlat1_d.yzw));
          u_xlat1_d.x = ((u_xlat9 * 0.167999998) + u_xlat1_d.x);
          u_xlat4.x = ((u_xlat9 * 0.330000013) + u_xlat4.x);
          u_xlat4.x = (u_xlat0_d.y * u_xlat4.x);
          u_xlat1_d.x = ((u_xlat1_d.x * u_xlat0_d.x) + u_xlat4.x);
          u_xlat4.xz = (((-float2(u_xlat9, u_xlat9)) * float2(0.497000009, 0.328000009)) + u_xlat4.yz);
          u_xlat9 = ((u_xlat9 * 0.291999996) + u_xlat4.y);
          out_f.color.x = ((u_xlat4.x * u_xlat0_d.z) + u_xlat1_d.x);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat4.z);
          u_xlat0_d.x = ((u_xlat2.x * u_xlat0_d.y) + u_xlat0_d.x);
          out_f.color.y = ((u_xlat9 * u_xlat0_d.z) + u_xlat0_d.x);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
