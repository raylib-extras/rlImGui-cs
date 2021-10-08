using System;
using System.Collections.Generic;
using System.Numerics;

using Raylib_cs;
using rlImGui_cs;
using ImGuiNET;

namespace editor_example
{
    public class SceneViewWindow : DocumentWindow
    {
        private Camera3D Camera = new Camera3D();
        private Texture2D GridTexture;

        public override void Setup()
        {
            ViewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

            Camera.fovy = 45;
            Camera.up.Y = 1;
            Camera.position.Y = 3;
            Camera.position.Z = -25;

            Image img = Raylib.GenImageChecked(256, 256, 32, 32, Color.DARKGRAY, Color.WHITE);
            GridTexture = Raylib.LoadTextureFromImage(img);
            Raylib.UnloadImage(img);
            Raylib.GenTextureMipmaps(ref GridTexture);
            Raylib.SetTextureFilter(GridTexture, TextureFilter.TEXTURE_FILTER_ANISOTROPIC_16X);
            Raylib.SetTextureWrap(GridTexture, TextureWrap.TEXTURE_WRAP_CLAMP);
        }
        public override void Shutdown()
        {
            Raylib.UnloadRenderTexture(ViewTexture);
            Raylib.UnloadTexture(GridTexture);
        }


        public override void Show()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
            ImGui.SetNextWindowSizeConstraints(new Vector2(400, 400), new Vector2((float)Raylib.GetScreenWidth(), (float)Raylib.GetScreenHeight()));

            if (ImGui.Begin("3D View", ref Open, ImGuiWindowFlags.NoScrollbar))
            {
                Focused = ImGui.IsWindowFocused(ImGuiFocusedFlags.ChildWindows);

                Vector2 size = ImGui.GetContentRegionAvail();

                Rectangle viewRect = new Rectangle();
                viewRect.x = ViewTexture.texture.width / 2 - size.X / 2;
                viewRect.y = ViewTexture.texture.height / 2 - size.Y / 2;
                viewRect.width = size.X;
                viewRect.height = -size.Y;

                // draw the view
                rlImGui.ImageRect(ViewTexture.texture, (int)size.X, (int)size.Y, viewRect);

                ImGui.End();
            }
            ImGui.PopStyleVar();
        }


        public override void Update()
        {
            if (!Open)
                return;

            if (Raylib.IsWindowResized())
            {
                Raylib.UnloadRenderTexture(ViewTexture);
                ViewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            }

            float period = 10;
            float magnitude = 25;

            Camera.position.X = (float)(Math.Sin(Raylib.GetTime() / period) * magnitude);

            Raylib.BeginTextureMode(ViewTexture);
            Raylib.ClearBackground(Color.SKYBLUE);

            Raylib.BeginMode3D(Camera);

            // grid of cube trees on a plane to make a "world"
            Raylib.DrawPlane(new Vector3(0, 0, 0 ), new Vector2(50, 50), Color.BEIGE); // simple world plane
            float spacing = 4;
            int count = 5;

            for (float x = -count * spacing; x <= count * spacing; x += spacing)
            {
                for (float z = -count * spacing; z <= count * spacing; z += spacing)
                {
                    Vector3 pos = new Vector3( x, 0.5f, z );
                    Vector3 min = new Vector3( x - 0.5f, 0, z - 0.5f );
                    Vector3 max = new Vector3( x + 0.5f, 1, z + 0.5f );

                    Raylib.DrawCubeTexture(GridTexture, new Vector3(x, 1.5f, z ), 1, 1, 1, Color.GREEN);
                    Raylib.DrawCubeTexture(GridTexture, new Vector3(x, 0.5f, z ), 0.25f, 1, 0.25f, Color.BROWN);
                }
            }

            Raylib.EndMode3D();
            Raylib.EndTextureMode();
        }
    }
}
