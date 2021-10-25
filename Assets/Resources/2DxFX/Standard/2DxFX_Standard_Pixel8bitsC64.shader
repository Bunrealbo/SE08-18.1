Shader "2DxFX/Standard/Pixel8bitsC64"
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
      uniform float _Offset;
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
      float4 u_xlat10_1;
      float3 u_xlat2;
      int u_xlatb2;
      float4 u_xlat3;
      int u_xlatb3;
      float3 u_xlat4;
      float3 u_xlat5;
      int u_xlatb6;
      float2 u_xlat7;
      float3 u_xlat9;
      int u_xlatb9;
      float u_xlat13;
      float u_xlat15;
      float u_xlat18;
      int u_xlatb18;
      float u_xlat20;
      int u_xlatb20;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat0_d.z = 1000;
          u_xlat1_d.xy = float2((float2(_Size, _Offset) * float2(64, 3)));
          u_xlat18 = (u_xlat1_d.x / u_xlat1_d.y);
          u_xlat2.x = (u_xlat18 * in_f.texcoord.x);
          u_xlat2.y = (u_xlat1_d.x * in_f.texcoord.y);
          u_xlat7.xy = floor(u_xlat2.xy);
          u_xlat2.x = (u_xlat7.x / u_xlat18);
          u_xlat2.y = (u_xlat7.y / u_xlat1_d.x);
          u_xlat10_1 = tex2D(_MainTex, u_xlat2.xy);
          u_xlat18 = (u_xlat2.y + u_xlat2.x);
          u_xlat1_d = (u_xlat10_1 * in_f.color);
          u_xlat2.x = (_Offset2 * _Offset2);
          u_xlat1_d.xyz = (u_xlat1_d.xyz * u_xlat2.xxx);
          u_xlat2.xyz = (u_xlat1_d.xyz * u_xlat1_d.xyz);
          u_xlat3.xyz = (u_xlat1_d.xyz * u_xlat2.xyz);
          u_xlat20 = dot(u_xlat3.xyz, u_xlat3.xyz);
          u_xlat0_d.y = min(u_xlat20, 1000);
          u_xlat3.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-1, (-1), (-1)));
          u_xlat0_d.x = dot(u_xlat3.xyz, u_xlat3.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat20 = (u_xlatb20)?(1):(float(0));
          u_xlat3.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.248746932, (-0.0272114873), (-0.0179268718)));
          u_xlat0_d.x = dot(u_xlat3.xyz, u_xlat3.xyz);
          u_xlatb3 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb3))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat9.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.0709900856, (-0.428501189), (-0.476837158)));
          u_xlat0_d.x = dot(u_xlat9.xyz, u_xlat9.xyz);
          u_xlatb9 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb9))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat15 = (u_xlatb3)?(u_xlat20):(float(0));
          u_xlat4.xyz = float3((int(u_xlatb3))?(float3(0.62890625, 0.30078125, 0.26171875)):(float3(u_xlat20, u_xlat20, u_xlat20)));
          u_xlat3.xzw = (int(u_xlatb9))?(u_xlat4.xyz):(float3(u_xlat15, u_xlat15, u_xlat15));
          u_xlat4.xyz = (int(u_xlatb9))?(float3(0.4140625, 0.75390625, 0.78125)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.253410816, (-0.0392498374), (-0.267751515)));
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat3.xyz = (int(u_xlatb20))?(u_xlat4.xyz):(u_xlat3.xzw);
          u_xlat4.xyz = (int(u_xlatb20))?(float3(0.6328125, 0.33984375, 0.64453125)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.0464134216, (-0.308615983), (-0.0511035323)));
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat3.xyz = (int(u_xlatb20))?(u_xlat4.xyz):(u_xlat3.xyz);
          u_xlat4.xyz = (int(u_xlatb20))?(float3(0.359375, 0.67578125, 0.37109375)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.0293874145, (-0.0187416077), (-0.226284027)));
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat3.xyz = (int(u_xlatb20))?(u_xlat4.xyz):(u_xlat3.xyz);
          u_xlat4.xyz = (int(u_xlatb20))?(float3(0.30859375, 0.265625, 0.609375)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.498618305, (-0.584146023), (-0.153264582)));
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat3.xyz = (int(u_xlatb20))?(u_xlat4.xyz):(u_xlat3.xyz);
          u_xlat4.xyz = (int(u_xlatb20))?(float3(0.79296875, 0.8359375, 0.53515625)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.258132637, (-0.0670471191), (-0.0116295815)));
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat3.xyz = (int(u_xlatb20))?(u_xlat4.xyz):(u_xlat3.xyz);
          u_xlat4.xyz = (int(u_xlatb20))?(float3(0.63671875, 0.40625, 0.2265625)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.0793337822, (-0.034081161), (-7.93337822E-05)));
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat3.xyz = (int(u_xlatb20))?(u_xlat4.xyz):(u_xlat3.xyz);
          u_xlat4.xyz = (int(u_xlatb20))?(float3(0.4296875, 0.32421875, 0.04296875)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.506023407, (-0.122093141), (-0.0979323387)));
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat3.xyz = (int(u_xlatb20))?(u_xlat4.xyz):(u_xlat3.xyz);
          u_xlat4.xyz = (int(u_xlatb20))?(float3(0.796875, 0.49609375, 0.4609375)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.0578343272, (-0.0578343272), (-0.0578343272)));
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat3.xyz = (int(u_xlatb20))?(u_xlat4.xyz):(u_xlat3.xyz);
          u_xlat4.xyz = (int(u_xlatb20))?(float3(0.38671875, 0.38671875, 0.38671875)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.160075366, (-0.160075366), (-0.160075366)));
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat3.xyz = (int(u_xlatb20))?(u_xlat4.xyz):(u_xlat3.xyz);
          u_xlat4.xyz = (int(u_xlatb20))?(float3(0.54296875, 0.54296875, 0.54296875)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.221960247, (-0.697200477), (-0.230663598)));
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb20 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat0_d.yz = (int(u_xlatb20))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat3.xyz = (int(u_xlatb20))?(u_xlat4.xyz):(u_xlat3.xyz);
          u_xlat4.xyz = (int(u_xlatb20))?(float3(0.60546875, 0.88671875, 0.61328125)):(u_xlat4.xyz);
          u_xlat5.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.156645298, (-0.122093141), (-0.513501465)));
          u_xlat1_d.xyz = ((u_xlat2.xyz * u_xlat1_d.xyz) + float3(-0.319443643, (-0.319443643), (-0.319443643)));
          u_xlat1_d.x = dot(u_xlat1_d.xyz, u_xlat1_d.xyz);
          u_xlat0_d.x = dot(u_xlat5.xyz, u_xlat5.xyz);
          u_xlatb2 = (u_xlat0_d.x<u_xlat0_d.y);
          u_xlat1_d.yz = (int(u_xlatb2))?(u_xlat0_d.xy):(u_xlat0_d.yz);
          u_xlat0_d.xyz = (int(u_xlatb2))?(u_xlat4.xyz):(u_xlat3.xyz);
          u_xlat2.xyz = (int(u_xlatb2))?(float3(0.5390625, 0.49609375, 0.80078125)):(u_xlat4.xyz);
          u_xlatb20 = (u_xlat1_d.x<u_xlat1_d.y);
          u_xlat1_d.xy = (int(u_xlatb20))?(u_xlat1_d.xy):(u_xlat1_d.yz);
          u_xlat1_d.xy = sqrt(u_xlat1_d.xy);
          u_xlat0_d.xyz = (int(u_xlatb20))?(u_xlat2.xyz):(u_xlat0_d.xyz);
          u_xlat2.xyz = (int(u_xlatb20))?(float3(0.68359375, 0.68359375, 0.68359375)):(u_xlat2.xyz);
          u_xlat13 = (u_xlat18 * 0.5);
          u_xlat13 = floor(u_xlat13);
          u_xlat18 = (((-u_xlat13) * 2) + u_xlat18);
          u_xlat1_d.x = (u_xlat1_d.y + u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.y / u_xlat1_d.x);
          u_xlat1_d.x = (u_xlat1_d.x + 1);
          u_xlatb18 = (u_xlat1_d.x<u_xlat18);
          out_f.color.xyz = (int(u_xlatb18))?(u_xlat0_d.xyz):(u_xlat2.xyz);
          u_xlat0_d.x = ((-_Alpha) + 1);
          u_xlat0_d.x = (u_xlat0_d.x * u_xlat1_d.w);
          u_xlatb6 = (u_xlat1_d.w<0.949999988);
          out_f.color.w = (u_xlatb6)?(0):(u_xlat0_d.x);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
