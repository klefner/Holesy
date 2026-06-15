using UnityEngine;

// Procedural fire and smoke on a crashed car.
// Uses emissive spheres that flicker and drift — no Particle System required.
public class CarFire : MonoBehaviour
{
    struct Ember
    {
        public Transform Xform;
        public Material  Mat;
        public float     Phase, Rate, Amp, BaseY;
    }

    Ember[] _flames;
    Ember[] _smoke;
    Light   _glow;
    float   _lifetime;

    const int   FLAME_N  = 5;
    const int   SMOKE_N  = 4;
    const float MAX_LIFE = 80f;

    void Start()
    {
        _lifetime = MAX_LIFE;

        _flames = new Ember[FLAME_N];
        for (int i = 0; i < FLAME_N; i++)
        {
            float baseY = 0.90f + i * 0.22f;
            var   go    = MakeSphere("Flame", baseY,
                Random.Range(-0.40f, 0.40f), Random.Range(-0.45f, 0.45f),
                Random.Range(0.22f, 0.48f));
            var mat = EmissiveMat(new Color(0.9f, 0.25f, 0f),
                Color.Lerp(new Color(3f, 0.3f, 0f), new Color(3f, 1.8f, 0.1f), Random.value));
            go.GetComponent<Renderer>().material = mat;
            _flames[i] = new Ember {
                Xform = go.transform, Mat = mat,
                Phase = Random.Range(0f, 6.28f),
                Rate  = Random.Range(5f, 10f),
                Amp   = Random.Range(0.06f, 0.22f),
                BaseY = baseY,
            };
        }

        _smoke = new Ember[SMOKE_N];
        for (int i = 0; i < SMOKE_N; i++)
        {
            float sy = 1.6f + i * 0.55f;
            var   go = MakeSphere("Smoke", sy,
                Random.Range(-0.28f, 0.28f), Random.Range(-0.28f, 0.28f),
                Random.Range(0.28f, 0.55f));
            float b   = Random.Range(0.05f, 0.12f);
            var   mat = OpaqueMat(new Color(b, b, b));
            go.GetComponent<Renderer>().material = mat;
            _smoke[i] = new Ember {
                Xform = go.transform, Mat = mat,
                Phase = Random.Range(0f, 6.28f),
                Rate  = Random.Range(0.8f, 2.0f),
                BaseY = sy,
            };
        }

        // Orange point light — flickering fire glow on surrounding road/buildings
        var lgO = new GameObject("FireGlow");
        lgO.transform.SetParent(transform, false);
        lgO.transform.localPosition = Vector3.up * 1.2f;
        _glow           = lgO.AddComponent<Light>();
        _glow.type      = LightType.Point;
        _glow.color     = new Color(1.0f, 0.40f, 0.02f);
        _glow.intensity = 9f;
        _glow.range     = 14f;
        _glow.shadows   = LightShadows.None;
    }

    void Update()
    {
        _lifetime -= Time.deltaTime;
        float fade = Mathf.Clamp01(_lifetime / 8f);
        float t    = Time.time;

        foreach (var f in _flames)
        {
            float bob = Mathf.Sin(t * f.Rate + f.Phase) * f.Amp;
            var   p   = f.Xform.localPosition;
            f.Xform.localPosition = new Vector3(p.x, f.BaseY + bob, p.z);

            float bright = fade * (0.65f + 0.35f * Mathf.Sin(t * f.Rate * 1.4f + f.Phase));
            f.Mat.SetColor("_EmissionColor",
                Color.Lerp(new Color(2.5f, 0.15f, 0f), new Color(3f, 1.6f, 0.05f), bright) * bright);
        }

        foreach (var s in _smoke)
        {
            float rise = (t * s.Rate * 0.25f + s.Phase) % 2.8f;
            var   p    = s.Xform.localPosition;
            s.Xform.localPosition = new Vector3(p.x, s.BaseY + rise, p.z);
            s.Xform.localScale    = Vector3.one * (0.30f + rise * 0.18f);
        }

        if (_glow != null)
            _glow.intensity = fade * (7f + 2f * Mathf.Sin(t * 6.5f));

        if (_lifetime <= 0f) Destroy(this);
    }

    void OnDestroy()
    {
        if (_flames != null) foreach (var f in _flames) if (f.Mat != null) Destroy(f.Mat);
        if (_smoke  != null) foreach (var s in _smoke)  if (s.Mat != null) Destroy(s.Mat);
    }

    GameObject MakeSphere(string name, float y, float ox, float oz, float scale)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        go.transform.SetParent(transform, false);
        go.transform.localPosition = new Vector3(ox, y, oz);
        go.transform.localScale    = Vector3.one * scale;
        Destroy(go.GetComponent<Collider>());
        return go;
    }

    static Material EmissiveMat(Color baseCol, Color emit)
    {
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetColor("_BaseColor",     baseCol);
        mat.SetFloat("_Smoothness",    0.15f);
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", emit);
        return mat;
    }

    static Material OpaqueMat(Color col)
    {
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetColor("_BaseColor",  col);
        mat.SetFloat("_Smoothness", 0.05f);
        return mat;
    }
}
