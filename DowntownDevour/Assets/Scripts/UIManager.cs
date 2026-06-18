using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// HUD and end-screen. Uses TextMeshProUGUI (included in URP Universal 3D template).
public class UIManager : MonoBehaviour
{
    public const string VERSION = "v0.47";

    private Canvas            _canvas;
    private GameObject        _hudRoot;     // all in-game HUD widgets; hidden on title/menu
    private TextMeshProUGUI   _timerText;
    private TextMeshProUGUI   _inHoleText;  // tier name + growth %, shown inside the hole
    private TextMeshProUGUI[] _scoreTexts;
    private GameObject        _startScreen;
    private GameObject        _endScreen;
    private TextMeshProUGUI   _endTitle;
    private TextMeshProUGUI   _endSubtitle;
    private TextMeshProUGUI   _endBody;
    private GameObject        _pauseScreen;
    private TextMeshProUGUI   _todLabel;   // updated each frame to show current TOD

    // Title-screen mode picker. Only "Timed" is implemented; the others are
    // placeholders (selectable, but the round always runs the timed game for now).
    private Image[] _modeBg;
    private int     _selectedMode;
    static readonly Color MODE_SEL   = new Color(0.20f, 0.55f, 0.95f, 0.90f);
    static readonly Color MODE_UNSEL = new Color(0.16f, 0.16f, 0.22f, 0.80f);

    public void Init()
    {
        BuildCanvas();
        BuildHUD();
        BuildStartScreen();
        BuildEndScreen();
        BuildPauseScreen();
        ShowStartScreen();   // title screen first; gameplay starts on PLAY
    }

    public void Tick()
    {
        if (_timerText == null) return;
        var gm = GameManager.Instance;

        int secs = Mathf.CeilToInt(gm.TimeRemaining);
        _timerText.text  = FormatTime(secs);
        _timerText.color = secs <= 10 ? new Color(1f, 0.25f, 0.25f) : Color.white;

        if (_todLabel != null)
            _todLabel.text = gm.CurrentTimeOfDay switch {
                GameManager.TimeOfDay.Morning   => "MORNING",
                GameManager.TimeOfDay.Afternoon => "AFTERNOON",
                GameManager.TimeOfDay.Evening   => "EVENING",
                GameManager.TimeOfDay.Night     => "NIGHT",
                _                               => ""
            };

        if (gm.Player != null)
        {
            float pct = gm.Player.Hole.Radius / GameManager.MIN_RADIUS * 100f;
            _inHoleText.text = GameManager.TierLabel(gm.Player.Hole.Radius)
                             + "\n" + Mathf.FloorToInt(pct).ToString("N0") + "%";
        }

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
        _hudRoot.SetActive(false);

        var gm     = GameManager.Instance;
        var player = gm.Player.Hole;
        var sorted = new List<HoleBase>(gm.AllHoles);
        sorted.Sort((a, b) => b.Score.CompareTo(a.Score));

        // Reason title + subtitle, mirroring the browser's endGame() framing.
        int aliveCount = 0;
        foreach (var h in gm.AllHoles) if (h.Alive) aliveCount++;

        string reason;
        if      (!player.Alive)                                   reason = "You got devoured.";
        else if (aliveCount == 1 && sorted[0] == player)          reason = "Last hole standing. Devour complete.";
        else if (gm.TimeRemaining <= 0f)                          reason = "Time's up!";
        else                                                      reason = "Round over.";

        string subtitle = player.Radius >= 6.0f
            ? "You grew massive. Absolute menace."
            : "Final leaderboard below.";

        _endTitle.text    = reason;
        _endTitle.color   = (sorted[0] == player) ? new Color(1f, 0.85f, 0.1f) : Color.white;
        _endSubtitle.text = subtitle;

        // Leaderboard — medals for top 3, (You) tag, (eaten) status.
        string body = "";
        for (int i = 0; i < sorted.Count; i++)
        {
            var h = sorted[i];
            string medal  = i == 0 ? "1st  " : i == 1 ? "2nd  " : i == 2 ? "3rd  " : (i + 1) + ".  ";
            string you    = h == player ? " (You)"   : "";
            string status = h.Alive     ? ""         : " (eaten)";
            body += medal + h.HoleName + you + status
                  + "    " + Mathf.FloorToInt(h.Score).ToString("N0") + "\n";
        }
        _endBody.text = body.TrimEnd();
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

        // EventSystem is required for all UI interaction (button clicks, hover).
        // Unity only creates one automatically when using the menu; we build ours in code.
        if (FindAnyObjectByType<EventSystem>() == null)
        {
            var evGO = new GameObject("EventSystem");
            evGO.AddComponent<EventSystem>();
            evGO.AddComponent<StandaloneInputModule>();
        }
    }

