extends CharacterBody3D

@export var move_speed: float = 2.5
@export var max_health: int = 3
@export var attack_range: float = 1.6
@export var attack_cooldown: float = 1.2
@export var damage: int = 1

@onready var player_detector: Area3D = $PlayerDetector
@onready var animation_player: AnimationPlayer = $AnimationPlayer

var health: int
var _time_to_next_attack: float = 0.0
var _target: Node3D = null

func _ready() -> void:
    health = max_health
    animation_player.play("idle")
    player_detector.body_entered.connect(_on_body_entered)
    player_detector.body_exited.connect(_on_body_exited)

func _physics_process(delta: float) -> void:
    if _target == null:
        animation_player.play("idle")
        velocity = Vector3.ZERO
        move_and_slide()
        return

    var direction = (_target.global_transform.origin - global_transform.origin)
    direction.y = 0
    var distance = direction.length()
    direction = direction.normalized()

    if distance > attack_range:
        animation_player.play("walk")
        velocity.x = direction.x * move_speed
        velocity.z = direction.z * move_speed
        velocity.y -= ProjectSettings.get_setting("physics/3d/default_gravity") * delta
        move_and_slide()
    else:
        velocity = Vector3.ZERO
        move_and_slide()
        if _time_to_next_attack <= 0.0:
            _attack_target()

    if _time_to_next_attack > 0.0:
        _time_to_next_attack -= delta

func _attack_target() -> void:
    animation_player.play("attack")
    if _target and _target.has_method("take_damage"):
        _target.take_damage(damage)
    _time_to_next_attack = attack_cooldown

func _on_body_entered(body: Node) -> void:
    if body.name == "Player":
        _target = body

func _on_body_exited(body: Node) -> void:
    if body == _target:
        _target = null

func take_damage(amount: int) -> void:
    health = max(health - amount, 0)
    if health == 0:
        animation_player.play("die")
        queue_free()
