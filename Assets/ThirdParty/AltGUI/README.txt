USES GUIDE


Because NGUI integration requires NGUI asset, we packed all NGUI tools into the ZIP package placed in AltGUI/NGUI folder.
If you use already have NGUI asset, please unpack AltGUI/NGUI/NGUI.zip package and enjoy AltGUI NGUI integration and demos.


After AltGUI Unity package has been installed you can find 3 example scenes placed in AltGUI package:

1. AltGUI\Common\Scenes\AltGUI_Native_Demo - the native AltGUI scene with demonstration of AltGUI Gwen Complex Demo

2. AltGUI\UnityUI\Scenes\AltGUI_UnityUI_Demo - the AltGUI new Unity UI Integration demonstration scene

3. AltGUI\NGUI\Scenes\AltGUI_NGUI_Demo - the AltGUI NGUI Integration demonstration scene

Note: Only fist application start may take some seconds because AltSketchDemo finding system fonts.
      This information cached and next starts became more faster. You can control of this process by 
      PCLTools.LoadSystemFonts() func (placed in AltGUI\_Integration\PCLTools\PCLTools.cs)


If you have questions, bug reports or feature requests you can post it on:

1. AltSketch Unity forum thread http://forum.unity3d.com/threads/altsketch-graphics-library-c-freetype-port-gui-vector-graphics-etc.258920/
   When you read this, AltGUI Unity forum thread already exists and you can find its link in AltSketch Unity forum thread

2. AltSoftLab forums http://www.altsoftlab.com/forums.aspx



INFO

AltGUI & AltSketch is multiplatform pure C# Graphics Library with System.Drawing like program interface

- GUI: QuickFont, Gwen (TEMPORARY), Awesomium Browser integration
- Interactive Vector Graphics, Geometry Transformations, Clipping, Boolean Operations
- Brushes, Pens, Custom Shader Materials, Graphics
- C# FreeType port, flexible Font Engine
- Direct Image Load/Save operations: PNG, JPEG, BMP, Animated Gif, TIFF, TGA
- AntiAliased and Solid Vector Data Triangulation
- HW Render to Texture, SW Render to Texture2D
- ZipFile & Folder storage Virtual File System
- Many integrated interactive packages: NPlot, OxyPlot, PieChart, ZedGraph, HTML Renderer, GMap.NET, PDFsharp, SVG.NET, Box2D, Farseer Physics, AForge.NET, etc.
- Multiplatform (Mobile devices supported)
- Common Keyboard and Mouse Input Framework
- Pure C# (.NET 2.0 Subset) code without native dependencies

YouTube video - http://www.youtube.com/watch?v=bm3dUnXH7tw
Unity Web demos - http://www.altsoftlab.com/unity-demo.aspx



DOCUMENTATION
-------------


UNITY BACKEND & INTEGRATION
---------------------------


AltSketch Unity Backend & AltGUI Integration files placed in _Integration Folder. Backend solves
all rendering operations. It includes:

Unity_Renderer - single render context handler (like render to Screen or to Texture)

Unity_RenderManager - common settings, textures container

Unity_RenderPrimitive - single render data processor

Unity_Texture - operates with Texture2D & RenderTextures


Also Backend contains BrushMaterial-s (placed in _Integration/Backend/BrushMaterials folder). Brush materials solve
to operate with Brush settings and move it to render pipeline. BrushMaterials includes:

Unity_SolidColorBrushMaterial - SolidColorBrush processor

Unity_LinearGradientBrushMaterial - LinearGradientBrush processor

Unity_RadialGradientBrushMaterial - RadialGradientBrush processor

Unity_ImageBrushMaterial - ImageBrush processor

Unity_ExtBrushMaterial - ExtBrush processor


AltSketch Unity Integration consists of 2 main objects:

AltSketchMonoBehaviour - main MonoBehaviour class that processes all needed Unity events and render

AltSketchScreenMonoBehaviour - inherits AltSketchMonoBehaviour and solves to process Mouse and Keyboard
in full screen mode



UNITY UI & NGUI INTEGRATION
___________________________


Unity UI & NGUI have 2 major UI elements:

-   AltSketch Paint
-   AltGUI Control Host


AltSketch Paint is the base drawing visual control. It makes all for the drawing process and can be used 
dirrectly as separated UI Element. There are 2 major parameters for it:

-   Max FPS - defines maximum refresh calls per second. For invalidating AltSketch Paint you must to call 
    its function Invalidate(). Without this call AltSketch Paint will redraw only on start or when it resized. 
    But if you call Invalidate() on every Update, it will redraw self no more then MaxFPS times per second. So 
    you can call Invalidate so often, as you want - it invalidates not often than MaxFPS times per second. It's 
    useful for the most UI controls, because it's state changes not so often, but can contain many drawing 
    operations. By default we set this parapeter to 60 fps (just the middle of 30 and 120). You can change it 
    to any other value per control.

