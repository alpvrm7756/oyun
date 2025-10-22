using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OyunRPG.Systems
{
    [CreateAssetMenu(fileName = "DialogueChannel", menuName = "OyunRPG/Dialogue Channel")]
    public class DialogueChannel : ScriptableObject
    {
        [System.Serializable]
        public class DialogueEvent : UnityEvent<List<string>> { }

        public DialogueEvent OnDialogueRequested = new DialogueEvent();

        public void BeginDialogue(List<string> lines)
        {
            if (lines == null || lines.Count == 0)
            {
                return;
            }

            OnDialogueRequested.Invoke(lines);
        }
    }
}
