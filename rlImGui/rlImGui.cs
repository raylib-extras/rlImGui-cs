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

        static Dictionary<KeyboardKey, ImGuiKey> RaylibKeyMap = new Dictionary<KeyboardKey, ImGuiKey>();

        internal static bool LastFrameFocused = false;

        internal static bool LastControlPressed = false;
        internal static bool LastShiftPressed = false;
        internal static bool LastAltPressed = false;
        internal static bool LastSuperPressed = false;

        internal static bool rlImGuiIsControlDown() { return Raylib.IsKeyDown(KeyboardKey.RightControl) || Raylib.IsKeyDown(KeyboardKey.LeftControl); }
        internal static bool rlImGuiIsShiftDown() { return Raylib.IsKeyDown(KeyboardKey.RightShift) || Raylib.IsKeyDown(KeyboardKey.LeftShift); }
        internal static bool rlImGuiIsAltDown() { return Raylib.IsKeyDown(KeyboardKey.RightAlt) || Raylib.IsKeyDown(KeyboardKey.LeftAlt); }
        internal static bool rlImGuiIsSuperDown() { return Raylib.IsKeyDown(KeyboardKey.RightSuper) || Raylib.IsKeyDown(KeyboardKey.LeftSuper); }

        public delegate void SetupUserFontsCallback(ImGuiIOPtr imGuiIo);

        /// <summary>
        /// Callback for cases where the user wants to install additional fonts.
        /// </summary>
        public static SetupUserFontsCallback SetupUserFonts = null;

        /// <summary>
        /// Sets up ImGui, loads fonts and themes
        /// </summary>
        /// <param name="darkTheme">when true(default) the dark theme is used, when false the light theme is used</param>
        /// <param name="enableDocking">when true(not default) docking support will be enabled/param>
        public static void Setup(bool darkTheme = true, bool enableDocking = false)
        {
            BeginInitImGui();

            if (darkTheme)
                ImGui.StyleColorsDark();
            else
                ImGui.StyleColorsLight();

            if (enableDocking)
                ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            EndInitImGui();
        }

        /// <summary>
        /// Custom initialization. Not needed if you call Setup. Only needed if you want to add custom setup code.
        /// must be followed by EndInitImGui
        /// </summary>
        public static void BeginInitImGui()
        {
            MouseCursorMap = new Dictionary<ImGuiMouseCursor, MouseCursor>();

            LastFrameFocused = Raylib.IsWindowFocused();
            LastControlPressed = false;
            LastShiftPressed = false;
            LastAltPressed = false;
            LastSuperPressed = false;

            FontTexture.Id = 0;

            SetupKeymap();

            ImGuiContext = ImGui.CreateContext();
        }

        internal static void SetupKeymap()
        {
            if (RaylibKeyMap.Count > 0)
                return;

            // build up a map of raylib keys to ImGuiKeys
            RaylibKeyMap[KeyboardKey.Apostrophe] = ImGuiKey.Apostrophe;
            RaylibKeyMap[KeyboardKey.Comma] = ImGuiKey.Comma;
            RaylibKeyMap[KeyboardKey.Minus] = ImGuiKey.Minus;
            RaylibKeyMap[KeyboardKey.Period] = ImGuiKey.Period;
            RaylibKeyMap[KeyboardKey.Slash] = ImGuiKey.Slash;
            RaylibKeyMap[KeyboardKey.Zero] = ImGuiKey._0;
            RaylibKeyMap[KeyboardKey.One] = ImGuiKey._1;
            RaylibKeyMap[KeyboardKey.Two] = ImGuiKey._2;
            RaylibKeyMap[KeyboardKey.Three] = ImGuiKey._3;
            RaylibKeyMap[KeyboardKey.Four] = ImGuiKey._4;
            RaylibKeyMap[KeyboardKey.Five] = ImGuiKey._5;
            RaylibKeyMap[KeyboardKey.Six] = ImGuiKey._6;
            RaylibKeyMap[KeyboardKey.Seven] = ImGuiKey._7;
            RaylibKeyMap[KeyboardKey.Eight] = ImGuiKey._8;
            RaylibKeyMap[KeyboardKey.Nine] = ImGuiKey._9;
            RaylibKeyMap[KeyboardKey.Semicolon] = ImGuiKey.Semicolon;
            RaylibKeyMap[KeyboardKey.Equal] = ImGuiKey.Equal;
            RaylibKeyMap[KeyboardKey.A] = ImGuiKey.A;
            RaylibKeyMap[KeyboardKey.B] = ImGuiKey.B;
            RaylibKeyMap[KeyboardKey.C] = ImGuiKey.C;
            RaylibKeyMap[KeyboardKey.D] = ImGuiKey.D;
            RaylibKeyMap[KeyboardKey.E] = ImGuiKey.E;
            RaylibKeyMap[KeyboardKey.F] = ImGuiKey.F;
            RaylibKeyMap[KeyboardKey.G] = ImGuiKey.G;
            RaylibKeyMap[KeyboardKey.H] = ImGuiKey.H;
            RaylibKeyMap[KeyboardKey.I] = ImGuiKey.I;
            RaylibKeyMap[KeyboardKey.J] = ImGuiKey.J;
            RaylibKeyMap[KeyboardKey.K] = ImGuiKey.K;
            RaylibKeyMap[KeyboardKey.L] = ImGuiKey.L;
            RaylibKeyMap[KeyboardKey.M] = ImGuiKey.M;
            RaylibKeyMap[KeyboardKey.N] = ImGuiKey.N;
            RaylibKeyMap[KeyboardKey.O] = ImGuiKey.O;
            RaylibKeyMap[KeyboardKey.P] = ImGuiKey.P;
            RaylibKeyMap[KeyboardKey.Q] = ImGuiKey.Q;
            RaylibKeyMap[KeyboardKey.R] = ImGuiKey.R;
            RaylibKeyMap[KeyboardKey.S] = ImGuiKey.S;
            RaylibKeyMap[KeyboardKey.T] = ImGuiKey.T;
            RaylibKeyMap[KeyboardKey.U] = ImGuiKey.U;
            RaylibKeyMap[KeyboardKey.V] = ImGuiKey.V;
            RaylibKeyMap[KeyboardKey.W] = ImGuiKey.W;
            RaylibKeyMap[KeyboardKey.X] = ImGuiKey.X;
            RaylibKeyMap[KeyboardKey.Y] = ImGuiKey.Y;
            RaylibKeyMap[KeyboardKey.Z] = ImGuiKey.Z;
            RaylibKeyMap[KeyboardKey.Space] = ImGuiKey.Space;
            RaylibKeyMap[KeyboardKey.Escape] = ImGuiKey.Escape;
            RaylibKeyMap[KeyboardKey.Enter] = ImGuiKey.Enter;
            RaylibKeyMap[KeyboardKey.Tab] = ImGuiKey.Tab;
            RaylibKeyMap[KeyboardKey.Backspace] = ImGuiKey.Backspace;
            RaylibKeyMap[KeyboardKey.Insert] = ImGuiKey.Insert;
            RaylibKeyMap[KeyboardKey.Delete] = ImGuiKey.Delete;
            RaylibKeyMap[KeyboardKey.Right] = ImGuiKey.RightArrow;
            RaylibKeyMap[KeyboardKey.Left] = ImGuiKey.LeftArrow;
            RaylibKeyMap[KeyboardKey.Down] = ImGuiKey.DownArrow;
            RaylibKeyMap[KeyboardKey.Up] = ImGuiKey.UpArrow;
            RaylibKeyMap[KeyboardKey.PageUp] = ImGuiKey.PageUp;
            RaylibKeyMap[KeyboardKey.PageDown] = ImGuiKey.PageDown;
            RaylibKeyMap[KeyboardKey.Home] = ImGuiKey.Home;
            RaylibKeyMap[KeyboardKey.End] = ImGuiKey.End;
            RaylibKeyMap[KeyboardKey.CapsLock] = ImGuiKey.CapsLock;
            RaylibKeyMap[KeyboardKey.ScrollLock] = ImGuiKey.ScrollLock;
            RaylibKeyMap[KeyboardKey.NumLock] = ImGuiKey.NumLock;
            RaylibKeyMap[KeyboardKey.PrintScreen] = ImGuiKey.PrintScreen;
            RaylibKeyMap[KeyboardKey.Pause] = ImGuiKey.Pause;
            RaylibKeyMap[KeyboardKey.F1] = ImGuiKey.F1;
            RaylibKeyMap[KeyboardKey.F2] = ImGuiKey.F2;
            RaylibKeyMap[KeyboardKey.F3] = ImGuiKey.F3;
            RaylibKeyMap[KeyboardKey.F4] = ImGuiKey.F4;
            RaylibKeyMap[KeyboardKey.F5] = ImGuiKey.F5;
            RaylibKeyMap[KeyboardKey.F6] = ImGuiKey.F6;
            RaylibKeyMap[KeyboardKey.F7] = ImGuiKey.F7;
            RaylibKeyMap[KeyboardKey.F8] = ImGuiKey.F8;
            RaylibKeyMap[KeyboardKey.F9] = ImGuiKey.F9;
            RaylibKeyMap[KeyboardKey.F10] = ImGuiKey.F10;
            RaylibKeyMap[KeyboardKey.F11] = ImGuiKey.F11;
            RaylibKeyMap[KeyboardKey.F12] = ImGuiKey.F12;
            RaylibKeyMap[KeyboardKey.LeftShift] = ImGuiKey.LeftShift;
            RaylibKeyMap[KeyboardKey.LeftControl] = ImGuiKey.LeftCtrl;
            RaylibKeyMap[KeyboardKey.LeftAlt] = ImGuiKey.LeftAlt;
            RaylibKeyMap[KeyboardKey.LeftSuper] = ImGuiKey.LeftSuper;
            RaylibKeyMap[KeyboardKey.RightShift] = ImGuiKey.RightShift;
            RaylibKeyMap[KeyboardKey.RightControl] = ImGuiKey.RightCtrl;
            RaylibKeyMap[KeyboardKey.RightAlt] = ImGuiKey.RightAlt;
            RaylibKeyMap[KeyboardKey.RightSuper] = ImGuiKey.RightSuper;
            RaylibKeyMap[KeyboardKey.KeyboardMenu] = ImGuiKey.Menu;
            RaylibKeyMap[KeyboardKey.LeftBracket] = ImGuiKey.LeftBracket;
            RaylibKeyMap[KeyboardKey.Backslash] = ImGuiKey.Backslash;
            RaylibKeyMap[KeyboardKey.RightBracket] = ImGuiKey.RightBracket;
            RaylibKeyMap[KeyboardKey.Grave] = ImGuiKey.GraveAccent;
            RaylibKeyMap[KeyboardKey.Kp0] = ImGuiKey.Keypad0;
            RaylibKeyMap[KeyboardKey.Kp1] = ImGuiKey.Keypad1;
            RaylibKeyMap[KeyboardKey.Kp2] = ImGuiKey.Keypad2;
            RaylibKeyMap[KeyboardKey.Kp3] = ImGuiKey.Keypad3;
            RaylibKeyMap[KeyboardKey.Kp4] = ImGuiKey.Keypad4;
            RaylibKeyMap[KeyboardKey.Kp5] = ImGuiKey.Keypad5;
            RaylibKeyMap[KeyboardKey.Kp6] = ImGuiKey.Keypad6;
            RaylibKeyMap[KeyboardKey.Kp7] = ImGuiKey.Keypad7;
            RaylibKeyMap[KeyboardKey.Kp8] = ImGuiKey.Keypad8;
            RaylibKeyMap[KeyboardKey.Kp9] = ImGuiKey.Keypad9;
            RaylibKeyMap[KeyboardKey.KpDecimal] = ImGuiKey.KeypadDecimal;
            RaylibKeyMap[KeyboardKey.KpDivide] = ImGuiKey.KeypadDivide;
            RaylibKeyMap[KeyboardKey.KpMultiply] = ImGuiKey.KeypadMultiply;
            RaylibKeyMap[KeyboardKey.KpSubtract] = ImGuiKey.KeypadSubtract;
            RaylibKeyMap[KeyboardKey.KpAdd] = ImGuiKey.KeypadAdd;
            RaylibKeyMap[KeyboardKey.KpEnter] = ImGuiKey.KeypadEnter;
            RaylibKeyMap[KeyboardKey.KpEqual] = ImGuiKey.KeypadEqual;
        }

        private static void SetupMouseCursors()
        {
            MouseCursorMap.Clear();
            MouseCursorMap[ImGuiMouseCursor.Arrow] = MouseCursor.Arrow;
            MouseCursorMap[ImGuiMouseCursor.TextInput] = MouseCursor.IBeam;
            MouseCursorMap[ImGuiMouseCursor.Hand] = MouseCursor.PointingHand;
            MouseCursorMap[ImGuiMouseCursor.ResizeAll] = MouseCursor.ResizeAll;
            MouseCursorMap[ImGuiMouseCursor.ResizeEW] = MouseCursor.ResizeEw;
            MouseCursorMap[ImGuiMouseCursor.ResizeNESW] = MouseCursor.ResizeNesw;
            MouseCursorMap[ImGuiMouseCursor.ResizeNS] = MouseCursor.ResizeNs;
            MouseCursorMap[ImGuiMouseCursor.ResizeNWSE] = MouseCursor.ResizeNwse;
            MouseCursorMap[ImGuiMouseCursor.NotAllowed] = MouseCursor.NotAllowed;
        }

        /// <summary>
        /// Forces the font texture atlas to be recomputed and re-cached
        /// </summary>
        public static unsafe void ReloadFonts()
        {
            ImGui.SetCurrentContext(ImGuiContext);
            ImGuiIOPtr io = ImGui.GetIO();

            int width, height, bytesPerPixel;
            io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out width, out height, out bytesPerPixel);

            Raylib_cs.Image image = new Image
            {
                Data = pixels,
                Width = width,
                Height = height,
                Mipmaps = 1,
                Format = PixelFormat.UncompressedR8G8B8A8,
            };

            if (Raylib.IsTextureValid(FontTexture))
                Raylib.UnloadTexture(FontTexture);

            FontTexture = Raylib.LoadTextureFromImage(image);

            io.Fonts.SetTexID(new IntPtr(FontTexture.Id));
        }

        unsafe internal static sbyte* rImGuiGetClipText(IntPtr userData)
        {
            return Raylib.GetClipboardText();
        }

        unsafe internal static void rlImGuiSetClipText(IntPtr userData, sbyte* text)
        {
            Raylib.SetClipboardText(text);
        }

        private unsafe delegate sbyte* GetClipTextCallback(IntPtr userData);
        private unsafe delegate void SetClipTextCallback(IntPtr userData, sbyte* text);

        private static GetClipTextCallback GetClipCallback = null!;
        private static SetClipTextCallback SetClipCallback = null!;
        /// <summary>
        /// End Custom initialization. Not needed if you call Setup. Only needed if you want to add custom setup code.
        /// must be proceeded by BeginInitImGui
        /// </summary>
        public static void EndInitImGui()
        {
            SetupMouseCursors();

            ImGui.SetCurrentContext(ImGuiContext);

            var fonts = ImGui.GetIO().Fonts;
            ImGui.GetIO().Fonts.AddFontDefault();

            // remove this part if you don't want font awesome
            unsafe
            {
                ImFontConfig* icons_config = ImGuiNative.ImFontConfig_ImFontConfig();
                icons_config->MergeMode = 1;                      // merge the glyph ranges into the default font
                icons_config->PixelSnapH = 1;                     // don't try to render on partial pixels
                icons_config->FontDataOwnedByAtlas = 0;           // the font atlas does not own this font data

                icons_config->GlyphMaxAdvanceX = float.MaxValue;
                icons_config->RasterizerMultiply = 1.0f;
                icons_config->OversampleH = 2;
                icons_config->OversampleV = 1;

                ushort[] IconRanges = new ushort[3];
                IconRanges[0] = IconFonts.FontAwesome6.IconMin;
                IconRanges[1] = IconFonts.FontAwesome6.IconMax;
                IconRanges[2] = 0;

                fixed (ushort* range = &IconRanges[0])
                {
                    // this unmanaged memory must remain allocated for the entire run of rlImgui
                    IconFonts.FontAwesome6.IconFontRanges = Marshal.AllocHGlobal(6);
                    Buffer.MemoryCopy(range, IconFonts.FontAwesome6.IconFontRanges.ToPointer(), 6, 6);
                    icons_config->GlyphRanges = (ushort*)IconFonts.FontAwesome6.IconFontRanges.ToPointer();

                    byte[] fontDataBuffer = Convert.FromBase64String(IconFonts.FontAwesome6.IconFontData);

                    fixed (byte* buffer = fontDataBuffer)
                    {
                        var fontPtr = ImGui.GetIO().Fonts.AddFontFromMemoryTTF(new IntPtr(buffer), fontDataBuffer.Length, 11, icons_config, IconFonts.FontAwesome6.IconFontRanges);
                    }
                }

                ImGuiNative.ImFontConfig_destroy(icons_config);
            }

            ImGuiIOPtr io = ImGui.GetIO();

            ImGuiPlatformIOPtr platformIO = ImGui.GetPlatformIO();

            if (SetupUserFonts != null)
                SetupUserFonts(io);

            io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors | ImGuiBackendFlags.HasSetMousePos | ImGuiBackendFlags.HasGamepad;

            io.MousePos.X = 0;
            io.MousePos.Y = 0;

            // copy/paste callbacks
            unsafe
            {
                SetClipCallback = new SetClipTextCallback(rlImGuiSetClipText);
                platformIO.Platform_SetClipboardTextFn = Marshal.GetFunctionPointerForDelegate(SetClipCallback);

                GetClipCallback = new GetClipTextCallback(rImGuiGetClipText);
                platformIO.Platform_GetClipboardTextFn = Marshal.GetFunctionPointerForDelegate(GetClipCallback);
            }

            platformIO.Platform_ClipboardUserData = IntPtr.Zero;
            ReloadFonts();
        }

        private static void SetMouseEvent(ImGuiIOPtr io, MouseButton rayMouse, ImGuiMouseButton imGuiMouse)
        {
            if (Raylib.IsMouseButtonPressed(rayMouse))
                io.AddMouseButtonEvent((int)imGuiMouse, true);
            else if (Raylib.IsMouseButtonReleased(rayMouse))
                io.AddMouseButtonEvent((int)imGuiMouse, false);
        }

        private static void NewFrame(float dt = -1)
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

            io.DisplayFramebufferScale = new Vector2(1, 1);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || Raylib.IsWindowState(ConfigFlags.HighDpiWindow))
                    io.DisplayFramebufferScale = Raylib.GetWindowScaleDPI();

            io.DeltaTime = dt >= 0 ? dt : Raylib.GetFrameTime();

            if (io.WantSetMousePos)
            {
                Raylib.SetMousePosition((int)io.MousePos.X, (int)io.MousePos.Y);
            }
            else
            {
                io.AddMousePosEvent(Raylib.GetMouseX(), Raylib.GetMouseY());
            }

            SetMouseEvent(io, MouseButton.Left, ImGuiMouseButton.Left);
            SetMouseEvent(io, MouseButton.Right, ImGuiMouseButton.Right);
            SetMouseEvent(io, MouseButton.Middle, ImGuiMouseButton.Middle);
            SetMouseEvent(io, MouseButton.Forward, ImGuiMouseButton.Middle + 1);
            SetMouseEvent(io, MouseButton.Back, ImGuiMouseButton.Middle + 2);

            var wheelMove = Raylib.GetMouseWheelMoveV();
            io.AddMouseWheelEvent(wheelMove.X, wheelMove.Y);

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
                                Raylib.SetMouseCursor(MouseCursor.Default);
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

            // gamepads
            if ((io.ConfigFlags & ImGuiConfigFlags.NavEnableGamepad) != 0 && Raylib.IsGamepadAvailable(0))
            {
                HandleGamepadButtonEvent(io, GamepadButton.LeftFaceUp, ImGuiKey.GamepadDpadUp);
                HandleGamepadButtonEvent(io, GamepadButton.LeftFaceRight, ImGuiKey.GamepadDpadRight);
                HandleGamepadButtonEvent(io, GamepadButton.LeftFaceDown, ImGuiKey.GamepadDpadDown);
                HandleGamepadButtonEvent(io, GamepadButton.LeftFaceLeft, ImGuiKey.GamepadDpadLeft);

                HandleGamepadButtonEvent(io, GamepadButton.RightFaceUp, ImGuiKey.GamepadFaceUp);
                HandleGamepadButtonEvent(io, GamepadButton.RightFaceRight, ImGuiKey.GamepadFaceLeft);
                HandleGamepadButtonEvent(io, GamepadButton.RightFaceDown, ImGuiKey.GamepadFaceDown);
                HandleGamepadButtonEvent(io, GamepadButton.RightFaceLeft, ImGuiKey.GamepadFaceRight);

                HandleGamepadButtonEvent(io, GamepadButton.LeftTrigger1, ImGuiKey.GamepadL1);
                HandleGamepadButtonEvent(io, GamepadButton.LeftTrigger2, ImGuiKey.GamepadL2);
                HandleGamepadButtonEvent(io, GamepadButton.RightTrigger1, ImGuiKey.GamepadR1);
                HandleGamepadButtonEvent(io, GamepadButton.RightTrigger2, ImGuiKey.GamepadR2);
                HandleGamepadButtonEvent(io, GamepadButton.LeftThumb, ImGuiKey.GamepadL3);
                HandleGamepadButtonEvent(io, GamepadButton.RightThumb, ImGuiKey.GamepadR3);

                HandleGamepadButtonEvent(io, GamepadButton.MiddleLeft, ImGuiKey.GamepadStart);
                HandleGamepadButtonEvent(io, GamepadButton.MiddleRight, ImGuiKey.GamepadBack);

                // left stick
                HandleGamepadStickEvent(io, GamepadAxis.LeftX, ImGuiKey.GamepadLStickLeft, ImGuiKey.GamepadLStickRight);
                HandleGamepadStickEvent(io, GamepadAxis.LeftY, ImGuiKey.GamepadLStickUp, ImGuiKey.GamepadLStickDown);

                // right stick
                HandleGamepadStickEvent(io, GamepadAxis.RightX, ImGuiKey.GamepadRStickLeft, ImGuiKey.GamepadRStickRight);
                HandleGamepadStickEvent(io, GamepadAxis.RightY, ImGuiKey.GamepadRStickUp, ImGuiKey.GamepadRStickDown);
            }
        }


        private static void HandleGamepadButtonEvent(ImGuiIOPtr io, GamepadButton button, ImGuiKey key)
        {
            if (Raylib.IsGamepadButtonPressed(0, button))
                io.AddKeyEvent(key, true);
            else if (Raylib.IsGamepadButtonReleased(0, button))
                io.AddKeyEvent(key, false);
        }

        private static void HandleGamepadStickEvent(ImGuiIOPtr io, GamepadAxis axis, ImGuiKey negKey, ImGuiKey posKey)
        {
            const float deadZone = 0.20f;

            float axisValue = Raylib.GetGamepadAxisMovement(0, axis);

            io.AddKeyAnalogEvent(negKey, axisValue < -deadZone, axisValue < -deadZone ? -axisValue : 0);
            io.AddKeyAnalogEvent(posKey, axisValue > deadZone, axisValue > deadZone ? axisValue : 0);
        }

        /// <summary>
        /// Starts a new ImGui Frame
        /// </summary>
        /// <param name="dt">optional delta time, any value < 0 will use raylib GetFrameTime</param>
        public static void Begin(float dt = -1)
        {
            ImGui.SetCurrentContext(ImGuiContext);

            NewFrame(dt);
            FrameEvents();
            ImGui.NewFrame();
        }

        private static void EnableScissor(float x, float y, float width, float height)
        {
            Rlgl.EnableScissorTest();
            ImGuiIOPtr io = ImGui.GetIO();

            Vector2 scale = new Vector2(1.0f, 1.0f);
            if (Raylib.IsWindowState(ConfigFlags.HighDpiWindow) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                scale = io.DisplayFramebufferScale;

            Rlgl.Scissor(   (int)(x * scale.X),
                            (int)((io.DisplaySize.Y - (int)(y + height)) * scale.Y),
                            (int)(width * scale.X),
                            (int)(height * scale.Y));
        }

        private static void TriangleVert(ImDrawVertPtr idx_vert)
        {
            Vector4 color = ImGui.ColorConvertU32ToFloat4(idx_vert.col);

            Rlgl.Color4f(color.X, color.Y, color.Z, color.W);
            Rlgl.TexCoord2f(idx_vert.uv.X, idx_vert.uv.Y);
            Rlgl.Vertex2f(idx_vert.pos.X, idx_vert.pos.Y);
        }

        private static void RenderTriangles(uint count, uint indexStart, ImVector<ushort> indexBuffer, ImPtrVector<ImDrawVertPtr> vertBuffer, IntPtr texturePtr)
        {
            if (count < 3)
                return;

            uint textureId = 0;
            if (texturePtr != IntPtr.Zero)
                textureId = (uint)texturePtr.ToInt32();

            Rlgl.Begin(DrawMode.Triangles);
            Rlgl.SetTexture(textureId);

            for (int i = 0; i <= (count - 3); i += 3)
            {
                if (Rlgl.CheckRenderBatchLimit(3))
                {
                    Rlgl.Begin(DrawMode.Triangles);
                    Rlgl.SetTexture(textureId);
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
            Rlgl.End();
        }

        private delegate void Callback(ImDrawListPtr list, ImDrawCmdPtr cmd);

        private static void RenderData()
        {
            Rlgl.DrawRenderBatchActive();
            Rlgl.DisableBackfaceCulling();

            var data = ImGui.GetDrawData();

            for (int l = 0; l < data.CmdListsCount; l++)
            {
                ImDrawListPtr commandList = data.CmdLists[l];

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

                    Rlgl.DrawRenderBatchActive();
                }
            }
            Rlgl.SetTexture(0);
            Rlgl.DisableScissorTest();
            Rlgl.EnableBackfaceCulling();
        }

        /// <summary>
        /// Ends an ImGui frame and submits all ImGui drawing to raylib for processing.
        /// </summary>
        public static void End()
        {
            ImGui.SetCurrentContext(ImGuiContext);
            ImGui.Render();
            RenderData();
        }

        /// <summary>
        /// Cleanup ImGui and unload font atlas
        /// </summary>
        public static void Shutdown()
        {
            Raylib.UnloadTexture(FontTexture);
            ImGui.DestroyContext();

            // remove this if you don't want font awesome support
            {
                if (IconFonts.FontAwesome6.IconFontRanges != IntPtr.Zero)
                    Marshal.FreeHGlobal(IconFonts.FontAwesome6.IconFontRanges);

                IconFonts.FontAwesome6.IconFontRanges = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Draw a texture as an image in an ImGui Context
        /// Uses the current ImGui Cursor position and the full texture size.
        /// </summary>
        /// <param name="image">The raylib texture to draw</param>
        public static void Image(Texture2D image)
        {
            ImGui.Image(new IntPtr(image.Id), new Vector2(image.Width, image.Height));
        }

        /// <summary>
        /// Draw a texture as an image in an ImGui Context at a specific size
        /// Uses the current ImGui Cursor position and the specified width and height
        /// The image will be scaled up or down to fit as needed
        /// </summary>
        /// <param name="image">The raylib texture to draw</param>
        /// <param name="width">The width of the drawn image</param>
        /// <param name="height">The height of the drawn image</param>
        public static void ImageSize(Texture2D image, int width, int height)
        {
            ImGui.Image(new IntPtr(image.Id), new Vector2(width, height));
        }

        /// <summary>
        /// Draw a texture as an image in an ImGui Context at a specific size
        /// Uses the current ImGui Cursor position and the specified size
        /// The image will be scaled up or down to fit as needed
        /// </summary>
        /// <param name="image">The raylib texture to draw</param>
        /// <param name="size">The size of drawn image</param>
        public static void ImageSize(Texture2D image, Vector2 size)
        {
            ImGui.Image(new IntPtr(image.Id), size);
        }

        /// <summary>
        /// Draw a portion texture as an image in an ImGui Context at a defined size
        /// Uses the current ImGui Cursor position and the specified size
        /// The image will be scaled up or down to fit as needed
        /// </summary>
        /// <param name="image">The raylib texture to draw</param>
        /// <param name="destWidth">The width of the drawn image</param>
        /// <param name="destHeight">The height of the drawn image</param>
        /// <param name="sourceRect">The portion of the texture to draw as an image. Negative values for the width and height will flip the image</param>
        public static void ImageRect(Texture2D image, int destWidth, int destHeight, Rectangle sourceRect)
        {
            Vector2 uv0 = new Vector2();
            Vector2 uv1 = new Vector2();

            if (sourceRect.Width < 0)
            {
                uv0.X = -((float)sourceRect.X / image.Width);
                uv1.X = (uv0.X - (float)(Math.Abs(sourceRect.Width) / image.Width));
            }
            else
            {
                uv0.X = (float)sourceRect.X / image.Width;
                uv1.X = uv0.X + (float)(sourceRect.Width / image.Width);
            }

            if (sourceRect.Height < 0)
            {
                uv0.Y = -((float)sourceRect.Y / image.Height);
                uv1.Y = (uv0.Y - (float)(Math.Abs(sourceRect.Height) / image.Height));
            }
            else
            {
                uv0.Y = (float)sourceRect.Y / image.Height;
                uv1.Y = uv0.Y + (float)(sourceRect.Height / image.Height);
            }

            ImGui.Image(new IntPtr(image.Id), new Vector2(destWidth, destHeight), uv0, uv1);
        }

        /// <summary>
        /// Draws a render texture as an image an ImGui Context, automatically flipping the Y axis so it will show correctly on screen
        /// </summary>
        /// <param name="image">The render texture to draw</param>
        public static void ImageRenderTexture(RenderTexture2D image)
        {
            ImageRect(image.Texture, image.Texture.Width, image.Texture.Height, new Rectangle(0, 0, image.Texture.Width, -image.Texture.Height));
        }

        /// <summary>
        /// Draws a render texture as an image to the current ImGui Context, flipping the Y axis so it will show correctly on the screen
        /// The texture will be scaled to fit the content are available, centered if desired
        /// </summary>
        /// <param name="image">The render texture to draw</param>
        /// <param name="center">When true the texture will be centered in the content area. When false the image will be left and top justified</param>
        public static void ImageRenderTextureFit(RenderTexture2D image, bool center = true)
        {
            Vector2 area = ImGui.GetContentRegionAvail();

            float scale = area.X / image.Texture.Width;

            float y = image.Texture.Height * scale;
            if (y > area.Y)
            {
                scale = area.Y / image.Texture.Height;
            }

            int sizeX = (int)(image.Texture.Width * scale);
            int sizeY = (int)(image.Texture.Height * scale);

            if (center)
            {
                ImGui.SetCursorPosX(0);
                ImGui.SetCursorPosX(area.X / 2 - sizeX / 2);
                ImGui.SetCursorPosY(ImGui.GetCursorPosY() + (area.Y / 2 - sizeY / 2));
            }

            ImageRect(image.Texture, sizeX, sizeY, new Rectangle(0,0, (image.Texture.Width), -(image.Texture.Height) ));
        }

        /// <summary>
        /// Draws a texture as an image button in an ImGui context. Uses the current ImGui cursor position and the full size of the texture
        /// </summary>
        /// <param name="name">The display name and ImGui ID for the button</param>
        /// <param name="image">The texture to draw</param>
        /// <returns>True if the button was clicked</returns>
        public static bool ImageButton(System.String name, Texture2D image)
        {
            return ImageButtonSize(name, image, new Vector2(image.Width, image.Height));
        }

        /// <summary>
        /// Draws a texture as an image button in an ImGui context. Uses the current ImGui cursor position and the specified size.
        /// </summary>
        /// <param name="name">The display name and ImGui ID for the button</param>
        /// <param name="image">The texture to draw</param>
        /// <param name="size">The size of the button/param>
        /// <returns>True if the button was clicked</returns>
        public static bool ImageButtonSize(System.String name, Texture2D image, Vector2 size)
        {
            return ImGui.ImageButton(name, new IntPtr(image.Id), size);
        }

    }
}
