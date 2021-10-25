Shader "2DxFX/Standard/Sharpen"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Distortion ("Distortion", Range(0, 1)) = 0
    _Color ("_Color", Color) = (1,1,1,1)
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
      float3 u_xlat0_d;
      float4 u_xlat1_d;
      float3 u_xlat16_1;
      float3 u_xlat10_1;
      float3 u_xlat16_2;
      float3 u_xlat10_2;
      float4 u_xlat10_3;
      float4 u_xlat4;
      float3 u_xlat10_4;
      float3 u_xlat10_5;
      float3 u_xlat16_6;
      float u_xlat7;
      float u_xlat16_14;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = ((_Distortion * 9) + 1);
          u_xlat7 = (_Distortion * 9);
          u_xlat1_d = (in_f.texcoord.xyxy + float4(-0.00390625, (-0), 0.00390625, 0));
          u_xlat10_2.xyz = tex2D(_MainTex, u_xlat1_d.zw).xyz;
          u_xlat10_1.xyz = tex2D(_MainTex, u_xlat1_d.xy).xyz;
          u_xlat10_3 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_14 = (u_xlat10_1.x + u_xlat10_3.x);
          u_xlat16_14 = (u_xlat10_2.x + u_xlat16_14);
          u_xlat4 = (in_f.texcoord.xyxy + float4(0, (-0.00390625), 0, 0.00390625));
          u_xlat10_5.xyz = tex2D(_MainTex, u_xlat4.xy).xyz;
          u_xlat10_4.xyz = tex2D(_MainTex, u_xlat4.zw).xyz;
          u_xlat16_14 = (u_xlat16_14 + u_xlat10_5.x);
          u_xlat16_14 = (u_xlat10_4.x + u_xlat16_14);
          u_xlat7 = (u_xlat7 * u_xlat16_14);
          u_xlat7 = (u_xlat7 * 0.200000003);
          u_xlat0_d.xyz = ((u_xlat0_d.xxx * u_xlat10_3.xyz) + (-float3(u_xlat7, u_xlat7, u_xlat7)));
          u_xlat16_6.xyz = min(u_xlat10_1.xyz, u_xlat10_3.xyz);
          u_xlat16_1.xyz = max(u_xlat10_1.xyz, u_xlat10_3.xyz);
          out_f.color.w = ((u_xlat10_3.w * in_f.color.w) + (-_Alpha));
          u_xlat16_1.xyz = max(u_xlat10_2.xyz, u_xlat16_1.xyz);
          u_xlat16_2.xyz = min(u_xlat10_2.xyz, u_xlat16_6.xyz);
          u_xlat16_2.xyz = min(u_xlat10_5.xyz, u_xlat16_2.xyz);
          u_xlat16_1.xyz = max(u_xlat10_5.xyz, u_xlat16_1.xyz);
          u_xlat16_1.xyz = max(u_xlat10_4.xyz, u_xlat16_1.xyz);
          u_xlat16_2.xyz = min(u_xlat10_4.xyz, u_xlat16_2.xyz);
          u_xlat0_d.xyz = max(u_xlat0_d.xyz, u_xlat16_2.xyz);
          u_xlat0_d.xyz = min(u_xlat16_1.xyz, u_xlat0_d.xyz);
          u_xlat0_d.xyz = (u_xlat0_d.xyz * in_f.color.xyz);
          out_f.color.xyz = u_xlat0_d.xyz;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
