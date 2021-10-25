Shader "GGPaintableTexture/Count"
{
  Properties
  {
    _Texture ("Texture", 2D) = "white" {}
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
      
      
      #define CODE_BLOCK_VERTEX
      uniform sampler2D _Texture;
      struct appdata_t
      {
          float2 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
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
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex.xy = ((in_v.texcoord.xy * float2(2, 2)) + float2(-1, (-1)));
          out_v.vertex.zw = float2(0.5, 1);
          out_v.texcoord.xy = in_v.texcoord.xy;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat16_0;
      float3 u_xlat10_0;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0.xyz = tex2D(_Texture, in_f.texcoord.xy).xyz;
          u_xlat16_0 = (u_xlat10_0.y + u_xlat10_0.x);
          u_xlat16_0 = (u_xlat10_0.z + u_xlat16_0);
          out_f.color.x = (u_xlat16_0 * 0.330000013);
          out_f.color.x = clamp(out_f.color.x, 0, 1);
          out_f.color.yzw = float3(1, 0, 1);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
