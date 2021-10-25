Shader "2DxFX/Standard/FlameAdditive"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _MainTex2 ("Base (RGB)", 2D) = "white" {}
    _Speed ("_Speed", Range(4, 128)) = 4
    _Intensity ("_Intensity", Range(4, 128)) = 4
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
      Blend SrcAlpha One
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
      uniform float _Speed;
      uniform float _Intensity;
      uniform float _Alpha;
      uniform sampler2D _MainTex2;
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
      float u_xlat10_0;
      float4 u_xlat1_d;
      float3 u_xlat2;
      float u_xlat4;
      float u_xlat16_4;
      float u_xlat10_4;
      float2 u_xlat5;
      float u_xlat6;
      float u_xlat10_6;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = 1.5;
          u_xlat0_d.y = (_Time.x * _Speed);
          u_xlat1_d.xy = ((float2(0, (-28.8000011)) * u_xlat0_d.xy) + in_f.texcoord.xy);
          u_xlat0_d.xy = (u_xlat0_d.yy * float2(128, (-19.2000008)));
          u_xlat1_d.z = (u_xlat1_d.y * 0.0833333358);
          u_xlat10_6 = tex2D(_MainTex2, u_xlat1_d.xz).y;
          u_xlat0_d.z = 0;
          u_xlat1_d.xy = (u_xlat0_d.zy + in_f.texcoord.xy);
          u_xlat0_d.x = cos(u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x * in_f.texcoord.y);
          u_xlat0_d.x = ((u_xlat0_d.x * 0.125) + u_xlat1_d.x);
          u_xlat1_d.z = (u_xlat1_d.y * 0.0625);
          u_xlat10_4 = tex2D(_MainTex2, u_xlat1_d.xz).y;
          u_xlat16_4 = ((-u_xlat10_4) + u_xlat10_6);
          u_xlat4 = ((u_xlat16_4 * _Intensity) + (-1));
          u_xlat4 = (float(1) / u_xlat4);
          u_xlat1_d.xy = (in_f.texcoord.xy + float2(-0.5, (-1)));
          u_xlat4 = (u_xlat4 * u_xlat1_d.y);
          u_xlat4 = clamp(u_xlat4, 0, 1);
          u_xlat6 = (((-abs(u_xlat1_d.x)) * 3) + 1.29999995);
          u_xlat1_d.x = ((u_xlat4 * (-2)) + 3);
          u_xlat4 = (u_xlat4 * u_xlat4);
          u_xlat4 = (u_xlat4 * u_xlat1_d.x);
          u_xlat4 = (u_xlat4 * u_xlat6);
          u_xlat4 = clamp(u_xlat4, 0, 1);
          u_xlat6 = (u_xlat4 * u_xlat4);
          u_xlat4 = (u_xlat6 * u_xlat4);
          u_xlat0_d.y = in_f.texcoord.y;
          u_xlat10_0 = tex2D(_MainTex, u_xlat0_d.xy).w;
          u_xlat2.x = ((in_f.texcoord.y * u_xlat10_0) + (-1.10000002));
          u_xlat2.x = (u_xlat2.x * (-0.833333313));
          u_xlat2.x = clamp(u_xlat2.x, 0, 1);
          u_xlat6 = ((u_xlat2.x * (-2)) + 3);
          u_xlat2.x = (u_xlat2.x * u_xlat2.x);
          u_xlat2.x = (u_xlat2.x * u_xlat6);
          u_xlat2.x = (u_xlat4 / u_xlat2.x);
          u_xlat0_d.x = (u_xlat10_0 * u_xlat2.x);
          u_xlat2.xyz = ((u_xlat0_d.xxx * float3(0, 0, 0.600000024)) + float3(0, 1, 0));
          u_xlat1_d.xy = (u_xlat0_d.xx * float2(0.625, 1.42857146));
          u_xlat1_d.xy = u_xlat1_d.xy;
          u_xlat1_d.xy = clamp(u_xlat1_d.xy, 0, 1);
          u_xlat5.xy = ((u_xlat1_d.xy * float2(-2, (-2))) + float2(3, 3));
          u_xlat1_d.xy = (u_xlat1_d.xy * u_xlat1_d.xy);
          u_xlat1_d.xy = (u_xlat1_d.xy * u_xlat5.xy);
          u_xlat2.xyz = ((u_xlat1_d.xxx * u_xlat2.xyz) + float3(1, 0, (-1)));
          u_xlat2.xyz = ((u_xlat1_d.yyy * u_xlat2.xyz) + float3(0, 0, 1));
          u_xlat1_d.x = 1.20000005;
          u_xlat1_d.yz = in_f.color.yz;
          u_xlat1_d.xyz = (u_xlat2.xyz * u_xlat1_d.xyz);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat1_d.x);
          u_xlat1_d.w = (u_xlat0_d.x * _Alpha);
          u_xlat0_d.xw = in_f.color.xw;
          u_xlat0_d.y = 1.20000005;
          out_f.color = (u_xlat0_d.xyyw * u_xlat1_d);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
