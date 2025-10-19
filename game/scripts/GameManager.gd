extends Node3D

@export var obelisk_scene:PackedScene
@export var obelisk_locations:Array[Vector3] = [
    Vector3(10, 0, 10),
    Vector3(-12, 0, 14),
    Vector3(-5, 0, -15)
]

var player:Node3D
var hud:CanvasLayer

func _ready() -> void:
    player = $Player
    hud = $HUD
    if hud.has_method("connect_player"):
        hud.connect_player(player)
    _register_quests()
    _spawn_obelisks()
    _setup_enemies()

func _register_quests() -> void:
    QuestLog.register_quest(
        "cleanse_obelisks",
        "Cleanse the Obelisks",
        "Purify three corrupted obelisks within the valley.",
        3,
        {
            "experience": 250,
            "items": ["Elixir of Clarity"]
        }
    )

func _spawn_obelisks() -> void:
    if obelisk_scene == null:
        obelisk_scene = preload("res://scenes/Obelisk.tscn")
    for location in obelisk_locations:
        var obelisk := obelisk_scene.instantiate()
        add_child(obelisk)
        obelisk.global_transform.origin = location
        obelisk.cleansed.connect(_on_obelisk_cleansed)

func _setup_enemies() -> void:
    for enemy in $EnemyCamp.get_children():
        if enemy.has_method("set_target"):
            enemy.set_target(player)

func _on_obelisk_cleansed(obelisk:Node) -> void:
    QuestLog.increment_progress("cleanse_obelisks")
