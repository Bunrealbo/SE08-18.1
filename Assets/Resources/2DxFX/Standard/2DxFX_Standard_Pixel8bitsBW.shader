Shader "2DxFX/Standard/Pixel8bitsBW"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Size ("Size", Range(0, 1)) = 0
    _Offset ("Offset", Range(0, 1)) = 0
    _Offset2 ("Offset2", Range(0, 1)) = 0
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
      uniform float _Size;
      uniform float _Offset2;
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
      int u_xlatb0;
      float2 u_xlat1_d;
      float4 u_xlat10_1;
      float3 u_xlat2;
      float3 u_xlat3;
      int u_xlatb3;
      int u_xlatb4;
      float u_xlat6;
      int u_xlatb6;
      float u_xlat9;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Size * 64);
          u_xlat3.xy = (u_xlat0_d.xx * in_f.texcoord.xy);
          u_xlat3.xy = floor(u_xlat3.xy);
          u_xlat0_d.xy = (u_xlat3.xy / u_xlat0_d.xx);
          u_xlat6 = (u_xlat0_d.y + u_xlat0_d.x);
          u_xlat10_1 = tex2D(_MainTex, u_xlat0_d.xy);
          u_xlat0_d.x = (u_xlat6 * 0.5);
          u_xlat0_d.x = floor(u_xlat0_d.x);
          u_xlat0_d.x = (((-u_xlat0_d.x) * 2) + u_xlat6);
          u_xlat2.z = 1000;
          u_xlat3.xyz = ((u_xlat10_1.xyz * float3(float3(_Offset2, _Offset2, _Offset2))) + float3(-0.0299999993, (-0.0299999993), (-0.0299999993)));
          u_xlat2.x = dot(u_xlat3.xyz, u_xlat3.xyz);
          u_xlat3.xyz = ((u_xlat10_1.xyz * float3(float3(_Offset2, _Offset2, _Offset2))) + float3(-0.980000019, (-0.980000019), (-0.980000019)));
          u_xlat3.x = dot(u_xlat3.xyz, u_xlat3.xyz);
          u_xlat2.y = min(u_xlat3.x, 1000);
          u_xlatb3 = (u_xlat3.x<1000);
          u_xlat3.x = (u_xlatb3)?(0.980000019):(float(0));
          u_xlatb6 = (u_xlat2.x<u_xlat2.y);
          u_xlat1_d.xy = (int(u_xlatb6))?(u_xlat2.xy):(u_xlat2.yz);
          u_xlat1_d.xy = sqrt(u_xlat1_d.xy);
          u_xlat9 = (u_xlat1_d.y + u_xlat1_d.x);
          u_xlat9 = (u_xlat1_d.y / u_xlat9);
          u_xlat9 = (u_xlat9 + 1);
          u_xlatb0 = (u_xlat9<u_xlat0_d.x);
          u_xlat9 = (u_xlatb6)?(0.0299999993):(u_xlat3.x);
          u_xlat3.x = (u_xlatb6)?(u_xlat3.x):(float(0));
          u_xlat0_d.xyz = (int(u_xlatb0))?(u_xlat3.xxx):(float3(u_xlat9, u_xlat9, u_xlat9));
          u_xlat1_d.x = ((-_Alpha) + 1);
          u_xlat1_d.x = (u_xlat1_d.x * u_xlat10_1.w);
          u_xlatb4 = (u_xlat10_1.w<0.949999988);
          u_xlat0_d.w = (u_xlatb4)?(0):(u_xlat1_d.x);
          out_f.color = (u_xlat0_d * in_f.color);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
