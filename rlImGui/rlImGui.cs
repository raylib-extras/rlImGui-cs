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
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using IconFonts;

using Raylib_cs;
using ImGuiNET;

namespace rlImGui_cs
{
    public static class rlImGui
    {
        internal static IntPtr ImGuiContext = IntPtr.Zero;

        private static ImGuiMouseCursor CurrentMouseCursor = ImGuiMouseCursor.COUNT;
        private static Dictionary<ImGuiMouseCursor, MouseCursor> MouseCursorMap = new Dictionary<ImGuiMouseCursor, MouseCursor>();
        private static Texture2D FontTexture;

        private static IntPtr IconFontRanges = IntPtr.Zero;


        static Dictionary<KeyboardKey, ImGuiKey> RaylibKeyMap = new Dictionary<KeyboardKey, ImGuiKey>();

        internal static bool LastFrameFocused = false;

        internal static bool LastControlPressed = false;
        internal static bool LastShiftPressed = false;
        internal static bool LastAltPressed = false;
        internal static bool LastSuperPressed = false;

        internal static bool rlImGuiIsControlDown() { return Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_CONTROL) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL); }
        internal static bool rlImGuiIsShiftDown() { return Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SHIFT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT); }
        internal static bool rlImGuiIsAltDown() { return Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_ALT) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_ALT); }
        internal static bool rlImGuiIsSuperDown() { return Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_SUPER) || Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SUPER); }


        public static void Setup(bool darkTheme = true)
        {
            MouseCursorMap = new Dictionary<ImGuiMouseCursor, MouseCursor>();
            MouseCursorMap = new Dictionary<ImGuiMouseCursor, MouseCursor>();

            LastFrameFocused = Raylib.IsWindowFocused();
            LastControlPressed = false;
            LastShiftPressed = false;
            LastAltPressed = false;
            LastSuperPressed = false;

            FontTexture.id = 0;

            BeginInitImGui();

            if (darkTheme)
                ImGui.StyleColorsDark();
            else
                ImGui.StyleColorsLight();

            EndInitImGui();
        }

        public static void BeginInitImGui()
        {
            SetupKeymap();

            ImGuiContext = ImGui.CreateContext();
        }

        internal static void SetupKeymap()
        {
            if (RaylibKeyMap.Count > 0)
                return;

            // build up a map of raylib keys to ImGuiKeys
            RaylibKeyMap[KeyboardKey.KEY_APOSTROPHE] = ImGuiKey.Apostrophe;
            RaylibKeyMap[KeyboardKey.KEY_COMMA] = ImGuiKey.Comma;
            RaylibKeyMap[KeyboardKey.KEY_MINUS] = ImGuiKey.Minus;
            RaylibKeyMap[KeyboardKey.KEY_PERIOD] = ImGuiKey.Period;
            RaylibKeyMap[KeyboardKey.KEY_SLASH] = ImGuiKey.Slash;
            RaylibKeyMap[KeyboardKey.KEY_ZERO] = ImGuiKey._0;
            RaylibKeyMap[KeyboardKey.KEY_ONE] = ImGuiKey._1;
            RaylibKeyMap[KeyboardKey.KEY_TWO] = ImGuiKey._2;
            RaylibKeyMap[KeyboardKey.KEY_THREE] = ImGuiKey._3;
            RaylibKeyMap[KeyboardKey.KEY_FOUR] = ImGuiKey._4;
            RaylibKeyMap[KeyboardKey.KEY_FIVE] = ImGuiKey._5;
            RaylibKeyMap[KeyboardKey.KEY_SIX] = ImGuiKey._6;
            RaylibKeyMap[KeyboardKey.KEY_SEVEN] = ImGuiKey._7;
            RaylibKeyMap[KeyboardKey.KEY_EIGHT] = ImGuiKey._8;
            RaylibKeyMap[KeyboardKey.KEY_NINE] = ImGuiKey._9;
            RaylibKeyMap[KeyboardKey.KEY_SEMICOLON] = ImGuiKey.Semicolon;
            RaylibKeyMap[KeyboardKey.KEY_EQUAL] = ImGuiKey.Equal;
            RaylibKeyMap[KeyboardKey.KEY_A] = ImGuiKey.A;
            RaylibKeyMap[KeyboardKey.KEY_B] = ImGuiKey.B;
            RaylibKeyMap[KeyboardKey.KEY_C] = ImGuiKey.C;
            RaylibKeyMap[KeyboardKey.KEY_D] = ImGuiKey.D;
            RaylibKeyMap[KeyboardKey.KEY_E] = ImGuiKey.E;
            RaylibKeyMap[KeyboardKey.KEY_F] = ImGuiKey.F;
            RaylibKeyMap[KeyboardKey.KEY_G] = ImGuiKey.G;
            RaylibKeyMap[KeyboardKey.KEY_H] = ImGuiKey.H;
            RaylibKeyMap[KeyboardKey.KEY_I] = ImGuiKey.I;
            RaylibKeyMap[KeyboardKey.KEY_J] = ImGuiKey.J;
            RaylibKeyMap[KeyboardKey.KEY_K] = ImGuiKey.K;
            RaylibKeyMap[KeyboardKey.KEY_L] = ImGuiKey.L;
            RaylibKeyMap[KeyboardKey.KEY_M] = ImGuiKey.M;
            RaylibKeyMap[KeyboardKey.KEY_N] = ImGuiKey.N;
            RaylibKeyMap[KeyboardKey.KEY_O] = ImGuiKey.O;
            RaylibKeyMap[KeyboardKey.KEY_P] = ImGuiKey.P;
            RaylibKeyMap[KeyboardKey.KEY_Q] = ImGuiKey.Q;
            RaylibKeyMap[KeyboardKey.KEY_R] = ImGuiKey.R;
            RaylibKeyMap[KeyboardKey.KEY_S] = ImGuiKey.S;
            RaylibKeyMap[KeyboardKey.KEY_T] = ImGuiKey.T;
            RaylibKeyMap[KeyboardKey.KEY_U] = ImGuiKey.U;
            RaylibKeyMap[KeyboardKey.KEY_V] = ImGuiKey.V;
            RaylibKeyMap[KeyboardKey.KEY_W] = ImGuiKey.W;
            RaylibKeyMap[KeyboardKey.KEY_X] = ImGuiKey.X;
            RaylibKeyMap[KeyboardKey.KEY_Y] = ImGuiKey.Y;
            RaylibKeyMap[KeyboardKey.KEY_Z] = ImGuiKey.Z;
            RaylibKeyMap[KeyboardKey.KEY_SPACE] = ImGuiKey.Space;
            RaylibKeyMap[KeyboardKey.KEY_ESCAPE] = ImGuiKey.Escape;
            RaylibKeyMap[KeyboardKey.KEY_ENTER] = ImGuiKey.Enter;
            RaylibKeyMap[KeyboardKey.KEY_TAB] = ImGuiKey.Tab;
            RaylibKeyMap[KeyboardKey.KEY_BACKSPACE] = ImGuiKey.Backspace;
            RaylibKeyMap[KeyboardKey.KEY_INSERT] = ImGuiKey.Insert;
            RaylibKeyMap[KeyboardKey.KEY_DELETE] = ImGuiKey.Delete;
            RaylibKeyMap[KeyboardKey.KEY_RIGHT] = ImGuiKey.RightArrow;
            RaylibKeyMap[KeyboardKey.KEY_LEFT] = ImGuiKey.LeftArrow;
            RaylibKeyMap[KeyboardKey.KEY_DOWN] = ImGuiKey.DownArrow;
            RaylibKeyMap[KeyboardKey.KEY_UP] = ImGuiKey.UpArrow;
            RaylibKeyMap[KeyboardKey.KEY_PAGE_UP] = ImGuiKey.PageUp;
            RaylibKeyMap[KeyboardKey.KEY_PAGE_DOWN] = ImGuiKey.PageDown;
            RaylibKeyMap[KeyboardKey.KEY_HOME] = ImGuiKey.Home;
            RaylibKeyMap[KeyboardKey.KEY_END] = ImGuiKey.End;
            RaylibKeyMap[KeyboardKey.KEY_CAPS_LOCK] = ImGuiKey.CapsLock;
            RaylibKeyMap[KeyboardKey.KEY_SCROLL_LOCK] = ImGuiKey.ScrollLock;
            RaylibKeyMap[KeyboardKey.KEY_NUM_LOCK] = ImGuiKey.NumLock;
            RaylibKeyMap[KeyboardKey.KEY_PRINT_SCREEN] = ImGuiKey.PrintScreen;
            RaylibKeyMap[KeyboardKey.KEY_PAUSE] = ImGuiKey.Pause;
            RaylibKeyMap[KeyboardKey.KEY_F1] = ImGuiKey.F1;
            RaylibKeyMap[KeyboardKey.KEY_F2] = ImGuiKey.F2;
            RaylibKeyMap[KeyboardKey.KEY_F3] = ImGuiKey.F3;
            RaylibKeyMap[KeyboardKey.KEY_F4] = ImGuiKey.F4;
            RaylibKeyMap[KeyboardKey.KEY_F5] = ImGuiKey.F5;
            RaylibKeyMap[KeyboardKey.KEY_F6] = ImGuiKey.F6;
            RaylibKeyMap[KeyboardKey.KEY_F7] = ImGuiKey.F7;
            RaylibKeyMap[KeyboardKey.KEY_F8] = ImGuiKey.F8;
            RaylibKeyMap[KeyboardKey.KEY_F9] = ImGuiKey.F9;
            RaylibKeyMap[KeyboardKey.KEY_F10] = ImGuiKey.F10;
            RaylibKeyMap[KeyboardKey.KEY_F11] = ImGuiKey.F11;
            RaylibKeyMap[KeyboardKey.KEY_F12] = ImGuiKey.F12;
            RaylibKeyMap[KeyboardKey.KEY_LEFT_SHIFT] = ImGuiKey.LeftShift;
            RaylibKeyMap[KeyboardKey.KEY_LEFT_CONTROL] = ImGuiKey.LeftCtrl;
            RaylibKeyMap[KeyboardKey.KEY_LEFT_ALT] = ImGuiKey.LeftAlt;
            RaylibKeyMap[KeyboardKey.KEY_LEFT_SUPER] = ImGuiKey.LeftSuper;
            RaylibKeyMap[KeyboardKey.KEY_RIGHT_SHIFT] = ImGuiKey.RightShift;
            RaylibKeyMap[KeyboardKey.KEY_RIGHT_CONTROL] = ImGuiKey.RightCtrl;
            RaylibKeyMap[KeyboardKey.KEY_RIGHT_ALT] = ImGuiKey.RightAlt;
            RaylibKeyMap[KeyboardKey.KEY_RIGHT_SUPER] = ImGuiKey.RightSuper;
            RaylibKeyMap[KeyboardKey.KEY_KB_MENU] = ImGuiKey.Menu;
            RaylibKeyMap[KeyboardKey.KEY_LEFT_BRACKET] = ImGuiKey.LeftBracket;
            RaylibKeyMap[KeyboardKey.KEY_BACKSLASH] = ImGuiKey.Backslash;
            RaylibKeyMap[KeyboardKey.KEY_RIGHT_BRACKET] = ImGuiKey.RightBracket;
            RaylibKeyMap[KeyboardKey.KEY_GRAVE] = ImGuiKey.GraveAccent;
            RaylibKeyMap[KeyboardKey.KEY_KP_0] = ImGuiKey.Keypad0;
            RaylibKeyMap[KeyboardKey.KEY_KP_1] = ImGuiKey.Keypad1;
            RaylibKeyMap[KeyboardKey.KEY_KP_2] = ImGuiKey.Keypad2;
            RaylibKeyMap[KeyboardKey.KEY_KP_3] = ImGuiKey.Keypad3;
            RaylibKeyMap[KeyboardKey.KEY_KP_4] = ImGuiKey.Keypad4;
            RaylibKeyMap[KeyboardKey.KEY_KP_5] = ImGuiKey.Keypad5;
            RaylibKeyMap[KeyboardKey.KEY_KP_6] = ImGuiKey.Keypad6;
            RaylibKeyMap[KeyboardKey.KEY_KP_7] = ImGuiKey.Keypad7;
            RaylibKeyMap[KeyboardKey.KEY_KP_8] = ImGuiKey.Keypad8;
            RaylibKeyMap[KeyboardKey.KEY_KP_9] = ImGuiKey.Keypad9;
            RaylibKeyMap[KeyboardKey.KEY_KP_DECIMAL] = ImGuiKey.KeypadDecimal;
            RaylibKeyMap[KeyboardKey.KEY_KP_DIVIDE] = ImGuiKey.KeypadDivide;
            RaylibKeyMap[KeyboardKey.KEY_KP_MULTIPLY] = ImGuiKey.KeypadMultiply;
            RaylibKeyMap[KeyboardKey.KEY_KP_SUBTRACT] = ImGuiKey.KeypadSubtract;
            RaylibKeyMap[KeyboardKey.KEY_KP_ADD] = ImGuiKey.KeypadAdd;
            RaylibKeyMap[KeyboardKey.KEY_KP_ENTER] = ImGuiKey.KeypadEnter;
            RaylibKeyMap[KeyboardKey.KEY_KP_EQUAL] = ImGuiKey.KeypadEqual;
        }

        private static void SetupMouseCursors()
        {
            MouseCursorMap.Clear();
            MouseCursorMap[ImGuiMouseCursor.Arrow] = MouseCursor.MOUSE_CURSOR_ARROW;
            MouseCursorMap[ImGuiMouseCursor.TextInput] = MouseCursor.MOUSE_CURSOR_IBEAM;
            MouseCursorMap[ImGuiMouseCursor.Hand] = MouseCursor.MOUSE_CURSOR_POINTING_HAND;
            MouseCursorMap[ImGuiMouseCursor.ResizeAll] = MouseCursor.MOUSE_CURSOR_RESIZE_ALL;
            MouseCursorMap[ImGuiMouseCursor.ResizeEW] = MouseCursor.MOUSE_CURSOR_RESIZE_EW;
            MouseCursorMap[ImGuiMouseCursor.ResizeNESW] = MouseCursor.MOUSE_CURSOR_RESIZE_NESW;
            MouseCursorMap[ImGuiMouseCursor.ResizeNS] = MouseCursor.MOUSE_CURSOR_RESIZE_NS;
            MouseCursorMap[ImGuiMouseCursor.ResizeNWSE] = MouseCursor.MOUSE_CURSOR_RESIZE_NWSE;
            MouseCursorMap[ImGuiMouseCursor.NotAllowed] = MouseCursor.MOUSE_CURSOR_NOT_ALLOWED;
        }

        public static unsafe void ReloadFonts()
        {
            ImGui.SetCurrentContext(ImGuiContext);
            ImGuiIOPtr io = ImGui.GetIO();

            int width, height, bytesPerPixel;
            io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out width, out height, out bytesPerPixel);

            Image image = new Image
            {
                data = pixels,
                width = width,
                height = height,
                mipmaps = 1,
                format = PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8,
            };

            FontTexture = Raylib.LoadTextureFromImage(image);

            io.Fonts.SetTexID(new IntPtr(FontTexture.id));
        }

        public static void EndInitImGui()
        {
            SetupMouseCursors();

            ImGui.SetCurrentContext(ImGuiContext);

            var fonts = ImGui.GetIO().Fonts;
            ImGui.GetIO().Fonts.AddFontDefault();

            // remove this part if you don't want font awesome
            unsafe
            {
                ImFontConfig icons_config = new ImFontConfig();
                icons_config.MergeMode = 1;                      // merge the glyph ranges into the default font
                icons_config.PixelSnapH = 1;                     // don't try to render on partial pixels
                icons_config.FontDataOwnedByAtlas = 0;           // the font atlas does not own this font data

                ushort[] IconRanges = new ushort[3];
                IconRanges[0] = FontAwesome6.IconMin;
                IconRanges[1] = FontAwesome6.IconMax;
                IconRanges[2] = 0;

                fixed (ushort* range = &IconRanges[0])
                {
                    // this unmanaged memory must remain allocated for the entire run of rlImgui
                    IconFontRanges = Marshal.AllocHGlobal(6);
                    Buffer.MemoryCopy(range, IconFontRanges.ToPointer(), 6, 6);
                    icons_config.GlyphRanges = (ushort*)IconFontRanges.ToPointer();

                    ImGui.GetIO().Fonts.AddFontFromFileTTF("resources/fa-solid-900.ttf", 11, &icons_config);
                }
            }

            ImGuiIOPtr io = ImGui.GetIO();
            io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;

            io.MousePos.X = 0;
            io.MousePos.Y = 0;

            // TODO, hook up callbacks
         //   io.SetClipboardTextFn = rlImGuiSetClipText;
         //   io.GetClipboardTextFn = rlImGuiGetClipText;

            io.ClipboardUserData = IntPtr.Zero;
            ReloadFonts();
        }

        private static void NewFrame()
        {
            ImGuiIOPtr io = ImGui.GetIO();

            if (Raylib.IsWindowFullscreen())
            {
                int monitor = Raylib.GetCurrentMonitor();
                io.DisplaySize = new Vector2(Raylib.GetMonitorWidth(monitor), Raylib.GetMonitorHeight(monitor));
            }
            else
            {
                io.DisplaySize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            }

            int width = Rlgl.rlGetFramebufferWidth();
            int height = Rlgl.rlGetFramebufferHeight();
            if (width > 0 && height > 0)
            {
                io.DisplayFramebufferScale = new Vector2(width / io.DisplaySize.X, height / io.DisplaySize.Y);
            }
            else
            {
                io.DisplayFramebufferScale = new Vector2(1.0f, 1.0f);
            }

            io.DeltaTime = Raylib.GetFrameTime();

            if (io.WantSetMousePos)
            {
                Raylib.SetMousePosition((int)io.MousePos.X, (int)io.MousePos.Y);
            }
            else
            {
                io.MousePos = Raylib.GetMousePosition();
            }

            io.MouseDown[0] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON);
            io.MouseDown[1] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON);
            io.MouseDown[2] = Raylib.IsMouseButtonDown(MouseButton.MOUSE_MIDDLE_BUTTON);

            if (Raylib.GetMouseWheelMove() > 0)
                io.MouseWheel += 1;
            else if (Raylib.GetMouseWheelMove() < 0)
                io.MouseWheel -= 1;

            if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0)
            {
                ImGuiMouseCursor imgui_cursor = ImGui.GetMouseCursor();
                if (imgui_cursor != CurrentMouseCursor || io.MouseDrawCursor)
                {
                    CurrentMouseCursor = imgui_cursor;
                    if (io.MouseDrawCursor || imgui_cursor == ImGuiMouseCursor.None)
                    {
                        Raylib.HideCursor();
                    }
                    else
                    {
                        Raylib.ShowCursor();

                        if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0)
                        {

                            if (!MouseCursorMap.ContainsKey(imgui_cursor))
                                Raylib.SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);
                            else
                                Raylib.SetMouseCursor(MouseCursorMap[imgui_cursor]);
                        }
                    }
                }
            }
        }


        private static void FrameEvents()
        {
            ImGuiIOPtr io = ImGui.GetIO();

            bool focused = Raylib.IsWindowFocused();
            if (focused != LastFrameFocused)
                io.AddFocusEvent(focused);
            LastFrameFocused = focused;


            // handle the modifyer key events so that shortcuts work
            bool ctrlDown = rlImGuiIsControlDown();
            if (ctrlDown != LastControlPressed)
                io.AddKeyEvent(ImGuiKey.ModCtrl, ctrlDown);
            LastControlPressed = ctrlDown;

            bool shiftDown = rlImGuiIsShiftDown();
            if (shiftDown != LastShiftPressed)
                io.AddKeyEvent(ImGuiKey.ModShift, shiftDown);
            LastShiftPressed = shiftDown;

            bool altDown = rlImGuiIsAltDown();
            if (altDown != LastAltPressed)
                io.AddKeyEvent(ImGuiKey.ModAlt, altDown);
            LastAltPressed = altDown;

            bool superDown = rlImGuiIsSuperDown();
            if (superDown != LastSuperPressed)
                io.AddKeyEvent(ImGuiKey.ModSuper, superDown);
            LastSuperPressed = superDown;

            // get the pressed keys, they are in event order
            int keyId = Raylib.GetKeyPressed();
            while (keyId != 0)
            {
                KeyboardKey key = (KeyboardKey)keyId;
                if (RaylibKeyMap.ContainsKey(key))
                    io.AddKeyEvent(RaylibKeyMap[key], true);
                keyId = Raylib.GetKeyPressed();
            }

            foreach (var keyItr in RaylibKeyMap)
                io.KeysData[(int)keyItr.Value].Down = (byte)(Raylib.IsKeyDown(keyItr.Key) ? 1 : 0);

            // look for any keys that were down last frame and see if they were down and are released
            foreach (var keyItr in RaylibKeyMap)
	        {
                if (Raylib.IsKeyReleased(keyItr.Key))
                    io.AddKeyEvent(keyItr.Value, false);
            }

            // add the text input in order
            var pressed = Raylib.GetCharPressed();
            while (pressed != 0)
            {
                io.AddInputCharacter((uint)pressed);
                pressed = Raylib.GetCharPressed();
            }
        }

        public static void Begin()
        {
            ImGui.SetCurrentContext(ImGuiContext);

            NewFrame();
            FrameEvents();
            ImGui.NewFrame();
        }

        private static void EnableScissor(float x, float y, float width, float height)
        {
            Rlgl.rlEnableScissorTest();
            Rlgl.rlScissor((int)x, Raylib.GetScreenHeight() - (int)(y + height), (int)width, (int)height);
        }

        private static void TriangleVert(ImDrawVertPtr idx_vert)
        {
            Vector4 color = ImGui.ColorConvertU32ToFloat4(idx_vert.col);

            Rlgl.rlColor4f(color.X, color.Y, color.Z, color.W);
            Rlgl.rlTexCoord2f(idx_vert.uv.X, idx_vert.uv.Y);
            Rlgl.rlVertex2f(idx_vert.pos.X, idx_vert.pos.Y);
        }

        private static void RenderTriangles(uint count, uint indexStart, ImVector<ushort> indexBuffer, ImPtrVector<ImDrawVertPtr> vertBuffer, IntPtr texturePtr)
        {
            if (count < 3)
                return;

            uint textureId = 0;
            if (texturePtr != IntPtr.Zero)
                textureId = (uint)texturePtr.ToInt32();

            Rlgl.rlBegin(DrawMode.TRIANGLES);
            Rlgl.rlSetTexture(textureId);

            for (int i = 0; i <= (count - 3); i += 3)
            {
                if (Rlgl.rlCheckRenderBatchLimit(3))
                {
                    Rlgl.rlBegin(DrawMode.TRIANGLES);
                    Rlgl.rlSetTexture(textureId);
                }

                ushort indexA = indexBuffer[(int)indexStart + i];
                ushort indexB = indexBuffer[(int)indexStart + i + 1];
                ushort indexC = indexBuffer[(int)indexStart + i + 2];

                ImDrawVertPtr vertexA = vertBuffer[indexA];
                ImDrawVertPtr vertexB = vertBuffer[indexB];
                ImDrawVertPtr vertexC = vertBuffer[indexC];

                TriangleVert(vertexA);
                TriangleVert(vertexB);
                TriangleVert(vertexC);
            }
            Rlgl.rlEnd();
        }

        private delegate void Callback(ImDrawListPtr list, ImDrawCmdPtr cmd);

        private static void RenderData()
        {
            Rlgl.rlDrawRenderBatchActive();
            Rlgl.rlDisableBackfaceCulling();

            var data = ImGui.GetDrawData();

            for (int l = 0; l < data.CmdListsCount; l++)
            {
                ImDrawListPtr commandList = data.CmdListsRange[l];

                for (int cmdIndex = 0; cmdIndex < commandList.CmdBuffer.Size; cmdIndex++)
                {
                    var cmd = commandList.CmdBuffer[cmdIndex];

                    EnableScissor(cmd.ClipRect.X - data.DisplayPos.X, cmd.ClipRect.Y - data.DisplayPos.Y, cmd.ClipRect.Z - (cmd.ClipRect.X - data.DisplayPos.X), cmd.ClipRect.W - (cmd.ClipRect.Y - data.DisplayPos.Y));
                    if (cmd.UserCallback != IntPtr.Zero)
                    {
                        Callback cb = Marshal.GetDelegateForFunctionPointer<Callback>(cmd.UserCallback);
                        cb(commandList, cmd);
                        continue;
                    }

                    RenderTriangles(cmd.ElemCount, cmd.IdxOffset, commandList.IdxBuffer, commandList.VtxBuffer, cmd.TextureId);

                    Rlgl.rlDrawRenderBatchActive();
                }
            }
            Rlgl.rlSetTexture(0);
            Rlgl.rlDisableScissorTest();
            Rlgl.rlEnableBackfaceCulling();
        }

        public static void End()
        {
            ImGui.SetCurrentContext(ImGuiContext);
            ImGui.Render();
            RenderData();
        }

        public static void Shutdown()
        {
            Raylib.UnloadTexture(FontTexture);
            ImGui.DestroyContext();

            if (IconFontRanges != IntPtr.Zero)
                Marshal.FreeHGlobal(IconFontRanges);

            IconFontRanges = IntPtr.Zero;
        }

        public static void Image(Texture2D image)
        {
            ImGui.Image(new IntPtr(image.id), new Vector2(image.width, image.height));
        }

        public static void ImageSize(Texture2D image, int width, int height)
        {
            ImGui.Image(new IntPtr(image.id), new Vector2(width, height));
        }

        public static void ImageSize(Texture2D image, Vector2 size)
        {
            ImGui.Image(new IntPtr(image.id), size);
        }

        public static void ImageRect(Texture2D image, int destWidth, int destHeight, Rectangle sourceRect)
        {
            Vector2 uv0 = new Vector2();
            Vector2 uv1 = new Vector2();

            if (sourceRect.width < 0)
            {
                uv0.X = -((float)sourceRect.x / image.width);
                uv1.X = (uv0.X - (float)(Math.Abs(sourceRect.width) / image.width));
            }
            else
            {
                uv0.X = (float)sourceRect.x / image.width;
                uv1.X = uv0.X + (float)(sourceRect.width / image.width);
            }

            if (sourceRect.height < 0)
            {
                uv0.Y = -((float)sourceRect.y / image.height);
                uv1.Y = (uv0.Y - (float)(Math.Abs(sourceRect.height) / image.height));
            }
            else
            {
                uv0.Y = (float)sourceRect.y / image.height;
                uv1.Y = uv0.Y + (float)(sourceRect.height / image.height);
            }

            ImGui.Image(new IntPtr(image.id), new Vector2(destWidth, destHeight), uv0, uv1);
        }
    }
}
