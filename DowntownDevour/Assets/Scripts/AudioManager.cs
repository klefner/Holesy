using System;
using System.Collections.Generic;
using UnityEngine;

// Procedural audio: generates PCM clips at runtime, no external files needed.
public class AudioManager : MonoBehaviour
{
    private AudioSource _musicSource;
    private static readonly Dictionary<string, AudioClip> _clipCache = new();

    const int   SAMPLE_RATE = 22050;
    const float MASTER_VOL  = 0.55f;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    void Awake()
    {
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop   = true;
        _musicSource.volume = 0.25f;
    }

    public void StartMusic()
    {
        _musicSource.clip = BuildMusicClip();
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    // ── Consumption sounds ────────────────────────────────────────────────────

    public void PlayConsume(ObjectCategory cat, Vector3 worldPos)
    {
        string key;
        switch (cat)
        {
            case ObjectCategory.Person:   key = "scream";    break;
            case ObjectCategory.Car:      key = "crash";     break;
            case ObjectCategory.Building: key = "rumble";    break;
            case ObjectCategory.Tree:     key = "crack";     break;
            default:                      key = "clang";     break;
        }
        PlayAt(GetClip(key), worldPos, MASTER_VOL);
    }

    public void PlayHoleEat(Vector3 worldPos)
    {
        PlayAt(GetClip("eat"), worldPos, MASTER_VOL * 1.4f);
    }

    // ── Clip retrieval with lazy build ────────────────────────────────────────

    AudioClip GetClip(string key)
    {
        if (_clipCache.TryGetValue(key, out var cached)) return cached;
        var clip = BuildClip(key);
        _clipCache[key] = clip;
        return clip;
    }

    AudioClip BuildClip(string key)
    {
        switch (key)
        {
            case "scream": return GenerateScream();
            case "crash":  return GenerateCrash();
            case "rumble": return GenerateRumble();
            case "crack":  return GenerateCrack();
            case "clang":  return GenerateClang();
            case "eat":    return GenerateHoleEat();
            default:       return GenerateBeep(440f, 0.1f);
        }
    }

    // ── PCM generators ────────────────────────────────────────────────────────

    AudioClip GenerateScream()
    {
        // Rising and falling pitch, short
        int    samples = (int)(SAMPLE_RATE * 0.35f);
        float[] data   = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t     = i / (float)samples;
            float freq  = 600f + Mathf.Sin(t * Mathf.PI) * 400f;
            float env   = Mathf.Sin(t * Mathf.PI);
            data[i] = Mathf.Sin(2f * Mathf.PI * freq * i / SAMPLE_RATE) * env * 0.6f;
        }
        return FromData("scream", data);
    }

    AudioClip GenerateCrash()
    {
        int    samples = (int)(SAMPLE_RATE * 0.5f);
        float[] data   = new float[samples];
        var     rng    = new System.Random(42);
        for (int i = 0; i < samples; i++)
        {
            float t   = i / (float)samples;
            float env = Mathf.Exp(-t * 5f);
            // White noise + low-freq component
            float noise = (float)(rng.NextDouble() * 2 - 1);
            float tone  = Mathf.Sin(2f * Mathf.PI * 80f * i / SAMPLE_RATE);
            data[i] = (noise * 0.7f + tone * 0.3f) * env * 0.8f;
        }
        return FromData("crash", data);
    }

    AudioClip GenerateRumble()
    {
        int    samples = (int)(SAMPLE_RATE * 0.8f);
        float[] data   = new float[samples];
        var     rng    = new System.Random(7);
        for (int i = 0; i < samples; i++)
        {
            float t   = i / (float)samples;
            float env = Mathf.Exp(-t * 3f) * (1f - Mathf.Pow(t, 0.1f) * 0.3f);
            float noise = (float)(rng.NextDouble() * 2 - 1);
            float low   = Mathf.Sin(2f * Mathf.PI * 40f * i / SAMPLE_RATE)
                        + Mathf.Sin(2f * Mathf.PI * 60f * i / SAMPLE_RATE) * 0.5f;
            data[i] = (noise * 0.4f + low * 0.6f) * env * 0.7f;
        }
        return FromData("rumble", data);
    }

    AudioClip GenerateCrack()
    {
        int    samples = (int)(SAMPLE_RATE * 0.25f);
        float[] data   = new float[samples];
        var     rng    = new System.Random(13);
        for (int i = 0; i < samples; i++)
        {
            float t   = i / (float)samples;
            float env = Mathf.Exp(-t * 12f);
            data[i] = (float)(rng.NextDouble() * 2 - 1) * env * 0.9f;
        }
        return FromData("crack", data);
    }

    AudioClip GenerateClang()
    {
        int    samples = (int)(SAMPLE_RATE * 0.4f);
        float[] data   = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t   = i / (float)samples;
            float env = Mathf.Exp(-t * 8f);
            data[i] = (Mathf.Sin(2f * Mathf.PI * 900f * i / SAMPLE_RATE)
                     + Mathf.Sin(2f * Mathf.PI * 1400f * i / SAMPLE_RATE) * 0.5f)
                     * env * 0.5f;
        }
        return FromData("clang", data);
    }

    AudioClip GenerateHoleEat()
    {
        int    samples = (int)(SAMPLE_RATE * 0.6f);
        float[] data   = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t    = i / (float)samples;
            float env  = Mathf.Sin(t * Mathf.PI) * Mathf.Exp(-t * 2f);
            float freq = 200f - t * 150f;
            data[i] = Mathf.Sin(2f * Mathf.PI * freq * i / SAMPLE_RATE) * env * 0.9f;
        }
        return FromData("eat", data);
    }

    // Simple looping bass drone for background music
    AudioClip BuildMusicClip()
    {
        int    loop    = SAMPLE_RATE * 4;  // 4-second loop
        float[] data   = new float[loop];
        // D-minor root: D2 ≈ 73.4 Hz
        float[] freqs  = { 73.4f, 110.0f, 146.8f, 220.0f };
        float[] vols   = { 0.4f,  0.25f,  0.2f,   0.15f  };

        for (int i = 0; i < loop; i++)
        {
            float t   = i / (float)SAMPLE_RATE;
            float smp = 0f;
            for (int k = 0; k < freqs.Length; k++)
                smp += Mathf.Sin(2f * Mathf.PI * freqs[k] * t) * vols[k];
            // Slow tremolo
            smp *= 0.8f + 0.2f * Mathf.Sin(2f * Mathf.PI * 0.5f * t);
            data[i] = smp;
        }
        return FromData("music", data);
    }

    AudioClip GenerateBeep(float freq, float dur)
    {
        int    samples = (int)(SAMPLE_RATE * dur);
        float[] data   = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t   = i / (float)samples;
            float env = Mathf.Sin(t * Mathf.PI);
            data[i] = Mathf.Sin(2f * Mathf.PI * freq * i / SAMPLE_RATE) * env * 0.5f;
        }
        return FromData("beep", data);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    static AudioClip FromData(string name, float[] data)
    {
        var clip = AudioClip.Create(name, data.Length, 1, SAMPLE_RATE, false);
        clip.SetData(data, 0);
        return clip;
    }

    static void PlayAt(AudioClip clip, Vector3 pos, float vol)
    {
        // Distance attenuation: fall off past 30 units, silent past 80
        float dist  = Vector3.Distance(Camera.main.transform.position, pos);
        float atten = Mathf.Clamp01(1f - (dist - 30f) / 50f);
        if (atten <= 0.02f) return;

        AudioSource.PlayClipAtPoint(clip, pos, vol * atten);
    }
}
