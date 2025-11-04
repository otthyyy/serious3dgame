# 🎮 MVP – First Person 3D Game (Unity)

A playable first-person locomotion prototype for testing movement, jumping, and sprinting in a simple 3D greybox environment.

## 🎯 Objective

Create a playable first-person prototype to test locomotion mechanics with smooth movement, sprint, and jump in a simple 3D environment. The game can be opened, modified, and launched from Unity Hub with a single click on Play.

## ⚙️ Technical Specifications

- **Engine**: Unity 2022.3 LTS (compatible with 2021.3 LTS and higher)
- **Pipeline**: Built-in Render Pipeline
- **Target Platform**: Windows PC
- **Player Controller**: Unity CharacterController
- **Input System**: Unity Input System (new)
- **Main Scene**: Main.unity

## 🎮 Controls

| Key | Action |
|-----|--------|
| W / A / S / D | Forward / Left / Backward / Right movement |
| Mouse | Camera rotation (first-person view) |
| Left Shift (hold) | Sprint (speed increase ×1.6) |
| Space | Jump |
| Esc | Pause / Game menu |

## 🧠 Implemented Mechanics

- **Smooth Walking**: Constant movement with controlled acceleration/deceleration
- **Sprint**: Speed increase ×1.6 when holding Shift
- **Jump**: Physical vertical impulse with custom gravity
- **Air Control**: Limited movement control while airborne
- **Collisions**: Via CharacterController.Move() to prevent clipping
- **Camera**: Mouse-controlled, vertical rotation limited to ±85°

## 🌍 Test Level ("Greybox")

A minimalist testing arena to validate movement:

- Main platform (40×40 m)
- Ramps at various slopes
- Jump blocks (1 m and 1.5 m)
- Open area for sprint testing
- Simple obstacles for collision testing

## 🖥️ UI and HUD

- Minimal crosshair at screen center
- Sprint indicator (optional)
- FPS counter (Debug mode only)
- Pause menu with:
  - Mouse sensitivity
  - FOV slider
  - Master volume
  - Quit Game button

## 🔊 Audio

- **Footsteps**: Short alternating sound, increased frequency during sprint
- **Jump**: High-pitched sound on takeoff
- **Landing**: Low-pitched sound when touching ground after a fall
- **Master Volume**: Adjustable via menu

## 🧩 Project Structure

```
Assets/
├── Scenes/
│   └── Main.unity
├── Scripts/
│   ├── FirstPersonController.cs
│   ├── FirstPersonCamera.cs
│   ├── GameManager.cs
│   └── UI/
│       ├── PauseMenu.cs
│       ├── Crosshair.cs
│       └── FPSCounter.cs
├── Prefabs/
│   ├── Player.prefab
│   └── GreyboxEnvironment.prefab
├── Materials/
│   ├── Grey.mat
│   ├── GreyLight.mat
│   └── GreyDark.mat
├── InputActions/
│   ├── PlayerInputActions.inputactions
│   └── PlayerInputActions.cs
└── Audio/
    └── (audio clips)
```

## 🧾 Default Parameters

| Parameter | Value | Description |
|-----------|-------|-------------|
| Walk Speed | 4.5 m/s | Base speed |
| Sprint Speed | 7.2 m/s | Speed with Shift |
| Jump Force | 4.5 m/s | Vertical impulse |
| Gravity | -20 m/s² | Gravity force |
| Look Sensitivity | 0.3 | Mouse sensitivity |
| Base FOV | 90° | Base field of view |
| Sprint FOV Kick | +3° | FOV variation during sprint |

## ✅ Acceptance Criteria

- ✅ Project opens and runs from Unity Hub without errors
- ✅ Player moves smoothly with WASD and Shift
- ✅ Camera responds to mouse without lag or jitter
- ✅ Collisions are solid (no clipping)
- ✅ Jump works even at platform edges
- ✅ FPS ≥ 60 constant at 1080p on medium hardware
- ✅ No crashes, freezes, or softlocks

## 🚀 Getting Started

### First Time Setup

1. **Install Unity 2022.3 LTS**
   - Open Unity Hub
   - Go to Installs → Install Editor
   - Select Unity 2022.3 LTS
   - Include Windows Build Support

2. **Open the project**
   - Open Unity Hub
   - Click "Add" or "Open"
   - Select the repository root folder
   - Unity Hub will recognize it as a Unity project
   - Click on the project to open it

3. **First run**
   - Unity will import all assets (may take a few minutes)
   - Once loaded, you should see the Main scene
   - Press the Play button ▶️ to test

### Playing the Game

1. Open the Main scene: `Assets/Scenes/Main.unity`
2. Press Play (▶️ button)
3. Use WASD to move, mouse to look, Shift to sprint, Space to jump
4. Press Esc to pause

## 🔧 Building

To create a standalone build:

1. Go to **File → Build Settings**
2. Select your target platform (PC, Mac & Linux Standalone)
3. Click **Switch Platform** (if needed)
4. Click **Build** or **Build And Run**

## 🔮 Future Extensions (Post-MVP)

- Stamina system for sprinting
- Crouch and slide
- Different surfaces with distinct sounds
- Key rebinding
- Checkpoint system
- VR support (basic head tracking)

## 💻 Development

The project uses:
- Unity 2022.3 LTS
- Built-in Render Pipeline
- Unity Input System (new)
- CharacterController for movement
- C# for scripting

## 📝 License

All rights reserved.
