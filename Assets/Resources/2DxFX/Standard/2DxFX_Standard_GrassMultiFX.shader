Shader "2DxFX/Standard/GrassMultiFX"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Distortion ("Distortion", Range(0, 1)) = 0
    _Wind ("_Wind", Range(0, 10)) = 1
    _Wind2 ("_Wind2", Range(0, 10)) = 1
    _Wind3 ("_Wind2", Range(0, 10)) = 1
    _Wind4 ("_Wind2", Range(0, 10)) = 1
    _Alpha ("Alpha", Range(0, 1)) = 1
    _Speed ("Speed", Range(0, 1)) = 1
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
      uniform float _Wind;
      uniform float _Wind2;
      uniform float _Wind3;
      uniform float _Wind4;
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
      float4 u_xlat10_0;
      float4 u_xlat1_d;
      float4 u_xlat10_1;
      float4 u_xlat2;
      float4 u_xlat10_2;
      float u_xlat3;
      int u_xlatb4;
      float3 u_xlat5;
      int u_xlatb5;
      float2 u_xlatb9;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (in_f.texcoord.y / _Distortion);
          u_xlat0_d.x = ((u_xlat0_d.x * _Wind) + in_f.texcoord.x);
          u_xlatb4 = (abs(u_xlat0_d.x)>=(-abs(u_xlat0_d.x)));
          u_xlat0_d.x = frac(abs(u_xlat0_d.x));
          u_xlat0_d.x = (u_xlatb4)?(u_xlat0_d.x):((-u_xlat0_d.x));
          u_xlat0_d.y = in_f.texcoord.y;
          u_xlat10_0 = tex2D(_MainTex, u_xlat0_d.xy);
          u_xlat0_d = ((u_xlat10_0 * in_f.color) + float4(-0.194999993, (-0.194999993), (-0.194999993), (-0.00999999978)));
          u_xlat1_d = (in_f.texcoord.xyxy + float4(0.200000003, 0.00999999978, 0.400000006, 0.0199999996));
          u_xlat5.xz = (u_xlat1_d.yw / float2(_Distortion, _Distortion));
          u_xlat1_d.xy = ((u_xlat5.xz * float2(_Wind2, _Wind3)) + u_xlat1_d.xz);
          u_xlatb9.xy = bool4(abs(u_xlat1_d.xyxy) >= (-abs(u_xlat1_d.xyxy))).xy;
          u_xlat1_d.xy = frac(abs(u_xlat1_d.xy));
          float4 hlslcc_movcTemp = u_xlat1_d;
          hlslcc_movcTemp.x = (u_xlatb9.x)?(u_xlat1_d.x):((-u_xlat1_d.x));
          hlslcc_movcTemp.y = (u_xlatb9.y)?(u_xlat1_d.y):((-u_xlat1_d.y));
          u_xlat1_d = hlslcc_movcTemp;
          u_xlat1_d.zw = (in_f.texcoord.yy + float2(0.00999999978, 0.0199999996));
          u_xlat10_2 = tex2D(_MainTex, u_xlat1_d.xz);
          u_xlat10_1 = tex2D(_MainTex, u_xlat1_d.yw);
          u_xlat1_d = ((u_xlat10_1 * in_f.color) + float4(-0.0649999976, (-0.0649999976), (-0.0649999976), (-0.00999999978)));
          u_xlat2 = ((u_xlat10_2 * in_f.color) + float4(-0.129999995, (-0.129999995), (-0.129999995), (-0.00999999978)));
          u_xlat3 = ((-u_xlat2.w) + 1);
          u_xlat2 = (u_xlat2.wwww * u_xlat2);
          u_xlat0_d = ((u_xlat0_d * float4(u_xlat3, u_xlat3, u_xlat3, u_xlat3)) + u_xlat2);
          u_xlat2.x = ((-u_xlat1_d.w) + 1);
          u_xlat1_d = (u_xlat1_d.wwww * u_xlat1_d);
          u_xlat0_d = ((u_xlat0_d * u_xlat2.xxxx) + u_xlat1_d);
          u_xlat1_d.xyw = (in_f.texcoord.xyy + float3(0.600000024, 0.0299999993, 0.0299999993));
          u_xlat5.x = (u_xlat1_d.y / _Distortion);
          u_xlat1_d.x = ((u_xlat5.x * _Wind4) + u_xlat1_d.x);
          u_xlatb5 = (abs(u_xlat1_d.x)>=(-abs(u_xlat1_d.x)));
          u_xlat1_d.x = frac(abs(u_xlat1_d.x));
          u_xlat1_d.z = (u_xlatb5)?(u_xlat1_d.x):((-u_xlat1_d.x));
          u_xlat10_1 = tex2D(_MainTex, u_xlat1_d.zw);
          u_xlat2 = ((u_xlat10_1 * in_f.color) + float4(0, 0, 0, (-0.00999999978)));
          u_xlat1_d.x = ((u_xlat10_1.w * in_f.color.w) + (-0.00999999978));
          u_xlat2 = (u_xlat1_d.xxxx * u_xlat2);
          u_xlat1_d.x = ((-u_xlat1_d.x) + 1);
          u_xlat0_d = ((u_xlat0_d * u_xlat1_d.xxxx) + u_xlat2);
          out_f.color.w = (u_xlat0_d.w + (-_Alpha));
          out_f.color.xyz = u_xlat0_d.xyz;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
