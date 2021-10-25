Shader "2DxFX/Standard/WaterAndBackground"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Distortion ("Distortion", Range(0, 1)) = 0
    _Alpha ("Alpha", Range(0, 1)) = 1
    _Speed ("Speed", Range(0, 1)) = 1
    EValue ("EValue", Range(0, 1)) = 1
    Light ("Light", Range(0, 1)) = 1
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
      }
      ZClip Off
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
      // m_ProgramMask = 0
      
    } // end phase
    Pass // ind: 2, name: 
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
      //uniform float4 _Time;
      uniform float _Distortion;
      uniform float _Alpha;
      uniform float _Speed;
      uniform float EValue;
      uniform float Light;
      uniform sampler2D _MainTex;
      uniform sampler2D _GrabTexture;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 color :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
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
          u_xlat0 = UnityObjectToClipPos(in_v.vertex);
          u_xlat1.xyz = (u_xlat0.xyw * float3(0.5, 0.5, 0.5));
          u_xlat1.xy = (u_xlat1.zz + u_xlat1.xy);
          out_v.texcoord1.xy = (u_xlat1.xy / u_xlat0.ww);
          out_v.vertex = u_xlat0;
          out_v.texcoord.xy = in_v.texcoord.xy;
          out_v.color = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float4 u_xlat10_0;
      float4 u_xlat1_d;
      float4 u_xlat2;
      float4 u_xlat10_2;
      float2 u_xlat3;
      float u_xlat4;
      float u_xlat5;
      float u_xlat6;
      float2 u_xlat8;
      float u_xlat9;
      int u_xlati9;
      float u_xlat10;
      float2 u_xlat14;
      int u_xlati14;
      float u_xlat15;
      int u_xlati15;
      int u_xlatb15;
      float u_xlat18;
      float u_xlat20;
      int u_xlatb20;
      float u_xlat21;
      int u_xlatb21;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat1_d.xyz = in_f.color.xyz;
          u_xlat1_d.w = u_xlat10_0.w;
          u_xlat1_d = (u_xlat1_d * in_f.color);
          u_xlat18 = (_Time.x * 10);
          u_xlat2.x = (u_xlat18 * _Distortion);
          u_xlat2.x = (u_xlat2.x * 0.25);
          u_xlat8.x = float(0);
          int u_xlati_loop_1 = int(0);
          while((u_xlati_loop_1<8))
          {
              u_xlat20 = float(u_xlati_loop_1);
              u_xlat20 = (u_xlat20 * 0.897597909);
              u_xlat3.x = sin(u_xlat20);
              u_xlat4 = cos(u_xlat20);
              u_xlat20 = (u_xlat18 * u_xlat4);
              u_xlat20 = ((u_xlat20 * _Speed) + u_xlat2.x);
              u_xlat20 = (u_xlat20 + in_f.texcoord1.x);
              u_xlat9 = (u_xlat18 * u_xlat3.x);
              u_xlat9 = ((u_xlat9 * _Speed) + (-u_xlat2.x));
              u_xlat9 = ((-u_xlat9) + in_f.texcoord1.y);
              u_xlat3.x = (u_xlat3.x * u_xlat9);
              u_xlat20 = ((u_xlat20 * u_xlat4) + (-u_xlat3.x));
              u_xlat20 = (u_xlat20 * 6);
              u_xlat20 = cos(u_xlat20);
              u_xlat8.x = (u_xlat20 + u_xlat8.x);
              u_xlati_loop_1 = (u_xlati_loop_1 + 1);
          }
          u_xlat8.x = cos(u_xlat8.x);
          u_xlat14.xy = (in_f.texcoord1.xy + float2(8.52999973, 8.52999973));
          u_xlat3.x = float(0);
          int u_xlati_loop_2 = int(0);
          while((u_xlati_loop_2<8))
          {
              u_xlat15 = float(u_xlati_loop_2);
              u_xlat15 = (u_xlat15 * 0.897597909);
              u_xlat4 = sin(u_xlat15);
              u_xlat5 = cos(u_xlat15);
              u_xlat15 = (u_xlat18 * u_xlat5);
              u_xlat15 = ((u_xlat15 * _Speed) + u_xlat2.x);
              u_xlat15 = (u_xlat14.x + u_xlat15);
              u_xlat21 = (u_xlat18 * u_xlat4);
              u_xlat21 = ((u_xlat21 * _Speed) + (-u_xlat2.x));
              u_xlat21 = ((-u_xlat21) + in_f.texcoord1.y);
              u_xlat21 = (u_xlat4 * u_xlat21);
              u_xlat15 = ((u_xlat15 * u_xlat5) + (-u_xlat21));
              u_xlat15 = (u_xlat15 * 6);
              u_xlat15 = cos(u_xlat15);
              u_xlat3.x = (u_xlat15 + u_xlat3.x);
              u_xlati_loop_2 = (u_xlati_loop_2 + 1);
          }
          u_xlat14.x = cos(u_xlat3.x);
          u_xlat14.x = ((-u_xlat14.x) + u_xlat8.x);
          u_xlat3.x = (u_xlat14.x * 0.00390625);
          u_xlat9 = float(0);
          int u_xlati_loop_3 = int(0);
          while((u_xlati_loop_3<8))
          {
              u_xlat21 = float(u_xlati_loop_3);
              u_xlat21 = (u_xlat21 * 0.897597909);
              u_xlat4 = sin(u_xlat21);
              u_xlat5 = cos(u_xlat21);
              u_xlat21 = (u_xlat18 * u_xlat5);
              u_xlat21 = ((u_xlat21 * _Speed) + u_xlat2.x);
              u_xlat21 = (u_xlat21 + in_f.texcoord1.x);
              u_xlat10 = (u_xlat18 * u_xlat4);
              u_xlat10 = ((u_xlat10 * _Speed) + (-u_xlat2.x));
              u_xlat10 = (u_xlat14.y + (-u_xlat10));
              u_xlat4 = (u_xlat4 * u_xlat10);
              u_xlat21 = ((u_xlat21 * u_xlat5) + (-u_xlat4));
              u_xlat21 = (u_xlat21 * 6);
              u_xlat21 = cos(u_xlat21);
              u_xlat9 = (u_xlat21 + u_xlat9);
              u_xlati_loop_3 = (u_xlati_loop_3 + 1);
          }
          u_xlat18 = cos(u_xlat9);
          u_xlat18 = ((-u_xlat18) + u_xlat8.x);
          u_xlat2.x = (u_xlat3.x * u_xlat18);
          u_xlat3.x = ((u_xlat14.x * 0.0078125) + in_f.texcoord1.x);
          u_xlat3.y = ((u_xlat18 * 0.0078125) + in_f.texcoord1.y);
          u_xlat18 = (u_xlat2.x * Light);
          u_xlat18 = ((u_xlat18 * 19.53125) + 1);
          u_xlat2.x = (u_xlat1_d.w * EValue);
          u_xlat8.xy = (u_xlat3.xy + (-in_f.texcoord1.xy));
          u_xlat2.xy = ((u_xlat2.xx * u_xlat8.xy) + in_f.texcoord1.xy);
          u_xlat8.x = ((u_xlat10_0.x * 0.25) + u_xlat2.y);
          u_xlat2.z = (u_xlat8.x + (-0.0399999991));
          u_xlat10_2 = tex2D(_GrabTexture, u_xlat2.xz);
          u_xlat2 = (float4(u_xlat18, u_xlat18, u_xlat18, u_xlat18) * u_xlat10_2);
          u_xlat0_d.xyz = (u_xlat10_0.xyz * u_xlat1_d.xyz);
          out_f.color.xyz = ((u_xlat0_d.xyz * float3(2, 2, 2)) + u_xlat2.xyz);
          u_xlat0_d.x = (u_xlat1_d.w * u_xlat2.w);
          u_xlat6 = ((-_Alpha) + 1);
          out_f.color.w = (u_xlat6 * u_xlat0_d.x);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
