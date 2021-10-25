Shader "2DxFX/Standard/Mystic_Distortion_Additive"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Pitch ("Pitch", Range(0, 0.5)) = 0
    _OffsetX ("OffsetX", Range(0, 128)) = 0
    _OffsetY ("OffsetY", Range(0, 128)) = 0
    _DistanceX ("DistanceX", Range(0, 1)) = 0
    _DistanceY ("DistanceY", Range(0, 1)) = 0
    _WaveTimeX ("WaveTimeX", Range(0, 360)) = 0
    _WaveTimeY ("WaveTimeY", Range(0, 360)) = 0
    _Color ("Tint", Color) = (1,1,1,1)
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
      uniform float _OffsetX;
      uniform float _OffsetY;
      uniform float _Pitch;
      uniform float _DistanceX;
      uniform float _DistanceY;
      uniform float _WaveTimeX;
      uniform float _WaveTimeY;
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
      float3 u_xlat0_d;
      float4 u_xlat10_0;
      int u_xlatb0;
      float2 u_xlat1_d;
      float u_xlat2;
      int u_xlatb2;
      float u_xlat3;
      int u_xlatb3;
      float2 u_xlat4;
      float u_xlat5;
      float u_xlat6;
      int u_xlatb7;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Pitch + (-0.5));
          u_xlat0_d.xy = (u_xlat0_d.xx * float2(4.44289351, (-22.214468)));
          u_xlat4.x = max(abs(u_xlat0_d.y), 1);
          u_xlat4.x = (float(1) / u_xlat4.x);
          u_xlat6 = min(abs(u_xlat0_d.y), 1);
          u_xlat4.x = (u_xlat4.x * u_xlat6);
          u_xlat6 = (u_xlat4.x * u_xlat4.x);
          u_xlat1_d.x = ((u_xlat6 * 0.0208350997) + (-0.0851330012));
          u_xlat1_d.x = ((u_xlat6 * u_xlat1_d.x) + 0.180141002);
          u_xlat1_d.x = ((u_xlat6 * u_xlat1_d.x) + (-0.330299497));
          u_xlat6 = ((u_xlat6 * u_xlat1_d.x) + 0.999866009);
          u_xlat1_d.x = (u_xlat6 * u_xlat4.x);
          u_xlat1_d.x = ((u_xlat1_d.x * (-2)) + 1.57079637);
          u_xlatb3 = (1<abs(u_xlat0_d.y));
          u_xlat1_d.x = (u_xlatb3)?(u_xlat1_d.x):(float(0));
          u_xlat4.x = ((u_xlat4.x * u_xlat6) + u_xlat1_d.x);
          u_xlat2 = min(u_xlat0_d.y, 1);
          u_xlatb2 = (u_xlat2<(-u_xlat2));
          u_xlat2 = (u_xlatb2)?((-u_xlat4.x)):(u_xlat4.x);
          u_xlat4.x = ((in_f.texcoord.y * _OffsetX) + _WaveTimeX);
          u_xlat4.x = sin(u_xlat4.x);
          u_xlat1_d.x = ((u_xlat4.x * _DistanceX) + in_f.texcoord.x);
          u_xlat4.x = ((u_xlat1_d.x * _OffsetY) + _WaveTimeY);
          u_xlat4.x = cos(u_xlat4.x);
          u_xlat1_d.y = ((u_xlat4.x * _DistanceY) + in_f.texcoord.y);
          u_xlat4.xy = (u_xlat1_d.xy + float2(-0.5, (-0.5)));
          u_xlat3 = length(u_xlat4.xy);
          u_xlat1_d.x = rsqrt(u_xlat1_d.x);
          u_xlat4.xy = (u_xlat4.xy * u_xlat1_d.xx);
          u_xlat0_d.x = ((-u_xlat0_d.x) * u_xlat3);
          u_xlat0_d.x = (u_xlat0_d.x * 10);
          u_xlat1_d.x = max(abs(u_xlat0_d.x), 1);
          u_xlat1_d.x = (float(1) / u_xlat1_d.x);
          u_xlat3 = min(abs(u_xlat0_d.x), 1);
          u_xlat1_d.x = (u_xlat1_d.x * u_xlat3);
          u_xlat3 = (u_xlat1_d.x * u_xlat1_d.x);
          u_xlat5 = ((u_xlat3 * 0.0208350997) + (-0.0851330012));
          u_xlat5 = ((u_xlat3 * u_xlat5) + 0.180141002);
          u_xlat5 = ((u_xlat3 * u_xlat5) + (-0.330299497));
          u_xlat3 = ((u_xlat3 * u_xlat5) + 0.999866009);
          u_xlat5 = (u_xlat3 * u_xlat1_d.x);
          u_xlat5 = ((u_xlat5 * (-2)) + 1.57079637);
          u_xlatb7 = (1<abs(u_xlat0_d.x));
          u_xlat0_d.x = min(u_xlat0_d.x, 1);
          u_xlatb0 = (u_xlat0_d.x<(-u_xlat0_d.x));
          u_xlat5 = (u_xlatb7)?(u_xlat5):(float(0));
          u_xlat1_d.x = ((u_xlat1_d.x * u_xlat3) + u_xlat5);
          u_xlat0_d.x = (u_xlatb0)?((-u_xlat1_d.x)):(u_xlat1_d.x);
          u_xlat0_d.xz = (u_xlat0_d.xx * u_xlat4.xy);
          u_xlat0_d.xz = (u_xlat0_d.xz * float2(0.5, 0.5));
          u_xlat0_d.xy = (u_xlat0_d.xz / float2(u_xlat2, u_xlat2));
          u_xlat0_d.xy = (u_xlat0_d.xy + float2(0.5, 0.5));
          u_xlat10_0 = tex2D(_MainTex, u_xlat0_d.xy);
          u_xlat0_d.xyz = (u_xlat10_0.xyz * in_f.color.xyz);
          out_f.color.w = ((u_xlat10_0.w * in_f.color.w) + (-_Alpha));
          out_f.color.xyz = u_xlat0_d.xyz;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
