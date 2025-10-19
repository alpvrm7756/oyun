extends Node

class_name StatsComponent

@export var level:int = 1
@export var max_health:int = 100
@export var max_mana:int = 40
@export var strength:int = 10
@export var agility:int = 8
@export var intelligence:int = 6
@export var experience:int = 0

var health:int
var mana:int

signal health_changed(current:int, maximum:int)
signal mana_changed(current:int, maximum:int)
signal leveled_up(new_level:int)
signal died(owner:Node)

func _ready() -> void:
    health = max_health
    mana = max_mana
    emit_signal("health_changed", health, max_health)
    emit_signal("mana_changed", mana, max_mana)

func apply_damage(amount:int) -> void:
    health = clamp(health - amount, 0, max_health)
    emit_signal("health_changed", health, max_health)
    if health == 0:
        emit_signal("died", get_parent())

func heal(amount:int) -> void:
    health = clamp(health + amount, 0, max_health)
    emit_signal("health_changed", health, max_health)

func consume_mana(amount:int) -> bool:
    if mana < amount:
        return false
    mana -= amount
    emit_signal("mana_changed", mana, max_mana)
    return true

func restore_mana(amount:int) -> void:
    mana = clamp(mana + amount, 0, max_mana)
    emit_signal("mana_changed", mana, max_mana)

func gain_experience(amount:int) -> void:
    experience += amount
    var required:int = level * 100
    if experience >= required:
        experience -= required
        level += 1
        max_health += 10
        max_mana += 5
        strength += 2
        agility += 1
        intelligence += 1
        health = max_health
        mana = max_mana
        emit_signal("leveled_up", level)
        emit_signal("health_changed", health, max_health)
        emit_signal("mana_changed", mana, max_mana)
