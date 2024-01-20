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

            Camera.FovY = 45;
            Camera.Up.Y = 1;
            Camera.Position.Y = 3;
            Camera.Position.Z = -25;

            Image img = Raylib.GenImageChecked(256, 256, 32, 32, Color.DarkGray, Color.White);
            GridTexture = Raylib.LoadTextureFromImage(img);
            Raylib.UnloadImage(img);
            Raylib.GenTextureMipmaps(ref GridTexture);
            Raylib.SetTextureFilter(GridTexture, TextureFilter.Anisotropic16X);
            Raylib.SetTextureWrap(GridTexture, TextureWrap.Clamp);
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

                // draw the view
                rlImGui.ImageRenderTextureFit(ViewTexture, true);

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

            Camera.Position.X = (float)(Math.Sin(Raylib.GetTime() / period) * magnitude);

            Raylib.BeginTextureMode(ViewTexture);
            Raylib.ClearBackground(Color.SkyBlue);

            Raylib.BeginMode3D(Camera);

            // grid of cube trees on a plane to make a "world"
            Raylib.DrawPlane(new Vector3(0, 0, 0), new Vector2(50, 50), Color.Beige); // simple world plane
            float spacing = 4;
            int count = 5;

            for (float x = -count * spacing; x <= count * spacing; x += spacing)
            {
                for (float z = -count * spacing; z <= count * spacing; z += spacing)
                {
                    Vector3 pos = new Vector3(x, 0.5f, z);
                    Vector3 min = new Vector3(x - 0.5f, 0, z - 0.5f);
                    Vector3 max = new Vector3(x + 0.5f, 1, z + 0.5f);

                    DrawCubeTexture(GridTexture, new Vector3(x, 1.5f, z), 1, 1, 1, Color.Green);
                    DrawCubeTexture(GridTexture, new Vector3(x, 0.5f, z), 0.25f, 1, 0.25f, Color.Brown);
                }
            }

            Raylib.EndMode3D();
            Raylib.EndTextureMode();
        }

        // Draw cube textured
        // NOTE: Cube position is the center position
        static void DrawCubeTexture(
            Texture2D texture,
            Vector3 position,
            float width,
            float height,
            float length,
            Color color
        )
        {
            float x = position.X;
            float y = position.Y;
            float z = position.Z;

            // Set desired texture to be enabled while drawing following vertex data
            Rlgl.SetTexture(texture.Id);

            // Vertex data transformation can be defined with the commented lines,
            // but in this example we calculate the transformed vertex data directly when calling Rlgl.rlVertex3f()
            // Rlgl.rlPushMatrix();
            // NOTE: Transformation is applied in inverse order (scale -> rotate -> translate)
            // Rlgl.rlTranslatef(2.0f, 0.0f, 0.0f);
            // Rlgl.rlRotatef(45, 0, 1, 0);
            // Rlgl.rlScalef(2.0f, 2.0f, 2.0f);

            Rlgl.Begin(DrawMode.Quads);
            Rlgl.Color4ub(color.R, color.G, color.B, color.A);

            // Front Face
            // Normal Pointing Towards Viewer
            Rlgl.Normal3f(0.0f, 0.0f, 1.0f);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);

            // Back Face
            // Normal Pointing Away From Viewer
            Rlgl.Normal3f(0.0f, 0.0f, -1.0f);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);

            // Top Face
            // Normal Pointing Up
            Rlgl.Normal3f(0.0f, 1.0f, 0.0f);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);

            // Bottom Face
            // Normal Pointing Down
            Rlgl.Normal3f(0.0f, -1.0f, 0.0f);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);

            // Right face
            // Normal Pointing Right
            Rlgl.Normal3f(1.0f, 0.0f, 0.0f);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);

            // Left Face
            // Normal Pointing Left
            Rlgl.Normal3f(-1.0f, 0.0f, 0.0f);
            Rlgl.TexCoord2f(0.0f, 0.0f);
            // Bottom Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
            Rlgl.TexCoord2f(1.0f, 0.0f);
            // Bottom Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);
            Rlgl.TexCoord2f(1.0f, 1.0f);
            // Top Right Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);
            Rlgl.TexCoord2f(0.0f, 1.0f);
            // Top Left Of The Texture and Quad
            Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
            Rlgl.End();
            //rlPopMatrix();

            Rlgl.SetTexture(0);
        }
    }
}
