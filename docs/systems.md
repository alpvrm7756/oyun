# Systems Architecture

## Engine & Technical Stack
- **Runtime:** Unity 2022.3 LTS project configured for the Built-in Render Pipeline.
- **Language:** C# scripts organised under `Assets/Scripts` with runtime-generated UI to minimise scene churn.
- **Rendering:** Simple directional light with primitive meshes; swap in authored assets as production advances.
- **Input:** Unity's legacy input axes for WASD + mouse look, sprint, jump, attack, and interaction.
- **State:** MonoBehaviours coordinate player stats, quest state, dialogue flow, and enemy behaviour.

## Core Modules
1. **Traversal & Camera**
   - `PlayerController` uses a `CharacterController` to process movement, sprinting, and jumping.
   - `CameraRig` orbits a follow target, clamping pitch and smoothing camera positioning.
2. **Combat**
   - `PlayerCombat` consumes stamina, performs cone checks with `Physics.OverlapSphere`, and applies damage to any `IDamageable` enemies.
   - `EnemyController` chases the player, runs cooldown-based melee strikes, and exposes a health-changed event for quest hooks.
3. **Quest Flow**
   - `QuestManager` tracks a linear chain of objectives, updates the HUD, and listens for enemy death + NPC interactions to progress.
   - `ObjectiveHUD` instantiates a screen-space canvas at runtime and renders objective text + completion banners.
4. **Dialogue**
   - `DialogueChannel` (ScriptableObject) lets world actors trigger UI updates without tight coupling.
   - `DialogueUIController` creates a dialogue panel, reveals text over time, and pauses/resumes cursor lock state during conversations.
5. **World & Feedback**
   - `NPCController` checks proximity to the player, funnels dialogue via the channel, and notifies the quest manager.
   - `GameBootstrap` locks the cursor, wires enemy targeting, and reacts to dialogue state transitions.

## Data & Content Management
- ScriptableObject channels live in `Assets/Data/` so they can be reused across scenes.
- Scene keeps the quest/NPC/enemy setup lean; designers can drop additional prefabs into `MainScene.unity` without touching code.
- Parameters like move speed, stamina costs, attack damage, and dialogue lines are all exposed as serialized fields.

## Technical Risks & Mitigations
- **Physics Feel:** Tune the `CharacterController` slope, step offset, and the movement/rotation speeds to achieve desired responsiveness.
- **Camera Comfort:** Adjust `CameraRig` sensitivity and pitch clamps to match target platforms (gamepad vs. mouse).
- **UI Scalability:** Runtime-generated HUD works for a prototype; switch to authored canvases + localization packages as the UI grows.
- **Content Variety:** Expand `QuestManager` to support branching states or a finite-state machine as narrative complexity increases.

## Testing Strategy
- Use the Unity Test Framework for play mode smoke tests (e.g., verifying quest completion triggers when the enemy dies).
- Wire up development-only gizmos and debug text to monitor stamina, health, and quest state changes in real time.
- Integrate with the Input System package if you need rebindable controls or gamepad-first iteration.

## DevOps & Release
- Store generated builds under a `Builds/` directory with semantic version tags.
- Use Unity Cloud Build or CI runners to produce Windows/macOS/Linux builds after each milestone.
- Document required packages and Unity versions in `ProjectSettings/ProjectVersion.txt` to keep collaborators in sync.
