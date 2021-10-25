Shader "2DxFX/Standard/Ghost"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _ClipLeft ("Clipping Left", Range(0, 1)) = 1
    _ClipRight ("Clipping Right", Range(0, 1)) = 1
    _ClipUp ("Clipping Up", Range(0, 1)) = 1
    _ClipDown ("Clipping Down", Range(0, 1)) = 1
    _offset ("offset", Range(0, 1)) = 1
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
      uniform float _Alpha;
      uniform float _offset;
      uniform float _ClipLeft;
      uniform float _ClipRight;
      uniform float _ClipUp;
      uniform float _ClipDown;
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
      float u_xlat0_d;
      int u_xlatb0;
      float4 u_xlat1_d;
      float4 u_xlat10_1;
      int u_xlatb1;
      float2 u_xlat2;
      float2 u_xlat4;
      int u_xlatb4;
      float2 u_xlat5;
      float u_xlat6;
      int u_xlatb6;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlatb0 = (_ClipUp<in_f.texcoord.y);
          u_xlat2.x = (_offset + 1);
          u_xlat4.xy = float2(((-float2(_ClipUp, _ClipDown)) + float2(1, 1)));
          u_xlat1_d.xy = (u_xlat2.xx / u_xlat4.xy);
          u_xlatb4 = (in_f.texcoord.y<u_xlat4.y);
          u_xlat5.xy = ((-in_f.texcoord.yx) + float2(1, 1));
          u_xlat6 = ((u_xlat1_d.x * u_xlat5.x) + (-_offset));
          u_xlat6 = clamp(u_xlat6, 0, 1);
          u_xlat1_d.x = ((u_xlat1_d.y * in_f.texcoord.y) + (-_offset));
          u_xlat1_d.x = clamp(u_xlat1_d.x, 0, 1);
          u_xlat0_d = (u_xlatb0)?(u_xlat6):(1);
          u_xlat6 = (u_xlat1_d.x * u_xlat0_d);
          u_xlat0_d = (u_xlatb4)?(u_xlat6):(u_xlat0_d);
          u_xlat4.xy = float2(((-float2(_ClipRight, _ClipLeft)) + float2(1, 1)));
          u_xlat2.xy = (u_xlat2.xx / u_xlat4.xy);
          u_xlatb6 = (in_f.texcoord.x<u_xlat4.y);
          u_xlat2.x = ((u_xlat2.x * u_xlat5.y) + (-_offset));
          u_xlat2.x = clamp(u_xlat2.x, 0, 1);
          u_xlat4.x = ((u_xlat2.y * in_f.texcoord.x) + (-_offset));
          u_xlat4.x = clamp(u_xlat4.x, 0, 1);
          u_xlat2.x = (u_xlat2.x * u_xlat0_d);
          u_xlatb1 = (_ClipRight<in_f.texcoord.x);
          u_xlat0_d = (u_xlatb1)?(u_xlat2.x):(u_xlat0_d);
          u_xlat2.x = (u_xlat4.x * u_xlat0_d);
          u_xlat0_d = (u_xlatb6)?(u_xlat2.x):(u_xlat0_d);
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat1_d = (u_xlat10_1 * in_f.color);
          out_f.color.w = ((u_xlat1_d.w * u_xlat0_d) + (-_Alpha));
          out_f.color.xyz = u_xlat1_d.xyz;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
