Shader "2DxFX/Standard/Pixel8bitsGB"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Size ("Size", Range(0, 1)) = 0
    _Offset ("Offset", Range(0, 1)) = 0
    _Offset2 ("Offset2", Range(0, 1)) = 0
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
      uniform float _Size;
      uniform float _Offset2;
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
      float4 u_xlat1_d;
      float3 u_xlat2;
      float4 u_xlat10_2;
      int u_xlatb2;
      float3 u_xlat3;
      float3 u_xlat4;
      int u_xlatb5;
      float3 u_xlat7;
      float u_xlat11;
      float u_xlat15;
      int u_xlatb15;
      float u_xlat17;
      int u_xlatb17;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.z = 1000;
          u_xlat15 = (_Size * 64);
          u_xlat1_d.xy = (float2(u_xlat15, u_xlat15) * in_f.texcoord.xy);
          u_xlat1_d.xy = floor(u_xlat1_d.xy);
          u_xlat1_d.xy = (u_xlat1_d.xy / float2(u_xlat15, u_xlat15));
          u_xlat10_2 = tex2D(_MainTex, u_xlat1_d.xy);
          u_xlat15 = (u_xlat1_d.y + u_xlat1_d.x);
          u_xlat1_d = (u_xlat10_2 * in_f.color);
          u_xlat1_d.xyz = (u_xlat1_d.xyz * float3(float3(_Offset2, _Offset2, _Offset2)));
          u_xlat2.xyz = (u_xlat1_d.xyz * u_xlat1_d.xyz);
          u_xlat3.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.314432025, (-0.493039042), (-0.0196830016)));
          u_xlat17 = dot(u_xlat3.xyz, u_xlat3.xyz);
          u_xlat0_d.y = min(u_xlat17, 1000);
          u_xlatb17 = (u_xlat17<1000);
          u_xlat3.xyz = float3(lerp(float3(0, 0, 0), float3(0.680000007, 0.790000021, 0.270000011), float3(bool3(u_xlatb17, u_xlatb17, u_xlatb17))));
          u_xlat4.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.103822999, (-0.328509003), (-0.0740879923)));
          u_xlat0_d.x = dot(u_xlat4.xyz, u_xlat4.xyz);
          u_xlatb17 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb17))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat4.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.00219699973, (-0.0795070007), (-0.0506530032)));
          u_xlat1_d.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-2.69999982E-05, (-0.00409599999), (-0.0359370038)));
          u_xlat1_d.x = dot(u_xlat1_d.xyz, u_xlat1_d.xyz);
          u_xlat0_d.x = dot(u_xlat4.xyz, u_xlat4.xyz);
          u_xlatb2 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat1_d.yz = (int(u_xlatb2))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat0_d.xyz = lerp(float3(0, 0, 0), u_xlat3.xyz, float3(bool3(u_xlatb17, u_xlatb17, u_xlatb17)));
          u_xlat7.xyz = (int(u_xlatb17))?(float3(0.469999999, 0.689999998, 0.419999987)):(u_xlat3.xyz);
          u_xlat0_d.xyz = (int(u_xlatb2))?(u_xlat7.xyz):(u_xlat0_d.xyz);
          u_xlat2.xyz = (int(u_xlatb2))?(float3(0.129999995, 0.430000007, 0.370000005)):(u_xlat7.xyz);
          u_xlatb17 = (u_xlat1_d.x<u_xlat1_d.y);
          u_xlat1_d.xy = (int(u_xlatb17))?(u_xlat1_d.xy):(u_xlat1_d.yz);
          u_xlat1_d.xy = sqrt(u_xlat1_d.xy);
          u_xlat0_d.xyz = (int(u_xlatb17))?(u_xlat2.xyz):(u_xlat0_d.xyz);
          u_xlat2.xyz = (int(u_xlatb17))?(float3(0.0299999993, 0.159999996, 0.330000013)):(u_xlat2.xyz);
          u_xlat11 = (u_xlat15 * 0.5);
          u_xlat11 = floor(u_xlat11);
          u_xlat15 = (((-u_xlat11) * 2) + u_xlat15);
          u_xlat1_d.x = (u_xlat1_d.y + u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.y / u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.x + 1);
          u_xlatb15 = (u_xlat1_d.x<u_xlat15);
          out_f.color.xyz = (int(u_xlatb15))?(u_xlat0_d.xyz):(u_xlat2.xyz);
          u_xlat0_d.x = ((-_Alpha) + 1);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat1_d.w);
          u_xlatb5 = (u_xlat1_d.w<0.949999988);
          out_f.color.w = (u_xlatb5)?(0):(u_xlat0_d.x);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
