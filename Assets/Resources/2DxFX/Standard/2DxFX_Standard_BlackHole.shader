Shader "2DxFX/Standard/BlackHole"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Size ("Size", Range(0, 1)) = 0
    _Distortion ("Distortion", Range(0, 1)) = 0
    _Hole ("Hole", Range(0, 1)) = 0
    _Speed ("Speed", Range(0, 1)) = 0
    _Alpha ("Alpha", Range(0, 1)) = 1
    _Color ("ColorX", Color) = (1,1,1,1)
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
      uniform float4 _ColorX;
      uniform float _Distortion;
      uniform float _Hole;
      uniform float _Speed;
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
      float4 u_xlat1_d;
      float4 u_xlat10_1;
      float2 u_xlat2;
      float3 u_xlat3;
      float2 u_xlat4;
      int u_xlatb4;
      float2 u_xlat8;
      float u_xlat12;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = sin(_Distortion);
          u_xlat4.x = (_Time.x * _Speed);
          u_xlat4.x = (u_xlat4.x * 5);
          u_xlat1_d.x = sin(u_xlat4.x);
          u_xlat2.x = cos(u_xlat4.x);
          u_xlat3.z = u_xlat1_d.x;
          u_xlat4.xy = (in_f.texcoord.xy + float2(-0.5, (-0.5)));
          u_xlat4.xy = (u_xlat4.xy * float2(1.24600005, 1.24600005));
          u_xlat3.y = u_xlat2.x;
          u_xlat3.x = (-u_xlat1_d.x);
          u_xlat1_d.y = dot(u_xlat4.xy, u_xlat3.xy);
          u_xlat1_d.x = dot(u_xlat4.xy, u_xlat3.yz);
          u_xlat4.x = length(u_xlat1_d.xy);
          u_xlat8.x = ((-u_xlat4.x) + 0.5);
          u_xlatb4 = (u_xlat4.x<0.5);
          u_xlat8.x = (u_xlat8.x + u_xlat8.x);
          u_xlat8.x = (u_xlat8.x * u_xlat8.x);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat8.x);
          u_xlat0_d.x = (u_xlat0_d.x * 16);
          u_xlat2.x = cos(u_xlat0_d.x);
          u_xlat0_d.x = sin(u_xlat0_d.x);
          u_xlat3.x = (-u_xlat0_d.x);
          u_xlat3.y = u_xlat2.x;
          u_xlat3.z = u_xlat0_d.x;
          u_xlat2.y = dot(u_xlat1_d.yx, u_xlat3.yz);
          u_xlat2.x = dot(u_xlat1_d.yx, u_xlat3.xy);
          u_xlat0_d.xy = (int(u_xlatb4))?(u_xlat2.xy):(u_xlat1_d.xy);
          u_xlat8.xy = (u_xlat1_d.xy + float2(0.5, 0.5));
          u_xlat8.xy = ((-u_xlat8.xy) + float2(0.5, 0.5));
          u_xlat8.x = length(u_xlat8.xy);
          u_xlat0_d.xy = (u_xlat0_d.xy + float2(0.5, 0.5));
          u_xlat10_1 = tex2D(_MainTex, u_xlat0_d.xy);
          u_xlat0_d.x = ((-_Alpha) + 1);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat10_1.w);
          u_xlat4.x = (u_xlat8.x + (-0.25));
          u_xlat4.y = (u_xlat8.x + (-_Hole));
          u_xlat4.xy = (u_xlat4.xy * float2(4, 6.66666651));
          u_xlat4.xy = clamp(u_xlat4.xy, 0, 1);
          u_xlat12 = ((u_xlat4.x * (-2)) + 3);
          u_xlat4.x = (u_xlat4.x * u_xlat4.x);
          u_xlat4.x = (((-u_xlat12) * u_xlat4.x) + 1);
          u_xlat12 = ((u_xlat4.y * (-2)) + 3);
          u_xlat8.x = (u_xlat4.y * u_xlat4.y);
          u_xlat8.x = (((-u_xlat12) * u_xlat8.x) + 1);
          u_xlat8.x = ((-u_xlat8.x) + 1);
          u_xlat4.x = (u_xlat8.x * u_xlat4.x);
          u_xlat1_d.xyz = (u_xlat8.xxx * u_xlat10_1.xyz);
          u_xlat1_d.w = (u_xlat4.x * u_xlat0_d.x);
          u_xlat0_d = (u_xlat1_d * in_f.color);
          out_f.color = (u_xlat0_d * _ColorX);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
