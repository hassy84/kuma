//Hard Surface Shader Package, Written for the Unity engine by Bruno Rime: http://www.behance.net/brunorime brunorime@gmail.com
Shader "HardSurface/Hardsurface Pro/Draw First/Opaque Bump"{
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
	_Shininess ("Shininess", Range (0.01, 3)) = 1.5
	_Gloss("Gloss", Range (0.00, 1)) = .5
	_Reflection("Reflection", Range (0.00, 1)) = 0.5
	_Cube ("Reflection Cubemap", Cube) = "Black" { TexGen CubeReflect } 
	_FrezPow("Fresnel Reflection",Range(0,2)) = .25
	_FrezFalloff("Fresnal/EdgeAlpha Falloff",Range(0,10)) = 4
	_EdgeAlpha("Edge Alpha",Range(0,1)) = 0
	_Metalics("Metalics",Range(0,1)) = .5
	
	_MainTex ("Diffuse(RGB) Alpha(A)",2D) = "White" {}
	_BumpMap ("Normalmap", 2D) = "Bump" {}
	
}

	SubShader {
		
		Tags {"Queue"="Geometry+100" "RenderType"="Opaque" "IgnoreProjector"="False" }
		UsePass "Hidden/Hardsurface Pro Front Opaque Bump/FORWARD"
		
		GrabPass {
			Tags {"LightMode" = "Always"}
		}
		
		Tags {"Queue"="Transparent" "IgnoreProjector"="False" "RenderType"="Transparent" "LightMode" = "Always"}
		UsePass "Hidden/Hardsurface Pro ScreenSpace Reflection/SSREFLECTIONBUMP"
		
	} 
		Fallback "Diffuse"
	}
