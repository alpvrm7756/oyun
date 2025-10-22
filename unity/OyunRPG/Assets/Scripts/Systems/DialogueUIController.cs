using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OyunRPG.Systems
{
    public class DialogueUIController : MonoBehaviour
    {
        [SerializeField] private DialogueChannel dialogueChannel;
        [SerializeField] private Text speakerName;
        [SerializeField] private Text dialogueLine;
        [SerializeField] private GameObject dialogueRoot;
        [SerializeField] private float textRevealSpeed = 35f;

        private Coroutine dialogueRoutine;

        private void Awake()
        {
            if (dialogueRoot == null || speakerName == null || dialogueLine == null)
            {
                BuildUI();
            }
        }

        private void OnEnable()
        {
            if (dialogueChannel != null)
            {
                dialogueChannel.OnDialogueRequested.AddListener(BeginDialogue);
            }
        }

        private void OnDisable()
        {
            if (dialogueChannel != null)
            {
                dialogueChannel.OnDialogueRequested.RemoveListener(BeginDialogue);
            }
        }

        private void BuildUI()
        {
            Canvas canvas = new GameObject("DialogueCanvas").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 5;
            canvas.gameObject.AddComponent<CanvasScaler>();
            canvas.gameObject.AddComponent<GraphicRaycaster>();

            dialogueRoot = new GameObject("DialogueRoot", typeof(RectTransform));
            dialogueRoot.transform.SetParent(canvas.transform, false);
            RectTransform rect = dialogueRoot.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.15f, 0.05f);
            rect.anchorMax = new Vector2(0.85f, 0.3f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image background = dialogueRoot.AddComponent<Image>();
            background.color = new Color(0f, 0f, 0f, 0.6f);

            speakerName = CreateText(dialogueRoot.transform, "Speaker", 20, FontStyle.Bold, new Vector2(0f, 0.65f), new Vector2(1f, 1f));
            speakerName.alignment = TextAnchor.MiddleLeft;

            dialogueLine = CreateText(dialogueRoot.transform, "Line", 18, FontStyle.Normal, new Vector2(0f, 0f), new Vector2(1f, 0.6f));
            dialogueLine.alignment = TextAnchor.UpperLeft;

            dialogueRoot.SetActive(false);
        }

        private Text CreateText(Transform parent, string name, int fontSize, FontStyle style, Vector2 anchorMin, Vector2 anchorMax)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = new Vector2(16f, 8f);
            rect.offsetMax = new Vector2(-16f, -8f);

            Text text = go.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = fontSize;
            text.fontStyle = style;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        private void BeginDialogue(List<string> lines)
        {
            if (dialogueRoutine != null)
            {
                StopCoroutine(dialogueRoutine);
            }

            dialogueRoutine = StartCoroutine(RunDialogue(lines));
        }

        private IEnumerator RunDialogue(List<string> lines)
        {
            dialogueRoot.SetActive(true);
            speakerName.text = "Camp Guide";
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            for (int i = 0; i < lines.Count; i++)
            {
                yield return RevealLine(lines[i]);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E));
                yield return null;
            }

            dialogueRoot.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private IEnumerator RevealLine(string line)
        {
            dialogueLine.text = string.Empty;
            float elapsed = 0f;
            while (elapsed < line.Length)
            {
                elapsed += textRevealSpeed * Time.deltaTime;
                int count = Mathf.Clamp(Mathf.FloorToInt(elapsed), 0, line.Length);
                dialogueLine.text = line.Substring(0, count);
                yield return null;
            }

            dialogueLine.text = line;
        }
    }
}
