Shader "2DxFX_Extra_Shaders/AlphaIntensity"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _AlphaIntensity_Fade_1 ("_AlphaIntensity_Fade_1", Range(0, 6)) = 1
    _SpriteFade ("SpriteFade", Range(0, 1)) = 1
    [HideInInspector] _StencilComp ("Stencil Comparison", float) = 8
    [HideInInspector] _Stencil ("Stencil ID", float) = 0
    [HideInInspector] _StencilOp ("Stencil Operation", float) = 0
    [HideInInspector] _StencilWriteMask ("Stencil Write Mask", float) = 255
    [HideInInspector] _StencilReadMask ("Stencil Read Mask", float) = 255
    [HideInInspector] _ColorMask ("Color Mask", float) = 15
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
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
      uniform float _SpriteFade;
      uniform float _AlphaIntensity_Fade_1;
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
      float4 u_xlat10_0;
      int u_xlatb1;
      float u_xlat3;
      float u_xlat6;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlatb1 = (u_xlat10_0.w<1);
          u_xlat3 = (u_xlat10_0.w * _AlphaIntensity_Fade_1);
          u_xlat6 = (u_xlatb1)?(u_xlat3):(u_xlat10_0.w);
          out_f.color.xyz = (u_xlat10_0.xyz * in_f.color.xyz);
          u_xlat0_d = (u_xlat6 * _SpriteFade);
          out_f.color.w = (u_xlat0_d * in_f.color.w);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
