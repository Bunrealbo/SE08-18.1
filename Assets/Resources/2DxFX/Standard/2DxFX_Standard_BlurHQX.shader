Shader "2DxFX/Standard/BlurHQX"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Distortion ("Distortion", Range(0, 1)) = 0
    _Alpha ("Alpha", Range(0, 1)) = 1
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
      uniform float _Distortion;
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
      float4 u_xlat0_d;
      float4 u_xlat1_d;
      float u_xlat2;
      int u_xlatb2;
      float3 u_xlat3;
      float4 u_xlat4;
      float4 u_xlat5;
      float4 u_xlat10_5;
      float u_xlat6;
      int2 u_xlati8;
      int2 u_xlati12;
      float u_xlat14;
      float u_xlat18;
      float u_xlat20;
      int u_xlatb20;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = ((_Distortion * 0.5) + 0.100000001);
          u_xlat0_d.x = dot(u_xlat0_d.xx, u_xlat0_d.xx);
          u_xlat1_d.x = float(0);
          u_xlat1_d.y = float(0);
          u_xlat1_d.z = float(0);
          u_xlat1_d.w = float(0);
          u_xlat6 = float(0);
          u_xlati12.x = int(0);
          int u_xlati_while_true_0 = 0;
          while((u_xlati_while_true_0<32767))
          {
              u_xlatb2 = (u_xlati12.x>=16);
              if(u_xlatb2)
              {
                  break;
              }
              u_xlat2 = float(u_xlati12.x);
              u_xlat2 = (u_xlat2 + (-8));
              u_xlat2 = (u_xlat2 * u_xlat2);
              u_xlat2 = ((-u_xlat2) / u_xlat0_d.x);
              u_xlat2 = (u_xlat2 * 1.44269502);
              u_xlat2 = exp2(u_xlat2);
              u_xlati12.xy = (u_xlati12.xx + int2(1, (-8)));
              u_xlat18 = float(u_xlati12.y);
              u_xlat3.y = (u_xlat18 * 0.00390625);
              u_xlat4 = u_xlat1_d;
              u_xlat18 = u_xlat6;
              u_xlati8.x = 0;
              int u_xlati_while_true_1 = 0;
              while((u_xlati_while_true_1<32767))
              {
                  u_xlatb20 = (u_xlati8.x>=16);
                  if(u_xlatb20)
                  {
                      break;
                  }
                  u_xlat20 = float(u_xlati8.x);
                  u_xlat20 = (u_xlat20 + (-8));
                  u_xlat20 = (u_xlat20 * u_xlat20);
                  u_xlat20 = ((-u_xlat20) / u_xlat0_d.x);
                  u_xlat20 = (u_xlat20 * 1.44269502);
                  u_xlat20 = exp2(u_xlat20);
                  u_xlati8.xy = (u_xlati8.xx + int2(1, (-8)));
                  u_xlat14 = float(u_xlati8.y);
                  u_xlat3.x = (u_xlat14 * 0.00390625);
                  u_xlat18 = ((u_xlat20 * u_xlat2) + u_xlat18);
                  u_xlat3.xz = (u_xlat3.xy + in_f.texcoord.xy);
                  u_xlat10_5 = tex2D(_MainTex, u_xlat3.xz);
                  u_xlat5 = (float4(u_xlat20, u_xlat20, u_xlat20, u_xlat20) * u_xlat10_5);
                  u_xlat4 = ((u_xlat5 * float4(u_xlat2, u_xlat2, u_xlat2, u_xlat2)) + u_xlat4);
                  u_xlati_while_true_1 = (u_xlati_while_true_1 + 1);
              }
              u_xlat1_d = u_xlat4;
              u_xlat6 = u_xlat18;
              u_xlati_while_true_0 = (u_xlati_while_true_0 + 1);
          }
          u_xlat0_d = (u_xlat1_d / float4(u_xlat6, u_xlat6, u_xlat6, u_xlat6));
          u_xlat1_d.x = ((-_Alpha) + 1);
          u_xlat0_d.w = (u_xlat0_d.w * u_xlat1_d.x);
          out_f.color = (u_xlat0_d * in_f.color);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
