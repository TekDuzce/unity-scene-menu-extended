# SceneMenuExtended

![Unity](https://shields.io)
![License](https://shields.io)
![Editor Only](https://shields.io)

Restores and adds Unity's **Create Object** menu (`Create Empty`, `3D Object`, `Audio`, `Effects`, `Light`, `Video`, `UI Toolkit`, `Camera`, `UI`, etc.) directly to the **Scene View right-click context menu**. 

Instead of only seeing basic Cut/Copy/Paste options when right-clicking in the Scene View, you can spawn objects directly where your mouse cursor is pointing.

---

## Requirements

* **Unity 2023.2 or newer** (required because Unity introduced the Scene View right-click menu in 2023.2).

---

## Installation

### Option 1: Using the `.unitypackage` (Recommended)
1. Download the latest `.unitypackage` from the [Releases](https://github.com[Your-Username]/[Your-Repo-Name]/releases) page.
2. Open your Unity project and go to **Assets > Import Package > Custom Package...**
3. Select the downloaded `.unitypackage` file and click **Import**.

### Option 2: Manual C# Script Installation
1. Download `SceneRightClickCreate.cs` from this repository.
2. Place it inside a folder named **`Editor`** anywhere in your project's `Assets` folder (e.g., `Assets/Editor/SceneRightClickCreate.cs`).
   > **Note:** This is an **Editor-only** script and **must** live inside a folder named `Editor` so Unity excludes it from game builds.

---

## How It Works

1. **Cursor Tracking:** Every time you right-click inside the Scene View, the tool calculates and remembers the exact 3D world point beneath your mouse cursor.
2. **Native Menu Execution:** Selecting an item runs Unity's built-in `GameObject/...` menu command, ensuring you get the exact same default object and components as the Hierarchy menu.
3. **Smart Placement:** The newly spawned object is automatically moved to the remembered world position where you right-clicked.

---

## Uninstallation

Because this tool is completely self-contained and leaves no leftover data in your scenes, simply **delete `SceneRightClickCreate.cs`** (or its `Editor` folder) to remove the feature.

---

## Acknowledgments

* Built with the help of AI tools for coding assistance and documentation, alongside human direction, design, and testing in Unity.

---

## License

**Copyright (c) 2026 TekDuzce. All Rights Reserved.**

* **Allowed:** You are completely free to use and modify this code inside Unity to **build and sell** your own personal or commercial games.
* **Not Allowed:** You may **NOT** steal, re-upload, or sell this source code or `.unitypackage` (modified or unmodified) as a standalone asset, plugin, or tool anywhere.

See the [LICENSE](LICENSE) file for full terms.
