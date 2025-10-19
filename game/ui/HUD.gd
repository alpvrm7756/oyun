extends CanvasLayer

@onready var health_bar:TextureProgressBar = $Root/HealthBar
@onready var mana_bar:TextureProgressBar = $Root/ManaBar
@onready var quest_list:RichTextLabel = $Root/QuestPanel/QuestList
@onready var dialogue_box:RichTextLabel = $Root/Dialogue
@onready var prompt_label:Label = $Root/Prompt

var dialogue_history:Array[String] = []
const MAX_DIALOGUE_LINES:int = 5

func _ready() -> void:
    DialogueBus.line_spoken.connect(_on_dialogue_spoken)
    QuestLog.quest_started.connect(_on_quest_started)
    QuestLog.quest_updated.connect(_on_quest_updated)
    QuestLog.quest_completed.connect(_on_quest_completed)

func connect_player(player:Node) -> void:
    if player.has_node("Stats"):
        var stats:StatsComponent = player.get_node("Stats")
        stats.health_changed.connect(_on_health_changed)
        stats.mana_changed.connect(_on_mana_changed)
    if player.has_signal("interaction_prompt"):
        player.interaction_prompt.connect(_on_interaction_prompt)

func _on_health_changed(current:int, maximum:int) -> void:
    health_bar.max_value = maximum
    health_bar.value = current

func _on_mana_changed(current:int, maximum:int) -> void:
    mana_bar.max_value = maximum
    mana_bar.value = current

func _on_dialogue_spoken(speaker:String, text:String) -> void:
    dialogue_history.append("[b]%s:[/b] %s" % [speaker, text])
    if dialogue_history.size() > MAX_DIALOGUE_LINES:
        dialogue_history = dialogue_history.slice(dialogue_history.size() - MAX_DIALOGUE_LINES, MAX_DIALOGUE_LINES)
    dialogue_box.text = "\n".join(dialogue_history)

func _on_quest_started(id:String) -> void:
    _refresh_quest_log()

func _on_quest_updated(id:String, progress:int, required:int) -> void:
    _refresh_quest_log()

func _on_quest_completed(id:String) -> void:
    _refresh_quest_log()
    _on_dialogue_spoken("System", "%s completed!" % id.capitalize())

func _refresh_quest_log() -> void:
    var lines:Array[String] = []
    for quest_id in QuestLog.active_quests:
        var quest:QuestLog.Quest = QuestLog.quests[quest_id]
        lines.append("[b]%s[/b]: %d/%d" % [quest.title, quest.progress, quest.required])
    if QuestLog.completed_quests:
        lines.append("\nCompleted:")
        for quest_id in QuestLog.completed_quests:
            var quest:QuestLog.Quest = QuestLog.quests[quest_id]
            lines.append("- %s" % quest.title)
    quest_list.text = "\n".join(lines)

func _on_interaction_prompt(text:String) -> void:
    prompt_label.text = text
