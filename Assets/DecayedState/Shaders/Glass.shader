Shader "Transparent/Glass" { 

    Properties { 

        _Color ("Color", Color) = (1,1,1,1) 

        _SpecColor ("Specular Color", Color) = (1,1,1,1)

        _Shininess ("Specular Falloff", Range (0.01, 1)) = 0.7

        _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5) 

        _MainTex ("Main Texture", 2D) = "white" {}

        _Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }

    }

    

    Category { 

        Tags {"Queue"="Transparent"}

        

        SubShader { 

            

            Pass {

                Cull Off

                ZWrite Off

                ZTest Less 

                Lighting On

                SeparateSpecular On

                Blend SrcAlpha OneMinusSrcAlpha

                AlphaTest Greater 0.01

                

                

                

                Material {

                    Diffuse [_Color]

                    Ambient [_Color]

                    Shininess [_Shininess]

                    Specular [_SpecColor]

                }

                

                

                //Reflection

                SetTexture [_Cube] {

                    ConstantColor [_ReflectColor]

                    combine texture * constant alpha, texture

                    Matrix [_Cube]

                }

                

                //Reflection illumination

                SetTexture [_Cube] {

                    ConstantColor [_ReflectColor]

                    combine constant * constant alpha - previous, previous

                    Matrix [_Cube]

                }

                

                //Texture

                SetTexture [_MainTex] {

                    ConstantColor [_Color]

                    combine texture +- previous, constant

                }

                

                //Texture illumination

                SetTexture [_MainTex] {

                    ConstantColor (1,1,1,0.5)

                    combine previous * primary double , previous

                }

                

            }

            

            

        }//End of Subshader

        

        Fallback "Diffuse"

        

    }//End of Category

    

}//End of Shader