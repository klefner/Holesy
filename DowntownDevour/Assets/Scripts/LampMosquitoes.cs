using UnityEngine;

// Tiny insects orbiting a street lamp head — visible only when the spotlight is on (night).
// Each bug traces an erratic elliptical path at a random radius, speed, and orbital tilt.
// The swarm gives lamps a warm, inhabited quality without any texture or shader work.
public class LampMosquitoes : MonoBehaviour
{
    struct Bug
    {
        public Transform Xform;
        public float     Radius;
        public float     Speed;
        public float     Phase;
        public float     WobbleRate;
        public float     TiltX;
        public float     TiltZ;
        public float     BobAmp;
        public float     BobRate;
    }

    Light    _spot;
    Vector3  _headLocal;
    Bug[]    _bugs;
    bool     _visible;

    static Material _bugMat;

    public void Init(Light spot, Vector3 headLocalPos)
    {
        _spot      = spot;
        _headLocal = headLocalPos;

        if (_bugMat == null)
        {
            _bugMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            _bugMat.SetColor("_BaseColor",  new Color(0.04f, 0.03f, 0.02f));
            _bugMat.SetFloat("_Smoothness", 0f);
            _bugMat.SetFloat("_Metallic",   0f);
        }

        int count = Random.Range(3, 7);
        _bugs = new Bug[count];
        for (int i = 0; i < count; i++)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Mosquito";
            go.transform.SetParent(transform, false);
            go.transform.localScale = Vector3.one * Random.Range(0.018f, 0.035f);
            go.GetComponent<Renderer>().sharedMaterial = _bugMat;
            Destroy(go.GetComponent<Collider>());

            _bugs[i] = new Bug
            {
                Xform      = go.transform,
                Radius     = Random.Range(0.18f, 0.55f),
                Speed      = Random.Range(1.2f, 3.5f) * (Random.value < 0.5f ? 1f : -1f),
                Phase      = Random.Range(0f, Mathf.PI * 2f),
                WobbleRate = Random.Range(1.8f, 4.5f),
                TiltX      = Random.Range(-0.30f, 0.30f),
                TiltZ      = Random.Range(-0.30f, 0.30f),
                BobAmp     = Random.Range(0.04f, 0.14f),
                BobRate    = Random.Range(2.2f, 5.5f),
            };
        }

        SetVisible(spot != null && spot.enabled);
    }

    void SetVisible(bool on)
    {
        if (on == _visible) return;
        _visible = on;
        if (_bugs == null) return;
        foreach (var b in _bugs)
            if (b.Xform != null)
                b.Xform.gameObject.SetActive(on);
    }

    void Update()
    {
        if (_spot == null || _bugs == null) return;

        bool on = _spot.enabled;
        SetVisible(on);
        if (!on) return;

        Vector3 headWorld = transform.TransformPoint(_headLocal);
        float   t         = Time.time;

        for (int i = 0; i < _bugs.Length; i++)
        {
            ref var b = ref _bugs[i];
            if (b.Xform == null) continue;

            float sv = 1f + 0.55f * Mathf.Sin(t * b.WobbleRate + b.Phase);
            float a  = t * b.Speed * sv + b.Phase;
            float x  = Mathf.Cos(a) * b.Radius;
            float z  = Mathf.Sin(a) * b.Radius;
            float y  = x * b.TiltX + z * b.TiltZ
                     + Mathf.Sin(t * b.BobRate + b.Phase) * b.BobAmp;

            b.Xform.position = headWorld + new Vector3(x, y, z);
        }
    }
}
