using System.Collections.Generic;
using OyunRPG.Systems;
using UnityEngine;

namespace OyunRPG.Gameplay
{
    public class NPCController : MonoBehaviour
    {
        [TextArea(2, 6)]
        [SerializeField] private List<string> preQuestDialogue = new List<string>();
        [TextArea(2, 6)]
        [SerializeField] private List<string> postQuestDialogue = new List<string>();
        [SerializeField] private float interactionRadius = 2f;
        [SerializeField] private DialogueChannel dialogueChannel;
        [SerializeField] private QuestManager questManager;

        private Transform player;

        private void Start()
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        private void Update()
        {
            if (player == null)
            {
                return;
            }

            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= interactionRadius && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }

        private void Interact()
        {
            if (questManager == null || dialogueChannel == null)
            {
                return;
            }

            var lines = questManager.IsQuestComplete ? postQuestDialogue : preQuestDialogue;
            dialogueChannel.BeginDialogue(lines);
            questManager.HandleNPCInteraction();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
#endif
    }
}
