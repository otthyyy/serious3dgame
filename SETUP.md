# Setup Guide for First-Person MVP

This guide will help you complete the setup of the first-person locomotion prototype in Unity.

## Automatic Setup (Recommended)

1. Open the project in Unity 2022.3 LTS
2. Wait for Unity to import all assets and compile scripts
3. Open `Assets/Scenes/Main.unity`
4. In the Unity menu, go to `GameObject → 3D Object → Create First-Person Scene`
5. The scene will be automatically populated with all required components

## Manual Setup (If Automatic Doesn't Work)

If you need to set up the scene manually, follow these steps:

### 1. Create the Player

1. In the Hierarchy, delete the existing "Main Camera" and "Player" if they exist
2. Drag `Assets/Prefabs/Player.prefab` into the scene
3. Position it at (0, 1, 0)
4. The Player prefab already contains:
   - CharacterController
   - FirstPersonController script
   - AudioSource
   - Camera with FirstPersonCamera script

### 2. Create the Environment

1. Drag `Assets/Prefabs/GreyboxEnvironment.prefab` into the scene
2. Position it at (0, 0, 0)
3. This contains:
   - Ground platform (40×40m)
   - Ramps (low and steep)
   - Jump blocks (1m and 1.5m heights)
   - Walls and columns
   - Additional platforms

### 3. Create the Game Manager

1. Create an empty GameObject: `GameObject → Create Empty`
2. Name it "GameManager"
3. Add the GameManager script component
4. Configure settings:
   - Master Volume: 1.0
   - Mouse Sensitivity: 0.3
   - Field of View: 90

### 4. Create the UI Canvas

1. Create a UI Canvas: `GameObject → UI → Canvas`
2. Set Canvas Scaler to "Scale With Screen Size"
3. Reference Resolution: 1920 × 1080

#### 4.1. Create Crosshair

1. Create a UI Image: `GameObject → UI → Image` as child of Canvas
2. Name it "Crosshair"
3. Set Anchor to center-center
4. Set Position to (0, 0, 0)
5. Set Size to (6, 6)
6. Set Color to white with alpha ~0.8
7. Add the `Crosshair` script component

#### 4.2. Create FPS Counter

1. Create a UI Text: `GameObject → UI → Text - TextMeshPro` (or Legacy Text if TMP not available)
2. Name it "FPS Counter"
3. Set Anchor to top-right
4. Set Position to (-20, -20, 0)
5. Set Size to (100, 30)
6. Set Font Size to 16
7. Set Color to green
8. Add the `FPSCounter` script component
9. Drag the Text component to the script's "FPS Text" field

#### 4.3. Create Pause Menu

1. Create a UI Panel: `GameObject → UI → Panel` as child of Canvas
2. Name it "PauseMenuPanel"
3. Set Color to semi-transparent black (0, 0, 0, 0.8)
4. Make it fill the screen (stretch anchor)

5. Add UI elements as children:
   - **Title Text**: "PAUSED" (centered, large font)
   - **Resume Button**: "Resume" button
   - **Sensitivity Slider**: With label "Mouse Sensitivity"
   - **FOV Slider**: With label "Field of View"
   - **Volume Slider**: With label "Master Volume"
   - **Quit Button**: "Quit Game" button

6. Create an empty GameObject under Canvas, name it "PauseMenuManager"
7. Add the `PauseMenu` script component
8. Connect the PauseMenuPanel to the script

### 5. Configure Lighting (Optional)

The scene should already have a Directional Light. If not:

1. Create: `GameObject → Light → Directional Light`
2. Rotation: (50, -30, 0)
3. Color: White
4. Intensity: 1

### 6. Configure Tags

Make sure these tags exist in your project:

1. Go to `Edit → Project Settings → Tags and Layers`
2. Add these tags if missing:
   - "Player"
   - "Ground"

### 7. Test the Game

1. Make sure the Player prefab is tagged as "Player"
2. Press Play (▶️)
3. Test controls:
   - WASD: Movement
   - Mouse: Look around
   - Space: Jump
   - Left Shift: Sprint
   - ESC: Pause menu

## Troubleshooting

### Input System Not Working

If you get an error about the Input System:

1. Go to `Edit → Project Settings → Player`
2. Find "Active Input Handling"
3. Set it to "Input System Package (New)" or "Both"
4. Unity will ask to restart - click Yes

### Scripts Not Compiling

1. Check the Console window for errors
2. Make sure all required packages are installed:
   - Go to `Window → Package Manager`
   - Ensure "Input System" package is installed

### Player Falls Through Ground

1. Make sure the GreyboxEnvironment prefab has colliders on all platforms
2. Check that the Player has a CharacterController component
3. Verify Ground Check settings in FirstPersonController

### Camera Not Moving

1. Check that the FirstPersonCamera script is attached to the camera
2. Verify "Player Body" reference points to the Player's transform
3. Make sure Cursor is locked (it should be in play mode)

### No Audio

1. Check that Audio Listener is on the Camera
2. Verify AudioSource is on the Player
3. Check that Master Volume is not 0
4. Audio clips should be in `Assets/Audio/`

## Next Steps

After setup is complete:

1. Adjust player settings in FirstPersonController to your liking
2. Tweak camera sensitivity in FirstPersonCamera
3. Add more platforms and obstacles to test movement
4. Test sprint FOV kick effect
5. Verify jump height on different platforms

## Building the Game

To create a standalone build:

1. `File → Build Settings`
2. Add the Main scene if not already there
3. Select PC, Mac & Linux Standalone
4. Click "Build" and choose output folder
5. Run the .exe file to test

Enjoy your first-person locomotion prototype! 🎮
