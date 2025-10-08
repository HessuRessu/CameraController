# 🎥 CameraController for Unity

A flexible and reusable camera controller system for Unity, designed for strategy and third-person style games.  
The system is built with **behaviour classes**, making it easy to extend and adapt for different camera styles.

---

## ✨ Features

- **Behaviour-based architecture**
  - `RTSCameraBehaviour` for free RTS-style movement  
  - `TargetedCameraBehaviour` for follow/target lock style  
  - etc.
  - Easily extendable with new behaviours  
- **Camera configuration**  
  - All settings are centralized in `CameraConfiguration`  
  - Supports zoom, rotation, smoothing, movement speed, bounds, and more  
- **Line-of-Sight (LOS) correction**  
  - Auto-pitch adjustment to maintain visibility of the target  
- **Collision handling**  
  - Lightweight RayCast-based camera collision correction  
- **Serializable state**  
  - Camera state is stored in `CameraData` and `CameraTransform`, allowing future save/load support  
- **Utility methods**  
  - `CameraUtils` provides helpers for axis smoothing, LOS correction, and collision handling  
- **Input configuration**  
  - Should work with both Legacy and New Input Systems  
  - Configurable via `CameraConfiguration`  

---

## 📂 Repository Structure

```
CameraController/
├─ Runtime/
│ ├─ CameraController.cs
│ ├─ CameraConfiguration.cs
│ ├─ CameraData.cs
│ ├─ CameraInputAsset.inputactions
│ ├─ Behaviour/
│ │ ├─ BaseCameraBehaviour.cs
│ │ ├─ CinematicCameraBehaviour.cs
│ │ ├─ FollowingCameraBehaviour.cs
│ │ ├─ FreeCameraBehaviour.cs
│ │ ├─ ICameraBehaviour.cs
│ │ └─ RTSCameraBehaviour.cs
│ ├─ Controls/
│ │ ├─ BaseInputControlManager.cs
│ │ ├─ IInputControlManager.cs
│ │ ├─ LegacyInputControlManager.cs
│ │ └─ NewInputControlManager.cs
│ └─ Utils/
│   ├─ CameraUtils.cs
│   ├─ CameraRay.cs
│   ├─ Area.cs
│   ├─ Boundary.cs
│   └─ CameraTransform.cs
└─ Editor/
  └─ CameraConfigurationDrawer.cs
```

---

## 🚀 Installation

### Option 1: Copy-paste
Simply copy the `Runtime` folder into your Unity project's `Assets/Scripts/Camera/` folder.

### Option 2: Unity Package Manager (recommended)
You can add this repository directly as a Git package:

1. Open **Unity → Window → Package Manager**  
2. Click **+ → Add package from git URL**  
3. Paste:  
   `https://github.com/hessuressu/CameraController.git`

---

## 🛠️ Usage

1. Attach `CameraController` script to GameObject containing Main Camera.  
2. Assign:  
   - A **Target** (e.g. the player or focus point)  
   - A **CameraConfiguration** values  
     Note: Assign **InputActionAsset** to CameraConfiguration if using new input system.  
     example InputActionAsset `CameraInputAsset` is provided with the package.
   - Click **Detectd Defaults** button to assign ray / bounds values based on Terrain object (Optional)
3. Choose your default behaviour (used with Behaviour index).
   - RTS = 0
   - Following = 1
   - Freeform = 2
   - Simple cinematic = 3
4. Play and test controls:  
   - **Zoom**: Mouse ScrollWheel  
   - **Rotate**: Mouse drag (or Q/E keys)  
   - **Move**: WASD / Arrow keys  

---

## 🔮 Roadmap

- [ ] Add save/load support (JSON serialization for `CameraData`)  

---

## 📜 License

MIT License. Free to use and modify in your projects.
