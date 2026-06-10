using UnityEngine;

// Plays real recorded samples from Assets/Resources/Audio/.
// Uses a round-robin AudioSource pool so pitch can vary per-shot without
// one sound cutting another when they overlap.
public class AudioManager : MonoBehaviour
{
    private AudioSource _musicSource;

    private const int     POOL = 8;
    private AudioSource[] _sfxPool;
    private int           _sfxIdx;

    // Minimum seconds between sounds of the same category to prevent bursts
    private readonly float[] _catCooldown  = new float[6];  // indexed by (int)ObjectCategory
    private const    float   BURST_COOLDOWN = 0.18f;

    private AudioClip[] _screams;
    private AudioClip[] _trees;
    private AudioClip[] _cars;
    private AudioClip[] _buildings;
    private AudioClip[] _metal;
    private AudioClip[] _gunshots;
    private AudioClip[] _soldierVoices;
    private AudioClip   _biteChew;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    void Awake()
    {
        _musicSource              = gameObject.AddComponent<AudioSource>();
        _musicSource.loop         = true;
        _musicSource.volume       = 0.28f;
        _musicSource.spatialBlend = 0f;

        _sfxPool = new AudioSource[POOL];
        for (int i = 0; i < POOL; i++)
        {
            _sfxPool[i]              = gameObject.AddComponent<AudioSource>();
            _sfxPool[i].spatialBlend = 0f;
        }

        LoadAllClips();
    }