    // ── HUD ───────────────────────────────────────────────────────────────────

    void BuildHUD()
    {
        // Version stamp — bottom-left, always visible (stays on canvas, not _hudRoot)
        var ver = MakeTMP("Version", VERSION,
            new Vector2(0f, 0f), new Vector2(10f, 10f), new Vector2(120f, 28f),
            15, TextAlignmentOptions.BottomLeft);
        ver.color = new Color(1f, 1f, 1f, 0.40f);

        // All in-game HUD widgets live under _hudRoot so they can be hidden as a
        // group while the title / menu screen is showing.
        _hudRoot = new GameObject("HUD");
        _hudRoot.transform.SetParent(_canvas.transform, false);
        var hudRT = _hudRoot.AddComponent<RectTransform>();
        hudRT.anchorMin = Vector2.zero;
        hudRT.anchorMax = Vector2.one;
        hudRT.offsetMin = hudRT.offsetMax = Vector2.zero;

        _timerText = MakeTMPChild(_hudRoot, "Timer", "2:00",
            new Vector2(0.5f, 1f), new Vector2(0f, -40f), new Vector2(300f, 60f),
            48, TextAlignmentOptions.Center);

        // Tier name and growth % sit inside the hole at screen center.
        // The camera follows the player so the hole is always roughly centered.
        _inHoleText = MakeTMPChild(_hudRoot, "InHole", "Pothole\n100%",
            new Vector2(0.5f, 0.5f), new Vector2(0f, 18f), new Vector2(220f, 52f),
            14, TextAlignmentOptions.Center);
        _inHoleText.color = new Color(1f, 1f, 1f, 0.55f);

        // ── MENU button — top-right, left of AI score column ──────────────────
        // The score column sits at anchor(1,1) offset -20, width 200 → left edge at -220.
        // We sit immediately left of it with a 10px gap.
        var pauseBtnGO = new GameObject("PauseBtn");
        pauseBtnGO.transform.SetParent(_hudRoot.transform, false);
        var pauseImg = pauseBtnGO.AddComponent<Image>();
        pauseImg.color = new Color(0.20f, 0.20f, 0.28f, 0.85f);
        var pauseRT = pauseBtnGO.GetComponent<RectTransform>();
        pauseRT.anchorMin        = new Vector2(1f, 1f);
        pauseRT.anchorMax        = new Vector2(1f, 1f);
        pauseRT.pivot            = new Vector2(1f, 1f);
        pauseRT.anchoredPosition = new Vector2(-230f, -20f);
        pauseRT.sizeDelta        = new Vector2(120f, 44f);
        pauseBtnGO.AddComponent<Button>().onClick.AddListener(() => GameManager.Instance.TogglePause());
        MakeTMPChild(pauseBtnGO, "PauseLabel", "II  MENU",
            new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(110f, 40f),
            18, TextAlignmentOptions.Center);

        // ── Time-of-day cycle button — below MENU, same column ────────────────
        var todBtnGO = new GameObject("TODBtn");
        todBtnGO.transform.SetParent(_hudRoot.transform, false);
        var todImg = todBtnGO.AddComponent<Image>();
        todImg.color = new Color(0.18f, 0.28f, 0.22f, 0.85f);
        var todRT = todBtnGO.GetComponent<RectTransform>();
        todRT.anchorMin        = new Vector2(1f, 1f);
        todRT.anchorMax        = new Vector2(1f, 1f);
        todRT.pivot            = new Vector2(1f, 1f);
        todRT.anchoredPosition = new Vector2(-230f, -72f);
        todRT.sizeDelta        = new Vector2(120f, 44f);
        todBtnGO.AddComponent<Button>().onClick.AddListener(() => GameManager.Instance.CycleTimeOfDay());
        _todLabel = MakeTMPChild(todBtnGO, "TODLabel", "AFTERNOON",
            new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(114f, 40f),
            14, TextAlignmentOptions.Center);

        int count = 4;
        _scoreTexts = new TextMeshProUGUI[count];
        for (int i = 0; i < count; i++)
        {
            float xPivot  = i == 0 ? 0f : 1f;
            float xOffset = i == 0 ? 20f : -20f;
            float yOff    = -20f - (i > 0 ? (i - 1) * 80f : 0f);
            var   align   = i == 0 ? TextAlignmentOptions.TopLeft : TextAlignmentOptions.TopRight;

            _scoreTexts[i] = MakeTMPChild(_hudRoot, "Score" + i, "",
                new Vector2(xPivot, 1f),
                new Vector2(xOffset, yOff),
                new Vector2(200f, 70f), 20, align);
        }
    }

