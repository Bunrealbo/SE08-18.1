Shader "2DxFX/Standard/Shiny_Reflect"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _MainTex2 ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Distortion ("Distortion", Range(0, 1)) = 0
    _Value2 ("Value 2", Range(0, 1)) = 0
    _Value3 ("Value 3", Range(0, 1)) = 0
    _Value4 ("Value 4", Range(0, 1)) = 0
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
      uniform float _Value2;
      uniform float _Value3;
      uniform float _Value4;
      uniform float _Value5;
      uniform float _Alpha;
      uniform sampler2D _MainTex;
      uniform sampler2D _MainTex2;
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
      float4 u_xlat1_d;
      float4 u_xlat10_1;
      float4 u_xlat2;
      float u_xlat3;
      float u_xlat6;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex2, in_f.texcoord.xy).x;
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat2 = (u_xlat10_1 * in_f.color);
          u_xlat3 = dot(u_xlat2.xyz, float3(0.298999995, 0.587000012, 0.114));
          u_xlat3 = (u_xlat3 * _Value5);
          u_xlat0_d.x = ((u_xlat10_0 * in_f.color.x) + u_xlat3);
          u_xlat3 = ((_Value2 * 0.5) + _Distortion);
          u_xlat0_d.x = ((-u_xlat0_d.x) + u_xlat3);
          u_xlat3 = (float(1) / _Value2);
          u_xlat0_d.x = (u_xlat3 * u_xlat0_d.x);
          u_xlat0_d.x = clamp(u_xlat0_d.x, 0, 1);
          u_xlat3 = ((u_xlat0_d.x * (-2)) + 3);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat0_d.x);
          u_xlat6 = (((-u_xlat3) * u_xlat0_d.x) + 1);
          u_xlat0_d.x = ((u_xlat3 * u_xlat0_d.x) + (-u_xlat6));
          u_xlat0_d.x = ((-abs(u_xlat0_d.x)) + 1);
          u_xlat0_d.x = (abs(u_xlat0_d.x) * _Value3);
          u_xlat1_d.xyz = ((u_xlat10_1.xyz * in_f.color.xyz) + u_xlat0_d.xxx);
          u_xlat3 = ((-_Alpha) + 1);
          u_xlat1_d.w = (u_xlat3 * u_xlat2.w);
          u_xlat0_d.w = (u_xlat0_d.x * u_xlat1_d.w);
          u_xlat0_d.x = float(1);
          u_xlat0_d.y = float(1);
          u_xlat0_d.z = float(1);
          u_xlat0_d = ((-u_xlat1_d) + u_xlat0_d);
          out_f.color = ((float4(float4(_Value4, _Value4, _Value4, _Value4)) * u_xlat0_d) + u_xlat1_d);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