-   Render Type - Software or Hardware. As you could understand we use buffered graphics for controls. You can 
    draw any compound graphics with just 1 draw call per frame. All control render makes to a Texture. In Unity 
    AltSketch can render in 2 ways: Software rendering to Texture2D using AltSketch powerful and fast internal 
    Software render, or Hardware rendering to RenderTexture. Software Render in many cases faster than HW 
    (because it’s no reason of triangulation and other addition procedures). Also no HW Draw Calls used. So SW 
    render used by default for controls. You can change it in the Object Inspector per control. Also you can 
    change it on the fly.

You can attach script to AltSketch Paint control and connect to onPaint event to process painting process. In 
Update func of script you can call Invalidate() func of AltPaint when you need to redraw your AltSketch Paint 
element. In the AltGUI SDK look at AltSketch Paint example - it's very simple.

The second major element is AltGUI Control Host. It uses AltSketch Paint for render itself and can process the 
most variety of UI events (keyboard, pointer, etc.). And it serves as host for AltGUI controls. The main property 
of this control is 'Child'. You can put AltGUI Control Host control to the scene, attach script that creates 
AltGUI control and set its instance to AltGUI Control Host 'Child' property. AltGUI control don't know anything 
about AltGUI Control Host, but can live in it usefully receiving all events.

    public class MyScript : MonoBehaviour
    {
        void Start ()
        {
            gameObject.GetComponent<AltGUIControlHost>().Child = new MyAltGUIControl();
        }
    }

Another way is to create Unity UI or NGUI control is to derive it from AltGUI Control Host and initialize 'Child' 
property in Start() function of it. Also you can add some cross-calls and Unity Editor properties to it to make 
this UI control more professional and useful. AltGUI SDK contains several such controls ready to use dirrectly in 
Unity UI & NGUI (menu structure equals for the Unity UI & NGUI)

So we already know all about host controls for the both UI systems. The last thing we must to do is just to create 
a new AltGUI control. It's very easy:

    public class MyAltGUIControl : Alt.GUI.UserControl // or can be derived from Alt.GUI.Control
    {
        public MyControl()
        {
            //  your code
        }

        protected override void OnPaint(Alt.GUI.PaintEventArgs e)
        {
            base.OnPaint(e);

            //  your code
        }

        protected override void OnPaintBackground(Alt.GUI.PaintEventArgs pevent)
        ...
        protected override void OnResize(Alt.GUI.ResizeEventArgs e)
        ...
        protected override void OnKeyDown(Alt.GUI.KeyEventArgs e)
        ...
        protected override void OnKeyUp(Alt.GUI.KeyEventArgs e)
        ...
        protected override void OnKeyPress(Alt.GUI.KeyPressEventArgs e)
        ...
        protected override void OnMouseDown(Alt.GUI.MouseEventArgs e)
        ...
        protected override void OnMouseUp(Alt.GUI.MouseEventArgs e)
        ...
        protected override void OnMouseMove(Alt.GUI.MouseEventArgs e)
        ...
        protected override void OnMouseWheel(Alt.GUI.MouseEventArgs e)
        ...
        protected override void OnMouseClick(Alt.GUI.MouseEventArgs e)
        ...
        protected override void OnMouseDoubleClick(Alt.GUI.MouseEventArgs e)
        ...
        protected override void OnMouseEnter(System.EventArgs e)
        ...
        protected override void OnMouseLeave(System.EventArgs e)
        ...
        protected override void OnMouseHover(System.EventArgs e)
        ...

        //  And hundreds additional event overrides...
    }

In addition to overrides of event hadlers AltGUI cotrols contain events in the same notation:

        Alt.GUI.PaintEventHandler Paint;
        ...
        Alt.GUI.KeyEventHandler KeyDown;
        ...
        Alt.GUI.MouseEventHandler MouseDown;
        ...


Almost all properties/events/functions of each AltGUI control is <summary> documented and can be viewed 
by intellisense or Assembly View. All functionality and API is like Windows.Forms controls, so it's easy 
to understand it's work.

Let's try AltGUI and enjoy it!



AltGUI DEMO
-----------

AltGUI SDK Demo examples located in:
1. AltGUI\Common\Scenes\AltGUI_Native_Demo - the native AltGUI scene with demonstration of AltGUI Gwen Complex Demo
2. AltGUI\UnityUI\Scenes\AltGUI_UnityUI_Demo - the AltGUI new Unity UI Integration demonstration scene
3. AltGUI\NGUI\Scenes\AltGUI_NGUI_Demo - the AltGUI NGUI Integration demonstration scene

In AltGUI\Common\Resources folder located AltGUI data used in examples. All this data zipped into zip file,
that used dirrectly by VirtualFileSystem and attached to AltGUI VFS in AltGUIIntegration.InitializePlatformSpecificTools() func
located in AltGUI\_Integration\AltGUIIntegration\AltGUIIntegration_Unity.cs



All addition information you need you can get on AltSketch project main site http://www.altsoftlab.com
and on it forums.
