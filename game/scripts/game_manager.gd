extends Node

@onready var quest_label: Label = $CanvasLayer/QuestPanel/MarginContainer/VBoxContainer/QuestLabel
@onready var interaction_label: Label = $CanvasLayer/QuestPanel/MarginContainer/VBoxContainer/InteractionLabel
@onready var npc: Node = get_node("../Campfire/NPC")
@onready var enemy: Node = get_node("../Enemy")

var quest_started := false
var quest_completed := false

func _ready() -> void:
    quest_label.text = "Talk to the guide by the campfire"
    interaction_label.text = "Press [E] to interact"
    interaction_label.visible = false
    npc.connect("interacted", Callable(self, "_on_npc_interacted"))
    if enemy:
        enemy.tree_exited.connect(_on_enemy_defeated)

func _process(_delta: float) -> void:
    if npc == null:
        return
    if not quest_started and npc.is_player_in_range:
        interaction_label.text = "Press [E] to interact"
        interaction_label.visible = true
    elif quest_started and not quest_completed:
        interaction_label.text = "Left click to attack"
        interaction_label.visible = true
    elif quest_completed and npc.is_player_in_range:
        interaction_label.text = "Press [E] to turn in"
        interaction_label.visible = true
    else:
        interaction_label.visible = false

func _on_npc_interacted() -> void:
    if quest_completed:
        quest_label.text = "Quest complete!"
        interaction_label.visible = false
        return
    quest_started = true
    quest_label.text = "Defeat the roaming spirit"
    interaction_label.text = "Left click to attack"

func _on_enemy_defeated() -> void:
    if not quest_started:
        return
    quest_completed = true
    quest_label.text = "Return to the guide"
    interaction_label.text = "Press [E] to turn in"
    npc.set_ready_for_turn_in()
