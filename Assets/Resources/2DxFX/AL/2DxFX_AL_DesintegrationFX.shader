Shader "2DxFX/AL/DesintegrationFX"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("_Color", Color) = (1,1,1,1)
    _Distortion ("Distortion", Range(0, 1)) = 0
    _ColorX ("Tint", Color) = (1,1,1,1)
    _Size ("Size", Range(0, 1)) = 0
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
      uniform float _Distortion;
      uniform float4 _ColorX;
      uniform float _Alpha;
      uniform float _Size;
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
      float4 u_xlat0_d;
      float4 u_xlat10_0;
      float u_xlat1_d;
      int u_xlatb1;
      float3 u_xlat2;
      float2 u_xlat3;
      float4 u_xlat4_d;
      float u_xlat16_5;
      float3 u_xlat6_d;
      float3 u_xlat7;
      float u_xlat8;
      float3 u_xlat16_11;
      float2 u_xlat13;
      int u_xlati14;
      float2 u_xlat15;
      float u_xlat20;
      int u_xlatb20;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat0_d = (u_xlat10_0.wxyz * in_f.texcoord3.wxyz);
          u_xlat1_d = (_Distortion * 0.999899983);
          u_xlat1_d = frac(u_xlat1_d);
          u_xlat7.x = (u_xlat1_d + 0.100000001);
          u_xlat13.xy = (in_f.texcoord.xy * float2(3.5, 3.5));
          u_xlat2.x = float(0);
          u_xlat8 = float(2);
          int u_xlati_loop_1 = int(0);
          while((u_xlati_loop_1<9))
          {
              u_xlat3.xy = (u_xlat13.xy * float2(u_xlat8, u_xlat8));
              u_xlat3.xy = floor(u_xlat3.xy);
              u_xlat15.xy = ((float2(u_xlat8, u_xlat8) * u_xlat13.xy) + (-u_xlat3.xy));
              u_xlat4_d.xy = (u_xlat15.xy * u_xlat15.xy);
              u_xlat15.xy = ((u_xlat15.xy * float2(-2, (-2))) + float2(3, 3));
              u_xlat15.xy = (u_xlat15.xy * u_xlat4_d.xy);
              u_xlat20 = ((u_xlat3.y * 7) + u_xlat3.x);
              u_xlat20 = sin(u_xlat20);
              u_xlat20 = (u_xlat20 * _Size);
              u_xlat20 = (u_xlat20 * 43);
              u_xlat20 = frac(u_xlat20);
              u_xlat4_d = (u_xlat3.xyxy + float4(1, 0, 0, 1));
              u_xlat4_d.xy = ((u_xlat4_d.yw * float2(7, 7)) + u_xlat4_d.xz);
              u_xlat4_d.xy = sin(u_xlat4_d.xy);
              u_xlat4_d.xy = (u_xlat4_d.xy * float2(float2(_Size, _Size)));
              u_xlat4_d.xy = (u_xlat4_d.xy * float2(43, 43));
              u_xlat4_d.xy = frac(u_xlat4_d.xy);
              u_xlat4_d.x = ((-u_xlat20) + u_xlat4_d.x);
              u_xlat20 = ((u_xlat15.x * u_xlat4_d.x) + u_xlat20);
              u_xlat3.xy = (u_xlat3.xy + float2(1, 1));
              u_xlat3.x = ((u_xlat3.y * 7) + u_xlat3.x);
              u_xlat3.x = sin(u_xlat3.x);
              u_xlat3.x = (u_xlat3.x * _Size);
              u_xlat3.x = (u_xlat3.x * 43);
              u_xlat3.x = frac(u_xlat3.x);
              u_xlat3.x = ((-u_xlat4_d.y) + u_xlat3.x);
              u_xlat3.x = ((u_xlat15.x * u_xlat3.x) + u_xlat4_d.y);
              u_xlat3.x = ((-u_xlat20) + u_xlat3.x);
              u_xlat20 = ((u_xlat15.y * u_xlat3.x) + u_xlat20);
              u_xlat20 = (u_xlat20 / u_xlat8);
              u_xlat2.x = (u_xlat20 + u_xlat2.x);
              u_xlat8 = (u_xlat8 + u_xlat8);
              u_xlati_loop_1 = (u_xlati_loop_1 + 1);
          }
          u_xlat7.x = (((-u_xlat1_d) * 0.833333313) + u_xlat7.x);
          u_xlat1_d = (((-u_xlat1_d) * 0.833333313) + u_xlat2.x);
          u_xlat7.x = (float(1) / u_xlat7.x);
          u_xlat1_d = (u_xlat7.x * u_xlat1_d);
          u_xlat1_d = clamp(u_xlat1_d, 0, 1);
          u_xlat7.x = ((u_xlat1_d * (-2)) + 3);
          u_xlat1_d = (u_xlat1_d * u_xlat1_d);
          u_xlat1_d = (u_xlat1_d * u_xlat7.x);
          u_xlat7.xyz = (u_xlat0_d.yzw * float3(u_xlat1_d, u_xlat1_d, u_xlat1_d));
          u_xlat2.x = (((-u_xlat0_d.x) * u_xlat1_d) + 1);
          u_xlat2.xyz = ((u_xlat7.xyz * u_xlat2.xxx) + _ColorX.xyz);
          u_xlat6_d.xyz = (((-u_xlat0_d.yzw) * float3(u_xlat1_d, u_xlat1_d, u_xlat1_d)) + u_xlat2.xyz);
          u_xlat6_d.xyz = ((float3(_Distortion, _Distortion, _Distortion) * u_xlat6_d.xyz) + u_xlat7.xyz);
          u_xlat0_d.x = ((u_xlat0_d.x * u_xlat1_d) + (-_Alpha));
          u_xlat6_d.xyz = (u_xlat0_d.xxx * u_xlat6_d.xyz);
          u_xlat16_5 = (u_xlat0_d.x + (-0.0500000007));
          u_xlatb1 = (u_xlat16_5<0);
          if(((int(u_xlatb1) * (-1))!=0))
          {
              discard;
          }
          u_xlat16_5 = dot(in_f.texcoord1.xyz, _WorldSpaceLightPos0.xyz);
          u_xlat16_5 = max(u_xlat16_5, 0);
          u_xlat16_11.xyz = (u_xlat6_d.xyz * _LightColor0.xyz);
          out_f.color.xyz = (float3(u_xlat16_5, u_xlat16_5, u_xlat16_5) * u_xlat16_11.xyz);
          out_f.color.w = u_xlat0_d.x;
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
      uniform float _Distortion;
      uniform float4 _ColorX;
      uniform float _Alpha;
      uniform float _Size;
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
      float3 u_xlat0_d;
      float4 u_xlat1_d;
      float4 u_xlat10_1;
      float3 u_xlat2_d;
      float3 u_xlat3;
      float2 u_xlat4;
      float4 u_xlat5;
      float3 u_xlat16_6;
      float3 u_xlat8;
      float2 u_xlat9;
      float u_xlat10_d;
      float2 u_xlat17;
      int u_xlatb17;
      float2 u_xlat18;
      float u_xlat21;
      float u_xlat22;
      int u_xlatb22;
      float u_xlat23;
      int u_xlati23;
      float u_xlat25;
      float u_xlat16_27;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.xyz = ((-in_f.texcoord2.xyz) + _WorldSpaceLightPos0.xyz);
          u_xlat0_d.xyz = normalize(u_xlat0_d.xyz);
          u_xlat10_1 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat1_d = (u_xlat10_1.wxyz * in_f.texcoord3.wxyz);
          u_xlat21 = (_Distortion * 0.999899983);
          u_xlat21 = frac(u_xlat21);
          u_xlat2_d.x = (u_xlat21 + 0.100000001);
          u_xlat9.xy = (in_f.texcoord.xy * float2(3.5, 3.5));
          u_xlat3.x = float(0);
          u_xlat10_d = float(2);
          int u_xlati_loop_1 = 0;
          while((u_xlati_loop_1<9))
          {
              u_xlat17.xy = (u_xlat9.xy * float2(u_xlat10_d, u_xlat10_d));
              u_xlat17.xy = floor(u_xlat17.xy);
              u_xlat4.xy = ((float2(u_xlat10_d, u_xlat10_d) * u_xlat9.xy) + (-u_xlat17.xy));
              u_xlat18.xy = (u_xlat4.xy * u_xlat4.xy);
              u_xlat4.xy = ((u_xlat4.xy * float2(-2, (-2))) + float2(3, 3));
              u_xlat4.xy = (u_xlat4.xy * u_xlat18.xy);
              u_xlat18.x = ((u_xlat17.y * 7) + u_xlat17.x);
              u_xlat18.x = sin(u_xlat18.x);
              u_xlat18.x = (u_xlat18.x * _Size);
              u_xlat18.x = (u_xlat18.x * 43);
              u_xlat18.x = frac(u_xlat18.x);
              u_xlat5 = (u_xlat17.xyxy + float4(1, 0, 0, 1));
              u_xlat5.xy = ((u_xlat5.yw * float2(7, 7)) + u_xlat5.xz);
              u_xlat5.xy = sin(u_xlat5.xy);
              u_xlat5.xy = (u_xlat5.xy * float2(float2(_Size, _Size)));
              u_xlat5.xy = (u_xlat5.xy * float2(43, 43));
              u_xlat5.xy = frac(u_xlat5.xy);
              u_xlat25 = ((-u_xlat18.x) + u_xlat5.x);
              u_xlat18.x = ((u_xlat4.x * u_xlat25) + u_xlat18.x);
              u_xlat17.xy = (u_xlat17.xy + float2(1, 1));
              u_xlat17.x = ((u_xlat17.y * 7) + u_xlat17.x);
              u_xlat17.x = sin(u_xlat17.x);
              u_xlat17.x = (u_xlat17.x * _Size);
              u_xlat17.x = (u_xlat17.x * 43);
              u_xlat17.x = frac(u_xlat17.x);
              u_xlat17.x = ((-u_xlat5.y) + u_xlat17.x);
              u_xlat17.x = ((u_xlat4.x * u_xlat17.x) + u_xlat5.y);
              u_xlat17.x = ((-u_xlat18.x) + u_xlat17.x);
              u_xlat17.x = ((u_xlat4.y * u_xlat17.x) + u_xlat18.x);
              u_xlat17.x = (u_xlat17.x / u_xlat10_d);
              u_xlat3.x = (u_xlat17.x + u_xlat3.x);
              u_xlat10_d = (u_xlat10_d + u_xlat10_d);
              u_xlati_loop_1 = (u_xlati_loop_1 + 1);
          }
          u_xlat2_d.x = (((-u_xlat21) * 0.833333313) + u_xlat2_d.x);
          u_xlat21 = (((-u_xlat21) * 0.833333313) + u_xlat3.x);
          u_xlat2_d.x = (float(1) / u_xlat2_d.x);
          u_xlat21 = (u_xlat21 * u_xlat2_d.x);
          u_xlat21 = clamp(u_xlat21, 0, 1);
          u_xlat2_d.x = ((u_xlat21 * (-2)) + 3);
          u_xlat21 = (u_xlat21 * u_xlat21);
          u_xlat21 = (u_xlat21 * u_xlat2_d.x);
          u_xlat2_d.xyz = (float3(u_xlat21, u_xlat21, u_xlat21) * u_xlat1_d.yzw);
          u_xlat23 = (((-u_xlat1_d.x) * u_xlat21) + 1);
          u_xlat3.xyz = ((u_xlat2_d.xyz * float3(u_xlat23, u_xlat23, u_xlat23)) + _ColorX.xyz);
          u_xlat8.xyz = (((-u_xlat1_d.yzw) * float3(u_xlat21, u_xlat21, u_xlat21)) + u_xlat3.xyz);
          u_xlat8.xyz = ((float3(_Distortion, _Distortion, _Distortion) * u_xlat8.xyz) + u_xlat2_d.xyz);
          u_xlat21 = ((u_xlat1_d.x * u_xlat21) + (-_Alpha));
          u_xlat1_d.xyz = (float3(u_xlat21, u_xlat21, u_xlat21) * u_xlat8.xyz);
          u_xlat16_6.x = (u_xlat21 + (-0.0500000007));
          u_xlatb22 = (u_xlat16_6.x<0);
          if(((int(u_xlatb22) * (-1))!=0))
          {
              discard;
          }
          u_xlat2_d.xyz = (in_f.texcoord2.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz);
          u_xlat2_d.xyz = ((conv_mxt4x4_0(unity_WorldToLight).xyz * in_f.texcoord2.xxx) + u_xlat2_d.xyz);
          u_xlat2_d.xyz = ((conv_mxt4x4_2(unity_WorldToLight).xyz * in_f.texcoord2.zzz) + u_xlat2_d.xyz);
          u_xlat2_d.xyz = (u_xlat2_d.xyz + conv_mxt4x4_3(unity_WorldToLight).xyz);
          u_xlat22 = dot(u_xlat2_d.xyz, u_xlat2_d.xyz);
          u_xlat22 = tex2D(_LightTexture0, float2(u_xlat22, u_xlat22)).x;
          u_xlat16_6.xyz = (float3(u_xlat22, u_xlat22, u_xlat22) * _LightColor0.xyz);
          u_xlat16_27 = dot(in_f.texcoord1.xyz, u_xlat0_d.xyz);
          u_xlat16_27 = max(u_xlat16_27, 0);
          u_xlat16_6.xyz = (u_xlat1_d.xyz * u_xlat16_6.xyz);
          out_f.color.xyz = (float3(u_xlat16_27, u_xlat16_27, u_xlat16_27) * u_xlat16_6.xyz);
          out_f.color.w = u_xlat21;
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
      uniform float _Distortion;
      uniform float _Alpha;
      uniform float _Size;
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
      float u_xlat0_d;
      float u_xlat10_0;
      int u_xlatb0;
      float2 u_xlat1_d;
      float2 u_xlat2;
      int u_xlatb2;
      float3 u_xlat3;
      float4 u_xlat4_d;
      float u_xlat16_5;
      float u_xlat6_d;
      float2 u_xlat9;
      float u_xlat12;
      float u_xlat13;
      float2 u_xlat14;
      int u_xlati18;
      float u_xlat19;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy).w;
          u_xlat0_d = (u_xlat10_0 * in_f.texcoord3.w);
          u_xlat6_d = (_Distortion * 0.999899983);
          u_xlat6_d = frac(u_xlat6_d);
          u_xlat12 = (u_xlat6_d + 0.100000001);
          u_xlat1_d.xy = (in_f.texcoord.xy * float2(3.5, 3.5));
          u_xlat13 = float(0);
          u_xlat19 = float(2);
          int u_xlati_loop_1 = 0;
          while((u_xlati_loop_1<9))
          {
              u_xlat2.xy = (u_xlat1_d.xy * float2(u_xlat19, u_xlat19));
              u_xlat2.xy = floor(u_xlat2.xy);
              u_xlat14.xy = ((float2(u_xlat19, u_xlat19) * u_xlat1_d.xy) + (-u_xlat2.xy));
              u_xlat3.xy = (u_xlat14.xy * u_xlat14.xy);
              u_xlat14.xy = ((u_xlat14.xy * float2(-2, (-2))) + float2(3, 3));
              u_xlat14.xy = (u_xlat14.xy * u_xlat3.xy);
              u_xlat3.x = ((u_xlat2.y * 7) + u_xlat2.x);
              u_xlat3.x = sin(u_xlat3.x);
              u_xlat3.x = (u_xlat3.x * _Size);
              u_xlat3.x = (u_xlat3.x * 43);
              u_xlat4_d = (u_xlat2.xyxy + float4(1, 0, 0, 1));
              u_xlat9.xy = ((u_xlat4_d.yw * float2(7, 7)) + u_xlat4_d.xz);
              u_xlat9.xy = sin(u_xlat9.xy);
              u_xlat9.xy = (u_xlat9.xy * float2(float2(_Size, _Size)));
              u_xlat3.yz = (u_xlat9.xy * float2(43, 43));
              u_xlat3.xyz = frac(u_xlat3.xyz);
              u_xlat9.x = ((-u_xlat3.x) + u_xlat3.y);
              u_xlat3.x = ((u_xlat14.x * u_xlat9.x) + u_xlat3.x);
              u_xlat2.xy = (u_xlat2.xy + float2(1, 1));
              u_xlat2.x = ((u_xlat2.y * 7) + u_xlat2.x);
              u_xlat2.x = sin(u_xlat2.x);
              u_xlat2.x = (u_xlat2.x * _Size);
              u_xlat2.x = (u_xlat2.x * 43);
              u_xlat2.x = frac(u_xlat2.x);
              u_xlat2.x = ((-u_xlat3.z) + u_xlat2.x);
              u_xlat2.x = ((u_xlat14.x * u_xlat2.x) + u_xlat3.z);
              u_xlat2.x = ((-u_xlat3.x) + u_xlat2.x);
              u_xlat2.x = ((u_xlat14.y * u_xlat2.x) + u_xlat3.x);
              u_xlat2.x = (u_xlat2.x / u_xlat19);
              u_xlat13 = (u_xlat13 + u_xlat2.x);
              u_xlat19 = (u_xlat19 + u_xlat19);
              u_xlati_loop_1 = (u_xlati_loop_1 + 1);
          }
          u_xlat12 = (((-u_xlat6_d) * 0.833333313) + u_xlat12);
          u_xlat6_d = (((-u_xlat6_d) * 0.833333313) + u_xlat13);
          u_xlat12 = (float(1) / u_xlat12);
          u_xlat6_d = (u_xlat12 * u_xlat6_d);
          u_xlat6_d = clamp(u_xlat6_d, 0, 1);
          u_xlat12 = ((u_xlat6_d * (-2)) + 3);
          u_xlat6_d = (u_xlat6_d * u_xlat6_d);
          u_xlat6_d = (u_xlat6_d * u_xlat12);
          u_xlat0_d = ((u_xlat0_d * u_xlat6_d) + (-_Alpha));
          u_xlat16_5 = (u_xlat0_d + (-0.0500000007));
          u_xlatb0 = (u_xlat16_5<0);
          if(((int(u_xlatb0) * (-1))!=0))
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
      uniform float _Distortion;
      uniform float4 _ColorX;
      uniform float _Alpha;
      uniform float _Size;
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
      float3 u_xlat1_d;
      float3 u_xlat10_1;
      int u_xlatb1;
      float3 u_xlat2;
      float2 u_xlat3;
      float4 u_xlat4;
      float3 u_xlat16_5;
      float3 u_xlat6;
      float3 u_xlat7;
      float u_xlat8_d;
      float2 u_xlat13;
      int u_xlati14;
      float2 u_xlat15;
      float u_xlat20;
      int u_xlatb20;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat0_d = (u_xlat10_0.wxyz * in_f.texcoord2.wxyz);
          u_xlat1_d.x = (_Distortion * 0.999899983);
          u_xlat1_d.x = frac(u_xlat1_d.x);
          u_xlat7.x = (u_xlat1_d.x + 0.100000001);
          u_xlat13.xy = (in_f.texcoord.xy * float2(3.5, 3.5));
          u_xlat2.x = float(0);
          u_xlat8_d = float(2);
          int u_xlati_loop_1 = int(0);
          while((u_xlati_loop_1<9))
          {
              u_xlat3.xy = (u_xlat13.xy * float2(u_xlat8_d, u_xlat8_d));
              u_xlat3.xy = floor(u_xlat3.xy);
              u_xlat15.xy = ((float2(u_xlat8_d, u_xlat8_d) * u_xlat13.xy) + (-u_xlat3.xy));
              u_xlat4.xy = (u_xlat15.xy * u_xlat15.xy);
              u_xlat15.xy = ((u_xlat15.xy * float2(-2, (-2))) + float2(3, 3));
              u_xlat15.xy = (u_xlat15.xy * u_xlat4.xy);
              u_xlat20 = ((u_xlat3.y * 7) + u_xlat3.x);
              u_xlat20 = sin(u_xlat20);
              u_xlat20 = (u_xlat20 * _Size);
              u_xlat20 = (u_xlat20 * 43);
              u_xlat20 = frac(u_xlat20);
              u_xlat4 = (u_xlat3.xyxy + float4(1, 0, 0, 1));
              u_xlat4.xy = ((u_xlat4.yw * float2(7, 7)) + u_xlat4.xz);
              u_xlat4.xy = sin(u_xlat4.xy);
              u_xlat4.xy = (u_xlat4.xy * float2(float2(_Size, _Size)));
              u_xlat4.xy = (u_xlat4.xy * float2(43, 43));
              u_xlat4.xy = frac(u_xlat4.xy);
              u_xlat4.x = ((-u_xlat20) + u_xlat4.x);
              u_xlat20 = ((u_xlat15.x * u_xlat4.x) + u_xlat20);
              u_xlat3.xy = (u_xlat3.xy + float2(1, 1));
              u_xlat3.x = ((u_xlat3.y * 7) + u_xlat3.x);
              u_xlat3.x = sin(u_xlat3.x);
              u_xlat3.x = (u_xlat3.x * _Size);
              u_xlat3.x = (u_xlat3.x * 43);
              u_xlat3.x = frac(u_xlat3.x);
              u_xlat3.x = ((-u_xlat4.y) + u_xlat3.x);
              u_xlat3.x = ((u_xlat15.x * u_xlat3.x) + u_xlat4.y);
              u_xlat3.x = ((-u_xlat20) + u_xlat3.x);
              u_xlat20 = ((u_xlat15.y * u_xlat3.x) + u_xlat20);
              u_xlat20 = (u_xlat20 / u_xlat8_d);
              u_xlat2.x = (u_xlat20 + u_xlat2.x);
              u_xlat8_d = (u_xlat8_d + u_xlat8_d);
              u_xlati_loop_1 = (u_xlati_loop_1 + 1);
          }
          u_xlat7.x = (((-u_xlat1_d.x) * 0.833333313) + u_xlat7.x);
          u_xlat1_d.x = (((-u_xlat1_d.x) * 0.833333313) + u_xlat2.x);
          u_xlat7.x = (float(1) / u_xlat7.x);
          u_xlat1_d.x = (u_xlat7.x * u_xlat1_d.x);
          u_xlat1_d.x = clamp(u_xlat1_d.x, 0, 1);
          u_xlat7.x = ((u_xlat1_d.x * (-2)) + 3);
          u_xlat1_d.x = (u_xlat1_d.x * u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.x * u_xlat7.x);
          u_xlat7.xyz = (u_xlat0_d.yzw * u_xlat1_d.xxx);
          u_xlat2.x = (((-u_xlat0_d.x) * u_xlat1_d.x) + 1);
          u_xlat2.xyz = ((u_xlat7.xyz * u_xlat2.xxx) + _ColorX.xyz);
          u_xlat6.xyz = (((-u_xlat0_d.yzw) * u_xlat1_d.xxx) + u_xlat2.xyz);
          u_xlat6.xyz = ((float3(_Distortion, _Distortion, _Distortion) * u_xlat6.xyz) + u_xlat7.xyz);
          u_xlat0_d.x = ((u_xlat0_d.x * u_xlat1_d.x) + (-_Alpha));
          u_xlat6.xyz = (u_xlat0_d.xxx * u_xlat6.xyz);
          u_xlat16_5.x = (u_xlat0_d.x + (-0.0500000007));
          u_xlatb1 = (u_xlat16_5.x<0);
          if(((int(u_xlatb1) * (-1))!=0))
          {
              discard;
          }
          u_xlat1_d.xy = (in_f.texcoord3.xy / in_f.texcoord3.ww);
          u_xlat10_1.xyz = tex2D(_LightBuffer, u_xlat1_d.xy).xyz;
          u_xlat16_5.xyz = max(u_xlat10_1.xyz, float3(0.00100000005, 0.00100000005, 0.00100000005));
          u_xlat16_5.xyz = log2(u_xlat16_5.xyz);
          u_xlat1_d.xyz = ((-u_xlat16_5.xyz) + in_f.texcoord5.xyz);
          out_f.color.xyz = (u_xlat6.xyz * u_xlat1_d.xyz);
          out_f.color.w = u_xlat0_d.x;
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
      uniform float _Distortion;
      uniform float4 _ColorX;
      uniform float _Alpha;
      uniform float _Size;
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
      float4 u_xlat10_0;
      int u_xlatb0;
      float u_xlat1_d;
      float3 u_xlat2;
      float2 u_xlat3;
      float4 u_xlat4_d;
      float u_xlat16_5;
      float3 u_xlat6_d;
      float3 u_xlat7;
      float u_xlat8;
      float2 u_xlat13;
      int u_xlati14;
      float2 u_xlat15;
      float u_xlat20;
      int u_xlatb20;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord.xy);
          u_xlat0_d = (u_xlat10_0.wxyz * in_f.texcoord3.wxyz);
          u_xlat1_d = (_Distortion * 0.999899983);
          u_xlat1_d = frac(u_xlat1_d);
          u_xlat7.x = (u_xlat1_d + 0.100000001);
          u_xlat13.xy = (in_f.texcoord.xy * float2(3.5, 3.5));
          u_xlat2.x = float(0);
          u_xlat8 = float(2);
          int u_xlati_loop_1 = int(0);
          while((u_xlati_loop_1<9))
          {
              u_xlat3.xy = (u_xlat13.xy * float2(u_xlat8, u_xlat8));
              u_xlat3.xy = floor(u_xlat3.xy);
              u_xlat15.xy = ((float2(u_xlat8, u_xlat8) * u_xlat13.xy) + (-u_xlat3.xy));
              u_xlat4_d.xy = (u_xlat15.xy * u_xlat15.xy);
              u_xlat15.xy = ((u_xlat15.xy * float2(-2, (-2))) + float2(3, 3));
              u_xlat15.xy = (u_xlat15.xy * u_xlat4_d.xy);
              u_xlat20 = ((u_xlat3.y * 7) + u_xlat3.x);
              u_xlat20 = sin(u_xlat20);
              u_xlat20 = (u_xlat20 * _Size);
              u_xlat20 = (u_xlat20 * 43);
              u_xlat20 = frac(u_xlat20);
              u_xlat4_d = (u_xlat3.xyxy + float4(1, 0, 0, 1));
              u_xlat4_d.xy = ((u_xlat4_d.yw * float2(7, 7)) + u_xlat4_d.xz);
              u_xlat4_d.xy = sin(u_xlat4_d.xy);
              u_xlat4_d.xy = (u_xlat4_d.xy * float2(float2(_Size, _Size)));
              u_xlat4_d.xy = (u_xlat4_d.xy * float2(43, 43));
              u_xlat4_d.xy = frac(u_xlat4_d.xy);
              u_xlat4_d.x = ((-u_xlat20) + u_xlat4_d.x);
              u_xlat20 = ((u_xlat15.x * u_xlat4_d.x) + u_xlat20);
              u_xlat3.xy = (u_xlat3.xy + float2(1, 1));
              u_xlat3.x = ((u_xlat3.y * 7) + u_xlat3.x);
              u_xlat3.x = sin(u_xlat3.x);
              u_xlat3.x = (u_xlat3.x * _Size);
              u_xlat3.x = (u_xlat3.x * 43);
              u_xlat3.x = frac(u_xlat3.x);
              u_xlat3.x = ((-u_xlat4_d.y) + u_xlat3.x);
              u_xlat3.x = ((u_xlat15.x * u_xlat3.x) + u_xlat4_d.y);
              u_xlat3.x = ((-u_xlat20) + u_xlat3.x);
              u_xlat20 = ((u_xlat15.y * u_xlat3.x) + u_xlat20);
              u_xlat20 = (u_xlat20 / u_xlat8);
              u_xlat2.x = (u_xlat20 + u_xlat2.x);
              u_xlat8 = (u_xlat8 + u_xlat8);
              u_xlati_loop_1 = (u_xlati_loop_1 + 1);
          }
          u_xlat7.x = (((-u_xlat1_d) * 0.833333313) + u_xlat7.x);
          u_xlat1_d = (((-u_xlat1_d) * 0.833333313) + u_xlat2.x);
          u_xlat7.x = (float(1) / u_xlat7.x);
          u_xlat1_d = (u_xlat7.x * u_xlat1_d);
          u_xlat1_d = clamp(u_xlat1_d, 0, 1);
          u_xlat7.x = ((u_xlat1_d * (-2)) + 3);
          u_xlat1_d = (u_xlat1_d * u_xlat1_d);
          u_xlat1_d = (u_xlat1_d * u_xlat7.x);
          u_xlat7.xyz = (u_xlat0_d.yzw * float3(u_xlat1_d, u_xlat1_d, u_xlat1_d));
          u_xlat2.x = (((-u_xlat0_d.x) * u_xlat1_d) + 1);
          u_xlat2.xyz = ((u_xlat7.xyz * u_xlat2.xxx) + _ColorX.xyz);
          u_xlat6_d.xyz = (((-u_xlat0_d.yzw) * float3(u_xlat1_d, u_xlat1_d, u_xlat1_d)) + u_xlat2.xyz);
          u_xlat6_d.xyz = ((float3(_Distortion, _Distortion, _Distortion) * u_xlat6_d.xyz) + u_xlat7.xyz);
          u_xlat0_d.x = ((u_xlat0_d.x * u_xlat1_d) + (-_Alpha));
          u_xlat6_d.xyz = (u_xlat0_d.xxx * u_xlat6_d.xyz);
          u_xlat16_5 = (u_xlat0_d.x + (-0.0500000007));
          u_xlatb0 = (u_xlat16_5<0);
          if(((int(u_xlatb0) * (-1))!=0))
          {
              discard;
          }
          out_f.color.xyz = u_xlat6_d.xyz;
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
      uniform float _Distortion;
      uniform float _Alpha;
      uniform float _Size;
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
      float u_xlat0_d;
      float u_xlat10_0;
      int u_xlatb0;
      float2 u_xlat1_d;
      float2 u_xlat2_d;
      int u_xlatb2;
      float3 u_xlat3_d;
      float4 u_xlat4;
      float u_xlat16_5;
      float u_xlat6;
      float2 u_xlat9_d;
      float u_xlat12_d;
      float u_xlat13;
      float2 u_xlat14;
      int u_xlati18;
      float u_xlat19;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.texcoord1.xy).w;
          u_xlat0_d = (u_xlat10_0 * in_f.texcoord3.w);
          u_xlat6 = (_Distortion * 0.999899983);
          u_xlat6 = frac(u_xlat6);
          u_xlat12_d = (u_xlat6 + 0.100000001);
          u_xlat1_d.xy = (in_f.texcoord1.xy * float2(3.5, 3.5));
          u_xlat13 = float(0);
          u_xlat19 = float(2);
          int u_xlati_loop_1 = 0;
          while((u_xlati_loop_1<9))
          {
              u_xlat2_d.xy = (u_xlat1_d.xy * float2(u_xlat19, u_xlat19));
              u_xlat2_d.xy = floor(u_xlat2_d.xy);
              u_xlat14.xy = ((float2(u_xlat19, u_xlat19) * u_xlat1_d.xy) + (-u_xlat2_d.xy));
              u_xlat3_d.xy = (u_xlat14.xy * u_xlat14.xy);
              u_xlat14.xy = ((u_xlat14.xy * float2(-2, (-2))) + float2(3, 3));
              u_xlat14.xy = (u_xlat14.xy * u_xlat3_d.xy);
              u_xlat3_d.x = ((u_xlat2_d.y * 7) + u_xlat2_d.x);
              u_xlat3_d.x = sin(u_xlat3_d.x);
              u_xlat3_d.x = (u_xlat3_d.x * _Size);
              u_xlat3_d.x = (u_xlat3_d.x * 43);
              u_xlat4 = (u_xlat2_d.xyxy + float4(1, 0, 0, 1));
              u_xlat9_d.xy = ((u_xlat4.yw * float2(7, 7)) + u_xlat4.xz);
              u_xlat9_d.xy = sin(u_xlat9_d.xy);
              u_xlat9_d.xy = (u_xlat9_d.xy * float2(float2(_Size, _Size)));
              u_xlat3_d.yz = (u_xlat9_d.xy * float2(43, 43));
              u_xlat3_d.xyz = frac(u_xlat3_d.xyz);
              u_xlat9_d.x = ((-u_xlat3_d.x) + u_xlat3_d.y);
              u_xlat3_d.x = ((u_xlat14.x * u_xlat9_d.x) + u_xlat3_d.x);
              u_xlat2_d.xy = (u_xlat2_d.xy + float2(1, 1));
              u_xlat2_d.x = ((u_xlat2_d.y * 7) + u_xlat2_d.x);
              u_xlat2_d.x = sin(u_xlat2_d.x);
              u_xlat2_d.x = (u_xlat2_d.x * _Size);
              u_xlat2_d.x = (u_xlat2_d.x * 43);
              u_xlat2_d.x = frac(u_xlat2_d.x);
              u_xlat2_d.x = ((-u_xlat3_d.z) + u_xlat2_d.x);
              u_xlat2_d.x = ((u_xlat14.x * u_xlat2_d.x) + u_xlat3_d.z);
              u_xlat2_d.x = ((-u_xlat3_d.x) + u_xlat2_d.x);
              u_xlat2_d.x = ((u_xlat14.y * u_xlat2_d.x) + u_xlat3_d.x);
              u_xlat2_d.x = (u_xlat2_d.x / u_xlat19);
              u_xlat13 = (u_xlat13 + u_xlat2_d.x);
              u_xlat19 = (u_xlat19 + u_xlat19);
              u_xlati_loop_1 = (u_xlati_loop_1 + 1);
          }
          u_xlat12_d = (((-u_xlat6) * 0.833333313) + u_xlat12_d);
          u_xlat6 = (((-u_xlat6) * 0.833333313) + u_xlat13);
          u_xlat12_d = (float(1) / u_xlat12_d);
          u_xlat6 = (u_xlat12_d * u_xlat6);
          u_xlat6 = clamp(u_xlat6, 0, 1);
          u_xlat12_d = ((u_xlat6 * (-2)) + 3);
          u_xlat6 = (u_xlat6 * u_xlat6);
          u_xlat6 = (u_xlat6 * u_xlat12_d);
          u_xlat0_d = ((u_xlat0_d * u_xlat6) + (-_Alpha));
          u_xlat16_5 = (u_xlat0_d + (-0.0500000007));
          u_xlatb0 = (u_xlat16_5<0);
          if(((int(u_xlatb0) * (-1))!=0))
          {
              discard;
          }
          out_f.color = float4(0, 0, 0, 0);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Transparent/VertexLit"
}
