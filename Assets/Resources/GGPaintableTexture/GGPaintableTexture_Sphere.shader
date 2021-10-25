Shader "GGPaintableTexture/Sphere"
{
  Properties
  {
  }
  SubShader
  {
    Tags
    { 
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZWrite Off
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      uniform float4 _Channel;
      uniform float3 _Position;
      uniform float _BrushSize;
      uniform float _BrushHardness;
      uniform float4 _Color;
      uniform sampler2D _Texture;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float2 texcoord :TEXCOORD0;
          float2 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float3 u_xlat0;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.xy = (in_v.texcoord1.xy * _Channel.yy);
          u_xlat0.xy = ((in_v.texcoord.xy * _Channel.xx) + u_xlat0.xy);
          out_v.vertex.xy = ((u_xlat0.xy * float2(2, 2)) + float2(-1, (-1)));
          out_v.texcoord.xy = u_xlat0.xy;
          out_v.vertex.zw = float2(0.5, 1);
          u_xlat0.xyz = (in_v.vertex.yyy * conv_mxt4x4_1(unity_ObjectToWorld).xyz);
          u_xlat0.xyz = ((conv_mxt4x4_0(unity_ObjectToWorld).xyz * in_v.vertex.xxx) + u_xlat0.xyz);
          u_xlat0.xyz = ((conv_mxt4x4_2(unity_ObjectToWorld).xyz * in_v.vertex.zzz) + u_xlat0.xyz);
          out_v.texcoord1.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat0_d;
      float4 u_xlat10_1;
      float4 u_xlat2;
      float u_xlat3;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.xyz = (in_f.texcoord1.xyz + (-_Position.xyz));
          u_xlat0_d.x = length(u_xlat0_d.xyz);
          u_xlat0_d.x = (((-_BrushSize) * _BrushHardness) + u_xlat0_d.x);
          u_xlat3 = (((-_BrushSize) * _BrushHardness) + _BrushSize);
          u_xlat3 = (float(1) / u_xlat3);
          u_xlat0_d.x = (u_xlat3 * u_xlat0_d.x);
          u_xlat0_d.x = clamp(u_xlat0_d.x, 0, 1);
          u_xlat3 = ((u_xlat0_d.x * (-2)) + 3);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat0_d.x);
          u_xlat0_d.x = (((-u_xlat3) * u_xlat0_d.x) + 1);
          u_xlat10_1 = tex2D(_Texture, in_f.texcoord.xy);
          u_xlat2 = ((-u_xlat10_1) + _Color);
          out_f.color = ((u_xlat0_d.xxxx * u_xlat2) + u_xlat10_1);
          out_f.color = clamp(out_f.color, 0, 1);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