    // ── Start / title screen ────────────────────────────────────────────────────
    // Mirrors the browser version's title overlay: name, pitch, mode picker
    // (Timed / Last Man Standing / Waves), play button, and control tips.

    void BuildStartScreen()
    {
        _startScreen = new GameObject("StartScreen");
        _startScreen.transform.SetParent(_canvas.transform, false);

        var overlay = _startScreen.AddComponent<Image>();
        overlay.color = new Color(0.02f, 0.02f, 0.05f, 0.88f);
        var rt = _startScreen.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        var title = MakeTMPChild(_startScreen, "Title", "Downtown Devour",
            new Vector2(0.5f, 0.92f), Vector2.zero, new Vector2(900f, 90f),
            66, TextAlignmentOptions.Center);
        title.fontStyle = FontStyles.Bold;
        title.color     = new Color(1f, 0.85f, 0.30f);

        var sub = MakeTMPChild(_startScreen, "Subtitle",
            "You're a hungry hole competing against 3 rival holes. Eat everything. " +
            "Grow bigger. Devour your rivals. Last hole standing wins.",
            new Vector2(0.5f, 0.82f), Vector2.zero, new Vector2(760f, 70f),
            20, TextAlignmentOptions.Center);
        sub.color = new Color(0.85f, 0.88f, 0.95f);

        MakeTMPChild(_startScreen, "PickTitle", "Choose Game Mode",
            new Vector2(0.5f, 0.70f), Vector2.zero, new Vector2(500f, 40f),
            24, TextAlignmentOptions.Center);

        // Three mode options, stacked.
        string[] labels = { "Timed", "Last Man Standing", "Waves" };
        string[] descs  = {
            "Two-minute round. Score wins. Classic Downtown Devour.",
            "No timer. No scoring pressure. Survive until only one hole remains.   (coming soon)",
            "Four progressively brutal waves. Smaller boards, deadlier soldiers. Score accumulates.   (coming soon)",
        };
        _modeBg = new Image[3];
        for (int i = 0; i < 3; i++)
        {
            int idx = i;
            var opt = new GameObject("Mode" + i);
            opt.transform.SetParent(_startScreen.transform, false);
            var img = opt.AddComponent<Image>();
            img.color  = i == 0 ? MODE_SEL : MODE_UNSEL;
            _modeBg[i] = img;
            var oRT = opt.GetComponent<RectTransform>();
            oRT.anchorMin        = new Vector2(0.5f, 0.62f);
            oRT.anchorMax        = new Vector2(0.5f, 0.62f);
            oRT.pivot            = new Vector2(0.5f, 1f);
            oRT.sizeDelta        = new Vector2(620f, 64f);
            oRT.anchoredPosition = new Vector2(0f, -i * 72f);
            opt.AddComponent<Button>().onClick.AddListener(() => SelectMode(idx));

            var lbl = MakeTMPChild(opt, "Label", labels[i],
                new Vector2(0f, 1f), new Vector2(18f, -8f), new Vector2(580f, 28f),
                20, TextAlignmentOptions.Left);
            lbl.fontStyle = FontStyles.Bold;
            var dsc = MakeTMPChild(opt, "Desc", descs[i],
                new Vector2(0f, 1f), new Vector2(18f, -34f), new Vector2(584f, 26f),
                13, TextAlignmentOptions.Left);
            dsc.color = new Color(0.80f, 0.84f, 0.90f);
        }

        // PLAY button
        var playGO = new GameObject("PlayBtn");
        playGO.transform.SetParent(_startScreen.transform, false);
        var playImg = playGO.AddComponent<Image>();
        playImg.color = new Color(0.20f, 0.70f, 0.30f, 0.95f);
        var pRT = playGO.GetComponent<RectTransform>();
        pRT.anchorMin        = new Vector2(0.5f, 0.30f);
        pRT.anchorMax        = new Vector2(0.5f, 0.30f);
        pRT.sizeDelta        = new Vector2(300f, 70f);
        pRT.anchoredPosition = Vector2.zero;
        playGO.AddComponent<Button>().onClick.AddListener(() => GameManager.Instance.StartPlaying());
        var playLbl = MakeTMPChild(playGO, "PlayLabel", "PLAY",
            new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(280f, 60f),
            30, TextAlignmentOptions.Center);
        playLbl.fontStyle = FontStyles.Bold;

        var tips = MakeTMPChild(_startScreen, "Tips",
            "Desktop: move your mouse OR use W/A/S/D (or arrow keys) to steer.\n" +
            "Mobile: drag in the direction you want to go (joystick-style).\n" +
            "Avoid bigger holes — they'll eat you. Hunt smaller ones.",
            new Vector2(0.5f, 0.17f), Vector2.zero, new Vector2(760f, 80f),
            15, TextAlignmentOptions.Center);
        tips.color = new Color(0.78f, 0.82f, 0.88f);

        var credit = MakeTMPChild(_startScreen, "MusicCredit",
            "Music: procedurally generated orchestral theme in D minor, 128 BPM",
            new Vector2(0.5f, 0.06f), Vector2.zero, new Vector2(500f, 26f),
            11, TextAlignmentOptions.Center);
        credit.color = new Color(1f, 1f, 1f, 0.40f);

        _startScreen.SetActive(false);
    }

