Shader "GGPaintableTexture/FixIslandEdges"
{
  Properties
  {
  }
  SubShader
  {
    Tags
    { 
      "RenderType" = "Opaque"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "RenderType" = "Opaque"
      }
      LOD 100
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _TexelSize;
      uniform sampler2D _MainTex;
      uniform sampler2D _IlsandMap;
      struct appdata_t
      {
          float4 vertex :POSITION0;
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
      
      float4 u_xlat0;
      float4 u_xlat1;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.texcoord.xy = in_v.texcoord.xy;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float4 u_xlat1_d;
      float2 u_xlatb1;
      float4 u_xlat2;
      float3 u_xlat10_2;
      float2 u_xlatb2;
      float3 u_xlat3;
      float3 u_xlat10_3;
      float3 u_xlat5;
      float2 u_xlat9;
      float2 u_xlatb9;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat1_d.x = tex2D(_IlsandMap, in_f.texcoord.xy).x;
          u_xlatb1.x = (u_xlat1_d.x<0.200000003);
          if(u_xlatb1.x)
          {
              u_xlat1_d = ((_TexelSize.xyxy * float4(-1.5, (-1.5), (-1.5), (-0.5))) + in_f.texcoord.xyxy);
              u_xlat10_2.xyz = tex2D(_MainTex, u_xlat1_d.xy).xyz;
              u_xlat1_d.x = tex2D(_IlsandMap, u_xlat1_d.xy).x;
              u_xlatb1.xy = bool4(u_xlat1_d.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat1_d.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb1.xy));
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat1_d.zw).xyz;
              u_xlat9.x = tex2D(_IlsandMap, u_xlat1_d.zw).x;
              u_xlatb9.xy = bool4(u_xlat9.xxxx >= float4(0.800000012, 0.100000001, 0.800000012, 0.100000001)).xy;
              u_xlat9.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb9.xy));
              u_xlat1_d.x = (u_xlat9.x + u_xlat1_d.x);
              u_xlat3.xyz = (u_xlat9.yyy * u_xlat10_3.xyz);
              u_xlat5.xyz = ((u_xlat10_2.xyz * u_xlat1_d.yyy) + u_xlat3.xyz);
              u_xlat2 = ((_TexelSize.xyxy * float4(-1.5, 0.5, (-1.5), 1.5)) + in_f.texcoord.xyxy);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.xy).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.xy).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.zw).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.zw).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat2 = ((_TexelSize.xyxy * float4(-0.5, (-1.5), (-0.5), (-0.5))) + in_f.texcoord.xyxy);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.xy).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.xy).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.zw).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.zw).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat2 = ((_TexelSize.xyxy * float4(-0.5, 0.5, (-0.5), 1.5)) + in_f.texcoord.xyxy);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.xy).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.xy).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.zw).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.zw).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat2 = ((_TexelSize.xyxy * float4(0.5, (-1.5), 0.5, (-0.5))) + in_f.texcoord.xyxy);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.xy).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.xy).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.zw).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.zw).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat2 = ((_TexelSize.xyxy * float4(0.5, 0.5, 0.5, 1.5)) + in_f.texcoord.xyxy);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.xy).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.xy).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.zw).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.zw).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat2 = ((_TexelSize.xyxy * float4(1.5, (-1.5), 1.5, (-0.5))) + in_f.texcoord.xyxy);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.xy).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.xy).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.zw).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.zw).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat2 = ((_TexelSize.xyxy * float4(1.5, 0.5, 1.5, 1.5)) + in_f.texcoord.xyxy);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.xy).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.xy).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat10_3.xyz = tex2D(_MainTex, u_xlat2.zw).xyz;
              u_xlat2.x = tex2D(_IlsandMap, u_xlat2.zw).x;
              u_xlatb2.xy = bool4(u_xlat2.xxxx >= float4(0.800000012, 0.100000001, 0, 0)).xy;
              u_xlat2.xy = lerp(float2(0, 0), float2(1, 1), float2(u_xlatb2.xy));
              u_xlat1_d.x = (u_xlat1_d.x + u_xlat2.x);
              u_xlat5.xyz = ((u_xlat10_3.xyz * u_xlat2.yyy) + u_xlat5.xyz);
              u_xlat0_d.xyz = (u_xlat5.xyz / u_xlat1_d.xxx);
          }
          out_f.color = u_xlat0_d;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
