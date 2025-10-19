extends Node3D

signal interacted

@export var dialogue_lines: Array[String] = [
    "Welcome, traveler!",
    "A restless spirit prowls nearby. Could you help us?",
    "Defeat it and return for your reward."
]

var is_player_in_range := false
var _current_line := 0
var _ready_for_turn_in := false

@onready var prompt_label: Label3D = $PromptLabel
@onready var detection_area: Area3D = $DetectionArea

func _ready() -> void:
    prompt_label.text = "Press E"
    prompt_label.visible = false
    detection_area.body_entered.connect(_on_detection_area_body_entered)
    detection_area.body_exited.connect(_on_detection_area_body_exited)

func _process(_delta: float) -> void:
    prompt_label.visible = is_player_in_range

func _on_detection_area_body_entered(body: Node) -> void:
    if body.name == "Player":
        is_player_in_range = true

func _on_detection_area_body_exited(body: Node) -> void:
    if body.name == "Player":
        is_player_in_range = false

func _unhandled_input(event: InputEvent) -> void:
    if not is_player_in_range:
        return
    if event.is_action_pressed("interact"):
        interacted.emit()
        _show_dialogue()

func _show_dialogue() -> void:
    if _ready_for_turn_in:
        prompt_label.text = "Quest Complete!"
        return
    prompt_label.text = dialogue_lines[_current_line]
    _current_line = (_current_line + 1) % dialogue_lines.size()

func set_ready_for_turn_in() -> void:
    _ready_for_turn_in = true
    dialogue_lines = [
        "You did it! The camp is safe again.",
        "Rest here whenever you need."
    ]
    _current_line = 0
    prompt_label.text = "Press E"
