using UnityEditor;
using UnityEngine;

// ------------------------------------------------------------
// SceneRightClickCreate  (EDITOR-ONLY, must live in a folder named "Editor")
//
// Adds Unity's "create object" menu (Create Empty, 3D Object, Audio, Effects,
// Light, Video, UI Toolkit, Camera, UI) to the Scene view right-click menu.
// The new object appears where the mouse was when you right-clicked.
//
// Needs Unity 2023.2 or newer (that's when the Scene view got a right-click menu).
// To remove the feature, delete this file.
//
// HOW IT WORKS
//   1. Every time you press the right mouse button in the Scene view,
//      we work out which point in the world the mouse is over and remember it.
//   2. Each menu entry below runs Unity's own "GameObject/..." menu command,
//      so you get exactly the same object the normal menu would make.
//   3. Then we move that new object to the remembered point.
// ------------------------------------------------------------
[InitializeOnLoad]   // makes Unity run the static constructor when the editor loads
public static class SceneRightClickCreate
{
    // Where our entries go: the Scene view right-click menu
    private const string M = "CONTEXT/GameObjectToolContext/";

    private static Vector3 lastPoint;   // world position under the last right-click

    static SceneRightClickCreate()
    {
        // duringSceneGui is called for every event in the Scene view
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        // rawType so we still see the click even if something else already used it
        if (e.rawType != EventType.MouseDown || e.button != 1) return;

        // A ray going from the camera through the mouse position
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        // 1) Is the mouse over an object? Then use that surface point.
        if (HandleUtility.PlaceObject(e.mousePosition, out Vector3 hitPoint, out _))
        {
            lastPoint = hitPoint;
            return;
        }

        // 2) Otherwise use the ground grid (the flat plane at height 0)
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        if (ground.Raycast(ray, out float distance) && distance < 500f)
        {
            lastPoint = ray.GetPoint(distance);
            return;
        }

        // 3) Looking at the sky: just put it 10 units in front of the camera
        lastPoint = ray.GetPoint(10f);
    }

    // Runs Unity's own menu command, then moves the object it created.
    private static void Make(string path)
    {
        // Nothing selected, so the new object is not made a child of something
        Selection.activeObject = null;

        // Same as clicking GameObject > ... in the top menu bar
        if (!EditorApplication.ExecuteMenuItem("GameObject/" + path)) return;

        // Unity selects whatever it just created
        GameObject go = Selection.activeGameObject;
        if (go == null) return;

        // UI elements live on a Canvas and are positioned differently, so leave them alone
        if (go.GetComponent<RectTransform>() != null) return;

        // Lift by half the height, so it sits ON the surface instead of half inside it
        Renderer rend = go.GetComponent<Renderer>();
        float lift = rend != null ? rend.bounds.extents.y : 0f;

        go.transform.position = lastPoint + Vector3.up * lift;
    }

    // ---------------- MENU ENTRIES ----------------
    // [MenuItem(path, isValidateFunction, priority)]
    // Priority sets the order. A gap of more than 10 draws a separator line.
    // To add your own: copy a line, change the path and the method name.

    [MenuItem(M + "Create Empty", false, 2000)] static void Empty() => Make("Create Empty");

    [MenuItem(M + "3D Object/Cube", false, 2001)]               static void Cube()     => Make("3D Object/Cube");
    [MenuItem(M + "3D Object/Sphere", false, 2002)]             static void Sphere()   => Make("3D Object/Sphere");
    [MenuItem(M + "3D Object/Capsule", false, 2003)]            static void Capsule()  => Make("3D Object/Capsule");
    [MenuItem(M + "3D Object/Cylinder", false, 2004)]           static void Cylinder() => Make("3D Object/Cylinder");
    [MenuItem(M + "3D Object/Plane", false, 2005)]              static void Plane_()   => Make("3D Object/Plane");
    [MenuItem(M + "3D Object/Quad", false, 2006)]               static void Quad()     => Make("3D Object/Quad");
    [MenuItem(M + "3D Object/Text - TextMeshPro", false, 2020)] static void Text3D()   => Make("3D Object/Text - TextMeshPro");
    [MenuItem(M + "3D Object/Terrain", false, 2040)]            static void Terrain()  => Make("3D Object/Terrain");
    [MenuItem(M + "3D Object/Tree", false, 2041)]               static void Tree()     => Make("3D Object/Tree");
    [MenuItem(M + "3D Object/Wind Zone", false, 2042)]          static void WindZone() => Make("3D Object/Wind Zone");

