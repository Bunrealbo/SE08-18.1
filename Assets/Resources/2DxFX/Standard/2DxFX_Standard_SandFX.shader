Shader "2DxFX/Standard/SandFX"
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
      float3 u_xlat0_d;
      float4 u_xlat10_1;
      float4 u_xlat2;
      float2 u_xlat3;
      int u_xlatb3;
      float u_xlat6;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = dot(in_f.texcoord.xy, float2(12.9898005, 78.2330017));
          u_xlat0_d.x = sin(u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x * 43758.5469);
          u_xlat0_d.x = frac(u_xlat0_d.x);
          u_xlat3.x = (in_f.texcoord.y * _Distortion);
          u_xlat3.xy = (u_xlat3.xx * float2(41.9400024, 10.4666672));
          u_xlat3.xy = sin(u_xlat3.xy);
          u_xlat3.xy = ((u_xlat3.xy * float2(0.00714285718, 0.0250000004)) + in_f.texcoord.xy);
          u_xlat10_1 = tex2D(_MainTex, u_xlat3.xy);
          u_xlat2 = (u_xlat10_1 * in_f.color);
          u_xlat3.x = dot(u_xlat2.xyz, float3(0.222000003, 0.707000017, 0.0710000023));
          u_xlat0_d.x = ((-u_xlat3.x) + u_xlat0_d.x);
          u_xlat6 = (_Distortion * 0.333333343);
          u_xlat0_d.x = ((u_xlat6 * u_xlat0_d.x) + u_xlat3.x);
          u_xlatb3 = (0.600000024<u_xlat0_d.x);
          u_xlat0_d.x = (u_xlatb3)?(0.600000024):(u_xlat0_d.x);
          u_xlatb3 = (u_xlat0_d.x<0.300000012);
          u_xlat0_d.x = (u_xlatb3)?(0.300000012):(u_xlat0_d.x);
          u_xlat0_d.xyz = (u_xlat0_d.xxx + float3(0.5, 0.300000012, (-0.300000012)));
          u_xlat0_d.xyz = (((-u_xlat10_1.xyz) * in_f.color.xyz) + u_xlat0_d.xyz);
          out_f.color.xyz = ((float3(_Distortion, _Distortion, _Distortion) * u_xlat0_d.xyz) + u_xlat2.xyz);
          u_xlat0_d.x = ((-_Alpha) + 1);
          out_f.color.w = (u_xlat0_d.x * u_xlat2.w);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
