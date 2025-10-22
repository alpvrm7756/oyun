using OyunRPG.Gameplay;
using UnityEngine;

namespace OyunRPG.Systems
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private DialogueChannel dialogueChannel;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (playerController != null)
            {
                var enemy = FindObjectOfType<EnemyController>();
                if (enemy != null)
                {
                    enemy.SetTarget(playerController.transform);
                }
            }

            if (dialogueChannel != null)
            {
                dialogueChannel.OnDialogueRequested.AddListener(HandleDialogueRequested);
            }
        }

        private void HandleDialogueRequested(System.Collections.Generic.List<string> _)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnDestroy()
        {
            if (dialogueChannel != null)
            {
                dialogueChannel.OnDialogueRequested.RemoveListener(HandleDialogueRequested);
            }
        }
    }
}
