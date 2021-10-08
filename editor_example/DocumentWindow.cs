using System;
using System.Collections.Generic;
using System.Text;

using Raylib_cs;

namespace editor_example
{
    public abstract class DocumentWindow
    {
        public bool Open = false;

        public RenderTexture2D ViewTexture;

        public abstract void Setup();
        public abstract void Shutdown();
        public abstract void Show();
        public abstract void Update();

        public bool Focused = false;
    }
}
