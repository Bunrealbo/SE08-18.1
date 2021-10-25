Shader "2DxFX/Standard/Hologram3"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Size ("Size", Range(0, 1)) = 0
    _Distortion ("Distortion", Range(0, 1)) = 0
    _Alpha ("Alpha", Range(0, 1)) = 1
    _Color ("_Color", Color) = (1,1,1,1)
    _ColorX ("_ColorX", Color) = (1,1,1,1)
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
      uniform float _Distortion;
      uniform float _Alpha;
      uniform float4 _ColorX;
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
      float4 u_xlatb0;
      float2 u_xlat1_d;
      float4 u_xlat2;
      int u_xlatb2;
      float2 u_xlat3;
      float4 u_xlat4;
      float u_xlat5;
      float2 u_xlat6;
      float u_xlat16_7;
      float u_xlat10_7;
      float2 u_xlat8;
      float2 u_xlat12;
      float u_xlat16_12;
      float u_xlat10_12;
      float u_xlat13;
      float2 u_xlat14;
      float u_xlat18;
      int u_xlatb18;
      float u_xlat19;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = floor(_Time.y);
          u_xlat0_d.x = ((-u_xlat0_d.x) + _Time.y);
          u_xlat6.xy = (in_f.texcoord.xy + float2(-0.5, (-0.5)));
          u_xlat1_d.xy = (u_xlat6.xy * u_xlat6.xy);
          u_xlat6.xy = (u_xlat6.yx * float2(4.19999981, 4.19999981));
          u_xlat18 = dot(u_xlat1_d.yy, u_xlat1_d.xx);
          u_xlat18 = (u_xlat18 + 0.238095239);
          u_xlat1_d.xy = ((u_xlat6.yx * float2(u_xlat18, u_xlat18)) + float2(0.5, 0.5));
          u_xlat6.xy = (float2(u_xlat18, u_xlat18) * u_xlat6.xy);
          u_xlat6.xy = (u_xlat6.xy * u_xlat6.xy);
          u_xlat0_d.x = ((-u_xlat0_d.x) + u_xlat1_d.y);
          u_xlat0_d.x = (u_xlat0_d.x * 20);
          u_xlat2 = (_Time.yyyy * float4(0.100000001, 4, 80, 3));
          u_xlat18 = floor(u_xlat2.x);
          u_xlat2.xyz = cos(u_xlat2.yzw);
          u_xlat18 = ((_Time.y * 0.100000001) + (-u_xlat18));
          u_xlat18 = ((-u_xlat18) + u_xlat1_d.y);
          u_xlat0_d.x = ((u_xlat0_d.x * u_xlat18) + 1);
          u_xlat0_d.x = (4 / u_xlat0_d.x);
          u_xlat18 = (u_xlat2.x + _Time.y);
          u_xlat18 = sin(u_xlat18);
          u_xlatb18 = (u_xlat18>=0.300000012);
          u_xlat18 = (u_xlatb18)?(1):(float(0));
          u_xlat13 = (_Distortion * 50);
          u_xlat19 = ((u_xlat1_d.y * 30) + _Time.y);
          u_xlat2.x = sin(u_xlat19);
          u_xlat13 = (u_xlat2.x / u_xlat13);
          u_xlat18 = (u_xlat18 * u_xlat13);
          u_xlat13 = (u_xlat2.y + 1);
          u_xlat2.x = ((u_xlat2.z * 2) + _Time.y);
          u_xlat2.x = sin(u_xlat2.x);
          u_xlatb2 = (u_xlat2.x>=0.899999976);
          u_xlat2.x = (u_xlatb2)?(0.400000006):(float(0));
          u_xlat18 = (u_xlat18 * u_xlat13);
          u_xlat3.x = ((u_xlat18 * u_xlat0_d.x) + u_xlat1_d.x);
          u_xlat4 = (_Time.yyyy * float4(200, 20, 1.5, 5));
          u_xlat0_d.xw = cos(u_xlat4.zw);
          u_xlat8.xy = sin(u_xlat4.xy);
          u_xlat0_d.x = ((u_xlat0_d.x * 2) + _Time.y);
          u_xlat0_d.w = ((u_xlat0_d.w * 5) + _Time.y);
          u_xlat0_d.xw = sin(u_xlat0_d.xw);
          u_xlat18 = ((u_xlat0_d.w * 0.300000012) + 3);
          u_xlat6.xy = (((-u_xlat6.xy) * float2(u_xlat18, u_xlat18)) + float2(1, 1));
          u_xlat6.x = (u_xlat6.y * u_xlat6.x);
          u_xlatb0.x = (u_xlat0_d.x>=0.899999976);
          u_xlat0_d.xz = (u_xlatb0.x)?(float2(-0.0500000007, (-0))):(float2(0, (-0)));
          u_xlat18 = (u_xlat8.y * 0.100000001);
          u_xlat4.x = sin(_Time.y);
          u_xlat5 = cos(_Time.y);
          u_xlat18 = (u_xlat18 * u_xlat5);
          u_xlat18 = ((u_xlat4.x * u_xlat8.x) + u_xlat18);
          u_xlat4.y = (u_xlat5 * _Time.y);
          u_xlat18 = (u_xlat18 + 0.5);
          u_xlat18 = ((u_xlat2.x * u_xlat18) + u_xlat1_d.y);
          u_xlat13 = floor(u_xlat18);
          u_xlat3.y = (u_xlat18 + (-u_xlat13));
          u_xlat0_d.xz = (u_xlat0_d.xz + u_xlat3.xy);
          u_xlat2.y = tex2D(_MainTex, u_xlat3.xy).w;
          u_xlat2.x = tex2D(_MainTex, u_xlat0_d.xz).x;
          u_xlat0_d.xz = (_Time.yy * float2(0.5, 0.629999995));
          u_xlat12.x = sin(u_xlat0_d.z);
          u_xlat0_d.x = ((u_xlat1_d.y * 4) + u_xlat0_d.x);
          u_xlat12.x = (u_xlat12.x + _Time.y);
          u_xlat12.x = sin(u_xlat12.x);
          u_xlat0_d.x = (u_xlat12.x + u_xlat0_d.x);
          u_xlat12.x = floor(u_xlat0_d.x);
          u_xlat0_d.x = ((-u_xlat12.x) + u_xlat0_d.x);
          u_xlat12.x = (u_xlat0_d.x + (-0.5));
          u_xlatb0.xw = bool4(u_xlat0_d.xxxx >= float4(0.5, 0, 0, 0.600000024)).xw;
          u_xlat0_d.x = (u_xlatb0.x)?(1):(float(0));
          u_xlat18 = (u_xlatb0.w)?((-1)):((-0));
          u_xlat0_d.x = (u_xlat18 + u_xlat0_d.x);
          u_xlat12.x = (u_xlat0_d.x * u_xlat12.x);
          u_xlat12.x = (((-u_xlat12.x) * 9.99999809) + 1);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat12.x);
          u_xlat12.xy = ((u_xlat1_d.xy * float2(0.5, 1)) + float2(6, 3));
          u_xlat4.x = _Time.y;
          u_xlat12.xy = ((u_xlat4.xy * float2(1.60000002, 1.60000002)) + u_xlat12.xy);
          u_xlat14.xy = (u_xlat4.xy * float2(1.60000002, 1.60000002));
          u_xlat14.xy = ((u_xlat1_d.xy * float2(2, 2)) + u_xlat14.xy);
          u_xlat1_d.xy = (u_xlat1_d.xy * _Time.yy);
          u_xlat1_d.x = dot(u_xlat1_d.xy, float2(12.9898005, 78.2330017));
          u_xlat1_d.x = sin(u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.x * 43758.5469);
          u_xlat1_d.x = frac(u_xlat1_d.x);
          u_xlat10_7 = tex2D(_MainTex, u_xlat14.xy).x;
          u_xlat16_7 = (u_xlat10_7 * u_xlat10_7);
          u_xlat10_12 = tex2D(_MainTex, u_xlat12.xy).x;
          u_xlat16_12 = (u_xlat10_12 * u_xlat10_12);
          u_xlat12.x = (u_xlat16_12 * _Distortion);
          u_xlat12.x = (u_xlat12.x * 3);
          u_xlat0_d.x = (u_xlat12.x * u_xlat0_d.x);
          u_xlat0_d.xz = ((u_xlat2.xy * in_f.color.xw) + u_xlat0_d.xx);
          u_xlat2.xw = ((float2(u_xlat16_7, u_xlat16_7) * float2(0.5, 0.5)) + u_xlat0_d.xz);
          u_xlat0_d.x = (u_xlat6.x * u_xlat2.x);
          u_xlat2.xyz = (u_xlat0_d.xxx * _ColorX.xyz);
          u_xlat0_d.x = floor(u_xlat19);
          u_xlat0_d.x = ((-u_xlat0_d.x) + u_xlat19);
          u_xlat0_d.x = (u_xlat0_d.x + 12);
          u_xlat0_d.x = (u_xlat0_d.x * 0.0769230798);
          u_xlat2 = (u_xlat0_d.xxxx * u_xlat2);
          u_xlat0_d.x = ((u_xlat1_d.x * 0.5) + u_xlat2.w);
          out_f.color.xyz = u_xlat2.xyz;
          u_xlat10_12 = tex2D(_MainTex, in_f.texcoord.xy).w;
          u_xlat0_d.x = (u_xlat10_12 * u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat6.x * u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x * 1.60000002);
          u_xlat6.x = ((-_Alpha) + 1);
          u_xlat0_d.x = (u_xlat6.x * u_xlat0_d.x);
          out_f.color.w = (u_xlat0_d.x * in_f.color.w);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
