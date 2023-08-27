/*******************************************************************************************
*
*   raylib-extras [ImGui] example - Simple Integration
*
*	This is a simple ImGui Integration
*	It is done using C++ but with C style code
*	It can be done in C as well if you use the C ImGui wrapper
*	https://github.com/cimgui/cimgui
*
*   Copyright (c) 2021 Jeffery Myers
*
********************************************************************************************/

using System;

using Raylib_cs;
using rlImGui_cs;
using ImGuiNET;

namespace rlImGui_cs
{
    class Program
    {
        static void Main(string[] args)
        {
            Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_VSYNC_HINT);
            Raylib.InitWindow(1280, 800, "raylib-Extras-cs [ImGui] example - simple ImGui Demo");
            Raylib.SetTargetFPS(144);

            rlImGui.Setup(true);

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DARKGRAY);

                rlImGui.Begin();
                ImGui.ShowDemoWindow();
                rlImGui.End();

                Raylib.EndDrawing();
            }

            rlImGui.Shutdown();
            Raylib.CloseWindow();
        }
    }
}
