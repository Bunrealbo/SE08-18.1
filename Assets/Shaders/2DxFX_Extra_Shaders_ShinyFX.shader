Shader "2DxFX_Extra_Shaders/ShinyFX"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _ShinyFX_Pos_1 ("_ShinyFX_Pos_1", Range(-1, 1)) = 0
    _ShinyFX_Size_1 ("_ShinyFX_Size_1", Range(-1, 1)) = -0.1
    _ShinyFX_Smooth_1 ("_ShinyFX_Smooth_1", Range(0, 1)) = 0.25
    _ShinyFX_Intensity_1 ("_ShinyFX_Intensity_1", Range(0, 4)) = 1
    _ShinyFX_Speed_1 ("_ShinyFX_Speed_1", Range(0, 8)) = 1
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
      //uniform float4 _Time;
      uniform float _SpriteFade;
      uniform float _ShinyFX_Pos_1;
      uniform float _ShinyFX_Size_1;
      uniform float _ShinyFX_Smooth_1;
      uniform float _ShinyFX_Intensity_1;
      uniform float _ShinyFX_Speed_1;
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
      float4 u_xlat0_d;
      float u_xlat1_d;
      float4 u_xlat10_1;
      float u_xlat2;
      int u_xlatb2;
      int u_xlatb3;
      float u_xlat4;
      float u_xlat6;
      int u_xlatb6;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Time.x * _ShinyFX_Speed_1);
          u_xlat0_d.x = (u_xlat0_d.x * 20);
          u_xlat0_d.x = sin(u_xlat0_d.x);
          u_xlat2 = (_ShinyFX_Pos_1 + 0.5);
          u_xlat0_d.x = ((u_xlat0_d.x * 0.5) + u_xlat2);
          u_xlat0_d.y = 0.5;
          u_xlat0_d.xy = ((-u_xlat0_d.xy) + in_f.texcoord.xy);
          u_xlat4 = max(abs(u_xlat0_d.y), abs(u_xlat0_d.x));
          u_xlat4 = (float(1) / u_xlat4);
          u_xlat6 = min(abs(u_xlat0_d.y), abs(u_xlat0_d.x));
          u_xlat4 = (u_xlat4 * u_xlat6);
          u_xlat6 = (u_xlat4 * u_xlat4);
          u_xlat1_d = ((u_xlat6 * 0.0208350997) + (-0.0851330012));
          u_xlat1_d = ((u_xlat6 * u_xlat1_d) + 0.180141002);
          u_xlat1_d = ((u_xlat6 * u_xlat1_d) + (-0.330299497));
          u_xlat6 = ((u_xlat6 * u_xlat1_d) + 0.999866009);
          u_xlat1_d = (u_xlat6 * u_xlat4);
          u_xlat1_d = ((u_xlat1_d * (-2)) + 1.57079637);
          u_xlatb3 = (abs(u_xlat0_d.y)<abs(u_xlat0_d.x));
          u_xlat1_d = (u_xlatb3)?(u_xlat1_d):(float(0));
          u_xlat4 = ((u_xlat4 * u_xlat6) + u_xlat1_d);
          u_xlatb6 = (u_xlat0_d.y<(-u_xlat0_d.y));
          u_xlat6 = (u_xlatb6)?((-3.14159274)):(float(0));
          u_xlat4 = (u_xlat6 + u_xlat4);
          u_xlat6 = min(u_xlat0_d.y, u_xlat0_d.x);
          u_xlatb6 = (u_xlat6<(-u_xlat6));
          u_xlat1_d = max(u_xlat0_d.y, u_xlat0_d.x);
          u_xlat0_d.x = length(u_xlat0_d.xy);
          u_xlatb2 = (u_xlat1_d>=(-u_xlat1_d));
          u_xlatb2 = (u_xlatb2 && u_xlatb6);
          u_xlat2 = (u_xlatb2)?((-u_xlat4)):(u_xlat4);
          u_xlat2 = (u_xlat2 + 1.39999998);
          u_xlat4 = ((u_xlat2 * 0.318319261) + 0.5);
          u_xlat4 = floor(u_xlat4);
          u_xlat2 = ((u_xlat4 * 3.1415) + (-u_xlat2));
          u_xlat2 = cos(u_xlat2);
          u_xlat0_d.x = ((u_xlat2 * u_xlat0_d.x) + (-_ShinyFX_Size_1));
          u_xlat2 = (float(1) / _ShinyFX_Smooth_1);
          u_xlat0_d.x = (u_xlat2 * u_xlat0_d.x);
          u_xlat0_d.x = clamp(u_xlat0_d.x, 0, 1);
          u_xlat2 = ((u_xlat0_d.x * (-2)) + 3);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat0_d.x);
          u_xlat0_d.x = (((-u_xlat2) * u_xlat0_d.x) + 1);
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat0_d.xyz = ((u_xlat0_d.xxx * float3(_ShinyFX_Intensity_1, _ShinyFX_Intensity_1, _ShinyFX_Intensity_1)) + u_xlat10_1.xyz);
          u_xlat0_d.w = (u_xlat10_1.w * _SpriteFade);
          out_f.color = (u_xlat0_d * in_f.color);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
