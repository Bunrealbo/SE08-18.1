Shader "2DxFX/Standard/PlasmaRainbow_Color"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Colors ("Colors", Range(4, 128)) = 4
    _Color ("Color", Color) = (1,1,1,1)
    _Offset ("Offset", Range(4, 128)) = 1
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
      uniform float _Colors;
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
      float3 u_xlat2;
      float2 u_xlat3;
      float3 u_xlat16_3;
      float u_xlat4;
      float u_xlat16_4;
      float3 u_xlatb4;
      float u_xlat8;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d = ((_Time.yyyy * float4(2.25, 1.76999998, 1.58000004, 2.02999997)) + float4(_Offset, _Offset, _Offset, _Offset));
          u_xlat0_d = (u_xlat0_d + float4(1.10000002, 0.5, 8.39999962, 610));
          u_xlat0_d.y = (((-in_f.texcoord.x) * 2) + u_xlat0_d.y);
          u_xlat0_d.xz = ((in_f.texcoord.xy * float2(2, 2)) + u_xlat0_d.xz);
          u_xlat0_d.w = ((in_f.texcoord.y * 5) + u_xlat0_d.w);
          u_xlat0_d = sin(u_xlat0_d);
          u_xlat0_d.x = (u_xlat0_d.y + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.z + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.w + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x + 5);
          u_xlat4 = (u_xlat0_d.x * 0.200000003);
          u_xlat4 = floor(u_xlat4);
          u_xlat0_d.x = ((u_xlat0_d.x * 0.200000003) + (-u_xlat4));
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_4 = dot(u_xlat10_1.xzy, float3(0.200000003, 0.200000003, 0.400000006));
          u_xlat0_d.x = (u_xlat16_4 + u_xlat0_d.x);
          u_xlat4 = floor(u_xlat0_d.x);
          u_xlat0_d.x = ((-u_xlat4) + u_xlat0_d.x);
          u_xlat4 = (u_xlat0_d.x * _Colors);
          u_xlat8 = u_xlat4;
          u_xlat8 = clamp(u_xlat8, 0, 1);
          u_xlatb4.xz = bool4(float4(u_xlat4, u_xlat4, u_xlat4, u_xlat4) < float4(2, 0, 4, 4)).xz;
          u_xlat2.xyz = (((-u_xlat0_d.xxx) * float3(float3(_Colors, _Colors, _Colors))) + float3(2, 4, 6));
          u_xlat2.xyz = clamp(u_xlat2.xyz, 0, 1);
          u_xlat3.xy = ((u_xlat0_d.xx * float2(float2(_Colors, _Colors))) + float2(-4, (-2)));
          u_xlat3.xy = clamp(u_xlat3.xy, 0, 1);
          u_xlat0_d.y = (u_xlatb4.x)?(u_xlat8):(u_xlat2.y);
          u_xlat0_d.z = (u_xlatb4.z)?(u_xlat3.y):(u_xlat2.z);
          u_xlat0_d.x = (u_xlat2.x + u_xlat3.x);
          u_xlat2.xyz = ((-u_xlat0_d.xyz) + u_xlat10_1.xyz);
          u_xlat16_3.xyz = (u_xlat10_1.xyz + u_xlat10_1.xyz);
          u_xlat0_d.xyz = ((u_xlat16_3.xyz * u_xlat2.xyz) + u_xlat0_d.xyz);
          u_xlat0_d.xyz = (((-u_xlat10_1.xyz) * float3(0.5, 0.5, 0.5)) + u_xlat0_d.xyz);
          u_xlat0_d.w = (u_xlat10_1.w + (-_Alpha));
          out_f.color = (u_xlat0_d * in_f.color);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
