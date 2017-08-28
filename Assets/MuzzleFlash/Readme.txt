The package contains a effects of muzzle flashes with textures and distortion shader.
To add a glow effect, you must add the standard bloom effect for the camera and activate HDR on main camera.

---------------------------------------------------------------------------------------------------------
Please read how to add bloom and other effects:
---------------------------------------------------------------------------------------------------------
NOTE:

For correct work as in demo scene you need enable "HDR" on main camera and. 

https://www.assetstore.unity3d.com/en/#!/content/83912 link on free unity physically correct bloom.
Use follow settings:
Threshold 2
Radius 7
Intencity 1
High quality true
Anti flicker true

In forward mode, HDR does not work with antialiasing. So you need disable antialiasing (edit->project settings->quality)
or use deffered rendering mode.

Add Post processing behaviour(script) into your camera and choose PostProcessingProfile.asset from Muzzle Flash folder

---------------------------------------------------------------------------------------------------------

Effects runs on PC with DX9/DX11/OpenGL/OpenGLES and with Forward/Deffered Renderer.
Support Unity 5.5

For simple using you should move the prefab to the scene.

Release notes:

Version 1.1:
Fixed distortion effect for unity 5.5.1f1
Fixed sprite sheet animation of some effects