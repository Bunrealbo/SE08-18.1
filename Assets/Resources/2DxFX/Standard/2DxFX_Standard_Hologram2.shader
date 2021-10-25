Shader "2DxFX/Standard/Hologram2"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Size ("Size", Range(0, 1)) = 0
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
      float2 u_xlat0_d;
      float4 u_xlat1_d;
      float4 u_xlat2;
      float2 u_xlat3;
      float4 u_xlat4;
      float2 u_xlat5;
      float u_xlat16_5;
      float u_xlat10_5;
      float2 u_xlatb5;
      float3 u_xlat7;
      float u_xlat10;
      float u_xlat11;
      int u_xlatb11;
      float u_xlat16_15;
      float u_xlat10_15;
      float u_xlat16;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Time.y * 0.629999995);
          u_xlat0_d.x = sin(u_xlat0_d.x);
          u_xlat0_d.x = (u_xlat0_d.x + _Time.y);
          u_xlat0_d.x = sin(u_xlat0_d.x);
          u_xlat1_d = (_Time.yyyy * float4(20, 1.5, 5, 0.5));
          u_xlat5.x = ((in_f.texcoord.y * 4) + u_xlat1_d.w);
          u_xlat0_d.x = (u_xlat0_d.x + u_xlat5.x);
          u_xlat5.x = floor(u_xlat0_d.x);
          u_xlat0_d.x = ((-u_xlat5.x) + u_xlat0_d.x);
          u_xlatb5.xy = bool4(u_xlat0_d.xxxx >= float4(0.5, 0.600000024, 0, 0)).xy;
          u_xlat0_d.x = (u_xlat0_d.x + (-0.5));
          u_xlat5.x = (u_xlatb5.x)?(1):(float(0));
          u_xlat10 = (u_xlatb5.y)?((-1)):((-0));
          u_xlat5.x = (u_xlat10 + u_xlat5.x);
          u_xlat0_d.x = (u_xlat5.x * u_xlat0_d.x);
          u_xlat0_d.x = (((-u_xlat0_d.x) * 9.99999809) + 1);
          u_xlat0_d.x = (u_xlat5.x * u_xlat0_d.x);
          u_xlat5.xy = ((in_f.texcoord.xy * float2(0.5, 1)) + float2(6, 3));
          u_xlat2.x = _Time.y;
          u_xlat3.x = sin(_Time.y);
          u_xlat4.x = cos(_Time.y);
          u_xlat2.y = (u_xlat4.x * _Time.y);
          u_xlat5.xy = ((u_xlat2.xy * float2(1.60000002, 1.60000002)) + u_xlat5.xy);
          u_xlat2.xy = (u_xlat2.xy * float2(1.60000002, 1.60000002));
          u_xlat2.xy = ((in_f.texcoord.xy * float2(2, 2)) + u_xlat2.xy);
          u_xlat10_15 = tex2D(_MainTex, u_xlat2.xy).x;
          u_xlat16_15 = (u_xlat10_15 * u_xlat10_15);
          u_xlat10_5 = tex2D(_MainTex, u_xlat5.xy).x;
          u_xlat16_5 = (u_xlat10_5 * u_xlat10_5);
          u_xlat5.x = (u_xlat16_5 * _Distortion);
          u_xlat5.x = (u_xlat5.x * 3);
          u_xlat0_d.x = (u_xlat5.x * u_xlat0_d.x);
          u_xlat5.x = sin(u_xlat1_d.x);
          u_xlat1_d.xy = cos(u_xlat1_d.yz);
          u_xlat10 = (u_xlat5.x * 0.100000001);
          u_xlat10 = (u_xlat4.x * u_xlat10);
          u_xlat5.x = ((u_xlat3.x * u_xlat5.x) + u_xlat10);
          u_xlat5.x = (u_xlat5.x + 0.5);
          u_xlat2 = (_Time.yyyy * float4(0.25, 4, 80, 3));
          u_xlat7.xyz = cos(u_xlat2.yzw);
          u_xlat10 = floor(u_xlat2.x);
          u_xlat10 = ((_Time.y * 0.25) + (-u_xlat10));
          u_xlat10 = ((-u_xlat10) + in_f.texcoord.y);
          u_xlat10 = (u_xlat10 * u_xlat10);
          u_xlat10 = ((u_xlat10 * 20) + 1);
          u_xlat10 = (float(1) / u_xlat10);
          u_xlat11 = ((u_xlat7.z * 2) + _Time.y);
          u_xlat11 = sin(u_xlat11);
          u_xlatb11 = (u_xlat11>=0.899999976);
          u_xlat11 = (u_xlatb11)?(0.400000006):(float(0));
          u_xlat5.x = ((u_xlat11 * u_xlat5.x) + in_f.texcoord.y);
          u_xlat11 = floor(u_xlat5.x);
          u_xlat3.y = (u_xlat5.x + (-u_xlat11));
          u_xlat5.x = ((u_xlat7.x * 4) + _Time.y);
          u_xlat11 = (u_xlat7.y + 1);
          u_xlat5.x = sin(u_xlat5.x);
          u_xlatb5.x = (u_xlat5.x>=0.300000012);
          u_xlat5.x = (u_xlatb5.x)?(1):(float(0));
          u_xlat16 = (_Distortion * 50);
          u_xlat2.x = ((in_f.texcoord.y * 30) + _Time.y);
          u_xlat7.x = sin(u_xlat2.x);
          u_xlat16 = (u_xlat7.x / u_xlat16);
          u_xlat5.x = (u_xlat5.x * u_xlat16);
          u_xlat5.x = (u_xlat11 * u_xlat5.x);
          u_xlat3.x = ((u_xlat5.x * u_xlat10) + in_f.texcoord.x);
          u_xlat5.x = ((u_xlat1_d.x * 2) + _Time.y);
          u_xlat5.y = ((u_xlat1_d.y * 5) + _Time.y);
          u_xlat5.xy = sin(u_xlat5.xy);
          u_xlat10 = ((u_xlat5.y * 0.300000012) + 3);
          u_xlatb5.x = (u_xlat5.x>=0.899999976);
          u_xlat1_d.xy = (u_xlatb5.x)?(float2(-0.0500000007, (-0))):(float2(0, (-0)));
          u_xlat1_d.zw = lerp(float2(0, 0), float2(0.0500000007, 0), float2(u_xlatb5.xx));
          u_xlat1_d = (u_xlat1_d + u_xlat3.xyxy);
          u_xlat4.z = tex2D(_MainTex, u_xlat1_d.zw).z;
          u_xlat4.yw = tex2D(_MainTex, u_xlat3.xy).yw;
          u_xlat4.x = tex2D(_MainTex, u_xlat1_d.xy).x;
          u_xlat1_d = ((u_xlat4 * in_f.color) + u_xlat0_d.xxxx);
          u_xlat1_d = ((float4(u_xlat16_15, u_xlat16_15, u_xlat16_15, u_xlat16_15) * float4(0.5, 0.5, 0.5, 0.5)) + u_xlat1_d);
          u_xlat0_d.xy = (in_f.texcoord.yx + float2(-0.5, (-0.5)));
          u_xlat0_d.xy = (u_xlat0_d.xy * u_xlat0_d.xy);
          u_xlat0_d.xy = (((-u_xlat0_d.xy) * float2(u_xlat10, u_xlat10)) + float2(1, 1));
          u_xlat0_d.x = (u_xlat0_d.y * u_xlat0_d.x);
          u_xlat1_d.x = (u_xlat0_d.x * u_xlat1_d.x);
          u_xlat5.xy = (in_f.texcoord.xy * _Time.yy);
          u_xlat5.x = dot(u_xlat5.xy, float2(12.9898005, 78.2330017));
          u_xlat5.x = sin(u_xlat5.x);
          u_xlat5.x = (u_xlat5.x * 43758.5469);
          u_xlat5.x = frac(u_xlat5.x);
          u_xlat1_d.w = ((u_xlat5.x * 0.5) + u_xlat1_d.w);
          u_xlat5.x = floor(u_xlat2.x);
          u_xlat5.x = ((-u_xlat5.x) + u_xlat2.x);
          u_xlat5.x = (u_xlat5.x + 12);
          u_xlat5.x = (u_xlat5.x * 0.0769230798);
          u_xlat1_d = (u_xlat5.xxxx * u_xlat1_d);
          u_xlat10_5 = tex2D(_MainTex, in_f.texcoord.xy).w;
          u_xlat5.x = (u_xlat10_5 * u_xlat1_d.w);
          out_f.color.xyz = u_xlat1_d.xyz;
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat5.x);
          u_xlat0_d.x = (u_xlat0_d.x * 0.600000024);
          u_xlat5.x = ((-_Alpha) + 1);
          u_xlat0_d.x = (u_xlat5.x * u_xlat0_d.x);
          out_f.color.w = (u_xlat0_d.x * in_f.color.w);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
