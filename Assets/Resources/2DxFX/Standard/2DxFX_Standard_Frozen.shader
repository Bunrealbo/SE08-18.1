Shader "2DxFX/Standard/Frozen"
{
  Properties
  {
    [HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
    [HideInInspector] _MainTex2 ("Pattern (RGB)", 2D) = "white" {}
    [HideInInspector] _Alpha ("Alpha", Range(0, 1)) = 1
    [HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
    [HideInInspector] _Value1 ("_Value1", Range(0, 1)) = 0
    [HideInInspector] _Value2 ("_Value2", Range(0, 1)) = 0
    [HideInInspector] _Value3 ("_Value3", Range(0, 1)) = 0
    [HideInInspector] _Value4 ("_Value4", Range(0, 1)) = 0
    [HideInInspector] _Value5 ("_Value5", Range(0, 1)) = 0
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
      uniform float _Alpha;
      uniform float _Value1;
      uniform float _Value2;
      uniform sampler2D _MainTex2;
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
      float u_xlat10_0;
      float4 u_xlat10_1;
      float4 u_xlat2;
      float2 u_xlat3;
      float2 u_xlat16_3;
      float3 u_xlat4;
      float2 u_xlat16_4;
      float u_xlat5;
      float u_xlat16_5;
      float2 u_xlat10;
      float u_xlat10_10;
      float u_xlat13;
      float u_xlat15;
      int u_xlatb15;
      float u_xlat16;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Value2 * 0.200000003);
          u_xlat5 = (((-_Value2) * 0.400000006) + 1);
          u_xlat10.xy = ((in_f.texcoord.xy * float2(u_xlat5, u_xlat5)) + u_xlat0_d.xx);
          u_xlat0_d.xy = ((u_xlat10.xy * float2(u_xlat5, u_xlat5)) + u_xlat0_d.xx);
          u_xlat10_10 = tex2D(_MainTex2, u_xlat10.xy).x;
          u_xlat10_0 = tex2D(_MainTex2, u_xlat0_d.xy).z;
          u_xlat16_5 = (u_xlat10_0 + u_xlat10_0);
          u_xlat0_d.x = (u_xlat10_0 * _Value2);
          u_xlat0_d.xw = ((u_xlat0_d.xx * float2(0.0299999993, 0.0299999993)) + in_f.texcoord.xy);
          u_xlat10_1 = tex2D(_MainTex, u_xlat0_d.xw);
          u_xlat2 = (u_xlat10_1 * in_f.color);
          u_xlat0_d.x = (u_xlat2.y + u_xlat2.x);
          u_xlat0_d.x = ((u_xlat10_1.z * in_f.color.z) + u_xlat0_d.x);
          u_xlatb15 = (u_xlat0_d.x<1.5);
          u_xlat16 = (((-u_xlat0_d.x) * 0.333333343) + 1);
          u_xlat16 = (u_xlat16 + u_xlat16);
          u_xlat16_3.xy = float2((((-float2(u_xlat10_10, u_xlat10_10)) * float2(0.100000001, 0.666666687)) + float2(1, 1)));
          u_xlat3.xy = (((-float2(u_xlat16, u_xlat16)) * u_xlat16_3.xy) + float2(1, 1));
          u_xlat13 = (u_xlat0_d.x * 0.666666687);
          u_xlat0_d.x = ((u_xlat0_d.x * 0.333333343) + (-_Value1));
          u_xlat0_d.x = (u_xlat0_d.x * 10);
          u_xlat0_d.x = clamp(u_xlat0_d.x, 0, 1);
          u_xlat16_4.xy = float2((float2(u_xlat10_10, u_xlat10_10) * float2(0.100000001, 0.666666687)));
          u_xlat4.xy = (float2(u_xlat13, u_xlat13) * u_xlat16_4.xy);
          u_xlat13 = (u_xlat10_10 * u_xlat13);
          u_xlat4.xy = (int(u_xlatb15))?(u_xlat4.xy):(u_xlat3.xy);
          u_xlat16_3.x = ((-u_xlat10_10) + 1);
          u_xlat16 = (((-u_xlat16) * u_xlat16_3.x) + 1);
          u_xlat4.z = (u_xlatb15)?(u_xlat13):(u_xlat16);
          u_xlat15 = ((u_xlat0_d.x * (-2)) + 3);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat0_d.x);
          u_xlat0_d.x = ((u_xlat15 * u_xlat0_d.x) + 0.200000003);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat10_10);
          u_xlat0_d.xyw = ((u_xlat0_d.xxx * float3(u_xlat16_5, u_xlat16_5, u_xlat16_5)) + u_xlat4.xyz);
          u_xlat3.x = (((-_Time.x) * 2) + (-_Value2));
          u_xlat3.y = 0;
          u_xlat3.xy = (u_xlat3.xy + in_f.texcoord.xy);
          u_xlat3.xy = (float2(u_xlat10_10, u_xlat10_10) + u_xlat3.xy);
          u_xlat10_10 = tex2D(_MainTex2, u_xlat3.xy).y;
          u_xlat0_d.xyz = ((float3(u_xlat10_10, u_xlat10_10, u_xlat10_10) * float3(0.5, 0.5, 0.5)) + u_xlat0_d.xyw);
          u_xlat0_d.xyz = (((-u_xlat10_1.xyz) * in_f.color.xyz) + u_xlat0_d.xyz);
          out_f.color.xyz = ((float3(float3(_Value2, _Value2, _Value2)) * u_xlat0_d.xyz) + u_xlat2.xyz);
          u_xlat0_d.x = ((-_Alpha) + 1);
          out_f.color.w = (u_xlat0_d.x * u_xlat2.w);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
