using System.Collections.Generic;
using UnityEngine;

// Plays real recorded samples loaded from Assets/Resources/Audio/.
// Falls back to a procedural beep if a file is missing so the game never crashes.
public class AudioManager : MonoBehaviour
{
    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    // Loaded clip banks
    private AudioClip[] _screams;
    private AudioClip[] _trees;
    private AudioClip[] _cars;
    private AudioClip[] _buildings;
    private AudioClip[] _metal;
    private AudioClip[] _gunshots;
    private AudioClip[] _soldierVoices;
    private AudioClip   _biteChew;

    const float MUSIC_VOL  = 0.30f;
    const float SFX_VOL    = 0.80f;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    void Awake()
    {
        _musicSource              = gameObject.AddComponent<AudioSource>();
        _musicSource.loop         = true;
        _musicSource.volume       = MUSIC_VOL;
        _musicSource.spatialBlend = 0f;

        _sfxSource              = gameObject.AddComponent<AudioSource>();
        _sfxSource.spatialBlend = 0f;
        _sfxSource.volume       = SFX_VOL;

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
        if (clip == null)
            Debug.LogWarning($"AudioManager: missing clip Audio/{name}");
        return clip;
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public void StartMusic()
    {
        // Music is procedural — use the bite_chew clip as a placeholder if none
        // provided, or just leave silent; the SFX are what matter most.
        _musicSource.clip = BuildMusicClip();
        _musicSource.Play();
    }

    public void StopMusic() => _musicSource.Stop();

    public void PlayConsume(ObjectCategory cat)
    {
        switch (cat)
        {
            case ObjectCategory.Person:   PlayRandom(_screams,   SFX_VOL);         break;
            case ObjectCategory.Car:      PlayRandom(_cars,      SFX_VOL);         break;
            case ObjectCategory.Building: PlayRandom(_buildings, SFX_VOL * 1.2f);  break;
            case ObjectCategory.Tree:     PlayRandom(_trees,     SFX_VOL);         break;
            default:                      PlayRandom(_metal,     SFX_VOL * 0.8f);  break;
        }
    }

    public void PlayHoleEat()
    {
        Play(_biteChew, SFX_VOL * 1.1f);
    }

    public void PlayGunshot()
    {
        PlayRandom(_gunshots, SFX_VOL * 0.6f);
    }

    public void PlaySoldierVoice()
    {
        PlayRandom(_soldierVoices, SFX_VOL * 0.7f);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    void PlayRandom(AudioClip[] bank, float vol)
    {
        if (bank == null || bank.Length == 0) return;
        var clip = bank[Random.Range(0, bank.Length)];
        Play(clip, vol);
    }

    void Play(AudioClip clip, float vol)
    {
        if (clip == null || _sfxSource == null) return;
        _sfxSource.PlayOneShot(clip, vol);
    }

    // ── Procedural music (keeps background alive with no music file) ──────────

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

            float[] chord  = { 261.6f, 329.6f, 392.0f, 523.2f };
            int     step   = ((int)(t * 4f)) % chord.Length;
            float   chime  = Mathf.Sin(2f * Mathf.PI * chord[step] * t) * 0.10f
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
}
