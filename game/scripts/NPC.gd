extends StaticBody3D

@export var npc_name:String = "Guide"
@export var dialogue:Array[String] = [
    "Welcome to the valley, traveler.",
    "Our world is under threat from the crystalline corruption.",
    "Could you help us cleanse the obelisks scattered around?"
]
@export var quest_id:String = "cleanse_obelisks"

var current_line:int = 0
var has_given_quest:bool = false

func get_interaction_text() -> String:
    return "Talk to %s" % npc_name

func on_interact(player:Node) -> void:
    if current_line < dialogue.size():
        DialogueBus.emit_dialogue(npc_name, dialogue[current_line])
        current_line += 1
    elif not has_given_quest:
        has_given_quest = true
        DialogueBus.emit_dialogue(npc_name, "Please cleanse three obelisks and return to me.")
        QuestLog.start_quest(quest_id)
    else:
        if QuestLog.is_quest_completed(quest_id):
            DialogueBus.emit_dialogue(npc_name, "You did it! Take this elixir as a token of gratitude.")
            QuestLog.grant_reward(quest_id, player)
        else:
            DialogueBus.emit_dialogue(npc_name, "Keep going, the valley still needs you.")
