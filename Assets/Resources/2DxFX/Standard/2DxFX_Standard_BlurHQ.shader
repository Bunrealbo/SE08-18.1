Shader "2DxFX/Standard/BlurHQ"
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
      float4 u_xlat16_0;
      float4 u_xlat10_0;
      float4 u_xlat1_d;
      float4 u_xlat10_1;
      float4 u_xlat2;
      float4 u_xlat10_2;
      float4 u_xlat3;
      float4 u_xlat10_3;
      float4 u_xlat10_4;
      float2 u_xlat12;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d = (((-float4(_Distortion, _Distortion, _Distortion, _Distortion)) * float4(0.00390625, 0.00390625, 0.001953125, 0.00390625)) + in_f.texcoord.xyxy);
          u_xlat10_1 = tex2D(_MainTex, u_xlat0_d.xy);
          u_xlat10_0 = tex2D(_MainTex, u_xlat0_d.zw);
          u_xlat16_0 = ((u_xlat10_0 * float4(4, 4, 4, 4)) + u_xlat10_1);
          u_xlat1_d.y = 0;
          u_xlat1_d.xzw = (float3(_Distortion, _Distortion, _Distortion) * float3(-0.00390625, 0.001953125, (-0.00390625)));
          u_xlat2 = (u_xlat1_d.yxzw + in_f.texcoord.xyxy);
          u_xlat1_d.xy = (u_xlat1_d.xy + in_f.texcoord.xy);
          u_xlat10_1 = tex2D(_MainTex, u_xlat1_d.xy);
          u_xlat10_3 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat2.zw);
          u_xlat16_0 = ((u_xlat10_3 * float4(6, 6, 6, 6)) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_2 * float4(4, 4, 4, 4)) + u_xlat16_0);
          u_xlat2.xy = ((float2(_Distortion, _Distortion) * float2(0.00390625, (-0.00390625))) + in_f.texcoord.xy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat16_0 = (u_xlat16_0 + u_xlat10_2);
          u_xlat2 = (((-float4(_Distortion, _Distortion, _Distortion, _Distortion)) * float4(0.00390625, 0.001953125, 0.001953125, 0.001953125)) + in_f.texcoord.xyxy);
          u_xlat10_3 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat2.zw);
          u_xlat16_0 = ((u_xlat10_3 * float4(4, 4, 4, 4)) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_2 * float4(16, 16, 16, 16)) + u_xlat16_0);
          u_xlat12.x = (_Distortion * (-0.001953125));
          u_xlat12.y = 0;
          u_xlat2 = (u_xlat12.yxxy + in_f.texcoord.xyxy);
          u_xlat10_3 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat2.zw);
          u_xlat16_0 = ((u_xlat10_3 * float4(24, 24, 24, 24)) + u_xlat16_0);
          u_xlat3 = ((float4(_Distortion, _Distortion, _Distortion, _Distortion) * float4(0.001953125, (-0.001953125), 0.00390625, (-0.001953125))) + in_f.texcoord.xyxy);
          u_xlat10_4 = tex2D(_MainTex, u_xlat3.xy);
          u_xlat10_3 = tex2D(_MainTex, u_xlat3.zw);
          u_xlat16_0 = ((u_xlat10_4 * float4(16, 16, 16, 16)) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_3 * float4(4, 4, 4, 4)) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_1 * float4(6, 6, 6, 6)) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_2 * float4(24, 24, 24, 24)) + u_xlat16_0);
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_0 = ((u_xlat10_1 * float4(36, 36, 36, 36)) + u_xlat16_0);
          u_xlat1_d.yw = (float2(_Distortion, _Distortion) * float2(0.001953125, 0.00390625));
          u_xlat1_d.x = float(0);
          u_xlat1_d.z = float(0);
          u_xlat2 = (u_xlat1_d.yxwz + in_f.texcoord.xyxy);
          u_xlat1_d = (u_xlat1_d + in_f.texcoord.xyxy);
          u_xlat10_3 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat2.zw);
          u_xlat16_0 = ((u_xlat10_3 * float4(24, 24, 24, 24)) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_2 * float4(6, 6, 6, 6)) + u_xlat16_0);
          u_xlat2 = ((float4(_Distortion, _Distortion, _Distortion, _Distortion) * float4(-0.00390625, 0.001953125, (-0.001953125), 0.001953125)) + in_f.texcoord.xyxy);
          u_xlat10_3 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat2.zw);
          u_xlat16_0 = ((u_xlat10_3 * float4(4, 4, 4, 4)) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_2 * float4(16, 16, 16, 16)) + u_xlat16_0);
          u_xlat10_2 = tex2D(_MainTex, u_xlat1_d.xy);
          u_xlat10_1 = tex2D(_MainTex, u_xlat1_d.zw);
          u_xlat16_0 = ((u_xlat10_2 * float4(24, 24, 24, 24)) + u_xlat16_0);
          u_xlat2 = ((float4(_Distortion, _Distortion, _Distortion, _Distortion) * float4(0.001953125, 0.001953125, 0.00390625, 0.001953125)) + in_f.texcoord.xyxy);
          u_xlat10_3 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat2.zw);
          u_xlat16_0 = ((u_xlat10_3 * float4(16, 16, 16, 16)) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_2 * float4(4, 4, 4, 4)) + u_xlat16_0);
          u_xlat2 = ((float4(_Distortion, _Distortion, _Distortion, _Distortion) * float4(-0.00390625, 0.00390625, (-0.001953125), 0.00390625)) + in_f.texcoord.xyxy);
          u_xlat10_3 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat2.zw);
          u_xlat16_0 = (u_xlat16_0 + u_xlat10_3);
          u_xlat16_0 = ((u_xlat10_2 * float4(4, 4, 4, 4)) + u_xlat16_0);
          u_xlat16_0 = ((u_xlat10_1 * float4(6, 6, 6, 6)) + u_xlat16_0);
          u_xlat1_d = ((float4(_Distortion, _Distortion, _Distortion, _Distortion) * float4(0.001953125, 0.00390625, 0.00390625, 0.00390625)) + in_f.texcoord.xyxy);
          u_xlat10_2 = tex2D(_MainTex, u_xlat1_d.xy);
          u_xlat10_1 = tex2D(_MainTex, u_xlat1_d.zw);
          u_xlat16_0 = ((u_xlat10_2 * float4(4, 4, 4, 4)) + u_xlat16_0);
          u_xlat16_0 = (u_xlat10_1 + u_xlat16_0);
          u_xlat1_d.xyz = in_f.color.xyz;
          u_xlat1_d.w = 0.00390625;
          u_xlat0_d = (u_xlat16_0 * u_xlat1_d);
          u_xlat1_d.x = ((-_Alpha) + 1);
          u_xlat0_d.w = (u_xlat0_d.w * u_xlat1_d.x);
          u_xlat1_d.x = 0.00390625;
          u_xlat1_d.w = in_f.color.w;
          out_f.color = (u_xlat0_d * u_xlat1_d.xxxw);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
