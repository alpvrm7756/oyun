using System.Collections.Generic;
using OyunRPG.Gameplay;
using UnityEngine;
using UnityEngine.Events;

namespace OyunRPG.Systems
{
    public class QuestManager : MonoBehaviour
    {
        [System.Serializable]
        public class QuestObjective
        {
            public string description;
            public bool isComplete;
        }

        [SerializeField] private PlayerCombat playerCombat;
        [SerializeField] private EnemyController questEnemy;
        [SerializeField] private DialogueChannel dialogueChannel;
        [SerializeField] private ObjectiveHUD objectiveHUD;
        [SerializeField] private List<QuestObjective> objectives = new List<QuestObjective>();

        public UnityEvent OnQuestCompleted;
        public bool IsQuestComplete { get; private set; }

        private void Awake()
        {
            if (objectives.Count == 0)
            {
                objectives.Add(new QuestObjective { description = "Speak with the camp guide" });
                objectives.Add(new QuestObjective { description = "Defeat the spirit haunting the grove" });
                objectives.Add(new QuestObjective { description = "Return to the camp guide" });
            }

            if (objectiveHUD == null)
            {
                objectiveHUD = GetComponent<ObjectiveHUD>();
            }
        }

        private void Start()
        {
            UpdateHUD();

            if (questEnemy != null)
            {
                questEnemy.OnHealthChanged.AddListener(HandleEnemyHealthChanged);
                questEnemy.SetTarget(playerCombat != null ? playerCombat.transform : null);
            }

            if (dialogueChannel != null)
            {
                dialogueChannel.OnDialogueRequested.AddListener(HandleDialogueOpened);
            }
        }

        private void OnDestroy()
        {
            if (questEnemy != null)
            {
                questEnemy.OnHealthChanged.RemoveListener(HandleEnemyHealthChanged);
            }

            if (dialogueChannel != null)
            {
                dialogueChannel.OnDialogueRequested.RemoveListener(HandleDialogueOpened);
            }
        }

        public void HandleNPCInteraction()
        {
            if (IsQuestComplete)
            {
                return;
            }

            if (!objectives[0].isComplete)
            {
                objectives[0].isComplete = true;
                UpdateHUD();
            }
            else if (objectives[1].isComplete)
            {
                objectives[2].isComplete = true;
                CompleteQuest();
            }
        }

        private void HandleEnemyHealthChanged(float current, float _)
        {
            if (current <= 0f && !objectives[1].isComplete)
            {
                objectives[1].isComplete = true;
                UpdateHUD();
            }
        }

        private void HandleDialogueOpened(List<string> _)
        {
            // Reserved for future cinematics or UI pausing.
        }

        private void CompleteQuest()
        {
            if (IsQuestComplete)
            {
                return;
            }

            IsQuestComplete = true;
            UpdateHUD();
            OnQuestCompleted?.Invoke();
        }

        private void UpdateHUD()
        {
            if (objectiveHUD == null)
            {
                return;
            }

            objectiveHUD.Render(objectives, IsQuestComplete);
        }
    }
}
