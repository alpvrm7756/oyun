# OyunRPG Unity Prototype

This repository now contains a Unity-based third-person RPG vertical slice. The playable scene showcases WASD traversal, sprinting, a stamina-gated melee attack, an aggressive enemy spirit, and a camp guide NPC that drives a short quest loop through dialogue.

## Features
- **Unity Runtime:** Configured for Unity 2022.3 LTS with a ready-made scene (`MainScene.unity`) under `Assets/Scenes`.
- **Third-Person Controller:** CharacterController-driven movement with sprinting, jumping, and responsive camera orbit controls.
- **Combat Loop:** Stamina-based sword swing that damages enemies in a cone and an enemy AI that pursues and retaliates.
- **Quest & Dialogue Systems:** Scriptable-object dialogue channel, interactive NPC, and quest manager that updates HUD objectives and completion state.
- **Runtime UI Bootstrap:** Objective tracker and dialogue panels are built at runtime so the scene stays lightweight and designer-friendly.

## Project Layout
```
unity/OyunRPG/              — Unity project root
  Assets/
    Scenes/MainScene.unity  — Playable prototype scene
    Scripts/                — Gameplay and systems C# scripts
    Data/                   — ScriptableObject assets (dialogue channels, etc.)
  ProjectSettings/          — Unity project configuration (build settings, editor version)
  Packages/                 — Package manifest with required Unity modules
```

## Getting Started
1. Install [Unity 2022.3 LTS](https://unity.com/releases/editor/whats-new/2022-lts) via Unity Hub.
2. From Unity Hub, choose **Open** → point to the `unity/OyunRPG` directory in this repository.
3. After the project imports, open `Assets/Scenes/MainScene.unity` and press <kbd>Play</kbd>.
4. Controls:
   - **WASD** / **Arrow Keys:** Move the hero
   - **Mouse:** Look around
   - **Left Shift:** Sprint
   - **Space:** Jump
   - **Left Mouse Button:** Attack
   - **E:** Interact / advance dialogue
   - **Space** or **E:** Advance dialogue lines when the dialogue panel is visible

## Extending the Slice
- Swap the primitive meshes with authored hero, enemy, and camp guide models (Animator hooks are ready through the existing scripts).
- Drop additional enemy prefabs into the scene and wire them to the `QuestManager` or extend it to support multi-step encounters.
- Expand the quest state machine by adding more objectives to the `QuestManager` list or introducing branching logic.
- Replace the runtime-generated HUD with designer-authored UI prefabs for deeper styling and localization.

## License
All C# scripts, scene files, and configuration assets are provided under the MIT license—use them as a foundation for your own RPG experiments.