    void LoadAllClips()
    {
        _screams       = LoadBank("scream",       7);
        _trees         = LoadBank("tree",         3);
        _cars          = LoadBank("car",          3);
        _buildings     = LoadBank("building",     4);
        _metal         = LoadBank("metal",        2);
        _gunshots      = LoadBank("gunshot",      3);
        _soldierVoices = LoadBank("soldiervoice", 8);
        _biteChew      = Load("bite_chew");
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public void StartMusic()
    {
        _musicSource.clip = BuildMusicClip();
        _musicSource.Play();
    }

    public void StopMusic() => _musicSource.Stop();

    // isPlayer: player sounds always play.
    // AI sounds: only play when the consuming hole is visible on screen.
    // All sounds: per-category burst cooldown to prevent simultaneous cacophony.
    public void PlayConsume(ObjectCategory cat, float size = 1f,
                            bool isPlayer = true, Vector3 sourcePos = default)
    {
        if (!isPlayer && !IsOnScreen(sourcePos)) return;

        int catIdx = (int)cat;
        if (Time.time - _catCooldown[catIdx] < BURST_COOLDOWN) return;
        _catCooldown[catIdx] = Time.time;
        switch (cat)
        {
            case ObjectCategory.Person:
                // Simulate voice variety: 4 profiles matching the browser game
                // female calm 1.00–1.15 | female panic 1.50–1.80
                // male calm   0.72–0.82 | male panic   1.00–1.15
                int profile = Random.Range(0, 4);
                float screamPitch = profile == 0 ? Random.Range(1.00f, 1.15f)
                                  : profile == 1 ? Random.Range(1.50f, 1.80f)
                                  : profile == 2 ? Random.Range(0.72f, 0.82f)
                                  :                Random.Range(1.00f, 1.15f);
                Play(_screams, Random.Range(0.55f, 0.80f), screamPitch);
                break;

            case ObjectCategory.Car:
                Play(_cars, Random.Range(0.55f, 0.75f), Random.Range(0.95f, 1.05f));
                break;

            case ObjectCategory.Building:
                // large (size > 9): deeper pitch, louder — small: higher, quieter
                float bPitch, bVol;
                if      (size > 9f) { bPitch = Random.Range(0.73f, 0.83f); bVol = Random.Range(0.80f, 0.90f); }
                else if (size > 6f) { bPitch = Random.Range(0.85f, 0.95f); bVol = Random.Range(0.67f, 0.77f); }
                else                { bPitch = Random.Range(1.00f, 1.10f); bVol = Random.Range(0.55f, 0.65f); }
                Play(_buildings, bVol, bPitch);
                break;

            case ObjectCategory.Tree:
                Play(_trees, Random.Range(0.50f, 0.70f), Random.Range(0.92f, 1.08f));
                break;

            default: // Prop / metal objects — widest pitch range of all
                Play(_metal, Random.Range(0.45f, 0.70f), Random.Range(0.82f, 1.22f));
                break;
        }
    }

    public void PlayHoleEat()
    {
        Play(_biteChew, 0.85f, Random.Range(0.92f, 1.08f));
    }

    public void PlayGunshot()
    {
        Play(_gunshots, Random.Range(0.40f, 0.60f), Random.Range(0.95f, 1.10f));
    }

    public void PlaySoldierVoice()
    {
        // Male soldier voice — low pitch range
        Play(_soldierVoices, Random.Range(0.60f, 0.75f), Random.Range(0.72f, 0.90f));
    }

    // ── Internal helpers ──────────────────────────────────────────────────────

    static bool IsOnScreen(Vector3 worldPos)
    {
        if (Camera.main == null) return true;
        var vp = Camera.main.WorldToViewportPoint(worldPos);
        return vp.z > 0f && vp.x >= -0.05f && vp.x <= 1.05f
                          && vp.y >= -0.05f && vp.y <= 1.05f;
    }

    void Play(AudioClip[] bank, float vol, float pitch)
    {
        if (bank == null || bank.Length == 0) return;
        Play(bank[Random.Range(0, bank.Length)], vol, pitch);
    }

    void Play(AudioClip clip, float vol, float pitch)
    {
        if (clip == null) return;
        var src = _sfxPool[_sfxIdx];
        _sfxIdx = (_sfxIdx + 1) % POOL;
        src.pitch = pitch;
        src.PlayOneShot(clip, vol);
    }

    // ── Procedural background music ───────────────────────────────────────────

    const int SAMPLE_RATE = 22050;

    AudioClip BuildMusicClip()
    {
        int     loop = SAMPLE_RATE * 4;
        float[] data = new float[loop];

        for (int i = 0; i < loop; i++)
        {
            float t = i / (float)SAMPLE_RATE;

            float bassP = (t * 130.8f) % 1f;
            float bass  = (bassP < 0.5f ? bassP * 4f - 1f : 3f - bassP * 4f) * 0.22f;
            bass *= 0.6f + 0.4f * Mathf.Sin(2f * Mathf.PI * 2f * t);

            float[] chord = { 261.6f, 329.6f, 392.0f, 523.2f };
            int     step  = ((int)(t * 4f)) % chord.Length;
            float   chime = Mathf.Sin(2f * Mathf.PI * chord[step] * t) * 0.10f
                          * Mathf.Exp(-((t * 4f) % 1f) * 6f);

            float pad = (Mathf.Sin(2f * Mathf.PI * 261.6f * t) * 0.14f
                       + Mathf.Sin(2f * Mathf.PI * 329.6f * t) * 0.10f
                       + Mathf.Sin(2f * Mathf.PI * 392.0f * t) * 0.08f)
                       * (0.7f + 0.3f * Mathf.Sin(2f * Mathf.PI * 0.25f * t));

            data[i] = Mathf.Clamp(bass + chime + pad, -1f, 1f);
        }

        var clip = AudioClip.Create("music", loop, 1, SAMPLE_RATE, false);
        clip.SetData(data, 0);
        return clip;
    }

    // ── Asset loading ─────────────────────────────────────────────────────────

    static AudioClip[] LoadBank(string prefix, int count)
    {
        var bank = new AudioClip[count];
        for (int i = 0; i < count; i++)
            bank[i] = Load($"{prefix}_{i}");
        return bank;
    }

    static AudioClip Load(string name)
    {
        var clip = Resources.Load<AudioClip>($"Audio/{name}");
        if (clip == null) Debug.LogWarning($"AudioManager: missing Audio/{name}");
        return clip;
    }
}
