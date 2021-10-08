# rlImGui-cs
<img align="left" src="https://github.com/raysan5/raylib/raw/master/logo/raylib_logo_animation.gif" width="64">
A Raylib-cs integration with DearImGui

rlImgui-cs provides a backend for [Dear ImGui](https://github.com/ocornut/imgui) using [Raylib](https://www.raylib.com/) for C# using https://github.com/mellinoe/ImGui.NET

# Building
rlImGui-cs is is a shared library that uses raylib-cs and ImGui.Net

# Setup

Using rlImGui in your code is very easy. Once you have included the library, or source files for rlImGui and ImGui in your project, simply do the following.
```
using Raylib_cs;
using rlImGui_cs;
using ImGuiNET;


// before your game loop
rlImGui.Setup(true);	// sets up ImGui with ether a dark or light default theme

// inside your game loop, between BeginDrawing() and EndDrawing()
rlImGui.Begin();			// starts the ImGui content mode. Make all ImGui calls after this

rlImGui.End();			// ends the ImGui content mode. Make all ImGui calls before this

// after your game loop is over, before you close the window

rlImGui.Shutdown();		// cleans up ImGui
```

# Examples
There are two example programs in the examples folder.

## Simple
This is the most simple use of ImGui in raylib, it just shows the ImGui demo window.
![image](https://user-images.githubusercontent.com/322174/136596910-da1b60ae-4a39-48f0-ae84-f568bc396870.png)


## Editor
This is a more complex example of ImGui, showing how to use raylib 2d and 3d cameras to draw into ImGui windows using render textures.
![image](https://user-images.githubusercontent.com/322174/136596949-033ffe0a-2476-4030-988a-5bf5b6e2ade7.png)


# Images
Raylib textures can be drawn in ImGui using the following functions
```
rlImGui.Image(Texture2D image);
rlImGui.ImageSize(Texture2D image, int width, int height);
rlImGui.ImageRect(Texture2D image, int destWidth, int destHeight, Rectangle sourceRect);
```


