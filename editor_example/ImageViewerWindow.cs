using System;
using System.Collections.Generic;
using System.Numerics;

using Raylib_cs;
using rlImGui_cs;
using ImGuiNET;

namespace editor_example
{
    public class ImageViewerWindow : DocumentWindow
    {
        Texture2D ImageTexture;
        Camera2D Camera = new Camera2D();

        Vector2 LastMousePos = new Vector2();
        Vector2 LastTarget = new Vector2();
        bool Dragging = false;

        Vector3 TintColor = new Vector3(1.0f, 1.0f, 1.0f);

        bool DirtyScene = false;
        enum ToolMode
        {
            None,
            Move,
        }

        ToolMode CurrentToolMode = ToolMode.None;

        public override void Setup()
        {
            Camera.Zoom = 1;
            Camera.Target.X = 0;
            Camera.Target.Y = 0;
            Camera.Rotation = 0;
            Camera.Offset.X = Raylib.GetScreenWidth() / 2.0f;
            Camera.Offset.Y = Raylib.GetScreenHeight() / 2.0f;

            ViewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            ImageTexture = Raylib.LoadTexture("resources/parrots.png");

            UpdateRenderTexture();
        }

        public override void Show()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
            ImGui.SetNextWindowSizeConstraints(new Vector2(400, 400), new Vector2((float)Raylib.GetScreenWidth(), (float)Raylib.GetScreenHeight()));

            if (ImGui.Begin("Image Viewer", ref Open, ImGuiWindowFlags.NoScrollbar))
            {
                Focused = ImGui.IsWindowFocused(ImGuiFocusedFlags.RootAndChildWindows);

                Vector2 size = ImGui.GetContentRegionAvail();

                // center the scratch pad in the view
                Rectangle viewRect = new Rectangle();
                viewRect.X = ViewTexture.Texture.Width / 2 - size.X / 2;
                viewRect.Y = ViewTexture.Texture.Height / 2 - size.Y / 2;
                viewRect.Width = size.X;
                viewRect.Height = -size.Y;

                if (ImGui.BeginChild("Toolbar", new Vector2(ImGui.GetContentRegionAvail().X, 25)))
                {
                    ImGui.SetCursorPosX(2);
                    ImGui.SetCursorPosY(3);

                    if (ImGui.Button("None"))
                    {
                        CurrentToolMode = ToolMode.None;
                    }
                    ImGui.SameLine();

                    if (ImGui.Button("Move"))
                    {
                        CurrentToolMode = ToolMode.Move;
                    }
                    ImGui.SameLine();

                    // temporarily reset window padding here so that the
                    // color picker window doesn't look weird
                    var windowPadding = ImGui.GetStyle().WindowPadding;
                    ImGui.PopStyleVar();
                    ImGui.ColorEdit3("Tint Color", ref TintColor, ImGuiColorEditFlags.NoInputs);
                    ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, windowPadding);

                    ImGui.SameLine();
                    switch (CurrentToolMode)
                    {
                        case ToolMode.None:
                            ImGui.TextUnformatted("No Tool");
                            break;
                        case ToolMode.Move:
                            ImGui.TextUnformatted("Move Tool");
                            break;
                        default:
                            break;
                    }

                    ImGui.SameLine();
                    ImGui.TextUnformatted(string.Format("camera target X{0} Y{1}", Camera.Target.X, Camera.Target.Y));
                    ImGui.EndChild();
                }

                var tintCol = new Color((int)(TintColor.X * 255) % 256, (int)(TintColor.Y * 255) % 256, (int)(TintColor.Z * 255) % 256, 255);
                rlImGui.ImageRect(ViewTexture.Texture, (int)size.X, (int)size.Y, viewRect, tintCol);

                ImGui.End();
            }
            ImGui.PopStyleVar();
        }

        public override void Shutdown()
        {
            Raylib.UnloadRenderTexture(ViewTexture);
            Raylib.UnloadTexture(ImageTexture);
        }

        public override void Update()
        {
            if (!Open)
                return;

            if (Raylib.IsWindowResized())
            {
                Raylib.UnloadRenderTexture(ViewTexture);
                ViewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());

                Camera.Offset.X = Raylib.GetScreenWidth() / 2.0f;
                Camera.Offset.Y = Raylib.GetScreenHeight() / 2.0f;
            }

            if (Focused)
            {
                if (CurrentToolMode == ToolMode.Move)
                {
                    if (Raylib.IsMouseButtonDown(MouseButton.Left))
                    {
                        if (!Dragging)
                        {
                            LastMousePos = Raylib.GetMousePosition();
                            LastTarget = Camera.Target;
                        }
                        Dragging = true;
                        Vector2 mousePos = Raylib.GetMousePosition();
                        Vector2 mouseDelta = Raymath.Vector2Subtract(LastMousePos, mousePos);

                        mouseDelta.X /= Camera.Zoom;
                        mouseDelta.Y /= Camera.Zoom;
                        Camera.Target = Raymath.Vector2Add(LastTarget, mouseDelta);

                        DirtyScene = true;

                    }
                    else
                    {
                        Dragging = false;
                    }
                }
            }
            else
            {
                Dragging = false;
            }

            if (DirtyScene)
            {
                DirtyScene = false;
                UpdateRenderTexture();
            }
        }

        protected void UpdateRenderTexture()
        {
            Raylib.BeginTextureMode(ViewTexture);
            Raylib.ClearBackground(Color.Blue);
            Raylib.BeginMode2D(Camera);
            Raylib.DrawTexture(ImageTexture, ImageTexture.Width / -2, ImageTexture.Height / -2, Color.White);
            Raylib.EndMode2D();
            Raylib.EndTextureMode();
        }
    }
}
