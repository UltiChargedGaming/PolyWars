Shader "PCIK/Unlit/Transparent" {
 Properties {
     _NotVisibleColor ("NotVisibleColor (RGB)", Color) = (0.3,0.3,0.3,1)
     _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
 }
 
 SubShader {
     Tags {"Queue" = "Geometry+500" "IgnoreProjector"="True" "RenderType"="Transparent"}
     LOD 100
     
     ZWrite Off
     Blend SrcAlpha OneMinusSrcAlpha 
 
     Pass {
             ZTest Greater
             Lighting Off
             ZWrite Off
             Color [_NotVisibleColor]
     }
 
     Pass {
         ZTest LEqual
         Lighting Off
         SetTexture [_MainTex] { combine texture } 
     }
 }
 }