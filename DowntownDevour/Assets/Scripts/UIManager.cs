using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Builds and drives the entire HUD: timer, scores, tier label, end screen.
public class UIManager : MonoBehaviour
{
    private Canvas _canvas;
    private Text   _timerText;
    private Text   _tierText;
    private Text[] _scoreTexts;   // one per hole (player first)
    private GameObject _endScreen;
    private Text       _endTitle;
    private Text       _endBody;

    private static Font _font;

    public void Init()
    {
        _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        BuildCanvas();
        BuildHUD();
        BuildEndScreen();
    }

    public void Tick()
    {
        var gm = GameManager.Instance;

        // Timer
        int secs = Mathf.CeilToInt(gm.TimeRemaining);
        _timerText.text  = FormatTime(secs);
        _timerText.color = secs <= 10 ? Color.red : Color.white;

        // Tier label for player
        if (gm.Player != null)
            _tierText.text = GameManager.TierLabel(gm.Player.Hole.Radius);

        // Per-hole scores
        for (int i = 0; i < gm.AllHoles.Count && i < _scoreTexts.Length; i++)
        {
            var h = gm.AllHoles[i];
            _scoreTexts[i].text = h.HoleName + "\n"
                + Mathf.FloorToInt(h.Score).ToString("N0");
            _scoreTexts[i].color = h.Alive ? h.HoleColor : new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }

    public void ShowEndScreen()
    {
        _endScreen.SetActive(true);

        // Sort holes by score
        var sorted = new List<HoleBase>(GameManager.Instance.AllHoles);
        sorted.Sort((a, b) => b.Score.CompareTo(a.Score));

        string body = "";
        for (int i = 0; i < sorted.Count; i++)
        {
            string medal = i == 0 ? "★ " : i == 1 ? "2. " : i == 2 ? "3. " : "4. ";
            body += medal + sorted[i].HoleName
                         + " – " + Mathf.FloorToInt(sorted[i].Score).ToString("N0") + "\n";
        }
        _endBody.text = body.TrimEnd();

        bool playerWon = sorted[0] == GameManager.Instance.Player.Hole;
        _endTitle.text  = playerWon ? "VICTORY" : "GAME OVER";
        _endTitle.color = playerWon ? new Color(1f, 0.85f, 0.1f) : Color.white;
    }

    // ── Canvas building ───────────────────────────────────────────────────────

    void BuildCanvas()
    {
        var go = new GameObject("HUD Canvas");
        _canvas = go.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.sortingOrder = 10;

        var cs = go.AddComponent<CanvasScaler>();
        cs.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920f, 1080f);
        cs.matchWidthOrHeight  = 0.5f;

        go.AddComponent<GraphicRaycaster>();
    }

    void BuildHUD()
    {
        // Timer (top-centre)
        _timerText = MakeText("Timer", "2:00",
            new Vector2(0.5f, 1f), new Vector2(0f, -40f), new Vector2(300f, 60f), 42, TextAnchor.MiddleCenter);

        // Tier label (top-left, below player score)
        _tierText = MakeText("Tier", "Pothole",
            new Vector2(0f, 1f), new Vector2(140f, -120f), new Vector2(240f, 36f), 22, TextAnchor.MiddleLeft);
        _tierText.color = new Color(0.8f, 0.8f, 0.5f);

        // Score panel per hole (top-left and top-right)
        int count = 4;  // 1 player + 3 AI
        _scoreTexts = new Text[count];
        for (int i = 0; i < count; i++)
        {
            float xPivot  = i == 0 ? 0f : 1f;
            float xOffset = i == 0 ? 20f : -20f;
            float yOff    = -20f - (i > 0 ? (i - 1) * 80f : 0f);
            TextAnchor anchor = i == 0 ? TextAnchor.UpperLeft : TextAnchor.UpperRight;

            _scoreTexts[i] = MakeText("Score" + i, "",
                new Vector2(xPivot, 1f),
                new Vector2(xOffset, yOff),
                new Vector2(200f, 70f), 20, anchor);
        }
    }

    void BuildEndScreen()
    {
        // Dark overlay
        _endScreen = new GameObject("EndScreen");
        _endScreen.transform.SetParent(_canvas.transform, false);
        var overlay = _endScreen.AddComponent<Image>();
        overlay.color = new Color(0f, 0f, 0f, 0.75f);
        var rt = _endScreen.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        // Title
        _endTitle = MakeTextChild(_endScreen, "Title", "GAME OVER",
            new Vector2(0.5f, 0.65f), new Vector2(0f, 0f), new Vector2(600f, 80f), 60,
            TextAnchor.MiddleCenter);
        _endTitle.fontStyle = FontStyle.Bold;

        // Leaderboard
        _endBody = MakeTextChild(_endScreen, "Scores", "",
            new Vector2(0.5f, 0.45f), new Vector2(0f, 0f), new Vector2(500f, 200f), 28,
            TextAnchor.UpperCenter);

        // Restart button
        var btnGO  = new GameObject("RestartBtn");
        btnGO.transform.SetParent(_endScreen.transform, false);
        var btnImg = btnGO.AddComponent<Image>();
        btnImg.color = new Color(0.2f, 0.6f, 1f, 0.9f);
        var btnRT = btnGO.GetComponent<RectTransform>();
        btnRT.anchorMin = new Vector2(0.5f, 0.25f);
        btnRT.anchorMax = new Vector2(0.5f, 0.25f);
        btnRT.sizeDelta = new Vector2(240f, 60f);
        btnRT.anchoredPosition = Vector2.zero;
        var btn = btnGO.AddComponent<Button>();
        btn.onClick.AddListener(() => GameManager.Instance.RestartGame());

        MakeTextChild(btnGO, "BtnLabel", "PLAY AGAIN",
            new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(220f, 50f), 24,
            TextAnchor.MiddleCenter);

        _endScreen.SetActive(false);
    }

    // ── Text factory helpers ──────────────────────────────────────────────────

    Text MakeText(string name, string text, Vector2 anchor, Vector2 pos, Vector2 size,
                  int fontSize, TextAnchor alignment)
    {
        return MakeTextChild(_canvas.gameObject, name, text, anchor, pos, size, fontSize, alignment);
    }

    Text MakeTextChild(GameObject parent, string name, string text, Vector2 anchor, Vector2 pos,
                       Vector2 size, int fontSize, TextAnchor alignment)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = anchor;
        rt.pivot            = anchor;
        rt.anchoredPosition = pos;
        rt.sizeDelta        = size;

        var t = go.AddComponent<Text>();
        t.text      = text;
        t.font      = _font;
        t.fontSize  = fontSize;
        t.color     = Color.white;
        t.alignment = alignment;

        // Drop shadow for readability
        var shadow = go.AddComponent<Shadow>();
        shadow.effectColor    = new Color(0f, 0f, 0f, 0.7f);
        shadow.effectDistance = new Vector2(2f, -2f);

        return t;
    }

    // ── Utilities ─────────────────────────────────────────────────────────────

    static string FormatTime(int secs)
    {
        int m = secs / 60, s = secs % 60;
        return $"{m}:{s:D2}";
    }
}
