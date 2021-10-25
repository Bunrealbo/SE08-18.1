Shader "2DxFX/Standard/Distortion"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _OffsetX ("OffsetX", Range(0, 128)) = 0
    _OffsetY ("OffsetY", Range(0, 128)) = 0
    _DistanceX ("DistanceX", Range(0, 1)) = 0
    _DistanceY ("DistanceY", Range(0, 1)) = 0
    _WaveTimeX ("WaveTimeX", Range(0, 360)) = 0
    _WaveTimeY ("WaveTimeY", Range(0, 360)) = 0
    _Color ("Tint", Color) = (1,1,1,1)
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
      uniform float _OffsetX;
      uniform float _OffsetY;
      uniform float _DistanceX;
      uniform float _DistanceY;
      uniform float _WaveTimeX;
      uniform float _WaveTimeY;
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
      float4 u_xlat10_0;
      float u_xlat2;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = ((in_f.texcoord.y * _OffsetX) + _WaveTimeX);
          u_xlat0_d.x = sin(u_xlat0_d.x);
          u_xlat0_d.x = ((u_xlat0_d.x * _DistanceX) + in_f.texcoord.x);
          u_xlat2 = ((u_xlat0_d.x * _OffsetY) + _WaveTimeY);
          u_xlat2 = cos(u_xlat2);
          u_xlat0_d.y = ((u_xlat2 * _DistanceY) + in_f.texcoord.y);
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
