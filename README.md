# serious3dgame

A Unity 2022.3 LTS 3D game project with built-in render pipeline.

## Requirements

- Unity Hub
- Unity 2022.3 LTS (any patch version)
- WebGL Build Support module (optional, for WebGL builds)

## Getting Started

### First Time Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd serious3dgame
   ```

2. **Install Unity 2022.3 LTS**
   - Open Unity Hub
   - Go to Installs → Install Editor
   - Select Unity 2022.3 LTS
   - During installation, make sure to include:
     - Windows Build Support (for Windows builds)
     - Mac Build Support (for macOS builds)
     - WebGL Build Support (for WebGL builds)

3. **Open the project**
   - Open Unity Hub
   - Click "Add" or "Open"
   - Navigate to and select the repository root folder (the folder containing this README)
   - Unity Hub will recognize it as a Unity project
   - Click on the project to open it in Unity 2022.3 LTS

4. **First run**
   - Unity will import all assets (this may take a few minutes on first launch)
   - Once loaded, you should see the Main scene in the Hierarchy
   - Press the Play button ▶️ to test the project

### Project Structure

```
Assets/
├── Scenes/
│   └── Main.unity          # Main game scene
└── Scripts/
    ├── GameInitializer.cs  # Initialization script (logs startup)
    └── PlayerController.cs # Player movement script (WASD + Space)
```

## Playing the Game

1. **Open the Main scene** (if not already open):
   - In Unity, navigate to `Assets/Scenes/Main.unity`
   - Double-click to open

2. **Press Play** (▶️ button at the top of the Unity Editor)

3. **Controls**:
   - **WASD** or **Arrow Keys**: Move the player cube on the XZ plane
   - **Space**: Jump (when grounded)
   - The camera is positioned to view the scene from above

4. **Check the Console** for startup messages from GameInitializer

## Build Targets

This project is configured for:
- **Standalone** (Windows/macOS/Linux)
- **WebGL** (requires WebGL Build Support module)

To build:
1. Go to **File → Build Settings**
2. Select your target platform
3. Click **Switch Platform** (if needed)
4. Click **Build** or **Build And Run**

## WebGL Notes

To build for WebGL:
1. Install the WebGL Build Support module via Unity Hub:
   - Unity Hub → Installs → ⚙️ (gear icon) next to Unity 2022.3 LTS → Add Modules
   - Check "WebGL Build Support"
   - Click "Install"

2. After installation, you can build for WebGL:
   - **File → Build Settings**
   - Select "WebGL"
   - Click "Build And Run" or "Build"

## Company and Product Information

- **Company Name**: Default
- **Product Name**: serious3dgame
- **Version**: 0.1.0

## Troubleshooting

- **Missing references**: Make sure the project was cloned completely with all files
- **Scene not loading**: Navigate to `Assets/Scenes/Main.unity` and open it manually
- **Scripts not compiling**: Wait for Unity to finish importing all assets
- **Build target unavailable**: Install the required build support module via Unity Hub

## Development

The project uses:
- Unity 2022.3 LTS
- Built-in Render Pipeline
- Legacy Input System (Horizontal/Vertical axes)
- C# for scripting

The Main scene includes:
- Main Camera with GameInitializer script
- Directional Light
- Ground plane (scaled cube)
- Player cube with Rigidbody and PlayerController script

## License

All rights reserved.
