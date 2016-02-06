using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WinFormAnimation2D
{
    /// This class will be passed to the Entity's GetSettings() so it make the scene look best.
    class DrawConfig
    {
        /// Seriously OpenGL settings
        /// here is a template:
        /// public bool _enable = false;
        public bool EnableTexture2D = false;
        public bool EnablePerspectiveCorrectionHint = false;
        public bool EnableDepthTest = false;
        public bool EnableFaceCounterClockwise = false;
        public bool EnableDisplayList = false;
        public bool EnablePolygonModeFill = false;
        public bool EnablePolygonModeLine = false;
        public bool EnableLight = false;

        public bool RenderWireframe = false;
        public bool RenderTextured = true;
        public bool RenderLit = true;

        public Pen DefaultPen = Pens.Gold;
        public Brush DefaultBrush = Brushes.Gold;

        // Font to be used for textual overlays in 3D view (size ~ 12px)
        public readonly Font DefaultFont12;
        // Font to be used for textual overlays in 3D view (size ~ 16px)
        public readonly Font DefaultFont16;

        public DrawConfig()
        {
            DefaultFont12 = new Font(FontFamily.GenericSansSerif, 12);
            DefaultFont16 = new Font(FontFamily.GenericSansSerif, 16);
        }
    }


    class GUIConfig
    {
        // should animation be playing. This should really go into GUISettings
        private bool Animating = false;

        /// Show we render Frames Per Second counter?
        public bool ShowFps = true;

        /// Currently active scene
        public Entity CurrentEntity;

        /// Enum of all supported camera modes.
        public enum CameraMode
        {
            Fps = 0,
            Orbit,
            _Max
        }
        public CameraMode CamMode = CameraMode.Orbit;

        public GUIConfig()
        {
            // nothing to do here
        }
    }

}
