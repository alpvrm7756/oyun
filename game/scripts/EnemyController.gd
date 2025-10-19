extends CharacterBody3D

@export var patrol_points:Array[Vector3] = []
@export var detection_range:float = 12.0
@export var attack_range:float = 2.0
@export var move_speed:float = 3.0
@export var attack_cooldown:float = 1.4

var stats:StatsComponent
var current_point_index:int = 0
var attack_timer:float = 0.0
var target:Node3D

signal died(enemy:Node)

func _ready() -> void:
    stats = $Stats
    stats.health_changed.connect(_on_health_changed)

func _physics_process(delta:float) -> void:
    attack_timer -= delta
    if target and is_instance_valid(target):
        _move_towards(target.global_transform.origin, delta)
        if global_transform.origin.distance_to(target.global_transform.origin) <= attack_range:
            _attempt_attack()
    else:
        _patrol(delta)

func set_target(new_target:Node3D) -> void:
    target = new_target

func _move_towards(destination:Vector3, delta:float) -> void:
    var direction := (destination - global_transform.origin)
    direction.y = 0
    if direction.length() > 0.1:
        direction = direction.normalized()
        velocity.x = direction.x * move_speed
        velocity.z = direction.z * move_speed
        look_at(global_transform.origin + direction, Vector3.UP)
    else:
        velocity.x = 0
        velocity.z = 0
    if not is_on_floor():
        velocity.y -= ProjectSettings.get_setting("physics/3d/default_gravity") * delta
    move_and_slide()

func _patrol(delta:float) -> void:
    if patrol_points.is_empty():
        velocity = Vector3.ZERO
        return
    var current_point := patrol_points[current_point_index]
    if global_transform.origin.distance_to(current_point) < 0.5:
        current_point_index = (current_point_index + 1) % patrol_points.size()
    _move_towards(current_point, delta)

func _attempt_attack() -> void:
    if attack_timer > 0.0:
        return
    attack_timer = attack_cooldown
    if target and target.has_node("Stats"):
        var target_stats:StatsComponent = target.get_node("Stats")
        target_stats.apply_damage(stats.strength)

func apply_damage(amount:int) -> void:
    stats.apply_damage(amount)

func _on_health_changed(current:int, maximum:int) -> void:
    if current <= 0:
        emit_signal("died", self)
        queue_free()
