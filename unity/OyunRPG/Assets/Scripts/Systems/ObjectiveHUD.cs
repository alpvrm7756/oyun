using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OyunRPG.Systems
{
    public class ObjectiveHUD : MonoBehaviour
    {
        [SerializeField] private Text objectiveTitle;
        [SerializeField] private Text objectiveDetails;
        [SerializeField] private Text completionBanner;

        private void Awake()
        {
            if (objectiveTitle == null || objectiveDetails == null || completionBanner == null)
            {
                BuildUI();
            }
        }

        private void BuildUI()
        {
            Canvas canvas = new GameObject("HUDCanvas").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;
            canvas.sortingOrder = 0;
            canvas.gameObject.AddComponent<CanvasScaler>();
            canvas.gameObject.AddComponent<GraphicRaycaster>();

            RectTransform root = canvas.GetComponent<RectTransform>();
            root.anchorMin = Vector2.zero;
            root.anchorMax = Vector2.one;
            root.offsetMin = Vector2.zero;
            root.offsetMax = Vector2.zero;

            GameObject panel = new GameObject("ObjectivePanel", typeof(RectTransform));
            panel.transform.SetParent(canvas.transform, false);
            RectTransform panelRect = panel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.02f, 0.75f);
            panelRect.anchorMax = new Vector2(0.35f, 0.98f);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;

            objectiveTitle = CreateText(panel.transform, "ObjectiveTitle", 20, FontStyle.Bold, new Vector2(0f, 0.65f), new Vector2(1f, 1f));
            objectiveDetails = CreateText(panel.transform, "ObjectiveDetails", 16, FontStyle.Normal, new Vector2(0f, 0f), new Vector2(1f, 0.6f));
            completionBanner = CreateText(panel.transform, "CompletionBanner", 18, FontStyle.Bold, new Vector2(0f, 0.6f), new Vector2(1f, 0.75f));
            completionBanner.alignment = TextAnchor.MiddleCenter;
            completionBanner.color = Color.yellow;
        }

        private Text CreateText(Transform parent, string name, int fontSize, FontStyle style, Vector2 anchorMin, Vector2 anchorMax)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = new Vector2(8f, 8f);
            rect.offsetMax = new Vector2(-8f, -8f);

            Text text = go.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = fontSize;
            text.fontStyle = style;
            text.color = Color.white;
            text.alignment = TextAnchor.UpperLeft;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        public void Render(List<QuestManager.QuestObjective> objectives, bool questComplete)
        {
            if (objectiveTitle == null || objectiveDetails == null || completionBanner == null)
            {
                return;
            }

            if (objectives == null || objectives.Count == 0)
            {
                objectiveTitle.text = "Objectives";
                objectiveDetails.text = string.Empty;
                completionBanner.text = string.Empty;
                return;
            }

            objectiveTitle.text = "Camp Tasks";
            completionBanner.text = questComplete ? "Quest Complete" : string.Empty;

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < objectives.Count; i++)
            {
                QuestManager.QuestObjective objective = objectives[i];
                builder.Append(objective.isComplete ? "<color=lime>✔</color>" : "<color=white>•</color>");
                builder.Append(' ');
                builder.Append(objective.description);
                if (i < objectives.Count - 1)
                {
                    builder.Append('\n');
                }
            }

            objectiveDetails.text = builder.ToString();
        }
    }
}
