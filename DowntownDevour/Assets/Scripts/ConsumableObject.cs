using UnityEngine;

public enum ObjectCategory { Person, Car, Tree, Building, Prop }

public class ConsumableObject : MonoBehaviour
{
    public float          Size            { get; private set; }
    public int            Tier            { get; private set; }
    public float          Value           { get; private set; }
    public ObjectCategory Category        { get; private set; }
    public bool           IsConsumed      { get; private set; }
    public float          FootprintRadius { get; private set; }

    private bool     _falling;
    private float    _spinVel;
    private Vector3  _spinAxis;
    private float    _fallVel;
    private Vector3  _startScale;
    private HoleBase _hole;

    const float FALL_GRAVITY = 18f;
    const float FADE_DEPTH   = 22f; // metres of visible fall below the surface

    public void Init(float size, int tier, float value, ObjectCategory category,
                     float footprintRadius = 0f)
    {
        Size            = size;
        Tier            = tier;
        Value           = value;
        Category        = category;
        FootprintRadius = footprintRadius;
        _startScale     = transform.localScale;
    }

    void Update()
    {
        if (!_falling)
        {
            // Physics debris that escapes the map (launched past the world
            // edge, where there is no ground to land on or to hide it) must
            // vanish once below the surface.  Nothing below ground is ever
            // visible unless it is falling inside a hole.
            if (transform.position.y < -1.5f) Destroy(gameObject);
            return;
        }

        // Tumble around a horizontal axis only — no Y spin, no vortex
        transform.Rotate(_spinAxis, _spinVel * Time.deltaTime, Space.World);

        // Fall straight down under custom gravity — no XZ interference at all
        _fallVel           += FALL_GRAVITY * Time.deltaTime;
        transform.position += Vector3.down * (_fallVel * Time.deltaTime);

        if (transform.position.y < -0.4f)
        {
            // Below the surface an object exists ONLY while the hole is still
            // over it.  The stencil trick hides underground objects by drawing
            // ground over them, so anywhere without ground (world edge, other
            // holes) they would show through — erase them the moment the hole
            // is no longer above.
            bool overHole = false;
            if (_hole != null && _hole.Alive)
            {
                float hdx = transform.position.x - _hole.transform.position.x;
                float hdz = transform.position.z - _hole.transform.position.z;
                overHole  = hdx * hdx + hdz * hdz < _hole.Radius * _hole.Radius;
            }
            if (!overHole) { Destroy(gameObject); return; }

            // Scale is tied to DEPTH, not a timer: full size at the surface,
            // gone after FADE_DEPTH metres of fall — objects keep falling and
            // dwindling until too small to see.
            float depth = -0.4f - transform.position.y;
            float t = Mathf.Clamp01(depth / FADE_DEPTH);
            transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, t);
            if (t >= 1f) { Destroy(gameObject); return; }
        }

        if (transform.position.y < -30f) Destroy(gameObject);
    }

    public void MarkConsumed(HoleBase hole)
    {
        if (IsConsumed) return;
        IsConsumed = true;
        _hole = hole;
        GameManager.Instance.AllObjects.Remove(this);

        // Hand off Rigidbody velocity to our scripted fall so there is no hitch
        _fallVel = 0f;
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (rb.linearVelocity.y < 0f) _fallVel = -rb.linearVelocity.y;
            rb.isKinematic = true;
        }

        _startScale  = transform.localScale;
        _falling     = true;
        _spinVel     = (Random.value < 0.5f ? 1f : -1f) * Random.Range(60f, 180f);
        _spinAxis    = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        if (_spinAxis.sqrMagnitude < 0.01f) _spinAxis = Vector3.right;
        _spinAxis.Normalize();
    }

    void OnDestroy()
    {
        if (!IsConsumed && GameManager.Instance != null)
            GameManager.Instance.AllObjects.Remove(this);
    }
}
