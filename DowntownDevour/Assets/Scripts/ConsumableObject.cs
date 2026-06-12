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
    private Vector3   _startScale;
    private HoleBase  _hole;
    private Rigidbody _rb;

    const float FALL_GRAVITY = 18f;
    const float SHRINK_DEPTH = 72f;    // perspective shrink: scale = e^(-depth/this)
    const float MIN_VISIBLE  = 0.025f; // ~a pixel — destroy below this fraction

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

            // Perspective-style shrink tied to depth: the deeper it falls the
            // smaller it gets, asymptotically — it never pops out at a fixed
            // depth.  Destroyed only once it is effectively a pixel.
            float depth = -0.4f - transform.position.y;
            float f = Mathf.Exp(-depth / SHRINK_DEPTH);
            transform.localScale = _startScale * f;
            if (f <= MIN_VISIBLE) { Destroy(gameObject); return; }
        }

        if (transform.position.y < -80f) Destroy(gameObject);
    }

    // Flying debris (and knocked-around props) can dislodge standing building
    // parts.  The building decides whether the hit is strong enough relative
    // to the part's weight; the impulse PhysX computed for this collision is
    // the "pressure" of the hit, and our own recoil is already handled by the
    // engine — equal and opposite.
    void OnCollisionEnter(Collision c)
    {
        if (IsConsumed || c.contactCount == 0) return;
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        if (_rb == null || _rb.isKinematic) return;

        Transform other = c.transform;
        if (other.parent == null) return;
        var building = other.parent.GetComponent<BuildingCollapse>();
        if (building == null) return;

        // Impulse magnitude along the direction the hit travels into the part
        Vector3 dir = -c.GetContact(0).normal;
        building.Impact(other, dir * c.impulse.magnitude);
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
