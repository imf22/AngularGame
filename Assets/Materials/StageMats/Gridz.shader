Shader "Unlit/Gridz"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _GridColor("Grid Color", Color) = (255,0,0,0)
        _GridScale("Grid Scale", Float) = 10.0
        _GridThickness("Line Thickness", Float) = 5.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            // Added
            float4 _GridColor;
            float _GridScale;
            float  _GridThickness;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float GridTester(float2 r) 
            {
                float result;
                if(_GridScale == 0)
                {
                    _GridScale = 1.0;
                }

                for (float i = 0.0; i < 1.0; i += (1 / _GridScale))
                {
                    for (int j = 0; j < 2; j++)
                    {
                        result += 1.0 - smoothstep(0.0,(_GridThickness/ 1000), abs(r[j] - i));
                    }
                }

                return result;

            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Apply grid color to gridlines on texture
                fixed4 gridColor = (_GridColor * GridTester(i.uv)) + tex2D(_MainTex, i.uv);
                return float4(gridColor);
            }
            ENDCG
        }
    }
}
