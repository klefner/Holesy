using System;
using System.Collections.Generic;
using UnityEngine;

// Procedural audio: generates PCM clips at runtime, no external files needed.
public class AudioManager : MonoBehaviour
{
    private AudioSource _musicSource;
    private AudioSource _sfxSource;
    private static readonly Dictionary<string, AudioClip> _clipCache = new();

    const int   SAMPLE_RATE = 22050;
    const float MASTER_VOL  = 0.7f;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    void Awake()
    {
        _musicSource              = gameObject.AddComponent<AudioSource>();
        _musicSource.loop         = true;
        _musicSource.volume       = 0.30f;
        _musicSource.spatialBlend = 0f; // 2D — music is global, not positional

        _sfxSource              = gameObject.AddComponent<AudioSource>();
        _sfxSource.spatialBlend = 0f; // 2D — avoids distance silence from top-down camera
        _sfxSource.volume       = MASTER_VOL;
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

    public void PlayConsume(ObjectCategory cat)
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
        PlayAt(GetClip(key), MASTER_VOL);
    }

    public void PlayHoleEat()
    {
        PlayAt(GetClip("eat"), MASTER_VOL * 1.4f);
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
        int     samples = (int)(SAMPLE_RATE * 0.55f);
        float[] data    = new float[samples];
        var     rng     = new System.Random(99);

        float phase1 = 0f, phase2 = 0f, phase3 = 0f;
        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)samples;

            // Pitch rises fast then wobbles (terror arc)
            float freq = 320f + 900f * Mathf.Pow(Mathf.Clamp01(t * 2.5f), 0.4f);
            freq *= 1f + 0.045f * Mathf.Sin(2f * Mathf.PI * 8f * t); // vibrato

            float dt = freq / SAMPLE_RATE;
            phase1 = (phase1 + dt) % 1f;
            phase2 = (phase2 + dt * 2f) % 1f;
            phase3 = (phase3 + dt * 3f) % 1f;

            // Voiced formants + breathiness noise
            float tone = Mathf.Sin(phase1 * 2f * Mathf.PI) * 0.55f
                       + Mathf.Sin(phase2 * 2f * Mathf.PI) * 0.28f
                       + Mathf.Sin(phase3 * 2f * Mathf.PI) * 0.12f;
            float noise = (float)(rng.NextDouble() * 2 - 1) * 0.18f;

            // Envelope: snap open, long sustain, quick cut
            float env = t < 0.04f ? t / 0.04f
                      : t < 0.75f ? 1f
                      : (1f - t) / 0.25f;

