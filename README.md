# OyunRPG Godot Vertical Slice

This repository now ships a compact third-person RPG prototype built with **Godot 4**. The slice includes WASD traversal, a melee attack, a roaming enemy, and a short quest that guides the player from NPC dialogue through combat and back to the quest giver.

## Features
- **Character Controller:** Mouse-look and WASD movement driven by a `CharacterBody3D` script.
- **Enemy AI:** A simple pursuer that aggroes when the player approaches and uses cooldown-based melee attacks.
- **Quest Loop:** Talk to the camp guide, defeat the spirit, then return for a completion state and updated dialogue.
- **HUD Overlay:** CanvasLayer UI describing the current objective and contextual prompts.
- **Interaction Prompts:** Floating 3D label on the NPC that illuminates when the player is within range.

## Project Layout
```
/game/project.godot     — Engine configuration and input bindings
/game/scenes/Main.tscn  — Root scene wiring the level, player, enemy, NPC, and UI
/game/scripts/*.gd      — Gameplay scripts for the player, enemy, NPC, and quest flow
/docs                   — Creative vision, systems notes, and long-term roadmap
```

## Running the Prototype
1. Install [Godot 4.2 or newer](https://godotengine.org/download).
2. Open the project by launching Godot and choosing **Import** → select the `game/project.godot` file.
3. Once the project loads, open `Main.tscn` and press <kbd>F5</kbd> to play, or run directly from the Godot project manager.
4. Controls:
   - **WASD:** Move the player
   - **Mouse:** Look around (pointer is captured automatically)
   - **Left Mouse Button:** Attack with the sword arc
   - **E:** Interact with the NPC when close

## Extending the Slice
- Swap primitive meshes for authored characters and environment dressing.
- Add health bars and damage feedback to the CanvasLayer for better combat readability.
- Introduce additional enemy types and quest steps by duplicating the existing scripts and adjusting parameters.

## License
All scripts and scene definitions are released under the MIT license. Use them as a springboard for your own adventures.
