Shader "2DxFX/AL/Pixel8bitsBW"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Size ("Size", Range(0, 1)) = 0
    _Offset ("Offset", Range(0, 1)) = 0
    _Offset2 ("Offset2", Range(0, 1)) = 0
    _Alpha ("Alpha", Range(0, 1)) = 1
    [HideInInspector] _SrcBlend ("_SrcBlend", float) = 0
    [HideInInspector] _DstBlend ("_DstBlend", float) = 0
    [HideInInspector] _BlendOp ("_BlendOp", float) = 0
    [HideInInspector] _Z ("_Z", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "RenderType" = "TransparentCutout"
    }
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDBASE"
        "PreviewType" = "Plane"
        "RenderType" = "TransparentCutout"
        "SHADOWSUPPORT" = "true"
      }
      ZWrite Off
      Cull Off
      Blend Zero Zero
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _ScreenParams;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform float4 _MainTex_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float _Size;
      uniform float _Offset2;
      uniform float _Alpha;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord6 :TEXCOORD6;
          float4 texcoord7 :TEXCOORD7;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float4 texcoord3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float2 u_xlat4;
      float u_xlat6;
      float roundEven(float x)
      {
          float y = floor((x + 0.5));
          return (((y - x)==0.5))?((floor((0.5 * y)) * 2)):(y);
      }
      
      float2 roundEven(float2 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          return a;
      }
      
      float3 roundEven(float3 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          return a;
      }
      
      float4 roundEven(float4 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          a.w = round(a.w);
          return a;
      }
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.xy = (in_v.vertex.xy / in_v.vertex.ww);
          u_xlat4.xy = (_ScreenParams.xy * float2(0.5, 0.5));
          u_xlat0.xy = (u_xlat4.xy * u_xlat0.xy);
          u_xlat0.xy = round(u_xlat0.xy);
          u_xlat0.xy = (u_xlat0.xy / u_xlat4.xy);
          u_xlat0.xy = (u_xlat0.xy * in_v.vertex.ww);
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * u_xlat0.xxxx) + u_xlat1);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          out_v.texcoord2.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz);
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          out_v.texcoord1.xyz = normalize(u_xlat0.xyz);
          out_v.texcoord3 = (in_v.color * _Color);
          out_v.texcoord6 = float4(0, 0, 0, 0);
          out_v.texcoord7 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat0_d;
      float4 u_xlat1_d;
      float4 u_xlat10_1;
      float3 u_xlat16_2;
      float3 u_xlat3;
      float3 u_xlat4_d;
      float2 u_xlat5;
      int u_xlatb5;
      float2 u_xlat10;
      int u_xlatb10;
      int u_xlatb15;
      float u_xlat16_17;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d = ((-_Alpha) + 1);
          u_xlat5.x = (_Size * 64);
          u_xlat10.xy = (u_xlat5.xx * in_f.texcoord.xy);
          u_xlat10.xy = floor(u_xlat10.xy);
          u_xlat5.xy = (u_xlat10.xy / u_xlat5.xx);
          u_xlat10_1 = tex2D(_MainTex, u_xlat5.xy);
          u_xlat5.x = (u_xlat5.y + u_xlat5.x);
          u_xlat1_d = (u_xlat10_1 * in_f.texcoord3);
          u_xlat0_d = (u_xlat0_d * u_xlat1_d.w);
          u_xlatb10 = (u_xlat1_d.w<0.949999988);
          u_xlat1_d.xyz = (u_xlat1_d.xyz * float3(float3(_Offset2, _Offset2, _Offset2)));
          u_xlat0_d = (u_xlatb10)?(0):(u_xlat0_d);
          u_xlat16_2.x = (u_xlat0_d + (-0.0500000007));
          u_xlatb10 = (u_xlat16_2.x<0);
          if(((int(u_xlatb10) * (-1))!=0))
          {
              discard;
          }
          u_xlat10.x = (u_xlat5.x * 0.5);
          u_xlat10.x = floor(u_xlat10.x);
          u_xlat5.x = (((-u_xlat10.x) * 2) + u_xlat5.x);
          u_xlat3.xyz = (u_xlat1_d.xyz * u_xlat1_d.xyz);
          u_xlat4_d.xyz = ((u_xlat3.xyz * u_xlat1_d.xyz) + float3(-2.69999982E-05, (-2.69999982E-05), (-2.69999982E-05)));
          u_xlat1_d.xyz = ((u_xlat3.xyz * u_xlat1_d.xyz) + float3(-0.941192091, (-0.941192091), (-0.941192091)));
          u_xlat10.x = dot(u_xlat1_d.xyz, u_xlat1_d.xyz);
          u_xlat1_d.x = dot(u_xlat4_d.xyz, u_xlat4_d.xyz);
          u_xlat1_d.y = min(u_xlat10.x, 1000);
          u_xlatb10 = (u_xlat10.x<1000);
          u_xlat10.x = (u_xlatb10)?(0.980000019):(float(0));
          u_xlatb15 = (u_xlat1_d.x<u_xlat1_d.y);
          u_xlat1_d.z = 1000;
          u_xlat1_d.xy = (int(u_xlatb15))?(u_xlat1_d.xy):(u_xlat1_d.yz);
          u_xlat1_d.xy = sqrt(u_xlat1_d.xy);
          u_xlat1_d.x = (u_xlat1_d.y + u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.y / u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.x + 1);
          u_xlatb5 = (u_xlat1_d.x<u_xlat5.x);
          u_xlat1_d.x = (u_xlatb15)?(0.0299999993):(u_xlat10.x);
          u_xlat10.x = (u_xlatb15)?(u_xlat10.x):(float(0));
          u_xlat5.x = (u_xlatb5)?(u_xlat10.x):(u_xlat1_d.x);
          u_xlat5.x = (u_xlat0_d * u_xlat5.x);
          out_f.color.w = u_xlat0_d;
          u_xlat16_2.xyz = (u_xlat5.xxx * _LightColor0.xyz);
          u_xlat16_17 = dot(in_f.texcoord1.xyz, _WorldSpaceLightPos0.xyz);
          u_xlat16_17 = max(u_xlat16_17, 0);
          out_f.color.xyz = (float3(u_xlat16_17, u_xlat16_17, u_xlat16_17) * u_xlat16_2.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDADD"
        "PreviewType" = "Plane"
        "RenderType" = "TransparentCutout"
        "SHADOWSUPPORT" = "true"
      }
      ZWrite Off
      Cull Off
      Blend One One
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile POINT
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _ScreenParams;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4x4 unity_WorldToLight;
      uniform float4 _Color;
      uniform float4 _MainTex_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float _Size;
      uniform float _Offset2;
      uniform float _Alpha;
      uniform sampler2D _MainTex;
      uniform sampler2D _LightTexture0;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float3 texcoord4 :TEXCOORD4;
          float4 texcoord5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat2;
      float2 u_xlat6;
      float u_xlat10;
      float roundEven(float x)
      {
          float y = floor((x + 0.5));
          return (((y - x)==0.5))?((floor((0.5 * y)) * 2)):(y);
      }
      
      float2 roundEven(float2 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          return a;
      }
      
      float3 roundEven(float3 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          return a;
      }
      
      float4 roundEven(float4 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          a.w = round(a.w);
          return a;
      }
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.xy = (in_v.vertex.xy / in_v.vertex.ww);
          u_xlat6.xy = (_ScreenParams.xy * float2(0.5, 0.5));
          u_xlat0.xy = (u_xlat6.xy * u_xlat0.xy);
          u_xlat0.xy = round(u_xlat0.xy);
          u_xlat0.xy = (u_xlat0.xy / u_xlat6.xy);
          u_xlat0.xy = (u_xlat0.xy * in_v.vertex.ww);
          u_xlat1 = mul(unity_ObjectToWorld, float4(u_xlat0.xyz,1.0));
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          u_xlat1.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat1.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat1.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          out_v.texcoord1.xyz = normalize(u_xlat1.xyz);
          out_v.texcoord2.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz);
          u_xlat0 = ((conv_mxt4x4_3(unity_ObjectToWorld) * in_v.vertex.wwww) + u_xlat0);
          out_v.texcoord3 = (in_v.color * _Color);
          u_xlat1.xyz = (u_xlat0.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz);
          u_xlat1.xyz = ((conv_mxt4x4_0(unity_WorldToLight).xyz * u_xlat0.xxx) + u_xlat1.xyz);
          u_xlat0.xyz = ((conv_mxt4x4_2(unity_WorldToLight).xyz * u_xlat0.zzz) + u_xlat1.xyz);
          out_v.texcoord4.xyz = ((conv_mxt4x4_3(unity_WorldToLight).xyz * u_xlat0.www) + u_xlat0.xyz);
          out_v.texcoord5 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float4 u_xlat1_d;
      float4 u_xlat10_1;
      float3 u_xlat16_2;
      float3 u_xlat3;
      float3 u_xlat4;
      float2 u_xlat5;
      int u_xlatb5;
      float2 u_xlat10_d;
      int u_xlatb10;
      float u_xlat15;
      int u_xlatb15;
      float u_xlat16_17;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = ((-_Alpha) + 1);
          u_xlat5.x = (_Size * 64);
          u_xlat10_d.xy = (u_xlat5.xx * in_f.texcoord.xy);
          u_xlat10_d.xy = floor(u_xlat10_d.xy);
          u_xlat5.xy = (u_xlat10_d.xy / u_xlat5.xx);
          u_xlat10_1 = tex2D(_MainTex, u_xlat5.xy);
          u_xlat5.x = (u_xlat5.y + u_xlat5.x);
          u_xlat1_d = (u_xlat10_1 * in_f.texcoord3);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat1_d.w);
          u_xlatb10 = (u_xlat1_d.w<0.949999988);
          u_xlat1_d.xyz = (u_xlat1_d.xyz * float3(float3(_Offset2, _Offset2, _Offset2)));
          u_xlat0_d.x = (u_xlatb10)?(0):(u_xlat0_d.x);
          u_xlat16_2.x = (u_xlat0_d.x + (-0.0500000007));
          u_xlatb10 = (u_xlat16_2.x<0);
          if(((int(u_xlatb10) * (-1))!=0))
          {
              discard;
          }
          u_xlat10_d.x = (u_xlat5.x * 0.5);
          u_xlat10_d.x = floor(u_xlat10_d.x);
          u_xlat5.x = (((-u_xlat10_d.x) * 2) + u_xlat5.x);
          u_xlat3.xyz = (u_xlat1_d.xyz * u_xlat1_d.xyz);
          u_xlat4.xyz = ((u_xlat3.xyz * u_xlat1_d.xyz) + float3(-2.69999982E-05, (-2.69999982E-05), (-2.69999982E-05)));
          u_xlat1_d.xyz = ((u_xlat3.xyz * u_xlat1_d.xyz) + float3(-0.941192091, (-0.941192091), (-0.941192091)));
          u_xlat10_d.x = dot(u_xlat1_d.xyz, u_xlat1_d.xyz);
          u_xlat1_d.x = dot(u_xlat4.xyz, u_xlat4.xyz);
          u_xlat1_d.y = min(u_xlat10_d.x, 1000);
          u_xlatb10 = (u_xlat10_d.x<1000);
          u_xlat10_d.x = (u_xlatb10)?(0.980000019):(float(0));
          u_xlatb15 = (u_xlat1_d.x<u_xlat1_d.y);
          u_xlat1_d.z = 1000;
          u_xlat1_d.xy = (int(u_xlatb15))?(u_xlat1_d.xy):(u_xlat1_d.yz);
          u_xlat1_d.xy = sqrt(u_xlat1_d.xy);
          u_xlat1_d.x = (u_xlat1_d.y + u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.y / u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.x + 1);
          u_xlatb5 = (u_xlat1_d.x<u_xlat5.x);
          u_xlat1_d.x = (u_xlatb15)?(0.0299999993):(u_xlat10_d.x);
          u_xlat10_d.x = (u_xlatb15)?(u_xlat10_d.x):(float(0));
          u_xlat5.x = (u_xlatb5)?(u_xlat10_d.x):(u_xlat1_d.x);
          u_xlat5.x = (u_xlat0_d.x * u_xlat5.x);
          out_f.color.w = u_xlat0_d.x;
          u_xlat0_d.xzw = (in_f.texcoord2.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz);
          u_xlat0_d.xzw = ((conv_mxt4x4_0(unity_WorldToLight).xyz * in_f.texcoord2.xxx) + u_xlat0_d.xzw);
          u_xlat0_d.xzw = ((conv_mxt4x4_2(unity_WorldToLight).xyz * in_f.texcoord2.zzz) + u_xlat0_d.xzw);
          u_xlat0_d.xzw = (u_xlat0_d.xzw + conv_mxt4x4_3(unity_WorldToLight).xyz);
          u_xlat0_d.x = dot(u_xlat0_d.xzw, u_xlat0_d.xzw);
          u_xlat0_d.x = tex2D(_LightTexture0, u_xlat0_d.xx).x;
          u_xlat16_2.xyz = (u_xlat0_d.xxx * _LightColor0.xyz);
          u_xlat16_2.xyz = (u_xlat5.xxx * u_xlat16_2.xyz);
          u_xlat0_d.xyz = ((-in_f.texcoord2.xyz) + _WorldSpaceLightPos0.xyz);
          u_xlat0_d.xyz = normalize(u_xlat0_d.xyz);
          u_xlat16_17 = dot(in_f.texcoord1.xyz, u_xlat0_d.xyz);
          u_xlat16_17 = max(u_xlat16_17, 0);
          out_f.color.xyz = (float3(u_xlat16_17, u_xlat16_17, u_xlat16_17) * u_xlat16_2.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: PREPASS
    {
      Name "PREPASS"
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "PREPASSBASE"
        "PreviewType" = "Plane"
        "RenderType" = "TransparentCutout"
      }
      ZWrite Off
      Cull Off
      Blend Zero Zero
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
      //uniform float4 _ScreenParams;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform float4 _MainTex_ST;
      uniform float _Size;
      uniform float _Alpha;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float4 texcoord3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float2 u_xlat4;
      float u_xlat6;
      float roundEven(float x)
      {
          float y = floor((x + 0.5));
          return (((y - x)==0.5))?((floor((0.5 * y)) * 2)):(y);
      }
      
      float2 roundEven(float2 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          return a;
      }
      
      float3 roundEven(float3 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          return a;
      }
      
      float4 roundEven(float4 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          a.w = round(a.w);
          return a;
      }
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.xy = (in_v.vertex.xy / in_v.vertex.ww);
          u_xlat4.xy = (_ScreenParams.xy * float2(0.5, 0.5));
          u_xlat0.xy = (u_xlat4.xy * u_xlat0.xy);
          u_xlat0.xy = round(u_xlat0.xy);
          u_xlat0.xy = (u_xlat0.xy / u_xlat4.xy);
          u_xlat0.xy = (u_xlat0.xy * in_v.vertex.ww);
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * u_xlat0.xxxx) + u_xlat1);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          out_v.texcoord2.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz);
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          out_v.texcoord1.xyz = normalize(u_xlat0.xyz);
          out_v.texcoord3 = (in_v.color * _Color);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float2 u_xlat0_d;
      float u_xlat10_0;
      float2 u_xlatb0;
      float2 u_xlat1_d;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Size * 64);
          u_xlat1_d.xy = (u_xlat0_d.xx * in_f.texcoord.xy);
          u_xlat1_d.xy = floor(u_xlat1_d.xy);
          u_xlat0_d.xy = (u_xlat1_d.xy / u_xlat0_d.xx);
          u_xlat10_0 = tex2D(_MainTex, u_xlat0_d.xy).w;
          u_xlat0_d.x = (u_xlat10_0 * in_f.texcoord3.w);
          u_xlat1_d.x = ((-_Alpha) + 1);
          u_xlat0_d.y = ((u_xlat0_d.x * u_xlat1_d.x) + (-0.0500000007));
          u_xlatb0.xy = bool4(u_xlat0_d.xyxx < float4(0.949999988, 0, 0, 0)).xy;
          u_xlatb0.x = (u_xlatb0.y || u_xlatb0.x);
          if(((int(u_xlatb0.x) * (-1))!=0))
          {
              discard;
          }
          out_f.color.xyz = ((in_f.texcoord1.xyz * float3(0.5, 0.5, 0.5)) + float3(0.5, 0.5, 0.5));
          out_f.color.w = 0;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 4, name: PREPASS
    {
      Name "PREPASS"
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "PREPASSFINAL"
        "PreviewType" = "Plane"
        "RenderType" = "TransparentCutout"
      }
      ZWrite Off
      Cull Off
      Blend Zero Zero
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
      //uniform float4 _ProjectionParams;
      //uniform float4 _ScreenParams;
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform float4 _MainTex_ST;
      uniform float _Size;
      uniform float _Offset2;
      uniform float _Alpha;
      uniform sampler2D _MainTex;
      uniform sampler2D _LightBuffer;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float3 texcoord5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float4 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float3 texcoord5 :TEXCOORD5;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat16_1;
      float3 u_xlat16_2;
      float3 u_xlat16_3;
      float2 u_xlat8;
      float u_xlat12;
      float roundEven(float x)
      {
          float y = floor((x + 0.5));
          return (((y - x)==0.5))?((floor((0.5 * y)) * 2)):(y);
      }
      
      float2 roundEven(float2 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          return a;
      }
      
      float3 roundEven(float3 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          return a;
      }
      
      float4 roundEven(float4 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          a.w = round(a.w);
          return a;
      }
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.xy = (in_v.vertex.xy / in_v.vertex.ww);
          u_xlat8.xy = (_ScreenParams.xy * float2(0.5, 0.5));
          u_xlat0.xy = (u_xlat8.xy * u_xlat0.xy);
          u_xlat0.xy = round(u_xlat0.xy);
          u_xlat0.xy = (u_xlat0.xy / u_xlat8.xy);
          u_xlat0.xy = (u_xlat0.xy * in_v.vertex.ww);
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * u_xlat0.xxxx) + u_xlat1);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          out_v.texcoord1.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz);
          u_xlat0 = mul(unity_MatrixVP, u_xlat1);
          out_v.vertex = u_xlat0;
          out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.texcoord2 = (in_v.color * _Color);
          u_xlat0.y = (u_xlat0.y * _ProjectionParams.x);
          u_xlat1.xzw = (u_xlat0.xwy * float3(0.5, 0.5, 0.5));
          out_v.texcoord3.zw = u_xlat0.zw;
          out_v.texcoord3.xy = (u_xlat1.zz + u_xlat1.xw);
          out_v.texcoord4 = float4(0, 0, 0, 0);
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat0.xyz = normalize(u_xlat0.xyz);
          u_xlat16_2.x = (u_xlat0.y * u_xlat0.y);
          u_xlat16_2.x = ((u_xlat0.x * u_xlat0.x) + (-u_xlat16_2.x));
          u_xlat16_1 = (u_xlat0.yzzx * u_xlat0.xyzz);
          u_xlat16_3.x = dot(unity_SHBr, u_xlat16_1);
          u_xlat16_3.y = dot(unity_SHBg, u_xlat16_1);
          u_xlat16_3.z = dot(unity_SHBb, u_xlat16_1);
          u_xlat16_2.xyz = ((unity_SHC.xyz * u_xlat16_2.xxx) + u_xlat16_3.xyz);
          u_xlat0.w = 1;
          u_xlat16_3.x = dot(unity_SHAr, u_xlat0);
          u_xlat16_3.y = dot(unity_SHAg, u_xlat0);
          u_xlat16_3.z = dot(unity_SHAb, u_xlat0);
          u_xlat16_2.xyz = (u_xlat16_2.xyz + u_xlat16_3.xyz);
          u_xlat16_2.xyz = max(u_xlat16_2.xyz, float3(0, 0, 0));
          u_xlat0.xyz = log2(u_xlat16_2.xyz);
          u_xlat0.xyz = (u_xlat0.xyz * float3(0.416666657, 0.416666657, 0.416666657));
          u_xlat0.xyz = exp2(u_xlat0.xyz);
          u_xlat0.xyz = ((u_xlat0.xyz * float3(1.05499995, 1.05499995, 1.05499995)) + float3(-0.0549999997, (-0.0549999997), (-0.0549999997)));
          out_v.texcoord5.xyz = max(u_xlat0.xyz, float3(0, 0, 0));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float4 u_xlat10_0;
      float4 u_xlat1_d;
      float4 u_xlat10_1;
      float3 u_xlat16_2_d;
      float3 u_xlat3;
      float3 u_xlat4;
      float2 u_xlat5;
      int u_xlatb5;
      float2 u_xlat10;
      int u_xlatb10;
      int u_xlatb15;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = ((-_Alpha) + 1);
          u_xlat5.x = (_Size * 64);
          u_xlat10.xy = (u_xlat5.xx * in_f.texcoord.xy);
          u_xlat10.xy = floor(u_xlat10.xy);
          u_xlat5.xy = (u_xlat10.xy / u_xlat5.xx);
          u_xlat10_1 = tex2D(_MainTex, u_xlat5.xy);
          u_xlat5.x = (u_xlat5.y + u_xlat5.x);
          u_xlat1_d = (u_xlat10_1 * in_f.texcoord2);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat1_d.w);
          u_xlatb10 = (u_xlat1_d.w<0.949999988);
          u_xlat1_d.xyz = (u_xlat1_d.xyz * float3(float3(_Offset2, _Offset2, _Offset2)));
          u_xlat0_d.x = (u_xlatb10)?(0):(u_xlat0_d.x);
          u_xlat16_2_d.x = (u_xlat0_d.x + (-0.0500000007));
          u_xlatb10 = (u_xlat16_2_d.x<0);
          if(((int(u_xlatb10) * (-1))!=0))
          {
              discard;
          }
          u_xlat10.x = (u_xlat5.x * 0.5);
          u_xlat10.x = floor(u_xlat10.x);
          u_xlat5.x = (((-u_xlat10.x) * 2) + u_xlat5.x);
          u_xlat3.xyz = (u_xlat1_d.xyz * u_xlat1_d.xyz);
          u_xlat4.xyz = ((u_xlat3.xyz * u_xlat1_d.xyz) + float3(-2.69999982E-05, (-2.69999982E-05), (-2.69999982E-05)));
          u_xlat1_d.xyz = ((u_xlat3.xyz * u_xlat1_d.xyz) + float3(-0.941192091, (-0.941192091), (-0.941192091)));
          u_xlat10.x = dot(u_xlat1_d.xyz, u_xlat1_d.xyz);
          u_xlat1_d.x = dot(u_xlat4.xyz, u_xlat4.xyz);
          u_xlat1_d.y = min(u_xlat10.x, 1000);
          u_xlatb10 = (u_xlat10.x<1000);
          u_xlat10.x = (u_xlatb10)?(0.980000019):(float(0));
          u_xlatb15 = (u_xlat1_d.x<u_xlat1_d.y);
          u_xlat1_d.z = 1000;
          u_xlat1_d.xy = (int(u_xlatb15))?(u_xlat1_d.xy):(u_xlat1_d.yz);
          u_xlat1_d.xy = sqrt(u_xlat1_d.xy);
          u_xlat1_d.x = (u_xlat1_d.y + u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.y / u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.x + 1);
          u_xlatb5 = (u_xlat1_d.x<u_xlat5.x);
          u_xlat1_d.x = (u_xlatb15)?(0.0299999993):(u_xlat10.x);
          u_xlat10.x = (u_xlatb15)?(u_xlat10.x):(float(0));
          u_xlat5.x = (u_xlatb5)?(u_xlat10.x):(u_xlat1_d.x);
          u_xlat5.x = (u_xlat0_d.x * u_xlat5.x);
          out_f.color.w = u_xlat0_d.x;
          u_xlat0_d.xz = (in_f.texcoord3.xy / in_f.texcoord3.ww);
          u_xlat10_0.xzw = tex2D(_LightBuffer, u_xlat0_d.xz).xyz;
          u_xlat16_2_d.xyz = max(u_xlat10_0.xzw, float3(0.00100000005, 0.00100000005, 0.00100000005));
          u_xlat16_2_d.xyz = log2(u_xlat16_2_d.xyz);
          u_xlat0_d.xzw = ((-u_xlat16_2_d.xyz) + in_f.texcoord5.xyz);
          out_f.color.xyz = (u_xlat0_d.xzw * u_xlat5.xxx);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 5, name: DEFERRED
    {
      Name "DEFERRED"
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "DEFERRED"
        "PreviewType" = "Plane"
        "RenderType" = "TransparentCutout"
      }
      ZWrite Off
      Cull Off
      Blend Zero Zero
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
      //uniform float4 _ScreenParams;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform float4 _MainTex_ST;
      uniform float _Size;
      uniform float _Offset2;
      uniform float _Alpha;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 texcoord4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float4 texcoord3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
          float4 color1 :SV_Target1;
          float4 color2 :SV_Target2;
          float4 color3 :SV_Target3;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float2 u_xlat4;
      float u_xlat6;
      float roundEven(float x)
      {
          float y = floor((x + 0.5));
          return (((y - x)==0.5))?((floor((0.5 * y)) * 2)):(y);
      }
      
      float2 roundEven(float2 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          return a;
      }
      
      float3 roundEven(float3 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          return a;
      }
      
      float4 roundEven(float4 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          a.w = round(a.w);
          return a;
      }
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.xy = (in_v.vertex.xy / in_v.vertex.ww);
          u_xlat4.xy = (_ScreenParams.xy * float2(0.5, 0.5));
          u_xlat0.xy = (u_xlat4.xy * u_xlat0.xy);
          u_xlat0.xy = round(u_xlat0.xy);
          u_xlat0.xy = (u_xlat0.xy / u_xlat4.xy);
          u_xlat0.xy = (u_xlat0.xy * in_v.vertex.ww);
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * u_xlat0.xxxx) + u_xlat1);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.vertex.zzzz) + u_xlat0);
          u_xlat1 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          out_v.texcoord2.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz);
          out_v.vertex = mul(unity_MatrixVP, u_xlat1);
          out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          out_v.texcoord1.xyz = normalize(u_xlat0.xyz);
          out_v.texcoord3 = (in_v.color * _Color);
          out_v.texcoord4 = float4(0, 0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float4 u_xlat1_d;
      float4 u_xlat10_1;
      float u_xlat16_2;
      float3 u_xlat3;
      float3 u_xlat4_d;
      float2 u_xlat5;
      int u_xlatb5;
      float2 u_xlat10;
      int u_xlatb10;
      int u_xlatb15;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = ((-_Alpha) + 1);
          u_xlat5.x = (_Size * 64);
          u_xlat10.xy = (u_xlat5.xx * in_f.texcoord.xy);
          u_xlat10.xy = floor(u_xlat10.xy);
          u_xlat5.xy = (u_xlat10.xy / u_xlat5.xx);
          u_xlat10_1 = tex2D(_MainTex, u_xlat5.xy);
          u_xlat5.x = (u_xlat5.y + u_xlat5.x);
          u_xlat1_d = (u_xlat10_1 * in_f.texcoord3);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat1_d.w);
          u_xlatb10 = (u_xlat1_d.w<0.949999988);
          u_xlat1_d.xyz = (u_xlat1_d.xyz * float3(float3(_Offset2, _Offset2, _Offset2)));
          u_xlat0_d.x = (u_xlatb10)?(0):(u_xlat0_d.x);
          u_xlat16_2 = (u_xlat0_d.x + (-0.0500000007));
          u_xlatb10 = (u_xlat16_2<0);
          if(((int(u_xlatb10) * (-1))!=0))
          {
              discard;
          }
          u_xlat10.x = (u_xlat5.x * 0.5);
          u_xlat10.x = floor(u_xlat10.x);
          u_xlat5.x = (((-u_xlat10.x) * 2) + u_xlat5.x);
          u_xlat3.xyz = (u_xlat1_d.xyz * u_xlat1_d.xyz);
          u_xlat4_d.xyz = ((u_xlat3.xyz * u_xlat1_d.xyz) + float3(-2.69999982E-05, (-2.69999982E-05), (-2.69999982E-05)));
          u_xlat1_d.xyz = ((u_xlat3.xyz * u_xlat1_d.xyz) + float3(-0.941192091, (-0.941192091), (-0.941192091)));
          u_xlat10.x = dot(u_xlat1_d.xyz, u_xlat1_d.xyz);
          u_xlat1_d.x = dot(u_xlat4_d.xyz, u_xlat4_d.xyz);
          u_xlat1_d.y = min(u_xlat10.x, 1000);
          u_xlatb10 = (u_xlat10.x<1000);
          u_xlat10.x = (u_xlatb10)?(0.980000019):(float(0));
          u_xlatb15 = (u_xlat1_d.x<u_xlat1_d.y);
          u_xlat1_d.z = 1000;
          u_xlat1_d.xy = (int(u_xlatb15))?(u_xlat1_d.xy):(u_xlat1_d.yz);
          u_xlat1_d.xy = sqrt(u_xlat1_d.xy);
          u_xlat1_d.x = (u_xlat1_d.y + u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.y / u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.x + 1);
          u_xlatb5 = (u_xlat1_d.x<u_xlat5.x);
          u_xlat1_d.x = (u_xlatb15)?(0.0299999993):(u_xlat10.x);
          u_xlat10.x = (u_xlatb15)?(u_xlat10.x):(float(0));
          u_xlat5.x = (u_xlatb5)?(u_xlat10.x):(u_xlat1_d.x);
          u_xlat0_d.xyz = (u_xlat0_d.xxx * u_xlat5.xxx);
          out_f.color.xyz = u_xlat0_d.xyz;
          out_f.color.w = 1;
          out_f.color1 = float4(0, 0, 0, 0);
          u_xlat0_d.xyz = ((in_f.texcoord1.xyz * float3(0.5, 0.5, 0.5)) + float3(0.5, 0.5, 0.5));
          u_xlat0_d.w = 1;
          out_f.color2 = u_xlat0_d;
          out_f.color3 = float4(1, 1, 1, 1);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 6, name: ShadowCaster
    {
      Name "ShadowCaster"
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "SHADOWCASTER"
        "PreviewType" = "Plane"
        "RenderType" = "TransparentCutout"
        "SHADOWSUPPORT" = "true"
      }
      Cull Off
      Blend Zero Zero
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile SHADOWS_DEPTH
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _ScreenParams;
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4 unity_LightShadowBias;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform float4 _MainTex_ST;
      uniform float _Size;
      uniform float _Alpha;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float2 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 texcoord3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 texcoord1 :TEXCOORD1;
          float4 texcoord3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float2 u_xlat1;
      float4 u_xlat2;
      float4 u_xlat3;
      float u_xlat8;
      float2 u_xlat9;
      float u_xlat12;
      int u_xlatb12;
      float roundEven(float x)
      {
          float y = floor((x + 0.5));
          return (((y - x)==0.5))?((floor((0.5 * y)) * 2)):(y);
      }
      
      float2 roundEven(float2 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          return a;
      }
      
      float3 roundEven(float3 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          return a;
      }
      
      float4 roundEven(float4 a)
      {
          a.x = round(a.x);
          a.y = round(a.y);
          a.z = round(a.z);
          a.w = round(a.w);
          return a;
      }
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat0.xyz = normalize(u_xlat0.xyz);
          u_xlat1.xy = (in_v.vertex.xy / in_v.vertex.ww);
          u_xlat9.xy = (_ScreenParams.xy * float2(0.5, 0.5));
          u_xlat1.xy = (u_xlat9.xy * u_xlat1.xy);
          u_xlat1.xy = round(u_xlat1.xy);
          u_xlat1.xy = (u_xlat1.xy / u_xlat9.xy);
          u_xlat1.xy = (u_xlat1.xy * in_v.vertex.ww);
          u_xlat2 = mul(unity_ObjectToWorld, u_xlat1);
          u_xlat3.xyz = (((-u_xlat2.xyz) * _WorldSpaceLightPos0.www) + _WorldSpaceLightPos0.xyz);
          u_xlat3.xyz = normalize(u_xlat3.xyz);
          u_xlat12 = dot(u_xlat0.xyz, u_xlat3.xyz);
          u_xlat12 = (((-u_xlat12) * u_xlat12) + 1);
          u_xlat12 = sqrt(u_xlat12);
          u_xlat12 = (u_xlat12 * unity_LightShadowBias.z);
          u_xlat0.xyz = (((-u_xlat0.xyz) * float3(u_xlat12, u_xlat12, u_xlat12)) + u_xlat2.xyz);
          u_xlatb12 = (unity_LightShadowBias.z!=0);
          u_xlat0.xyz = (int(u_xlatb12))?(u_xlat0.xyz):(u_xlat2.xyz);
          u_xlat0 = mul(unity_MatrixVP, u_xlat0);
          u_xlat9.x = (unity_LightShadowBias.x / u_xlat0.w);
          u_xlat9.x = clamp(u_xlat9.x, 0, 1);
          u_xlat8 = (u_xlat0.z + u_xlat9.x);
          u_xlat9.x = max((-u_xlat0.w), u_xlat8);
          out_v.vertex.xyw = u_xlat0.xyw;
          u_xlat0.x = ((-u_xlat8) + u_xlat9.x);
          out_v.vertex.z = ((unity_LightShadowBias.y * u_xlat0.x) + u_xlat8);
          out_v.texcoord1.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          u_xlat0.xyz = (u_xlat1.yyy * conv_mxt4x4_1(unity_ObjectToWorld).xyz);
          u_xlat0.xyz = ((conv_mxt4x4_0(unity_ObjectToWorld).xyz * u_xlat1.xxx) + u_xlat0.xyz);
          u_xlat0.xyz = ((conv_mxt4x4_2(unity_ObjectToWorld).xyz * in_v.vertex.zzz) + u_xlat0.xyz);
          out_v.texcoord2.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz);
          out_v.texcoord3 = (in_v.color * _Color);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float2 u_xlat0_d;
      float u_xlat10_0;
      float2 u_xlatb0;
      float2 u_xlat1_d;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.x = (_Size * 64);
          u_xlat1_d.xy = (u_xlat0_d.xx * in_f.texcoord1.xy);
          u_xlat1_d.xy = floor(u_xlat1_d.xy);
          u_xlat0_d.xy = (u_xlat1_d.xy / u_xlat0_d.xx);
          u_xlat10_0 = tex2D(_MainTex, u_xlat0_d.xy).w;
          u_xlat0_d.x = (u_xlat10_0 * in_f.texcoord3.w);
          u_xlat1_d.x = ((-_Alpha) + 1);
          u_xlat0_d.y = ((u_xlat0_d.x * u_xlat1_d.x) + (-0.0500000007));
          u_xlatb0.xy = bool4(u_xlat0_d.xyxx < float4(0.949999988, 0, 0, 0)).xy;
          u_xlatb0.x = (u_xlatb0.y || u_xlatb0.x);
          if(((int(u_xlatb0.x) * (-1))!=0))
          {
              discard;
          }
          out_f.color = float4(0, 0, 0, 0);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
