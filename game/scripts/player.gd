extends CharacterBody3D

signal died

@export var move_speed: float = 5.0
@export var mouse_sensitivity: float = 0.2
@export var max_health: int = 5
var health: int

@onready var camera: Camera3D = $Pivot/Camera3D
@onready var pivot: Node3D = $Pivot
@onready var sword_area: Area3D = $SwordArea

var is_attacking := false
var attack_cooldown := 0.5
var _attack_timer := 0.0

func _ready() -> void:
    Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
    camera.current = true
    health = max_health
    sword_area.monitoring = false
    sword_area.body_entered.connect(_on_sword_area_body_entered)

func _process(delta: float) -> void:
    if Input.is_action_just_pressed("attack") and not is_attacking:
        _start_attack()
    if is_attacking:
        _attack_timer -= delta
        if _attack_timer <= 0.0:
            _end_attack()

func _start_attack() -> void:
    is_attacking = true
    _attack_timer = attack_cooldown
    sword_area.monitoring = true

func _end_attack() -> void:
    is_attacking = false
    sword_area.monitoring = false

func _unhandled_input(event: InputEvent) -> void:
    if event is InputEventMouseMotion:
        pivot.rotate_x(deg_to_rad(-event.relative.y * mouse_sensitivity))
        rotate_y(deg_to_rad(-event.relative.x * mouse_sensitivity))
        pivot.rotation.x = clamp(pivot.rotation.x, deg_to_rad(-60), deg_to_rad(60))

func _physics_process(delta: float) -> void:
    var input_dir = Vector3.ZERO
    if Input.is_action_pressed("move_forward"):
        input_dir.z -= 1
    if Input.is_action_pressed("move_backward"):
        input_dir.z += 1
    if Input.is_action_pressed("move_left"):
        input_dir.x -= 1
    if Input.is_action_pressed("move_right"):
        input_dir.x += 1
    input_dir = input_dir.normalized()

    var direction = (transform.basis * input_dir)
    direction.y = 0
    velocity.x = direction.x * move_speed
    velocity.z = direction.z * move_speed
    velocity.y -= ProjectSettings.get_setting("physics/3d/default_gravity") * delta
    move_and_slide()

func take_damage(amount: int) -> void:
    health = max(health - amount, 0)
    if health == 0:
        emit_signal("died")

func heal(amount: int) -> void:
    health = min(health + amount, max_health)

func _on_sword_area_body_entered(body: Node) -> void:
    if body.has_method("take_damage"):
        body.take_damage(1)
