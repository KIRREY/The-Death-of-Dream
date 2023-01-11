//��shader�޷�����Edge�Ĳ���������ͼƬ��UV,��Ҫ_UVScale�����ֶ�ƥ��UV��_Edge����һ�仯��_UVScale�����ض�Ҫ�޸�
//��shaderֻ������UGUI�����һ��ͼƬ�ķֱ����Զ���Сһ����ֵ������ķֱ������������⴦��
//��shader��Ե���Ǹ��ݳ�������õ������Ǹ������ؿ�ȣ���˼����˵������Խ��������Խ��Խ�̣�����Խ��
Shader "Custom/Edge"
{
	Properties
	{
		_Edge("Edge", Range(0, 0.5)) = 0.1
		_EdgeColor("EdgeColor", Color) = (1, 1, 1, 1)
		_MainTex("MainTex", 2D) = "white" {}
		_UVScale("UVScale", Range(0, 30)) = 0.13
		_Intensity("Intensity", Range(0, 3)) = 1.86
	}
		SubShader
		{

		 Tags
			{
				"Queue" = "Transparent"
			}
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				fixed _Edge;
				fixed4 _EdgeColor;
				sampler2D _MainTex;
				float _UVScale;
				float _Intensity;
				float _Test;

				struct appdata
				{
					float4 vertex : POSITION;
					fixed2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float4 objVertex : TEXCOORD0;
					fixed2 uv : TEXCOORD1;

				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.objVertex = v.vertex;
					o.uv = v.uv;


					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{


					fixed x = i.uv.x;
					fixed y = i.uv.y;



					//ȷ������ľ��ε��ĸ���

					float2 leftUp = float2(_Edge,1 - _Edge);

					float2 leftDown = float2(_Edge,_Edge);

					float2 RightUp = float2(1 - _Edge,1 - _Edge);

					float2 RightDown = float2(1 - _Edge,_Edge);



					//ȷ�������ص���� �ĸ�λ�õľ���

					float leftUpD = distance(leftUp,i.uv);

					float2 leftDownD = distance(leftDown,i.uv);

					float2 RightUpD = distance(RightUp,i.uv);

					float2 RightDownD = distance(RightDown,i.uv);


					float alpha = 0;

					//�����жϣ��жϸ������ھŹ�����ĸ�λ�ã�Ȼ������alpha��ֵ����

					if (x < _Edge && (1 - y) < _Edge)//����
						alpha = pow((_Edge - leftUpD) / _Edge,_Intensity);
					else if (x < _Edge && y < _Edge)//����
						alpha = pow((_Edge - leftDownD) / _Edge,_Intensity);
					else if ((1 - x) < _Edge && y < _Edge)//����
						alpha = pow((_Edge - RightDownD) / _Edge,_Intensity);
					else if ((1 - x) < _Edge && (1 - y) < _Edge)//����
						alpha = pow((_Edge - RightUpD) / _Edge,_Intensity);
					else if ((x < _Edge))//���
						alpha = pow(x / _Edge,_Intensity);
					else if (1 - x < _Edge)//�ұ�
						alpha = pow((1 - x) / _Edge,_Intensity);
					else if (1 - y < _Edge)//�ϱ�
						alpha = pow((1 - y) / _Edge,_Intensity);
					else if (y < _Edge)    //�±� 
						alpha = pow(y / _Edge,_Intensity);
					else //�м���ʾ��ͼ��
					{
						  float4 addUV = float4(-_UVScale,-_UVScale,1 + _UVScale * 2,1 + _UVScale * 2);
						 fixed4 col = tex2D(_MainTex, i.uv * addUV.zw + addUV.xy);
						 alpha = 1;
						 _EdgeColor.xyz = col.xyz;
					}

				  return fixed4(_EdgeColor.xyz,alpha);
				}
				ENDCG
			}
		}
}