Shader "2DxFX/Standard/Teleportation"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Distortion ("Distortion", Range(0, 1)) = 0
    _Alpha ("Alpha", Range(0, 1)) = 1
    _Color ("Tint", Color) = (1,1,1,1)
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
      float2 u_xlat1_d;
      float4 u_xlat10_1;
      float3 u_xlat2;
      float u_xlat16_2;
      float u_xlat3;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Distortion * 10);
          u_xlat0_d.xy = (u_xlat0_d.xx * in_f.texcoord.xy);
          u_xlat0_d.z = (((-u_xlat0_d.x) * 2) + 46.0999985);
          u_xlat0_d.xw = ((u_xlat0_d.xy * float2(2, 2)) + float2(46.0999985, 46.0999985));
          u_xlat0_d.y = ((u_xlat0_d.y * 5) + 46.0999985);
          u_xlat0_d = sin(u_xlat0_d);
          u_xlat0_d.x = (u_xlat0_d.z + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.w + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.y + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x + 5);
          u_xlat2.x = (u_xlat0_d.x * 0.200000003);
          u_xlat2.x = floor(u_xlat2.x);
          u_xlat0_d.x = ((u_xlat0_d.x * 0.200000003) + (-u_xlat2.x));
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_2 = dot(u_xlat10_1.xyz, float3(11.1999998, 8.39999962, 4.19999981));
          u_xlat0_d.x = (u_xlat16_2 + u_xlat0_d.x);
          u_xlat2.x = floor(u_xlat0_d.x);
          u_xlat0_d.x = ((-u_xlat2.x) + u_xlat0_d.x);
          u_xlat2.x = ((u_xlat0_d.x * 6) + (-2));
          u_xlat2.x = clamp(u_xlat2.x, 0, 1);
          u_xlat0_d.x = (((-u_xlat0_d.x) * 6) + 2);
          u_xlat0_d.x = clamp(u_xlat0_d.x, 0, 1);
          u_xlat0_d.x = (u_xlat0_d.x + u_xlat2.x);
          u_xlat0_d.x = ((-u_xlat0_d.x) + 1);
          u_xlat2.x = dot(in_f.texcoord.xy, float2(12.9898005, 78.2330017));
          u_xlat2.x = sin(u_xlat2.x);
          u_xlat2.x = (u_xlat2.x * _Time.x);
          u_xlat2.x = (u_xlat2.x * 43758.5469);
          u_xlat2.x = frac(u_xlat2.x);
          u_xlat2.xyz = ((-u_xlat10_1.xyz) + u_xlat2.xxx);
          u_xlat2.xyz = ((float3(_Distortion, _Distortion, _Distortion) * u_xlat2.xyz) + u_xlat10_1.xyz);
          u_xlat0_d.xyz = ((u_xlat0_d.xxx * float3(_Distortion, _Distortion, _Distortion)) + u_xlat2.xyz);
          u_xlat1_d.xy = float2(((-float2(_Distortion, _Alpha)) + float2(1, 1)));
          u_xlat0_d.xz = (u_xlat0_d.xz * u_xlat1_d.xx);
          u_xlat3 = (u_xlat1_d.y * u_xlat10_1.w);
          u_xlat0_d.w = (u_xlat1_d.x * u_xlat3);
          out_f.color = (u_xlat0_d * in_f.color);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
