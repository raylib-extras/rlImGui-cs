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

namespace editor_example
{
    class Program
    {
        static bool Quit = false;
        static bool ImGuiDemoOpen = false;

        static ImageViewerWindow ImageViewer;
        static SceneViewWindow SceneView;

        private static void DoMainMenu()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Exit"))
                        Quit = true;

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Window"))
                {
                    ImGui.MenuItem("ImGui Demo", string.Empty, ref ImGuiDemoOpen);
                    ImGui.MenuItem("Image Viewer", string.Empty, ref ImageViewer.Open);
                    ImGui.MenuItem("3D View", string.Empty, ref SceneView.Open);

                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }
        }

        static void Main(string[] args)
        {

            Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_VSYNC_HINT);
            Raylib.InitWindow(1280, 800, "raylib-Extras-cs [ImGui] example - editor ImGui Demo");
            Raylib.SetTargetFPS(144);

            rlImGui.Setup(true);

            ImGui.GetIO().ConfigWindowsMoveFromTitleBarOnly = true;

            ImageViewer = new ImageViewerWindow();
            ImageViewer.Setup();
            ImageViewer.Open = true;

            SceneView = new SceneViewWindow();
            SceneView.Setup();
            SceneView.Open = true;

            while (!Raylib.WindowShouldClose() && !Quit)
            {
                ImageViewer.Update();
                SceneView.Update();

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DARKGRAY);

                rlImGui.Begin();
                DoMainMenu();

                if (ImGuiDemoOpen)
                    ImGui.ShowDemoWindow(ref ImGuiDemoOpen);

                if (ImageViewer.Open)
                    ImageViewer.Show();

                if (SceneView.Open)
                    SceneView.Show();

                rlImGui.End();

                Raylib.EndDrawing();
            }

            rlImGui.Shutdown();

            ImageViewer.Shutdown();
            SceneView.Shutdown();

            Raylib.CloseWindow();
        }
    }
}

