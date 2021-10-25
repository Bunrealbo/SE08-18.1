Shader "2DxFX/Standard/MetalFX"
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
      float3 u_xlat1_d;
      float4 u_xlat10_1;
      float3 u_xlat2;
      float3 u_xlat3;
      float u_xlat16_3;
      float u_xlat6;
      float u_xlat9;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = ((_Time.x * 11.25) + 1.10000002);
          u_xlat0_d.y = (((-in_f.texcoord.x) * 5) + u_xlat0_d.x);
          u_xlat0_d.xzw = ((in_f.texcoord.xyy * float3(5, 5, 12.5)) + u_xlat0_d.xxx);
          u_xlat0_d = sin(u_xlat0_d);
          u_xlat0_d.x = (u_xlat0_d.y + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.z + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.w + u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x + 5);
          u_xlat3.x = (u_xlat0_d.x * 0.200000003);
          u_xlat3.x = floor(u_xlat3.x);
          u_xlat0_d.x = ((u_xlat0_d.x * 0.200000003) + (-u_xlat3.x));
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_3 = dot(u_xlat10_1.xzy, float3(0.200000003, 0.200000003, 0.400000006));
          u_xlat0_d.x = (u_xlat16_3 + u_xlat0_d.x);
          u_xlat3.x = floor(u_xlat0_d.x);
          u_xlat0_d.x = ((-u_xlat3.x) + u_xlat0_d.x);
          u_xlat3.x = ((u_xlat0_d.x * 6) + (-2));
          u_xlat3.x = clamp(u_xlat3.x, 0, 1);
          u_xlat0_d.x = (((-u_xlat0_d.x) * 6) + 2);
          u_xlat0_d.x = clamp(u_xlat0_d.x, 0, 1);
          u_xlat0_d.x = (u_xlat0_d.x + u_xlat3.x);
          u_xlat0_d.x = ((u_xlat0_d.x * u_xlat10_1.w) + (-_Alpha));
          u_xlat3.xyz = (u_xlat10_1.xyz * in_f.color.xyz);
          out_f.color.w = ((u_xlat10_1.w * in_f.color.w) + (-_Alpha));
          u_xlat3.x = dot(u_xlat3.xyz, float3(0.412600011, 0.815199971, 0.172199994));
          u_xlat6 = (((-u_xlat3.x) * 0.115896732) + 1);
          u_xlat9 = (u_xlat3.x * u_xlat3.x);
          u_xlat6 = ((u_xlat9 * 2.58329701) + u_xlat6);
          u_xlat1_d.xyz = ((u_xlat3.xxx * float3(0.616473019, 3.36968088, 0.169122502)) + float3(0.860117733, 1, 0.317398727));
          u_xlat2.xyz = (u_xlat3.xxx * float3(2.4000001, 1.5999999, 1.5999999));
          u_xlat2.xyz = (u_xlat2.xyz * u_xlat2.xyz);
          u_xlat2.xyz = (u_xlat2.xyz * u_xlat2.xyz);
          u_xlat1_d.xyz = ((float3(u_xlat9, u_xlat9, u_xlat9) * float3(2.05825949, 11.3303223, 0.672770679)) + u_xlat1_d.xyz);
          u_xlat3.x = (u_xlat1_d.z / u_xlat6);
          u_xlat6 = (u_xlat1_d.x / u_xlat1_d.y);
          u_xlat9 = (u_xlat6 + u_xlat6);
          u_xlat3.y = (u_xlat6 * 3);
          u_xlat9 = (((-u_xlat3.x) * 8) + u_xlat9);
          u_xlat3.x = (u_xlat3.x + u_xlat3.x);
          u_xlat9 = (u_xlat9 + 4);
          u_xlat0_d.yz = (u_xlat3.xy / float2(u_xlat9, u_xlat9));
          u_xlat0_d.xw = ((-u_xlat0_d.xz) + float2(1, 1));
          u_xlat0_d.w = ((-u_xlat0_d.y) + u_xlat0_d.w);
          u_xlat3.x = (float(1) / u_xlat0_d.y);
          u_xlat1_d.xz = (u_xlat0_d.wz * u_xlat3.xx);
          u_xlat1_d.y = 1;
          u_xlat3.xyz = (u_xlat1_d.xyz / float3(_Distortion, _Distortion, _Distortion));
          u_xlat0_d.xyz = ((u_xlat3.xyz * u_xlat2.xyz) + u_xlat0_d.xxx);
          u_xlat1_d.xyz = ((u_xlat0_d.xyz * float3(0.5, 0.5, 0.5)) + float3(0.100000001, 0.100000001, 0.100000001));
          u_xlat0_d.x = dot(u_xlat0_d.xyz, float3(0.212599993, 0.215200007, 0.172199994));
          out_f.color.xyz = (u_xlat0_d.xxx + u_xlat1_d.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
