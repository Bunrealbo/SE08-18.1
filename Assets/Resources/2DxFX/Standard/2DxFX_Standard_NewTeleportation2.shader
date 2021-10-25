Shader "2DxFX/Standard/NewTeleportation2"
{
  Properties
  {
    [HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
    [HideInInspector] _MainTex2 ("Pattern (RGB)", 2D) = "white" {}
    [HideInInspector] _Alpha ("Alpha", Range(0, 1)) = 1
    [HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
    [HideInInspector] _Distortion ("_Distortion", Range(0, 1)) = 0
    [HideInInspector] _Value2 ("_Value2", Range(0, 1)) = 0
    [HideInInspector] _Value3 ("_Value3", Range(0, 1)) = 0
    [HideInInspector] _Value4 ("_Value4", Range(0, 1)) = 0
    [HideInInspector] _Value5 ("_Value5", Range(0, 1)) = 0
    [HideInInspector] TeleportationColor ("Teleportation Color", Color) = (1,1,1,1)
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
      uniform float _Alpha;
      uniform float _Distortion;
      uniform float4 TeleportationColor;
      uniform float _HDR_Intensity;
      uniform float _Fade;
      uniform sampler2D _MainTex;
      uniform sampler2D _MainTex2;
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
      float2 u_xlat0_d;
      float u_xlat10_0;
      float4 u_xlat1_d;
      float4 u_xlat16_1;
      float4 u_xlat2;
      float3 u_xlat3;
      float4 u_xlat10_3;
      float3 u_xlat4;
      float u_xlat10_4;
      float u_xlat8;
      float2 u_xlat9;
      float u_xlat12;
      float u_xlat10_12;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = 0;
          u_xlat1_d = (float4(_Fade, _HDR_Intensity, _Fade, _Fade) + float4(-0.300000012, (-1), (-0.100000001), (-0.349999994)));
          u_xlat2.xyz = (u_xlat1_d.xzw * float3(1.42857146, 4, 1.05263162));
          u_xlat2.xyz = clamp(u_xlat2.xyz, 0, 1);
          u_xlat3.xyz = ((u_xlat2.xyz * float3(-2, (-2), (-2))) + float3(3, 3, 3));
          u_xlat2.xyz = (u_xlat2.xyz * u_xlat2.xyz);
          u_xlat2.xyw = (u_xlat2.xyz * u_xlat3.xyz);
          u_xlat0_d.y = (u_xlat2.x * 0.400000006);
          u_xlat0_d.xy = ((-u_xlat0_d.xy) + in_f.texcoord.xy);
          u_xlat10_0 = tex2D(_MainTex2, u_xlat0_d.xy).x;
          u_xlat4.xy = ((float2(u_xlat10_0, u_xlat10_0) * float2(0.300000012, 0.200000003)) + in_f.texcoord.xy);
          u_xlat10_4 = tex2D(_MainTex2, u_xlat4.xy).z;
          u_xlat4.x = (((-u_xlat3.z) * u_xlat2.z) + u_xlat10_4);
          u_xlat4.x = (u_xlat4.x * 100);
          u_xlat4.x = clamp(u_xlat4.x, 0, 1);
          u_xlat8 = ((u_xlat4.x * (-2)) + 3);
          u_xlat4.x = (u_xlat4.x * u_xlat4.x);
          u_xlat12 = (u_xlat4.x * u_xlat8);
          u_xlat1_d.x = (u_xlat2.w + u_xlat2.w);
          u_xlat12 = ((u_xlat10_0 * u_xlat12) + (-u_xlat1_d.x));
          u_xlat0_d.x = (u_xlat10_0 * _Distortion);
          u_xlat3.x = ((u_xlat0_d.x * 0.0500000007) + in_f.texcoord.x);
          u_xlat0_d.x = (u_xlat12 + 1);
          u_xlat3.y = _Fade;
          u_xlat1_d.xw = ((-u_xlat3.xy) + in_f.texcoord.xy);
          u_xlat10_12 = tex2D(_MainTex2, u_xlat1_d.xw).y;
          u_xlat16_1.xw = (float2(u_xlat10_12, u_xlat10_12) * float2(8, 16));
          u_xlat4.x = ((u_xlat8 * u_xlat4.x) + u_xlat16_1.w);
          u_xlat10_3 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat4.x = (u_xlat4.x * u_xlat10_3.w);
          u_xlat0_d.x = ((u_xlat0_d.x * u_xlat4.x) + (-u_xlat10_3.w));
          u_xlat4.x = (u_xlat1_d.z + u_xlat1_d.z);
          u_xlat4.x = clamp(u_xlat4.x, 0, 1);
          u_xlat8 = ((u_xlat4.x * (-2)) + 3);
          u_xlat4.x = (u_xlat4.x * u_xlat4.x);
          u_xlat4.x = (u_xlat4.x * u_xlat8);
          u_xlat0_d.x = ((u_xlat4.x * u_xlat0_d.x) + u_xlat10_3.w);
          u_xlat4.xy = float2((float2(float2(_Fade, _Fade)) + float2(-0.119999997, (-0.899999976))));
          u_xlat4.xy = (u_xlat4.xy * float2(0.847457647, 9.99999809));
          u_xlat4.xy = clamp(u_xlat4.xy, 0, 1);
          u_xlat9.xy = ((u_xlat4.xy * float2(-2, (-2))) + float2(3, 3));
          u_xlat4.xy = (u_xlat4.xy * u_xlat4.xy);
          u_xlat4.xy = (u_xlat4.xy * u_xlat9.xy);
          u_xlat0_d.x = ((u_xlat4.y * (-u_xlat0_d.x)) + u_xlat0_d.x);
          u_xlat0_d.x = clamp(u_xlat0_d.x, 0, 1);
          u_xlat4.x = (u_xlat4.x * u_xlat16_1.x);
          u_xlat4.x = clamp(u_xlat4.x, 0, 1);
          u_xlat8 = ((-_Alpha) + 1);
          out_f.color.w = (u_xlat8 * u_xlat0_d.x);
          u_xlat0_d.x = (_Fade * 5);
          u_xlat0_d.x = clamp(u_xlat0_d.x, 0, 1);
          u_xlat8 = ((u_xlat0_d.x * (-2)) + 3);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat8);
          u_xlat1_d.xzw = ((u_xlat0_d.xxx * TeleportationColor.xyz) + u_xlat10_3.xyz);
          u_xlat0_d.x = ((u_xlat0_d.x * u_xlat1_d.y) + 1);
          u_xlat2.xzw = ((TeleportationColor.xyz * TeleportationColor.xyz) + TeleportationColor.xyz);
          u_xlat2.xzw = ((-u_xlat1_d.xzw) + u_xlat2.xzw);
          u_xlat1_d.xyz = ((u_xlat2.yyy * u_xlat2.xzw) + u_xlat1_d.xzw);
          u_xlat2.xyz = ((-u_xlat1_d.xyz) + float3(1.64999998, 1.64999998, 1.64999998));
          u_xlat4.xyz = ((u_xlat4.xxx * u_xlat2.xyz) + u_xlat1_d.xyz);
          out_f.color.xyz = (u_xlat0_d.xxx * u_xlat4.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
