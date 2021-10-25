Shader "2DxFX/Standard/ColorChange"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _ColorX ("Tint", Color) = (1,1,1,1)
    _HueShift ("Hue", Range(0, 1)) = 1
    _Alpha ("Alpha", Range(0, 1)) = 1
    _Tolerance ("Tolerance", Range(0, 1)) = 1
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
      uniform float _Tolerance;
      uniform float4 _ColorX;
      uniform float _Alpha;
      uniform float _Sat;
      uniform float _Val;
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
      float3 u_xlat1_d;
      float3 u_xlatb1;
      float4 u_xlat2;
      float4 u_xlat3;
      float u_xlat5;
      float2 u_xlat9;
      float u_xlat12;
      float u_xlat13;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat1_d.xyz = ((u_xlat10_0.xyz * in_f.color.xyz) + (-_ColorX.xyz));
          u_xlatb1.xyz = bool4(abs(u_xlat1_d.xyzx) < float4(float4(_Tolerance, _Tolerance, _Tolerance, _Tolerance))).xyz;
          u_xlatb1.x = (u_xlatb1.y || u_xlatb1.x);
          u_xlatb1.x = (u_xlatb1.z || u_xlatb1.x);
          u_xlat5 = (_HueShift * 0.0174532942);
          u_xlat2.x = sin(u_xlat5);
          u_xlat3.x = cos(u_xlat5);
          u_xlat5 = (_Sat * _Val);
          u_xlat9.x = (u_xlat3.x * u_xlat5);
          u_xlat5 = (u_xlat2.x * u_xlat5);
          u_xlat2 = (u_xlat9.xxxx * float4(0.412999988, 0.300000012, 0.588, 0.885999978));
          u_xlat3 = (u_xlat9.xxxx * float4(0.700999975, 0.587000012, 0.114, 0.298999995));
          u_xlat9.xy = ((float2(float2(_Val, _Val)) * float2(0.298999995, 0.587000012)) + (-u_xlat2.yz));
          u_xlat2.xy = ((float2(float2(_Val, _Val)) * float2(0.587000012, 0.114)) + u_xlat2.xw);
          u_xlat9.x = ((u_xlat5 * 1.25) + u_xlat9.x);
          u_xlat13 = (((-u_xlat5) * 1.04999995) + u_xlat9.y);
          u_xlat0_d.xyz = (u_xlat10_0.xyz * in_f.color.xyz);
          out_f.color.w = ((u_xlat10_0.w * in_f.color.w) + (-_Alpha));
          u_xlat12 = (u_xlat0_d.y * u_xlat13);
          u_xlat12 = ((u_xlat9.x * u_xlat0_d.x) + u_xlat12);
          u_xlat9.x = (((-u_xlat5) * 0.202999994) + u_xlat2.y);
          u_xlat13 = ((u_xlat5 * 0.0350000001) + u_xlat2.x);
          u_xlat2.z = ((u_xlat9.x * u_xlat0_d.z) + u_xlat12);
          u_xlat12 = ((_Val * 0.298999995) + u_xlat3.x);
          u_xlat3.xyz = ((float3(float3(_Val, _Val, _Val)) * float3(0.587000012, 0.114, 0.298999995)) + (-u_xlat3.yzw));
          u_xlat12 = ((u_xlat5 * 0.167999998) + u_xlat12);
          u_xlat9.x = ((u_xlat5 * 0.330000013) + u_xlat3.x);
          u_xlat9.x = (u_xlat0_d.y * u_xlat9.x);
          u_xlat12 = ((u_xlat12 * u_xlat0_d.x) + u_xlat9.x);
          u_xlat3.xz = (((-float2(u_xlat5, u_xlat5)) * float2(0.497000009, 0.328000009)) + u_xlat3.yz);
          u_xlat5 = ((u_xlat5 * 0.291999996) + u_xlat3.y);
          u_xlat2.x = ((u_xlat3.x * u_xlat0_d.z) + u_xlat12);
          u_xlat12 = (u_xlat0_d.x * u_xlat3.z);
          u_xlat12 = ((u_xlat13 * u_xlat0_d.y) + u_xlat12);
          u_xlat2.y = ((u_xlat5 * u_xlat0_d.z) + u_xlat12);
          out_f.color.xyz = (u_xlatb1.x)?(u_xlat2.xyz):(u_xlat0_d.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
