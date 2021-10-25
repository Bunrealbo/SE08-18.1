Shader "2DxFX/Standard/PlasmaShield"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Size ("Size", Range(4, 128)) = 4
    _Color ("Tint", Color) = (1,1,1,1)
    _ColorX ("Tint", Color) = (1,1,1,1)
    _Offset ("Offset", Range(4, 128)) = 4
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
      //uniform float4 _Time;
      uniform float _Offset;
      uniform float _Alpha;
      uniform float4 _ColorX;
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
      float4 u_xlat10_1;
      float u_xlat2;
      float u_xlat16_2;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = ((_Time.y * 2.25) + _Offset);
          u_xlat0_d.x = (u_xlat0_d.x + 1.10000002);
          u_xlat0_d.y = (((-in_f.texcoord.x) * 2) + u_xlat0_d.x);
          u_xlat0_d.zw = ((in_f.texcoord.xy * float2(2, 2)) + u_xlat0_d.xx);
          u_xlat0_d.x = ((in_f.texcoord.y * 5) + u_xlat0_d.x);
          u_xlat0_d = sin(u_xlat0_d);
          u_xlat2 = (u_xlat0_d.y + u_xlat0_d.z);
          u_xlat2 = (u_xlat0_d.w + u_xlat2);
          u_xlat0_d.x = (u_xlat0_d.x + u_xlat2);
          u_xlat0_d.x = (u_xlat0_d.x + 5);
          u_xlat2 = (u_xlat0_d.x * 0.200000003);
          u_xlat2 = floor(u_xlat2);
          u_xlat0_d.x = ((u_xlat0_d.x * 0.200000003) + (-u_xlat2));
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_2 = dot(u_xlat10_1.xzy, float3(0.200000003, 0.200000003, 0.400000006));
          u_xlat0_d.x = (u_xlat16_2 + u_xlat0_d.x);
          u_xlat2 = floor(u_xlat0_d.x);
          u_xlat0_d.x = ((-u_xlat2) + u_xlat0_d.x);
          u_xlat2 = ((u_xlat0_d.x * _Size) + (-2));
          u_xlat2 = clamp(u_xlat2, 0, 1);
          u_xlat0_d.x = (((-u_xlat0_d.x) * _Size) + 2);
          u_xlat0_d.x = clamp(u_xlat0_d.x, 0, 1);
          u_xlat0_d.x = (u_xlat0_d.x + u_xlat2);
          u_xlat0_d.x = ((-u_xlat0_d.x) + 1);
          u_xlat0_d.x = (u_xlat0_d.x + (-_ColorX.w));
          u_xlat0_d.x = (u_xlat0_d.x + 1);
          u_xlat0_d.x = ((u_xlat0_d.x * u_xlat10_1.w) + (-_Alpha));
          out_f.color.w = (u_xlat0_d.x * in_f.color.w);
          out_f.color.xyz = (in_f.color.xyz * _ColorX.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