    [MenuItem(M + "Audio/Audio Source", false, 2100)]      static void AudioSource() => Make("Audio/Audio Source");
    [MenuItem(M + "Audio/Audio Reverb Zone", false, 2101)] static void ReverbZone()  => Make("Audio/Audio Reverb Zone");

    [MenuItem(M + "Effects/Particle System", false, 2200)]             static void Particles()  => Make("Effects/Particle System");
    [MenuItem(M + "Effects/Particle System Force Field", false, 2201)] static void ForceField() => Make("Effects/Particle System Force Field");
    [MenuItem(M + "Effects/Trail", false, 2202)]                       static void Trail()      => Make("Effects/Trail");
    [MenuItem(M + "Effects/Line", false, 2203)]                        static void Line()       => Make("Effects/Line");

    [MenuItem(M + "Light/Directional Light", false, 2300)] static void DirLight()   => Make("Light/Directional Light");
    [MenuItem(M + "Light/Point Light", false, 2301)]       static void PointLight() => Make("Light/Point Light");
    [MenuItem(M + "Light/Spotlight", false, 2302)]         static void Spotlight()  => Make("Light/Spotlight");
    [MenuItem(M + "Light/Area Light", false, 2303)]        static void AreaLight()  => Make("Light/Area Light");
    [MenuItem(M + "Light/Reflection Probe", false, 2320)]  static void ReflProbe()  => Make("Light/Reflection Probe");
    [MenuItem(M + "Light/Light Probe Group", false, 2321)] static void LightProbes() => Make("Light/Light Probe Group");

    [MenuItem(M + "Video/Video Player", false, 2400)] static void VideoPlayer() => Make("Video/Video Player");

    [MenuItem(M + "UI Toolkit/UI Document", false, 2500)] static void UIDocument() => Make("UI Toolkit/UI Document");

    [MenuItem(M + "Camera", false, 2600)] static void Camera_() => Make("Camera");

    [MenuItem(M + "UI/Canvas", false, 2700)]                     static void Canvas()      => Make("UI/Canvas");
    [MenuItem(M + "UI/Panel", false, 2701)]                      static void Panel()       => Make("UI/Panel");
    [MenuItem(M + "UI/Image", false, 2702)]                      static void Image()       => Make("UI/Image");
    [MenuItem(M + "UI/Raw Image", false, 2703)]                  static void RawImage()    => Make("UI/Raw Image");
    [MenuItem(M + "UI/Text - TextMeshPro", false, 2704)]         static void TextUI()      => Make("UI/Text - TextMeshPro");
    [MenuItem(M + "UI/Button - TextMeshPro", false, 2705)]       static void Button()      => Make("UI/Button - TextMeshPro");
    [MenuItem(M + "UI/Toggle", false, 2706)]                     static void Toggle()      => Make("UI/Toggle");
    [MenuItem(M + "UI/Slider", false, 2707)]                     static void Slider()      => Make("UI/Slider");
    [MenuItem(M + "UI/Scrollbar", false, 2708)]                  static void Scrollbar()   => Make("UI/Scrollbar");
    [MenuItem(M + "UI/Scroll View", false, 2709)]                static void ScrollView()  => Make("UI/Scroll View");
    [MenuItem(M + "UI/Dropdown - TextMeshPro", false, 2710)]     static void Dropdown()    => Make("UI/Dropdown - TextMeshPro");
    [MenuItem(M + "UI/Input Field - TextMeshPro", false, 2711)]  static void InputField()  => Make("UI/Input Field - TextMeshPro");
    [MenuItem(M + "UI/Event System", false, 2720)]               static void EventSystem() => Make("UI/Event System");
}
// Copyright (c) 2026 TekDuzce. All Rights Reserved.
// Free to use in Unity projects. Redistribution or resale of this script is strictly prohibited.

