extends StaticBody3D

signal cleansed(obelisk:Node)

var cleansed_state:bool = false

func get_interaction_text() -> String:
    return cleansed_state ? "Obelisk cleansed" : "Cleanse Obelisk"

func on_interact(player:Node) -> void:
    if cleansed_state:
        return
    if player.has_node("Stats"):
        var stats:StatsComponent = player.get_node("Stats")
        if stats.consume_mana(10):
            _cleanse()
            DialogueBus.emit_dialogue("Obelisk", "The corruption fades...")
        else:
            DialogueBus.emit_dialogue("Obelisk", "You lack the mana to purify me.")

func _cleanse() -> void:
    cleansed_state = true
    $Glow.light_color = Color(0.4, 0.9, 0.5, 1)
    $Glow.light_energy = 4.0
    emit_signal("cleansed", self)
