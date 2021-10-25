Shader "Custom/LightningBoltShaderMesh"
{
  Properties
  {
    _MainTex ("Main Texture (RGBA)", 2D) = "white" {}
    _TintColor ("Tint Color (RGB)", Color) = (1,1,1,1)
    _InvFade ("Soft Particles Factor", Range(0.01, 100)) = 1
    _JitterMultiplier ("Jitter Multiplier (Float)", float) = 0
    _Turbulence ("Turbulence (Float)", float) = 0
    _TurbulenceVelocity ("Turbulence Velocity (Vector)", Vector) = (0,0,0,0)
    _SrcBlendMode ("SrcBlendMode (Source Blend Mode)", float) = 5
    _DstBlendMode ("DstBlendMode (Destination Blend Mode)", float) = 1
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent"
    }
    Pass // ind: 1, name: GlowPass
    {
      Name "GlowPass"
      Tags
      { 
        "QUEUE" = "Transparent"
      }
      LOD 400
      ZWrite Off
      Cull Off
      Blend Zero Zero
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _LightningTime;
      uniform float _JitterMultiplier;
      uniform float _Turbulence;
      uniform float4 _TurbulenceVelocity;
      uniform float4 _TintColor;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 tangent :TANGENT0;
          float4 color :COLOR0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
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
      int u_xlatb0;
      float4 u_xlat1;
      float4 u_xlat2;
      float u_xlat3;
      float u_xlat6;
      float2 u_xlat7;
      float u_xlat9;
      float u_xlat10;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.texcoord.xy = in_v.texcoord.xy;
          u_xlatb0 = (_LightningTime.y<in_v.texcoord1.y);
          u_xlat3 = (u_xlatb0)?(1):(float(0));
          u_xlat1.xyz = ((-in_v.texcoord1.xxz) + in_v.texcoord1.wyw);
          u_xlat6 = max(u_xlat1.z, 9.99999975E-06);
          u_xlat7.xy = ((-in_v.texcoord1.xz) + _LightningTime.yy);
          u_xlat6 = (u_xlat7.y / u_xlat6);
          u_xlat6 = clamp(u_xlat6, 0, 1);
          u_xlat6 = ((-u_xlat6) + 1);
          u_xlat0.x = (u_xlatb0)?(0):(u_xlat6);
          u_xlat6 = (u_xlat1.y + 9.99999975E-06);
          u_xlat9 = (u_xlat7.x / u_xlat1.x);
          u_xlat6 = max(u_xlat6, 9.99999975E-06);
          u_xlat6 = (u_xlat7.x / u_xlat6);
          u_xlat6 = clamp(u_xlat6, 0, 1);
          u_xlat0.x = ((u_xlat3 * u_xlat6) + u_xlat0.x);
          u_xlat0.x = min(u_xlat0.x, 1);
          u_xlat1 = (u_xlat0.xxxx * _TintColor.wxyz);
          u_xlat2.xyz = (u_xlat1.yzw * in_v.color.xyz);
          u_xlat2.w = (u_xlat1.x * in_v.texcoord.w);
          out_v.color = u_xlat2;
          u_xlat0.x = dot(in_v.tangent.xz, in_v.tangent.xz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat0.xy = (u_xlat0.xx * in_v.tangent.xz);
          u_xlat0.xy = ((float2(u_xlat9, u_xlat9) * _TurbulenceVelocity.xz) + u_xlat0.xy);
          u_xlat6 = max(abs(in_v.tangent.w), 0.5);
          u_xlat6 = (_Turbulence / u_xlat6);
          u_xlat6 = (u_xlat6 * u_xlat9);
          u_xlat0.xz = (float2(u_xlat6, u_xlat6) * u_xlat0.xy);
          u_xlat1.xyz = (in_v.vertex.xyz * _LightningTime.xyz);
          u_xlat9 = dot(u_xlat1.xyz, float3(12.9898005, 78.2330017, 45.5432014));
          u_xlat9 = sin(u_xlat9);
          u_xlat9 = (u_xlat9 * 43758.5469);
          u_xlat9 = frac(u_xlat9);
          u_xlat9 = (u_xlat9 * _JitterMultiplier);
          u_xlat9 = ((u_xlat9 * 0.0500000007) + 1);
          u_xlat1.x = dot(in_v.normal.xz, in_v.normal.xz);
          u_xlat1.x = rsqrt(u_xlat1.x);
          u_xlat1.xy = (u_xlat1.xx * in_v.normal.xz);
          u_xlat7.x = (in_v.texcoord.x + (-0.5));
          u_xlat7.x = (u_xlat7.x + u_xlat7.x);
          u_xlat1.xy = (u_xlat7.xx * u_xlat1.xy);
          u_xlat7.x = (abs(in_v.tangent.w) + abs(in_v.tangent.w));
          u_xlat7.x = (u_xlat7.x * in_v.texcoord.z);
          u_xlat1.xy = (u_xlat7.xx * u_xlat1.xy);
          u_xlat1.xy = ((u_xlat1.xy * float2(1.5, 1.5)) + in_v.vertex.xz);
          u_xlat2.xy = (in_v.normal.zx * float2(-1, 1));
          u_xlat2.xy = normalize(u_xlat2.xy);
          u_xlat7.xy = (u_xlat7.xx * u_xlat2.xy);
          u_xlat2.x = (in_v.tangent.w / abs(in_v.tangent.w));
          u_xlat7.xy = (u_xlat7.xy * u_xlat2.xx);
          u_xlat1.xz = ((u_xlat7.xy * float2(u_xlat9, u_xlat9)) + u_xlat1.xy);
          u_xlat1.y = in_v.vertex.y;
          u_xlat0.y = 0;
          u_xlat0.xyz = (u_xlat0.xyz + u_xlat1.xyz);
          out_v.vertex = UnityObjectToClipPos(u_xlat0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat10_0;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          out_f.color = (u_xlat10_0 * in_f.color);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
