extends Node

class Quest extends RefCounted:
    var id:String
    var title:String
    var description:String
    var required:int
    var progress:int
    var reward:Dictionary

var quests:Dictionary = {}
var active_quests:Array[String] = []
var completed_quests:Array[String] = []

signal quest_started(id:String)
signal quest_updated(id:String, progress:int, required:int)
signal quest_completed(id:String)

func register_quest(id:String, title:String, description:String, required:int, reward:Dictionary) -> void:
    var quest := Quest.new()
    quest.id = id
    quest.title = title
    quest.description = description
    quest.required = required
    quest.progress = 0
    quest.reward = reward
    quests[id] = quest

func start_quest(id:String) -> void:
    if not quests.has(id):
        push_warning("Quest %s not registered" % id)
        return
    if id in active_quests or id in completed_quests:
        return
    active_quests.append(id)
    emit_signal("quest_started", id)

func increment_progress(id:String, amount:int = 1) -> void:
    if not quests.has(id):
        return
    var quest:Quest = quests[id]
    if id not in active_quests:
        return
    quest.progress = clamp(quest.progress + amount, 0, quest.required)
    emit_signal("quest_updated", id, quest.progress, quest.required)
    if quest.progress >= quest.required:
        _complete_quest(id)

func _complete_quest(id:String) -> void:
    if id in completed_quests:
        return
    completed_quests.append(id)
    active_quests.erase(id)
    emit_signal("quest_completed", id)

func is_quest_completed(id:String) -> bool:
    return id in completed_quests

func grant_reward(id:String, player:Node) -> void:
    if not quests.has(id):
        return
    var quest:Quest = quests[id]
    if quest.reward.has("experience") and player.has_node("Stats"):
        player.get_node("Stats").gain_experience(quest.reward["experience"])
    if quest.reward.has("items"):
        for item in quest.reward["items"]:
            DialogueBus.emit_dialogue("System", "Received %s" % item)
