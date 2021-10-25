Shader "2DxFX/Standard/Waterfall"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    [HideInInspector] _MainTex2 ("Pattern (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    Lightcolor ("Lightcolor", Color) = (1,1,1,1)
    _Distortion ("Distortion", Range(0, 1)) = 0
    _Alpha ("Alpha", Range(0, 1)) = 1
    _Speed ("Speed", Range(0, 1)) = 1
    EValue ("EValue", Range(0, 1)) = 1
    TValue ("TValue", Range(0, 1)) = 1
    Light ("Light", Range(0, 1)) = 1
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
      }
      ZClip Off
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
      // m_ProgramMask = 0
      
    } // end phase
    Pass // ind: 2, name: 
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
      uniform float _Speed;
      uniform float EValue;
      uniform float TValue;
      uniform float4 Lightcolor;
      uniform float Light;
      uniform sampler2D _MainTex;
      uniform sampler2D _MainTex2;
      uniform sampler2D _GrabTexture;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
          float4 color :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
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
          u_xlat0 = UnityObjectToClipPos(in_v.vertex);
          u_xlat1.xyz = (u_xlat0.xyw * float3(0.5, 0.5, 0.5));
          u_xlat1.xy = (u_xlat1.zz + u_xlat1.xy);
          out_v.texcoord1.xy = (u_xlat1.xy / u_xlat0.ww);
          out_v.vertex = u_xlat0;
          out_v.texcoord.xy = in_v.texcoord.xy;
          out_v.color = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float u_xlat16_0;
      float4 u_xlat10_0;
      float2 u_xlat1_d;
      float4 u_xlat10_1;
      float4 u_xlat2;
      float u_xlat10_2;
      float3 u_xlat3;
      float u_xlat16_4;
      float3 u_xlat6;
      float u_xlat10_8;
      float u_xlat16_12;
      float u_xlat10_12;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Time.x * _Speed);
          u_xlat1_d.x = ((in_f.texcoord.x * 0.166666672) + u_xlat0_d.x);
          u_xlat0_d.xyz = (u_xlat0_d.xxx * float3(6, 8, 4));
          u_xlat1_d.y = ((in_f.texcoord.y * 0.166666672) + u_xlat0_d.z);
          u_xlat10_8 = tex2D(_MainTex2, u_xlat1_d.xy).x;
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat0_d.xy = ((u_xlat10_1.ww * u_xlat10_1.xx) + u_xlat0_d.xy);
          u_xlat2.y = (u_xlat0_d.x + in_f.texcoord.y);
          u_xlat2.w = (u_xlat0_d.x + u_xlat2.y);
          u_xlat0_d.y = (u_xlat0_d.y + u_xlat2.w);
          u_xlat2.xz = in_f.texcoord.xx;
          u_xlat10_12 = tex2D(_MainTex2, u_xlat2.zw).y;
          u_xlat10_2 = tex2D(_MainTex2, u_xlat2.xy).x;
          u_xlat16_12 = (u_xlat10_12 + u_xlat10_2);
          u_xlat0_d.x = in_f.texcoord.x;
          u_xlat10_0.x = tex2D(_MainTex2, u_xlat0_d.xy).z;
          u_xlat16_0 = (u_xlat10_0.x + u_xlat16_12);
          u_xlat16_4 = ((u_xlat16_0 * 0.333333343) + u_xlat10_8);
          u_xlat0_d.x = (u_xlat16_0 * _Distortion);
          u_xlat0_d.xz = (((-u_xlat0_d.xx) * float2(0.00520833349, 0.00520833349)) + in_f.texcoord1.xy);
          u_xlat0_d.xz = (((-float2(u_xlat16_4, u_xlat16_4)) * float2(0.015625, 0.015625)) + u_xlat0_d.xz);
          u_xlat0_d.xz = (u_xlat0_d.xz + (-in_f.texcoord1.xy));
          u_xlat0_d.xz = ((float2(float2(_Alpha, _Alpha)) * u_xlat0_d.xz) + in_f.texcoord1.xy);
          u_xlat10_0.xzw = tex2D(_GrabTexture, u_xlat0_d.xz).xyz;
          u_xlat2.x = (u_xlat16_4 * Light);
          u_xlat16_4 = max(u_xlat16_4, 0.600000024);
          u_xlat16_4 = min(u_xlat16_4, 1);
          u_xlat16_4 = (u_xlat16_4 + (-0.600000024));
          u_xlat6.xyz = (u_xlat10_1.xyz * float3(Light, Light, Light));
          u_xlat6.xyz = (u_xlat6.xyz * Lightcolor.xyz);
          u_xlat2.xyz = ((u_xlat2.xxx * Lightcolor.xyz) + u_xlat6.xyz);
          u_xlat2.xyz = (float3(u_xlat16_4, u_xlat16_4, u_xlat16_4) + u_xlat2.xyz);
          u_xlat3.xyz = (Lightcolor.xyz * Lightcolor.www);
          u_xlat0_d.xyz = ((u_xlat10_0.xzw * in_f.color.xyz) + u_xlat3.xyz);
          u_xlat0_d.xyz = ((u_xlat2.xyz * float3(float3(EValue, EValue, EValue))) + u_xlat0_d.xyz);
          out_f.color.xyz = ((u_xlat10_1.xxx * float3(TValue, TValue, TValue)) + u_xlat0_d.xyz);
          out_f.color.w = (u_xlat10_1.w * _Alpha);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
