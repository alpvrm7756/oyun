# Systems Architecture

## Engine & Technical Stack
- **Runtime:** Godot 4 project targeting desktop platforms with the Forward+ renderer.
- **Language:** GDScript for all gameplay code; scenes stored as `.tscn` files for readability in version control.
- **Rendering:** Procedural sky environment, simple directional lighting, and primitive meshes for greybox iteration.
- **Input:** Built-in Godot action map configured for WASD, mouse look, attack, and interaction actions.
- **State:** Node-based scripts manage player stats, quest state, and enemy behaviour.

## Core Modules
1. **Traversal & Camera**
   - `Player` (`CharacterBody3D`) processes move input every physics tick.
   - Mouse delta rotates the character and a pivot that carries the third-person camera.
2. **Combat**
   - Player sword swing toggles an `Area3D` used for proximity-based hit detection.
   - Enemy pursues the player, applying damage on a cooldown when inside melee range.
3. **Quest Flow**
   - `GameManager` tracks quest flags and updates the HUD text as objectives change.
   - `NPC` emits an `interacted` signal that the manager listens to for quest progression.
4. **World & Feedback**
   - Floating `Label3D` prompts appear when the player enters the NPC interaction radius.
   - A `CanvasLayer` HUD shows the current objective and contextual instructions.
5. **Content Authoring**
   - All gameplay parameters (speed, health, attack cooldowns) exposed as exported variables for quick tuning in the editor.
   - Scene graph keeps each gameplay element modular so designers can duplicate or swap components without touching code.

## Data & Content Management
- Godot scenes and resources live in the `game/` folder to keep import paths stable.
- Sub-resources inside `Main.tscn` define primitive meshes, collision shapes, and placeholder animations to avoid missing assets.
- Quest logic is intentionally linear; future branches can extend `GameManager` with additional states or a formal state machine.

## Technical Risks & Mitigations
- **Physics Feel:** Tune `move_speed`, gravity scale, and camera sensitivity inside the inspector to match target responsiveness.
- **Enemy Pathing:** Current behaviour is direct pursuit; add navigation meshes and `NavigationAgent3D` when environments grow complex.
- **Content Polish:** Replace primitive meshes and empty animations with authored models to prevent immersion-breaking visuals.

## Testing Strategy
- Use Godot's built-in `--headless --run` mode for automated smoke tests once CLI access is available.
- Add debug UI labels showing player health, enemy health, and quest state during iteration.
- Exercise input rebinds through the project settings menu before shipping wider builds.

## DevOps & Release
- Store exported builds under a `builds/` directory with semantic version tags.
- Use Godot's export presets to target Windows, Linux, and macOS with the same content.
- Consider enabling the Godot editor's asset library integration for designers to pull in environment dressing quickly.
