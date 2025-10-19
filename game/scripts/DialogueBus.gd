extends Node

signal line_spoken(speaker:String, text:String)

func emit_dialogue(speaker:String, text:String) -> void:
    emit_signal("line_spoken", speaker, text)
