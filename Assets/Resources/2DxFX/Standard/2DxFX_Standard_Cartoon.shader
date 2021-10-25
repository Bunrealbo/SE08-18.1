Shader "2DxFX/Standard/Cartoon"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _ColorLevel ("ColorLevel", Range(0, 1)) = 0
    _EdgeSize ("EdgeSize", Range(0, 1)) = 0
    _ColorB ("ColorB", Range(0, 1)) = 0
    _Size ("Size", Range(0, 1)) = 0
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
      uniform float _ColorLevel;
      uniform float _EdgeSize;
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
      float4 u_xlat16_0;
      float4 u_xlat10_0;
      int u_xlatb0;
      float4 u_xlat1_d;
      float4 u_xlat16_1;
      float4 u_xlat10_1;
      float4 u_xlat2;
      float4 u_xlat16_2;
      float4 u_xlat10_2;
      float4 u_xlat16_3;
      float4 u_xlat10_3;
      float4 u_xlat4;
      float4 u_xlat10_4;
      float4 u_xlat10_5;
      float4 u_xlat6;
      float4 u_xlat10_6;
      float4 u_xlat10_7;
      float4 u_xlat8;
      float4 u_xlat10_8;
      float4 u_xlat10_9;
      float4 u_xlat10_10;
      float4 u_xlat11;
      float4 u_xlat16_11;
      float4 u_xlat10_11;
      float4 u_xlat12;
      float4 u_xlat10_12;
      float4 u_xlat16_13;
      float4 u_xlat10_13;
      float3 u_xlat14;
      float3 u_xlat16_14;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d = ((in_f.texcoord.xyxy * float4(256, 256, 256, 256)) + float4(0, (-2), (-1), (-2)));
          u_xlat0_d = (u_xlat0_d * float4(0.00390625, 0.00390625, 0.00390625, 0.00390625));
          u_xlat10_1 = tex2D(_MainTex, u_xlat0_d.zw);
          u_xlat10_0 = tex2D(_MainTex, u_xlat0_d.xy);
          u_xlat2 = ((in_f.texcoord.xyxy * float4(256, 256, 256, 256)) + float4(-1, 2, 1, (-2)));
          u_xlat2 = (u_xlat2 * float4(0.00390625, 0.00390625, 0.00390625, 0.00390625));
          u_xlat10_3 = tex2D(_MainTex, u_xlat2.zw);
          u_xlat10_2 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat4 = ((in_f.texcoord.xyxy * float4(256, 256, 256, 256)) + float4(0, (-1), (-1), (-1)));
          u_xlat4 = (u_xlat4 * float4(0.00390625, 0.00390625, 0.00390625, 0.00390625));
          u_xlat10_5 = tex2D(_MainTex, u_xlat4.zw);
          u_xlat10_4 = tex2D(_MainTex, u_xlat4.xy);
          u_xlat6 = ((in_f.texcoord.xyxy * float4(256, 256, 256, 256)) + float4(-1, 0, 1, (-1)));
          u_xlat6 = (u_xlat6 * float4(0.00390625, 0.00390625, 0.00390625, 0.00390625));
          u_xlat10_7 = tex2D(_MainTex, u_xlat6.zw);
          u_xlat10_6 = tex2D(_MainTex, u_xlat6.xy);
          u_xlat8 = ((in_f.texcoord.xyxy * float4(256, 256, 256, 256)) + float4(-1, 1, 1, 0));
          u_xlat8 = (u_xlat8 * float4(0.00390625, 0.00390625, 0.00390625, 0.00390625));
          u_xlat10_9 = tex2D(_MainTex, u_xlat8.zw);
          u_xlat10_8 = tex2D(_MainTex, u_xlat8.xy);
          u_xlat10_10 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_11 = (u_xlat10_9 + u_xlat10_10);
          u_xlat16_11 = (u_xlat10_6 + u_xlat16_11);
          u_xlat16_11 = (u_xlat10_7 + u_xlat16_11);
          u_xlat16_11 = (u_xlat10_4 + u_xlat16_11);
          u_xlat16_11 = (u_xlat10_5 + u_xlat16_11);
          u_xlat16_3 = (u_xlat10_3 + u_xlat16_11);
          u_xlat16_0 = (u_xlat10_0 + u_xlat16_3);
          u_xlat16_0 = (u_xlat10_1 + u_xlat16_0);
          u_xlat16_0 = (u_xlat16_0 * float4(0.111111112, 0.111111112, 0.111111112, 0.111111112));
          u_xlat1_d = ((in_f.texcoord.xyxy * float4(256, 256, 256, 256)) + float4(1, 2, 0, 2));
          u_xlat1_d = (u_xlat1_d * float4(0.00390625, 0.00390625, 0.00390625, 0.00390625));
          u_xlat10_3 = tex2D(_MainTex, u_xlat1_d.xy);
          u_xlat10_1 = tex2D(_MainTex, u_xlat1_d.zw);
          u_xlat16_1 = (u_xlat10_1 + u_xlat10_3);
          u_xlat16_1 = (u_xlat10_2 + u_xlat16_1);
          u_xlat2 = ((in_f.texcoord.xyxy * float4(256, 256, 256, 256)) + float4(1, 1, 0, 1));
          u_xlat2 = (u_xlat2 * float4(0.00390625, 0.00390625, 0.00390625, 0.00390625));
          u_xlat10_3 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat2.zw);
          u_xlat16_1 = (u_xlat16_1 + u_xlat10_3);
          u_xlat16_1 = (u_xlat10_2 + u_xlat16_1);
          u_xlat16_1 = (u_xlat10_8 + u_xlat16_1);
          u_xlat16_1 = (u_xlat10_9 + u_xlat16_1);
          u_xlat16_1 = (u_xlat10_10 + u_xlat16_1);
          u_xlat16_1 = (u_xlat10_6 + u_xlat16_1);
          u_xlat16_0 = ((u_xlat16_1 * float4(0.111111112, 0.111111112, 0.111111112, 0.111111112)) + (-u_xlat16_0));
          u_xlat16_1 = (u_xlat10_8 + u_xlat10_2);
          u_xlat11 = ((in_f.texcoord.xyxy * float4(256, 256, 256, 256)) + float4(2, (-1), (-2), 1));
          u_xlat11 = (u_xlat11 * float4(0.00390625, 0.00390625, 0.00390625, 0.00390625));
          u_xlat10_12 = tex2D(_MainTex, u_xlat11.zw);
          u_xlat10_11 = tex2D(_MainTex, u_xlat11.xy);
          u_xlat16_1 = (u_xlat16_1 + u_xlat10_12);
          u_xlat16_1 = (u_xlat10_10 + u_xlat16_1);
          u_xlat16_1 = (u_xlat10_6 + u_xlat16_1);
          u_xlat12 = ((in_f.texcoord.xyxy * float4(256, 256, 256, 256)) + float4(-2, 0, (-2), (-1)));
          u_xlat12 = (u_xlat12 * float4(0.00390625, 0.00390625, 0.00390625, 0.00390625));
          u_xlat10_13 = tex2D(_MainTex, u_xlat12.xy);
          u_xlat10_12 = tex2D(_MainTex, u_xlat12.zw);
          u_xlat16_1 = (u_xlat16_1 + u_xlat10_13);
          u_xlat16_1 = (u_xlat10_4 + u_xlat16_1);
          u_xlat16_1 = (u_xlat10_5 + u_xlat16_1);
          u_xlat16_1 = (u_xlat10_12 + u_xlat16_1);
          u_xlat16_1 = (u_xlat16_1 * float4(0.111111112, 0.111111112, 0.111111112, 0.111111112));
          u_xlat12 = ((in_f.texcoord.xyxy * float4(256, 256, 256, 256)) + float4(2, 1, 2, 0));
          u_xlat12 = (u_xlat12 * float4(0.00390625, 0.00390625, 0.00390625, 0.00390625));
          u_xlat10_13 = tex2D(_MainTex, u_xlat12.xy);
          u_xlat10_12 = tex2D(_MainTex, u_xlat12.zw);
          u_xlat16_13 = (u_xlat10_3 + u_xlat10_13);
          u_xlat16_3.xyz = (u_xlat10_2.xyz + u_xlat10_3.xyz);
          u_xlat16_2 = (u_xlat10_2 + u_xlat16_13);
          u_xlat16_2 = (u_xlat10_12 + u_xlat16_2);
          u_xlat16_2 = (u_xlat10_9 + u_xlat16_2);
          u_xlat16_2 = (u_xlat10_10 + u_xlat16_2);
          u_xlat16_2 = (u_xlat10_11 + u_xlat16_2);
          u_xlat16_2 = (u_xlat10_7 + u_xlat16_2);
          u_xlat16_2 = (u_xlat10_4 + u_xlat16_2);
          u_xlat16_1 = ((u_xlat16_2 * float4(0.111111112, 0.111111112, 0.111111112, 0.111111112)) + (-u_xlat16_1));
          u_xlat16_0 = (abs(u_xlat16_0) + abs(u_xlat16_1));
          u_xlat16_0 = (u_xlat16_0 * float4(0.5, 0.5, 0.5, 0.5));
          u_xlat0_d.x = length(u_xlat16_0);
          u_xlat16_14.xyz = (u_xlat10_8.xyz + u_xlat16_3.xyz);
          u_xlat16_14.xyz = (u_xlat10_9.xyz + u_xlat16_14.xyz);
          u_xlat16_14.xyz = (u_xlat10_10.xyz + u_xlat16_14.xyz);
          out_f.color.w = (u_xlat10_10.w + (-_Alpha));
          u_xlat16_14.xyz = (u_xlat10_6.xyz + u_xlat16_14.xyz);
          u_xlat16_14.xyz = (u_xlat10_7.xyz + u_xlat16_14.xyz);
          u_xlat16_14.xyz = (u_xlat10_4.xyz + u_xlat16_14.xyz);
          u_xlat16_14.xyz = (u_xlat10_5.xyz + u_xlat16_14.xyz);
          u_xlat14.xyz = (u_xlat16_14.xyz * in_f.color.xyz);
          u_xlat14.xyz = (u_xlat14.xyz * float3(0.777777791, 0.777777791, 0.777777791));
          u_xlat14.xyz = floor(u_xlat14.xyz);
          u_xlat14.xyz = (u_xlat14.xyz / float3(_ColorLevel, _ColorLevel, _ColorLevel));
          u_xlat1_d.x = (_EdgeSize + 0.0500000007);
          u_xlatb0 = (u_xlat1_d.x<u_xlat0_d.x);
          out_f.color.xyz = (int(u_xlatb0))?(float3(0, 0, 0)):(u_xlat14.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
