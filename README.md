# OyunRPG Prototype

OyunRPG is a playable third-person prototype built in **Godot 4** that showcases core loops for an expansive 3D action RPG. The project now contains a vertical slice featuring traversal, light melee combat, quest progression, interactive world objects, and a minimal HUD. All assets are procedural or engine-generated so the project can be cloned and explored without external dependencies.

## Features
- **Player controller** with WASD+mouse movement, sprint-style acceleration, jumping, and directional combat raycasts.
- **Stats system** shared by the player and enemies that handles leveling, health, mana, and experience rewards.
- **Enemy AI** that patrols and pursues the player, attacking within range and dealing damage over time.
- **Quest pipeline** driven by a global QuestLog singleton. The included "Cleanse the Obelisks" quest tracks progress and grants rewards when complete.
- **Interactive world objects** like corrupted obelisks that consume mana to cleanse and NPCs that gate quests via branching dialogue.
- **Reactive HUD** that mirrors health/mana, quest progress, interaction prompts, and streamed dialogue.

## Project Layout
```
/game/project.godot   — Godot project configuration with input map and autoloads
/game/scenes          — Packed scenes for the player, enemies, NPCs, obelisks, and the main world
/game/scripts         — Gameplay logic for characters, quests, dialogue, combat, and world state
/game/ui              — HUD scene + script wiring runtime signals into UI widgets
/docs                 — High-level vision, systems documentation, and production roadmap
```

## Getting Started
1. Install **Godot 4.1+**.
2. Open the project by selecting `game/project.godot` in the Godot project manager.
3. Press ▶️ to launch the main scene. Use **WASD** to move, **Space** to jump, **Left Click** to attack, **Right Click** to interact, and **Esc** to release the mouse.
4. Talk to the guide NPC, cleanse the three obelisks scattered throughout the valley, defeat hostile spheres, and watch the HUD update in real time.

## Extending the Slice
- Replace primitive meshes with authored environment pieces, characters, and VFX.
- Expand the `QuestLog` rewards to drop inventory items and trigger cutscenes.
- Add additional enemy archetypes with bespoke AI states and abilities.
- Build crafting, dialogue choice trees, and save/load systems on top of the provided architecture.

## License
This prototype ships with engine-generated geometry and code only. You are free to adapt, extend, and commercialize the project. Attribution is appreciated but not required.