            data[i] = Mathf.Clamp((tone + noise) * env * 0.75f, -1f, 1f);
        }
        return FromData("scream", data);
    }

    AudioClip GenerateCrash()
    {
        int     samples = (int)(SAMPLE_RATE * 1.2f);
        float[] data    = new float[samples];
        var     rng     = new System.Random(42);

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)samples;

            // Initial impact thud (low body)
            float thud  = (Mathf.Sin(2f * Mathf.PI * 90f  * i / SAMPLE_RATE) * 0.5f
                         + Mathf.Sin(2f * Mathf.PI * 140f * i / SAMPLE_RATE) * 0.3f)
                         * Mathf.Exp(-t * 14f);

            // Metal resonance layers (the "crash" character)
            float metal = (Mathf.Sin(2f * Mathf.PI * 680f  * i / SAMPLE_RATE) * 0.4f
                         + Mathf.Sin(2f * Mathf.PI * 1050f * i / SAMPLE_RATE) * 0.3f
                         + Mathf.Sin(2f * Mathf.PI * 1840f * i / SAMPLE_RATE) * 0.2f)
                         * Mathf.Exp(-t * 5f);

            // Scrape noise tail
            float noise = (float)(rng.NextDouble() * 2 - 1)
                        * Mathf.Exp(-t * 4f) * 0.55f;

            data[i] = Mathf.Clamp(thud + metal + noise, -1f, 1f);
        }
        return FromData("crash", data);
    }

    AudioClip GenerateRumble()
    {
        int     samples = (int)(SAMPLE_RATE * 2.2f);
        float[] data    = new float[samples];
        var     rng     = new System.Random(7);
        var     rng2    = new System.Random(71);

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)samples;

            // Deep sub-bass collapse (the "weight" of a building)
            float sub = (Mathf.Sin(2f * Mathf.PI * 28f * i / SAMPLE_RATE) * 0.5f
                       + Mathf.Sin(2f * Mathf.PI * 42f * i / SAMPLE_RATE) * 0.35f
                       + Mathf.Sin(2f * Mathf.PI * 58f * i / SAMPLE_RATE) * 0.25f)
                       * Mathf.Exp(-t * 1.8f);

            // Structural creak / groan (mid)
            float creak = Mathf.Sin(2f * Mathf.PI * 185f * i / SAMPLE_RATE)
                        * Mathf.Exp(-t * 2.5f) * 0.35f
                        * (1f + 0.3f * Mathf.Sin(2f * Mathf.PI * 3.5f * t));

            // Debris shower (bandpass noise)
            float debris = (float)(rng.NextDouble() * 2 - 1) * 0.45f
                         * Mathf.Exp(-t * 2.0f);
            // Occasional impact pops in debris
            float pop = (float)(rng2.NextDouble() * 2 - 1) * 0.6f
                      * (rng2.NextDouble() < 0.003 ? 1f : 0f);

            data[i] = Mathf.Clamp(sub + creak + debris + pop, -1f, 1f);
        }
        return FromData("rumble", data);
    }

    AudioClip GenerateCrack()
    {
        int     samples = (int)(SAMPLE_RATE * 0.3f);
        float[] data    = new float[samples];
        var     rng     = new System.Random(13);

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)samples;

            // Sharp woody transient
            float snap  = (float)(rng.NextDouble() * 2 - 1) * Mathf.Exp(-t * 40f);
            // Woody resonance body
            float wood  = (Mathf.Sin(2f * Mathf.PI * 220f * i / SAMPLE_RATE) * 0.5f
                         + Mathf.Sin(2f * Mathf.PI * 380f * i / SAMPLE_RATE) * 0.3f)
                         * Mathf.Exp(-t * 18f);

            data[i] = Mathf.Clamp(snap * 0.7f + wood * 0.5f, -1f, 1f);
        }
        return FromData("crack", data);
    }

    AudioClip GenerateClang()
    {
        int     samples = (int)(SAMPLE_RATE * 0.5f);
        float[] data    = new float[samples];
        var     rng     = new System.Random(55);

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)samples;

            // Metal clang: inharmonic partials (real metal isn't perfectly harmonic)
            float ring = Mathf.Sin(2f * Mathf.PI * 820f  * i / SAMPLE_RATE) * 0.40f * Mathf.Exp(-t * 9f)
                       + Mathf.Sin(2f * Mathf.PI * 1380f * i / SAMPLE_RATE) * 0.30f * Mathf.Exp(-t * 11f)
                       + Mathf.Sin(2f * Mathf.PI * 2100f * i / SAMPLE_RATE) * 0.20f * Mathf.Exp(-t * 14f)
                       + Mathf.Sin(2f * Mathf.PI * 3200f * i / SAMPLE_RATE) * 0.10f * Mathf.Exp(-t * 18f);
            // Impact noise burst
            float hit = (float)(rng.NextDouble() * 2 - 1) * Mathf.Exp(-t * 60f) * 0.5f;

            data[i] = Mathf.Clamp(ring + hit, -1f, 1f);
        }
        return FromData("clang", data);
    }

    AudioClip GenerateHoleEat()
    {
        int     samples = (int)(SAMPLE_RATE * 0.5f);
        float[] data    = new float[samples];
        var     rng     = new System.Random(77);

        float phase = 0f;
        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)samples;

            // Descending "gulp" — pitch drops hard (200→55Hz)
            float freq = 200f * Mathf.Pow(0.28f, t * 2.2f);
            phase = (phase + freq / SAMPLE_RATE) % 1f;

            float tone  = Mathf.Sin(phase * 2f * Mathf.PI) * 0.6f
                        + Mathf.Sin(phase * 4f * Mathf.PI) * 0.25f;
            // Suction whoosh
            float whoosh = (float)(rng.NextDouble() * 2 - 1)
                         * Mathf.Exp(-Mathf.Abs(t - 0.15f) * 20f) * 0.4f;

            float env = t < 0.05f ? t / 0.05f : Mathf.Exp(-(t - 0.05f) * 4f);
            data[i] = Mathf.Clamp((tone + whoosh) * env * 0.9f, -1f, 1f);
        }
        return FromData("eat", data);
    }

    // Upbeat city-vibe loop — major key, punchy rhythm
    AudioClip BuildMusicClip()
    {
        int     loop = SAMPLE_RATE * 4;
        float[] data = new float[loop];

        // C major feel: C3=130.8, E3=164.8, G3=196.0, C4=261.6, G2=98.0
        float[] bassFreqs = { 130.8f, 98.0f,  130.8f, 164.8f };
        int[]   bassBeats = { 0,      22050,   33075,  44100  }; // offsets in samples

        for (int i = 0; i < loop; i++)
        {
            float t   = i / (float)SAMPLE_RATE;
            float smp = 0f;

            // Warm bass pulse (triangle wave approximation)
            float bassFreq = 130.8f;
            float bassP    = (t * bassFreq) % 1f;
            float bass     = (bassP < 0.5f ? bassP * 4f - 1f : 3f - bassP * 4f) * 0.25f;
            bass *= 0.7f + 0.3f * Mathf.Sin(2f * Mathf.PI * 2f * t); // rhythmic pulse

            // Bright arpeggiated chime (C major triad)
            float[] chord = { 261.6f, 329.6f, 392.0f, 523.2f };
            int     step  = ((int)(t * 4f)) % chord.Length;
            float   chimeF = chord[step];
            float   chime = Mathf.Sin(2f * Mathf.PI * chimeF * t) * 0.12f
                          * Mathf.Exp(-((t * 4f) % 1f) * 6f); // pluck envelope per step

            // Soft pad (whole tone sustain)
            float pad = (Mathf.Sin(2f * Mathf.PI * 261.6f * t) * 0.15f
                       + Mathf.Sin(2f * Mathf.PI * 329.6f * t) * 0.12f
                       + Mathf.Sin(2f * Mathf.PI * 392.0f * t) * 0.10f)
                       * (0.7f + 0.3f * Mathf.Sin(2f * Mathf.PI * 0.25f * t));

            smp = bass + chime + pad;
            data[i] = Mathf.Clamp(smp, -1f, 1f);
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

    void PlayAt(AudioClip clip, float vol)
    {
        if (_sfxSource == null) return;
        _sfxSource.PlayOneShot(clip, vol);
    }
}
