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

    private bool      _falling;
    private float     _spinVel;
    private Vector3   _spinAxis;
    private float     _scaleVel;
    private float     _fallVel;
    private Vector3   _startScale;
    private Transform _holeTransform;
    private Vector2   _holeOffset;

    const float FALL_GRAVITY = 14f;
    const float SHRINK_TIME  = 0.6f;

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
        if (!_falling) return;

        // Follow the hole as it moves, but keep this object's own offset from
        // the hole center — it falls where it was, never pulled to the middle.
        if (_holeTransform != null)
        {
            var p  = transform.position;
            var hp = _holeTransform.position;
            p.x = hp.x + _holeOffset.x;
            p.z = hp.z + _holeOffset.y;
            transform.position = p;
        }

        // Tumble around a horizontal axis — never spin around Y (vortex look)
        transform.Rotate(_spinAxis, _spinVel * Time.deltaTime, Space.World);

        _fallVel           += FALL_GRAVITY * Time.deltaTime;
        transform.position += Vector3.down * (_fallVel * Time.deltaTime);

        // Shrink only once the object has dropped below the ground surface, so
        // nothing visibly compresses while still at street level
        if (transform.position.y < 0f)
        {
            _scaleVel           += Time.deltaTime / SHRINK_TIME;
            float t              = Mathf.Clamp01(_scaleVel);
            transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, t);
            if (t >= 1f) { Destroy(gameObject); return; }
        }

        if (transform.position.y < -25f) Destroy(gameObject);
    }

    public void MarkConsumed(HoleBase hole)
    {
        if (IsConsumed) return;
        IsConsumed = true;
        GameManager.Instance.AllObjects.Remove(this);

        _holeTransform = hole.transform;

        // Preserve where the object is relative to the hole, clamped just inside
        // the rim so it sinks through the void, not through solid ground
        Vector3 rel = transform.position - hole.transform.position;
        var offset  = new Vector2(rel.x, rel.z);
        float maxOff = Mathf.Max(0f, hole.Radius - 0.25f);
        if (offset.sqrMagnitude > maxOff * maxOff)
            offset = offset.normalized * maxOff;
        _holeOffset = offset;

        // Carry physics momentum into the scripted fall so there's no hitch
        _fallVel = 0f;
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (rb.linearVelocity.y < 0f) _fallVel = -rb.linearVelocity.y;
            rb.isKinematic = true;
        }

        _startScale = transform.localScale;
        _falling    = true;
        _spinVel    = (Random.value < 0.5f ? 1f : -1f) * Random.Range(40f, 140f);
        _spinAxis   = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        if (_spinAxis.sqrMagnitude < 0.01f) _spinAxis = Vector3.right;
        _spinAxis.Normalize();
        _scaleVel   = 0f;
    }

    void OnDestroy()
    {
        if (!IsConsumed && GameManager.Instance != null)
            GameManager.Instance.AllObjects.Remove(this);
    }
}
