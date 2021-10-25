Shader "2DxFX/Standard/CompressionFX"
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
      //uniform float4 _Time;
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
      float4 u_xlat10_0;
      float2 u_xlat1_d;
      float u_xlat2;
      float u_xlat4;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d = (in_f.texcoord.xyxy * float4(24, 19, 38, 14));
          u_xlat0_d = floor(u_xlat0_d);
          u_xlat0_d = (u_xlat0_d * float4(4, 4, 4, 4));
          u_xlat1_d.xy = (_Time.xy + float2(0.100000001, 0.100000001));
          u_xlat1_d.xy = ((u_xlat1_d.xy * float2(12, 12)) + float2(50, 50));
          u_xlat1_d.xy = floor(u_xlat1_d.xy);
          u_xlat0_d = (u_xlat0_d * u_xlat1_d.xyxy);
          u_xlat1_d.xy = (u_xlat1_d.xy * float2(2, 1));
          u_xlat1_d.x = dot(u_xlat1_d.xy, float2(127.099998, 311.700012));
          u_xlat1_d.x = sin(u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.x * 43758.5469);
          u_xlat1_d.x = frac(u_xlat1_d.x);
          u_xlat0_d.x = dot(u_xlat0_d.xy, float2(127.099998, 311.700012));
          u_xlat0_d.y = dot(u_xlat0_d.zw, float2(127.099998, 311.700012));
          u_xlat0_d.xy = sin(u_xlat0_d.xy);
          u_xlat0_d.xy = (u_xlat0_d.xy * float2(43758.5469, 43758.5469));
          u_xlat0_d.xy = frac(u_xlat0_d.xy);
          u_xlat4 = (u_xlat0_d.x * u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat4 * u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x * _Distortion);
          u_xlat4 = (u_xlat0_d.y * u_xlat0_d.y);
          u_xlat2 = (u_xlat4 * u_xlat0_d.y);
          u_xlat0_d.x = (u_xlat2 * u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x * 0.0199999996);
          u_xlat0_d.x = (u_xlat1_d.x * u_xlat0_d.x);
          u_xlat0_d.y = 0;
          u_xlat0_d.xy = (u_xlat0_d.xy + in_f.texcoord.xy);
          u_xlat10_0 = tex2D(_MainTex, u_xlat0_d.xy);
          u_xlat0_d.xyz = (u_xlat10_0.xyz * in_f.color.xyz);
          out_f.color.w = ((u_xlat10_0.w * in_f.color.w) + (-_Alpha));
          out_f.color.xyz = u_xlat0_d.xyz;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
