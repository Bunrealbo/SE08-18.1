Shader "2DxFX/Standard/DestroyedFX"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Distortion ("Distortion", Range(0, 1)) = 0
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
      uniform float _Distortion;
      uniform float _Alpha;
      uniform float _Size;
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
      float4 u_xlat10_0;
      float u_xlat1_d;
      float u_xlat2;
      float2 u_xlat3;
      float4 u_xlat4;
      float3 u_xlat5;
      float2 u_xlat6;
      float u_xlat7;
      float2 u_xlat11;
      int u_xlati12;
      float2 u_xlat13;
      float u_xlat17;
      int u_xlatb17;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat0_d = (u_xlat10_0.wxyz * in_f.color.wxyz);
          u_xlat1_d = (_Distortion * 0.999899983);
          u_xlat1_d = frac(u_xlat1_d);
          u_xlat6.x = (u_xlat1_d + 0.100000001);
          u_xlat11.xy = (in_f.texcoord.xy * float2(3.5, 3.5));
          u_xlat2 = float(0);
          u_xlat7 = float(2);
          int u_xlati_loop_1 = int(0);
          while((u_xlati_loop_1<9))
          {
              u_xlat3.xy = (u_xlat11.xy * float2(u_xlat7, u_xlat7));
              u_xlat3.xy = floor(u_xlat3.xy);
              u_xlat13.xy = ((float2(u_xlat7, u_xlat7) * u_xlat11.xy) + (-u_xlat3.xy));
              u_xlat4.xy = (u_xlat13.xy * u_xlat13.xy);
              u_xlat13.xy = ((u_xlat13.xy * float2(-2, (-2))) + float2(3, 3));
              u_xlat13.xy = (u_xlat13.xy * u_xlat4.xy);
              u_xlat17 = ((u_xlat3.y * 7) + u_xlat3.x);
              u_xlat17 = sin(u_xlat17);
              u_xlat17 = (u_xlat17 * _Size);
              u_xlat17 = (u_xlat17 * 43);
              u_xlat17 = frac(u_xlat17);
              u_xlat4 = (u_xlat3.xyxy + float4(1, 0, 0, 1));
              u_xlat4.xy = ((u_xlat4.yw * float2(7, 7)) + u_xlat4.xz);
              u_xlat4.xy = sin(u_xlat4.xy);
              u_xlat4.xy = (u_xlat4.xy * float2(float2(_Size, _Size)));
              u_xlat4.xy = (u_xlat4.xy * float2(43, 43));
              u_xlat4.xy = frac(u_xlat4.xy);
              u_xlat4.x = ((-u_xlat17) + u_xlat4.x);
              u_xlat17 = ((u_xlat13.x * u_xlat4.x) + u_xlat17);
              u_xlat3.xy = (u_xlat3.xy + float2(1, 1));
              u_xlat3.x = ((u_xlat3.y * 7) + u_xlat3.x);
              u_xlat3.x = sin(u_xlat3.x);
              u_xlat3.x = (u_xlat3.x * _Size);
              u_xlat3.x = (u_xlat3.x * 43);
              u_xlat3.x = frac(u_xlat3.x);
              u_xlat3.x = ((-u_xlat4.y) + u_xlat3.x);
              u_xlat3.x = ((u_xlat13.x * u_xlat3.x) + u_xlat4.y);
              u_xlat3.x = ((-u_xlat17) + u_xlat3.x);
              u_xlat17 = ((u_xlat13.y * u_xlat3.x) + u_xlat17);
              u_xlat17 = (u_xlat17 / u_xlat7);
              u_xlat2 = (u_xlat17 + u_xlat2);
              u_xlat7 = (u_xlat7 + u_xlat7);
              u_xlati_loop_1 = (u_xlati_loop_1 + 1);
          }
          u_xlat6.x = (((-u_xlat1_d) * 0.833333313) + u_xlat6.x);
          u_xlat1_d = (((-u_xlat1_d) * 0.833333313) + u_xlat2);
          u_xlat6.x = (float(1) / u_xlat6.x);
          u_xlat1_d = (u_xlat6.x * u_xlat1_d);
          u_xlat1_d = clamp(u_xlat1_d, 0, 1);
          u_xlat6.x = ((u_xlat1_d * (-2)) + 3);
          u_xlat1_d = (u_xlat1_d * u_xlat1_d);
          u_xlat1_d = (u_xlat1_d * u_xlat6.x);
          u_xlat5.xyz = (u_xlat0_d.yzw * float3(u_xlat1_d, u_xlat1_d, u_xlat1_d));
          u_xlat6.x = (u_xlat5.x * 15);
          u_xlat11.x = (((-u_xlat0_d.x) * u_xlat1_d) + 1);
          u_xlat6.x = (u_xlat11.x * u_xlat6.x);
          u_xlat6.x = ((u_xlat6.x * 8) + (-u_xlat5.x));
          out_f.color.x = ((_Distortion * u_xlat6.x) + u_xlat5.x);
          u_xlat6.xy = (u_xlat5.yz * u_xlat11.xx);
          u_xlat6.xy = ((u_xlat6.xy * float2(40, 5)) + (-u_xlat5.yz));
          out_f.color.yz = ((float2(_Distortion, _Distortion) * u_xlat6.xy) + u_xlat5.yz);
          out_f.color.w = ((u_xlat0_d.x * u_xlat1_d) + (-_Alpha));
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
