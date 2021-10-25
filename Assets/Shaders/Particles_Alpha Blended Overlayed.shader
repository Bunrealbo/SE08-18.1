Shader "Particles/Alpha Blended Overlayed"
{
  Properties
  {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Particle Texture", 2D) = "white" {}
    _InvFade ("Soft Particles Factor", Range(0.01, 3)) = 1
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float4 _TintColor;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 color :COLOR0;
          float2 texcoord :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 color :COLOR0;
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
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.color = in_v.color;
          out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat16_0;
      float4 u_xlat10_1;
      int u_xlatb1;
      float4 u_xlat16_2;
      float4 u_xlat16_3;
      float u_xlat16_12;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat16_0.xyz = (((-in_f.color.xyz) * _TintColor.xyz) + float3(1, 1, 1));
          u_xlat16_0.xyz = (u_xlat16_0.xyz + u_xlat16_0.xyz);
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat16_2.xyz = ((-u_xlat10_1.xyz) + float3(1, 1, 1));
          u_xlat16_0.xyz = (((-u_xlat16_0.xyz) * u_xlat16_2.xyz) + float3(1, 1, 1));
          u_xlat16_2 = (u_xlat10_1.yxyz + u_xlat10_1.xxyz);
          u_xlat16_12 = (u_xlat10_1.z + u_xlat16_2.x);
          u_xlatb1 = (1.5<u_xlat16_12);
          u_xlat16_3 = (in_f.color * _TintColor);
          u_xlat16_2.xyz = (u_xlat16_2.yzw * u_xlat16_3.xyz);
          out_f.color.w = (u_xlat10_1.w * u_xlat16_3.w);
          out_f.color.xyz = (int(u_xlatb1))?(u_xlat16_0.xyz):(u_xlat16_2.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
