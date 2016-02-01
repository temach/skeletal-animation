using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WinFormAnimation2D
{
    /// This class will be passed to the Entity's GetSettings() so it make the scene look best.
    class DrawSettings
    {
        /// Seriously OpenGL settings
        /// here is a template:
        /// public bool _enable = false;
        public bool _enableTexture2D = false;
        public bool _enablePerspectiveCorrectionHint = false;
        public bool _enableDepthTest = false;
        public bool _enableFaceCounterClockwise = false;
        public bool _enableDisplayList = false;
        public bool _enablePolygonModeFill = false;
        public bool _enablePolygonModeLine = false;
        public bool _enableLight = false;

        /// Enum of all supported camera modes.
        public enum CameraMode
        {
            Fps = 0,
            Orbit,
            _Max
        }

        public bool RenderWireframe;
        public bool RenderTextured = true;
        public bool RenderLit = true;

        public bool ShowFps = true;

        public CameraMode CamMode = CameraMode.Orbit;

        public Entity _currentEntity;

        /// Font to be used for textual overlays in 3D view (size ~ 12px)
        public readonly Font DefaultFont12;

        /// Font to be used for textual overlays in 3D view (size ~ 16px)
        public readonly Font DefaultFont16;

        public DrawSettings()
        {
            DefaultFont12 = new Font(FontFamily.GenericSansSerif, 12);
            DefaultFont16 = new Font(FontFamily.GenericSansSerif, 16);
        }
    }


    class GUISettings
    {
        /// The preferred global configuration for rendering.
        public DrawSettings DrawSet = new DrawSettings();

        /// Show we render Frames Per Second counter?
        public bool ShowFps = true;

        /// Currently active scene
        public Entity _currentEntity;

        public GUISettings()
        {
            // nothing to do here
        }
    }

}
