Shader "2DxFX/Standard/GoldenFX"
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
      float u_xlat16_0;
      float3 u_xlat10_0;
      float4 u_xlat1_d;
      float3 u_xlat10_1;
      float3 u_xlat10_2;
      float u_xlat16_3;
      float u_xlat16_6;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d = ((float4(_Distortion, _Distortion, _Distortion, _Distortion) * float4(-0.00600000005, (-0.00600000005), 0.00600000005, (-0.00600000005))) + in_f.texcoord.xyxy);
          u_xlat10_1.xyz = tex2D(_MainTex, u_xlat0_d.xy).xyz;
          u_xlat10_0.xyz = tex2D(_MainTex, u_xlat0_d.zw).xyz;
          u_xlat16_0 = dot(u_xlat10_0.xyz, float3(0.333332986, 0.333332986, 0.333332986));
          u_xlat16_3 = dot(u_xlat10_1.xyz, float3(0.333332986, 0.333332986, 0.333332986));
          u_xlat1_d = ((float4(_Distortion, _Distortion, _Distortion, _Distortion) * float4(-0.00600000005, 0.00600000005, 0.00600000005, 0.00600000005)) + in_f.texcoord.xyxy);
          u_xlat10_2.xyz = tex2D(_MainTex, u_xlat1_d.zw).xyz;
          u_xlat10_1.xyz = tex2D(_MainTex, u_xlat1_d.xy).xyz;
          u_xlat16_6 = dot(u_xlat10_1.xyz, float3(0.333332986, 0.333332986, 0.333332986));
          u_xlat16_0 = ((-u_xlat16_6) + u_xlat16_0);
          u_xlat16_6 = dot(u_xlat10_2.xyz, float3(0.333332986, 0.333332986, 0.333332986));
          u_xlat16_3 = ((-u_xlat16_6) + u_xlat16_3);
          u_xlat16_0 = max(abs(u_xlat16_0), abs(u_xlat16_3));
          u_xlat16_0 = rsqrt(u_xlat16_0);
          u_xlat16_0 = (float(1) / u_xlat16_0);
          u_xlat0_d.xyz = float3(((float3(u_xlat16_0, u_xlat16_0, u_xlat16_0) * float3(4.5, 1.80000007, (-1.50000012))) + float3(0.100000001, 0.180000007, 0.300000012)));
          u_xlat10_1.x = tex2D(_MainTex, in_f.texcoord.xy).w;
          u_xlat0_d.w = (u_xlat10_1.x + (-_Alpha));
          out_f.color = (u_xlat0_d * in_f.color);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
