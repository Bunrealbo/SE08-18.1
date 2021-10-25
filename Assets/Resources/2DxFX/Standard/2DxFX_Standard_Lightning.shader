Shader "2DxFX/Standard/Lightning"
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
      float2 u_xlat0_d;
      float3 u_xlat16_0;
      float3 u_xlat10_0;
      float4 u_xlat1_d;
      float3 u_xlat16_1;
      float4 u_xlat10_1;
      float2 u_xlat2;
      float3 u_xlat10_2;
      float u_xlat3;
      int u_xlatb3;
      float2 u_xlat6;
      int u_xlatb6;
      float u_xlat10;
      int u_xlatb10;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Time.x * _Value1);
          u_xlat3 = (u_xlat0_d.x * 0.125);
          u_xlatb6 = (u_xlat3>=(-u_xlat3));
          u_xlat3 = frac(abs(u_xlat3));
          u_xlat3 = (u_xlatb6)?(u_xlat3):((-u_xlat3));
          u_xlat3 = (u_xlat3 * 8);
          u_xlat3 = floor(u_xlat3);
          u_xlat6.xy = (in_f.texcoord.xy * float2(0.125, 0.125));
          u_xlat3 = (((-u_xlat3) * 0.125) + u_xlat6.y);
          u_xlat1_d.y = (u_xlat3 + 1);
          u_xlatb3 = (u_xlat0_d.x>=(-u_xlat0_d.x));
          u_xlat0_d.x = frac(abs(u_xlat0_d.x));
          u_xlat0_d.x = (u_xlatb3)?(u_xlat0_d.x):((-u_xlat0_d.x));
          u_xlat0_d.x = (u_xlat0_d.x * 8);
          u_xlat0_d.x = floor(u_xlat0_d.x);
          u_xlat1_d.x = ((u_xlat0_d.x * 0.125) + u_xlat6.x);
          u_xlat10_1.xyz = tex2D(_MainTex2, u_xlat1_d.xy).xyz;
          u_xlat0_d.x = (_Time.x + 0.200000003);
          u_xlat3 = (u_xlat0_d.x * _Value1);
          u_xlat0_d.x = (u_xlat0_d.x + _Time.x);
          u_xlat0_d.x = (u_xlat0_d.x + 0.600000024);
          u_xlat0_d.x = (u_xlat0_d.x * _Value1);
          u_xlatb10 = (u_xlat3>=(-u_xlat3));
          u_xlat2.x = frac(abs(u_xlat3));
          u_xlat3 = (u_xlat3 * 0.125);
          u_xlat10 = (u_xlatb10)?(u_xlat2.x):((-u_xlat2.x));
          u_xlat10 = (u_xlat10 * 8);
          u_xlat10 = floor(u_xlat10);
          u_xlat2.x = ((u_xlat10 * 0.125) + u_xlat6.x);
          u_xlatb10 = (u_xlat3>=(-u_xlat3));
          u_xlat3 = frac(abs(u_xlat3));
          u_xlat3 = (u_xlatb10)?(u_xlat3):((-u_xlat3));
          u_xlat3 = (u_xlat3 * 8);
          u_xlat3 = floor(u_xlat3);
          u_xlat3 = (((-u_xlat3) * 0.125) + u_xlat6.y);
          u_xlat2.y = (u_xlat3 + 1);
          u_xlat10_2.xyz = tex2D(_MainTex2, u_xlat2.xy).xyz;
          u_xlat16_1.xyz = (u_xlat10_1.xyz + u_xlat10_2.xyz);
          u_xlatb3 = (u_xlat0_d.x>=(-u_xlat0_d.x));
          u_xlat10 = frac(abs(u_xlat0_d.x));
          u_xlat0_d.y = (u_xlatb3)?(u_xlat10):((-u_xlat10));
          u_xlat0_d.xy = (u_xlat0_d.xy * float2(0.125, 8));
          u_xlat3 = floor(u_xlat0_d.y);
          u_xlat2.x = ((u_xlat3 * 0.125) + u_xlat6.x);
          u_xlatb3 = (u_xlat0_d.x>=(-u_xlat0_d.x));
          u_xlat0_d.x = frac(abs(u_xlat0_d.x));
          u_xlat0_d.x = (u_xlatb3)?(u_xlat0_d.x):((-u_xlat0_d.x));
          u_xlat0_d.x = (u_xlat0_d.x * 8);
          u_xlat0_d.x = floor(u_xlat0_d.x);
          u_xlat0_d.x = (((-u_xlat0_d.x) * 0.125) + u_xlat6.y);
          u_xlat2.y = (u_xlat0_d.x + 1);
          u_xlat10_0.xyz = tex2D(_MainTex2, u_xlat2.xy).xyz;
          u_xlat16_0.xyz = (u_xlat10_0.xyz + u_xlat16_1.xyz);
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat1_d = (u_xlat10_1 * in_f.color);
          out_f.color.xyz = ((u_xlat16_0.xyz * float3(float3(_Value2, _Value2, _Value2))) + u_xlat1_d.xyz);
          u_xlat0_d.x = ((-_Alpha) + 1);
          out_f.color.w = (u_xlat0_d.x * u_xlat1_d.w);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
