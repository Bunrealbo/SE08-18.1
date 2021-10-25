Shader "2DxFX/Standard/Liquid"
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
      int u_xlatb1;
      float2 u_xlat2;
      float4 u_xlat10_2;
      float2 u_xlat3;
      float2 u_xlat4;
      float2 u_xlat5;
      float u_xlat6;
      float u_xlat7;
      int u_xlatb7;
      float3 u_xlat8;
      float u_xlat9;
      float u_xlat11;
      float u_xlat13;
      float u_xlat14;
      float u_xlat15;
      float u_xlat16;
      float u_xlat17;
      float2 u_xlat18;
      float u_xlat19;
      float u_xlat21;
      float u_xlat22;
      float u_xlat23;
      float u_xlat24;
      float u_xlat26;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Time.x * 10);
          u_xlat0_d.xy = (u_xlat0_d.xx * float2(_Distortion, _Speed));
          u_xlat14 = (_Time.x * _Speed);
          u_xlat1_d.xy = ((float2(u_xlat14, u_xlat14) * float2(6.23489809, (-2.22520947))) + u_xlat0_d.xx);
          u_xlat21 = (u_xlat1_d.x + in_f.texcoord.x);
          u_xlat1_d.x = (u_xlat1_d.y + u_xlat21);
          u_xlat15 = (u_xlat1_d.x * (-0.222520947));
          u_xlat2.xy = ((float2(u_xlat14, u_xlat14) * float2(7.81831503, 9.74927902)) + (-u_xlat0_d.xx));
          u_xlat1_d.w = ((-u_xlat2.x) + in_f.texcoord.y);
          u_xlat16 = (u_xlat1_d.w + 8.52999973);
          u_xlat23 = ((-u_xlat2.y) + u_xlat16);
          u_xlat15 = (((-u_xlat23) * 0.974927902) + u_xlat15);
          u_xlat15 = (u_xlat15 * 6);
          u_xlat15 = cos(u_xlat15);
          u_xlat3.x = (u_xlat21 * 0.623489797);
          u_xlat16 = (((-u_xlat16) * 0.781831503) + u_xlat3.x);
          u_xlat16 = (u_xlat16 * 6);
          u_xlat16 = cos(u_xlat16);
          u_xlat15 = (u_xlat15 + u_xlat16);
          u_xlat3.xy = ((float2(u_xlat14, u_xlat14) * float2(-9.00968933, (-9.00968838))) + u_xlat0_d.xx);
          u_xlat16 = (u_xlat1_d.x + u_xlat3.x);
          u_xlat17 = (u_xlat16 * (-0.900968909));
          u_xlat4.xy = ((float2(u_xlat14, u_xlat14) * float2(4.33883619, (-4.33883762))) + (-u_xlat0_d.xx));
          u_xlat23 = (u_xlat23 + (-u_xlat4.x));
          u_xlat17 = (((-u_xlat23) * 0.433883607) + u_xlat17);
          u_xlat23 = ((-u_xlat4.y) + u_xlat23);
          u_xlat17 = (u_xlat17 * 6);
          u_xlat17 = cos(u_xlat17);
          u_xlat15 = (u_xlat15 + u_xlat17);
          u_xlat17 = (u_xlat3.y + u_xlat16);
          u_xlat24 = (u_xlat17 * (-0.90096885));
          u_xlat24 = (((-u_xlat23) * (-0.433883756)) + u_xlat24);
          u_xlat24 = (u_xlat24 * 6);
          u_xlat24 = cos(u_xlat24);
          u_xlat15 = (u_xlat15 + u_xlat24);
          u_xlat18.xy = ((float2(u_xlat14, u_xlat14) * float2(-9.74927902, (-7.81831312))) + (-u_xlat0_d.xx));
          u_xlat23 = (u_xlat23 + (-u_xlat18.x));
          u_xlat5.xy = ((float2(u_xlat14, u_xlat14) * float2(-2.22521019, 6.23490047)) + u_xlat0_d.xx);
          u_xlat24 = (u_xlat17 + u_xlat5.x);
          u_xlat19 = (u_xlat24 * (-0.222521007));
          u_xlat19 = (((-u_xlat23) * (-0.974927902)) + u_xlat19);
          u_xlat23 = ((-u_xlat18.y) + u_xlat23);
          u_xlat19 = (u_xlat19 * 6);
          u_xlat19 = cos(u_xlat19);
          u_xlat15 = (u_xlat15 + u_xlat19);
          u_xlat19 = (u_xlat5.y + u_xlat24);
          u_xlat26 = (u_xlat19 * 0.623490036);
          u_xlat26 = (((-u_xlat23) * (-0.781831324)) + u_xlat26);
          u_xlat26 = (u_xlat26 * 6);
          u_xlat26 = cos(u_xlat26);
          u_xlat15 = (u_xlat15 + u_xlat26);
          u_xlat26 = ((u_xlat14 * 1.74845559E-06) + (-u_xlat0_d.x));
          u_xlat14 = ((u_xlat14 * 6.23489761) + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x + u_xlat0_d.y);
          u_xlat7 = (u_xlat23 + (-u_xlat26));
          u_xlat23 = (u_xlat0_d.x + u_xlat19);
          u_xlat6 = (((-u_xlat7) * 1.74845553E-07) + u_xlat23);
          u_xlat7 = ((-u_xlat2.x) + u_xlat7);
          u_xlat6 = (u_xlat6 * 6);
          u_xlat6 = cos(u_xlat6);
          u_xlat15 = (u_xlat15 + u_xlat6);
          u_xlat6 = (u_xlat14 + u_xlat23);
          u_xlat13 = (u_xlat6 * 0.623489738);
          u_xlat7 = (((-u_xlat7) * 0.781831503) + u_xlat13);
          u_xlat7 = (u_xlat7 * 6);
          u_xlat7 = cos(u_xlat7);
          u_xlat7 = (u_xlat7 + u_xlat15);
          u_xlat7 = cos(u_xlat7);
          u_xlat15 = ((-u_xlat2.y) + u_xlat1_d.w);
          u_xlat9 = (u_xlat15 * 0.974927902);
          u_xlat1_d.z = ((-u_xlat4.x) + u_xlat15);
          u_xlat4.x = ((-u_xlat4.y) + u_xlat1_d.z);
          u_xlat1_d.x = ((u_xlat1_d.x * (-0.222520947)) + (-u_xlat9));
          u_xlat1_d.xzw = (u_xlat1_d.xzw * float3(6, 0.433883607, 0.781831503));
          u_xlat1_d.x = cos(u_xlat1_d.x);
          u_xlat11 = ((u_xlat21 * 0.623489797) + (-u_xlat1_d.w));
          u_xlat21 = (u_xlat21 + 8.52999973);
          u_xlat11 = (u_xlat11 * 6);
          u_xlat11 = cos(u_xlat11);
          u_xlat1_d.x = (u_xlat1_d.x + u_xlat11);
          u_xlat16 = ((u_xlat16 * (-0.900968909)) + (-u_xlat1_d.z));
          u_xlat16 = (u_xlat16 * 6);
          u_xlat16 = cos(u_xlat16);
          u_xlat1_d.x = (u_xlat1_d.x + u_xlat16);
          u_xlat16 = (u_xlat4.x * (-0.433883756));
          u_xlat4.x = ((-u_xlat18.x) + u_xlat4.x);
          u_xlat11 = ((-u_xlat18.y) + u_xlat4.x);
          u_xlat4.x = (u_xlat4.x * (-0.974927902));
          u_xlat17 = ((u_xlat17 * (-0.90096885)) + (-u_xlat16));
          u_xlat17 = (u_xlat17 * 6);
          u_xlat17 = cos(u_xlat17);
          u_xlat1_d.x = (u_xlat1_d.x + u_xlat17);
          u_xlat17 = ((u_xlat24 * (-0.222521007)) + (-u_xlat4.x));
          u_xlat17 = (u_xlat17 * 6);
          u_xlat17 = cos(u_xlat17);
          u_xlat1_d.x = (u_xlat1_d.x + u_xlat17);
          u_xlat17 = (u_xlat11 * (-0.781831324));
          u_xlat24 = ((-u_xlat26) + u_xlat11);
          u_xlat11 = ((u_xlat19 * 0.623490036) + (-u_xlat17));
          u_xlat11 = (u_xlat11 * 6);
          u_xlat11 = cos(u_xlat11);
          u_xlat1_d.x = (u_xlat1_d.x + u_xlat11);
          u_xlat23 = (((-u_xlat24) * 1.74845553E-07) + u_xlat23);
          u_xlat23 = (u_xlat23 * 6);
          u_xlat23 = cos(u_xlat23);
          u_xlat1_d.x = (u_xlat1_d.x + u_xlat23);
          u_xlat2.x = ((-u_xlat2.x) + u_xlat24);
          u_xlat2.x = (u_xlat2.x * 0.781831503);
          u_xlat23 = ((u_xlat6 * 0.623489738) + (-u_xlat2.x));
          u_xlat23 = (u_xlat23 * 6);
          u_xlat23 = cos(u_xlat23);
          u_xlat1_d.x = (u_xlat1_d.x + u_xlat23);
          u_xlat1_d.x = cos(u_xlat1_d.x);
          u_xlat7 = ((-u_xlat7) + u_xlat1_d.x);
          u_xlat23 = (u_xlat7 * 0.00833333377);
          u_xlat8.x = (u_xlat1_d.y + u_xlat21);
          u_xlat21 = ((u_xlat21 * 0.623489797) + (-u_xlat1_d.w));
          u_xlat21 = (u_xlat21 * 6);
          u_xlat21 = cos(u_xlat21);
          u_xlat8.z = ((u_xlat8.x * (-0.222520947)) + (-u_xlat9));
          u_xlat8.x = (u_xlat3.x + u_xlat8.x);
          u_xlat9 = (u_xlat3.y + u_xlat8.x);
          u_xlat8.x = ((u_xlat8.x * (-0.900968909)) + (-u_xlat1_d.z));
          u_xlat8.xy = (u_xlat8.xz * float2(6, 6));
          u_xlat8.xy = cos(u_xlat8.xy);
          u_xlat21 = (u_xlat21 + u_xlat8.y);
          u_xlat21 = (u_xlat8.x + u_xlat21);
          u_xlat8.x = ((u_xlat9 * (-0.90096885)) + (-u_xlat16));
          u_xlat15 = (u_xlat5.x + u_xlat9);
          u_xlat22 = (u_xlat5.y + u_xlat15);
          u_xlat8.y = ((u_xlat15 * (-0.222521007)) + (-u_xlat4.x));
          u_xlat8.xy = (u_xlat8.xy * float2(6, 6));
          u_xlat8.xy = cos(u_xlat8.xy);
          u_xlat21 = (u_xlat21 + u_xlat8.x);
          u_xlat21 = (u_xlat8.y + u_xlat21);
          u_xlat8.x = ((u_xlat22 * 0.623490036) + (-u_xlat17));
          u_xlat0_d.x = (u_xlat0_d.x + u_xlat22);
          u_xlat8.x = (u_xlat8.x * 6);
          u_xlat8.x = cos(u_xlat8.x);
          u_xlat21 = (u_xlat21 + u_xlat8.x);
          u_xlat8.x = (((-u_xlat24) * 1.74845553E-07) + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat14 + u_xlat0_d.x);
          u_xlat0_d.x = ((u_xlat0_d.x * 0.623489738) + (-u_xlat2.x));
          u_xlat0_d.x = (u_xlat0_d.x * 6);
          u_xlat0_d.z = (u_xlat8.x * 6);
          u_xlat0_d.xz = cos(u_xlat0_d.xz);
          u_xlat14 = (u_xlat0_d.z + u_xlat21);
          u_xlat0_d.x = (u_xlat0_d.x + u_xlat14);
          u_xlat0_d.x = cos(u_xlat0_d.x);
          u_xlat0_d.x = ((-u_xlat0_d.x) + u_xlat1_d.x);
          u_xlat14 = (u_xlat0_d.x * 0.00833333377);
          u_xlat14 = (u_xlat23 * u_xlat14);
          u_xlat14 = (u_xlat14 * Light);
          u_xlat14 = ((u_xlat14 * 700) + 1);
          u_xlat21 = log2(u_xlat14);
          u_xlat1_d.x = ((u_xlat7 * 0.00833333377) + (-0.0120000001));
          u_xlat2.y = ((u_xlat7 * 0.0166666675) + in_f.texcoord.y);
          u_xlat7 = ((u_xlat0_d.x * 0.00833333377) + (-0.0120000001));
          u_xlat2.x = ((u_xlat0_d.x * 0.0166666675) + in_f.texcoord.x);
          u_xlat8.xy = (u_xlat2.xy + (-in_f.texcoord.xy));
          u_xlat8.xy = ((float2(float2(EValue, EValue)) * u_xlat8.xy) + in_f.texcoord.xy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat8.xy);
          u_xlat0_d.x = (u_xlat1_d.x * u_xlat7);
          u_xlatb1 = (0<u_xlat1_d.x);
          u_xlatb7 = (0<u_xlat7);
          u_xlatb7 = (u_xlatb1 && u_xlatb7);
          u_xlat0_d.x = (u_xlat0_d.x * 200000);
          u_xlat0_d.x = (u_xlat21 * u_xlat0_d.x);
          u_xlat0_d.x = exp2(u_xlat0_d.x);
          u_xlat0_d.x = (u_xlatb7)?(u_xlat0_d.x):(u_xlat14);
          u_xlat0_d = (u_xlat0_d.xxxx * u_xlat10_2);
          u_xlat0_d = (u_xlat0_d * in_f.color);
          u_xlat1_d.x = ((-_Alpha) + 1);
          out_f.color.w = (u_xlat0_d.w * u_xlat1_d.x);
          out_f.color.xyz = u_xlat0_d.xyz;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
