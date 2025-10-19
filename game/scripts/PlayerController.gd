extends CharacterBody3D

@export var speed:float = 5.0
@export var acceleration:float = 6.0
@export var mouse_sensitivity:float = 0.15
@export var jump_velocity:float = 5.5
@export var gravity:float = ProjectSettings.get_setting("physics/3d/default_gravity")

var camera_pivot:Node3D
var stats:StatsComponent
var velocity_target:Vector3 = Vector3.ZERO
var look_rotation:Vector2 = Vector2.ZERO
var combat_cooldown:float = 0.6
var combat_timer:float = 0.0

signal attacked(target:Node)
signal interaction_prompt(text:String)
signal interacted(target:Node)

func _ready() -> void:
    Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
    camera_pivot = $CameraPivot
    stats = $Stats
    stats.health_changed.connect(_on_health_changed)
    stats.mana_changed.connect(_on_mana_changed)
    stats.leveled_up.connect(_on_leveled_up)
    stats.died.connect(_on_player_died)

func _unhandled_input(event:InputEvent) -> void:
    if event is InputEventMouseMotion:
        look_rotation.x -= event.relative.y * mouse_sensitivity * 0.01
        look_rotation.y -= event.relative.x * mouse_sensitivity * 0.01
        look_rotation.x = clamp(look_rotation.x, deg_to_rad(-60), deg_to_rad(65))
        _apply_look()

func _physics_process(delta:float) -> void:
    _handle_movement(delta)
    _handle_jump()
    _handle_combat(delta)
    _handle_interaction()

func _handle_movement(delta:float) -> void:
    var input_dir := Vector2.ZERO
    input_dir.y = Input.get_action_strength("action_move_backward") - Input.get_action_strength("action_move_forward")
    input_dir.x = Input.get_action_strength("action_move_right") - Input.get_action_strength("action_move_left")

    if input_dir.length() > 0.01:
        input_dir = input_dir.normalized()
        var forward := -transform.basis.z
        var right := transform.basis.x
        velocity_target = (forward * input_dir.y + right * input_dir.x) * speed
    else:
        velocity_target = Vector3.ZERO

    velocity.x = lerp(velocity.x, velocity_target.x, acceleration * delta)
    velocity.z = lerp(velocity.z, velocity_target.z, acceleration * delta)
    if not is_on_floor():
        velocity.y -= gravity * delta

    move_and_slide()

func _handle_jump() -> void:
    if Input.is_action_just_pressed("action_jump") and is_on_floor():
        velocity.y = jump_velocity

func _handle_combat(delta:float) -> void:
    combat_timer -= delta
    if Input.is_action_just_pressed("action_primary") and combat_timer <= 0.0:
        combat_timer = combat_cooldown
        var ray := $InteractRay
        ray.force_raycast_update()
        if ray.is_colliding():
            var collider := ray.get_collider()
            if collider.has_method("apply_damage"):
                collider.apply_damage(10)
                stats.gain_experience(15)
                emit_signal("attacked", collider)

func _handle_interaction() -> void:
    var ray := $InteractRay
    ray.force_raycast_update()
    if ray.is_colliding():
        var collider := ray.get_collider()
        if collider.has_method("on_interact"):
            var prompt_text := ""
            if collider.has_method("get_interaction_text"):
                prompt_text = collider.get_interaction_text()
            emit_signal("interaction_prompt", prompt_text)
            if Input.is_action_just_pressed("action_secondary"):
                collider.on_interact(self)
                emit_signal("interacted", collider)
        else:
            emit_signal("interaction_prompt", "")
    else:
        emit_signal("interaction_prompt", "")

func _apply_look() -> void:
    rotation.y = look_rotation.y
    camera_pivot.rotation.x = look_rotation.x

func _on_health_changed(current:int, maximum:int) -> void:
    if current <= 0:
        Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)

func _on_mana_changed(current:int, maximum:int) -> void:
    pass

func _on_leveled_up(new_level:int) -> void:
    speed += 0.2

func _on_player_died(actor:Node) -> void:
    DialogueBus.emit_dialogue("System", "You have fallen. Press Esc to exit.")