    void SelectMode(int idx)
    {
        _selectedMode = idx;
        for (int i = 0; i < _modeBg.Length; i++)
            _modeBg[i].color = i == idx ? MODE_SEL : MODE_UNSEL;
    }

    public void ShowStartScreen()
    {
        if (_startScreen != null) _startScreen.SetActive(true);
        if (_hudRoot     != null) _hudRoot.SetActive(false);
    }

    public void HideStartScreen()
    {
        if (_startScreen != null) _startScreen.SetActive(false);
        if (_hudRoot     != null) _hudRoot.SetActive(true);
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
            new Vector2(0.5f, 0.82f), new Vector2(0f, 0f), new Vector2(800f, 80f),
            56, TextAlignmentOptions.Center);
        _endTitle.fontStyle = FontStyles.Bold;

        _endSubtitle = MakeTMPChild(_endScreen, "Subtitle", "",
            new Vector2(0.5f, 0.73f), new Vector2(0f, 0f), new Vector2(700f, 40f),
            20, TextAlignmentOptions.Center);
        _endSubtitle.color = new Color(0.85f, 0.88f, 0.95f);

        var scoresTitle = MakeTMPChild(_endScreen, "ScoresTitle", "Final Scores",
            new Vector2(0.5f, 0.64f), new Vector2(0f, 0f), new Vector2(500f, 36f),
            24, TextAlignmentOptions.Center);
        scoresTitle.color = new Color(1f, 0.85f, 0.30f);

