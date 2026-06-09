using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// HUD and end-screen. Uses TextMeshProUGUI (included in URP Universal 3D template).
public class UIManager : MonoBehaviour
{
    private Canvas            _canvas;
    private TextMeshProUGUI   _timerText;
    private TextMeshProUGUI   _tierText;
    private TextMeshProUGUI[] _scoreTexts;
    private GameObject        _endScreen;
    private TextMeshProUGUI   _endTitle;
    private TextMeshProUGUI   _endBody;

    public void Init()
    {
        BuildCanvas();
        BuildHUD();
        BuildEndScreen();
    }

    public void Tick()
    {
        if (_timerText == null) return;
        var gm = GameManager.Instance;

        int secs = Mathf.CeilToInt(gm.TimeRemaining);
        _timerText.text  = FormatTime(secs);
        _timerText.color = secs <= 10 ? new Color(1f, 0.25f, 0.25f) : Color.white;

        if (gm.Player != null)
            _tierText.text = GameManager.TierLabel(gm.Player.Hole.Radius);

        for (int i = 0; i < gm.AllHoles.Count && i < _scoreTexts.Length; i++)
        {
            var h = gm.AllHoles[i];
            _scoreTexts[i].text  = h.HoleName + "\n" + Mathf.FloorToInt(h.Score).ToString("N0");
            _scoreTexts[i].color = h.Alive ? h.HoleColor : new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }

    public void ShowEndScreen()
    {
        _endScreen.SetActive(true);

        var sorted = new List<HoleBase>(GameManager.Instance.AllHoles);
        sorted.Sort((a, b) => b.Score.CompareTo(a.Score));

        string body = "";
        for (int i = 0; i < sorted.Count; i++)
        {
            string medal = i == 0 ? "★ " : (i + 1) + ". ";
            body += medal + sorted[i].HoleName
                         + "  " + Mathf.FloorToInt(sorted[i].Score).ToString("N0") + "\n";
        }
        _endBody.text = body.TrimEnd();

        bool playerWon   = sorted[0] == GameManager.Instance.Player.Hole;
        _endTitle.text   = playerWon ? "VICTORY" : "GAME OVER";
        _endTitle.color  = playerWon ? new Color(1f, 0.85f, 0.1f) : Color.white;
    }

    // ── Canvas ────────────────────────────────────────────────────────────────

    void BuildCanvas()
    {
        var go = new GameObject("HUD Canvas");
        _canvas = go.AddComponent<Canvas>();
        _canvas.renderMode    = RenderMode.ScreenSpaceOverlay;
        _canvas.sortingOrder  = 10;

        var cs = go.AddComponent<CanvasScaler>();
        cs.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920f, 1080f);
        cs.matchWidthOrHeight  = 0.5f;

        go.AddComponent<GraphicRaycaster>();
    }

    // ── HUD ───────────────────────────────────────────────────────────────────

    void BuildHUD()
    {
        _timerText = MakeTMP("Timer", "2:00",
            new Vector2(0.5f, 1f), new Vector2(0f, -40f), new Vector2(300f, 60f),
            48, TextAlignmentOptions.Center);

        _tierText = MakeTMP("Tier", "Pothole",
            new Vector2(0f, 1f), new Vector2(140f, -120f), new Vector2(240f, 36f),
            22, TextAlignmentOptions.MidlineLeft);
        _tierText.color = new Color(0.9f, 0.85f, 0.45f);

        int count = 4;
        _scoreTexts = new TextMeshProUGUI[count];
        for (int i = 0; i < count; i++)
        {
            float xPivot  = i == 0 ? 0f : 1f;
            float xOffset = i == 0 ? 20f : -20f;
            float yOff    = -20f - (i > 0 ? (i - 1) * 80f : 0f);
            var   align   = i == 0 ? TextAlignmentOptions.TopLeft : TextAlignmentOptions.TopRight;

            _scoreTexts[i] = MakeTMP("Score" + i, "",
                new Vector2(xPivot, 1f),
                new Vector2(xOffset, yOff),
                new Vector2(200f, 70f), 20, align);
        }
    }

    // ── End screen ────────────────────────────────────────────────────────────

    void BuildEndScreen()
    {
        _endScreen = new GameObject("EndScreen");
        _endScreen.transform.SetParent(_canvas.transform, false);

        var overlay = _endScreen.AddComponent<Image>();
        overlay.color = new Color(0f, 0f, 0f, 0.78f);
        var rt = _endScreen.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        _endTitle = MakeTMPChild(_endScreen, "Title", "GAME OVER",
            new Vector2(0.5f, 0.65f), new Vector2(0f, 0f), new Vector2(700f, 90f),
            64, TextAlignmentOptions.Center);
        _endTitle.fontStyle = FontStyles.Bold;

        _endBody = MakeTMPChild(_endScreen, "Scores", "",
            new Vector2(0.5f, 0.45f), new Vector2(0f, 0f), new Vector2(500f, 200f),
            30, TextAlignmentOptions.Top);

        // Restart button
        var btnGO = new GameObject("RestartBtn");
        btnGO.transform.SetParent(_endScreen.transform, false);
        var btnImg = btnGO.AddComponent<Image>();
        btnImg.color = new Color(0.20f, 0.60f, 1f, 0.92f);
        var btnRT = btnGO.GetComponent<RectTransform>();
        btnRT.anchorMin        = new Vector2(0.5f, 0.25f);
        btnRT.anchorMax        = new Vector2(0.5f, 0.25f);
        btnRT.sizeDelta        = new Vector2(260f, 64f);
        btnRT.anchoredPosition = Vector2.zero;
        btnGO.AddComponent<Button>().onClick.AddListener(() => GameManager.Instance.RestartGame());

        MakeTMPChild(btnGO, "BtnLabel", "PLAY AGAIN",
            new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(240f, 54f),
            26, TextAlignmentOptions.Center);

        _endScreen.SetActive(false);
    }

    // ── TMP factory helpers ───────────────────────────────────────────────────

    TextMeshProUGUI MakeTMP(string name, string text, Vector2 anchor, Vector2 pos,
                            Vector2 size, int fontSize, TextAlignmentOptions alignment)
        => MakeTMPChild(_canvas.gameObject, name, text, anchor, pos, size, fontSize, alignment);

    TextMeshProUGUI MakeTMPChild(GameObject parent, string name, string text,
                                 Vector2 anchor, Vector2 pos, Vector2 size,
                                 int fontSize, TextAlignmentOptions alignment)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin        = rt.anchorMax = anchor;
        rt.pivot            = anchor;
        rt.anchoredPosition = pos;
        rt.sizeDelta        = size;

        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text      = text;
        tmp.fontSize  = fontSize;
        tmp.color     = Color.white;
        tmp.alignment = alignment;

        // Subtle outline for readability over any background
        tmp.outlineWidth = 0.15f;
        tmp.outlineColor = new Color32(0, 0, 0, 180);

        return tmp;
    }

    // ── Utility ───────────────────────────────────────────────────────────────

    static string FormatTime(int secs)
    {
        int m = secs / 60, s = secs % 60;
        return $"{m}:{s:D2}";
    }
}