        _endBody = MakeTMPChild(_endScreen, "Scores", "",
            new Vector2(0.5f, 0.40f), new Vector2(0f, 0f), new Vector2(560f, 220f),
            24, TextAlignmentOptions.Top);

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

        // Exit button — below PLAY AGAIN
        var exitGO = new GameObject("ExitBtn");
        exitGO.transform.SetParent(_endScreen.transform, false);
        var exitImg = exitGO.AddComponent<Image>();
        exitImg.color = new Color(0.75f, 0.15f, 0.15f, 0.92f);
        var exitRT = exitGO.GetComponent<RectTransform>();
        exitRT.anchorMin        = new Vector2(0.5f, 0.14f);
        exitRT.anchorMax        = new Vector2(0.5f, 0.14f);
        exitRT.sizeDelta        = new Vector2(260f, 64f);
        exitRT.anchoredPosition = Vector2.zero;
        exitGO.AddComponent<Button>().onClick.AddListener(() => GameManager.Instance.QuitGame());

        MakeTMPChild(exitGO, "ExitLabel", "EXIT",
            new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(240f, 54f),
            26, TextAlignmentOptions.Center);

        _endScreen.SetActive(false);
    }

    // ── Pause screen ──────────────────────────────────────────────────────────

    void BuildPauseScreen()
    {
        _pauseScreen = new GameObject("PauseScreen");
        _pauseScreen.transform.SetParent(_canvas.transform, false);

        var overlay = _pauseScreen.AddComponent<Image>();
        overlay.color = new Color(0f, 0f, 0f, 0.82f);
        var rt = _pauseScreen.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        var title = MakeTMPChild(_pauseScreen, "PauseTitle", "PAUSED",
            new Vector2(0.5f, 0.62f), new Vector2(0f, 0f), new Vector2(700f, 90f),
            64, TextAlignmentOptions.Center);
        title.fontStyle = FontStyles.Bold;

        // Resume button
        var resumeGO = new GameObject("ResumeBtn");
        resumeGO.transform.SetParent(_pauseScreen.transform, false);
        var resumeImg = resumeGO.AddComponent<Image>();
        resumeImg.color = new Color(0.20f, 0.60f, 1f, 0.92f);
        var resumeRT = resumeGO.GetComponent<RectTransform>();
        resumeRT.anchorMin        = new Vector2(0.5f, 0.42f);
        resumeRT.anchorMax        = new Vector2(0.5f, 0.42f);
        resumeRT.sizeDelta        = new Vector2(260f, 64f);
        resumeRT.anchoredPosition = Vector2.zero;
        resumeGO.AddComponent<Button>().onClick.AddListener(() => GameManager.Instance.Resume());
        MakeTMPChild(resumeGO, "ResumeLabel", "RESUME",
            new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(240f, 54f),
            26, TextAlignmentOptions.Center);

        // Exit button — leaves the game mid-play
        var exitGO = new GameObject("PauseExitBtn");
        exitGO.transform.SetParent(_pauseScreen.transform, false);
        var exitImg = exitGO.AddComponent<Image>();
        exitImg.color = new Color(0.75f, 0.15f, 0.15f, 0.92f);
        var exitRT = exitGO.GetComponent<RectTransform>();
        exitRT.anchorMin        = new Vector2(0.5f, 0.30f);
        exitRT.anchorMax        = new Vector2(0.5f, 0.30f);
        exitRT.sizeDelta        = new Vector2(260f, 64f);
        exitRT.anchoredPosition = Vector2.zero;
        exitGO.AddComponent<Button>().onClick.AddListener(() => GameManager.Instance.QuitGame());
        MakeTMPChild(exitGO, "PauseExitLabel", "EXIT",
            new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(240f, 54f),
            26, TextAlignmentOptions.Center);

        _pauseScreen.SetActive(false);
    }

    public void ShowPauseScreen() { if (_pauseScreen != null) _pauseScreen.SetActive(true); }
    public void HidePauseScreen() { if (_pauseScreen != null) _pauseScreen.SetActive(false); }

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
