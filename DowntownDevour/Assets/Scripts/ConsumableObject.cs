using UnityEngine;

public enum ObjectCategory { Person, Car, Tree, Building, Prop }

public class ConsumableObject : MonoBehaviour
{
    public float          Size           { get; private set; }
    public int            Tier           { get; private set; }
    public float          Value          { get; private set; }
    public ObjectCategory Category       { get; private set; }
    public bool           IsConsumed     { get; private set; }
    public float          FootprintRadius { get; private set; }

    private bool      _falling;
    private float     _spinVel;
    private float     _scaleVel;
    private float     _fallVel;
    private Vector3   _startScale;
    private Transform _holeTransform;

    const float FALL_GRAVITY = 7f;
    const float SHRINK_TIME  = 0.85f;

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

        // Keep debris under the hole as the hole moves so it stays inside the dark void
        if (_holeTransform != null)
        {
            var p  = transform.position;
            var hp = _holeTransform.position;
            p.x = hp.x;
            p.z = hp.z;
            transform.position = p;
        }

        transform.Rotate(Vector3.up, _spinVel * Time.deltaTime, Space.World);

        _fallVel           += FALL_GRAVITY * Time.deltaTime;
        transform.position += Vector3.down * (_fallVel * Time.deltaTime);

        _scaleVel            += Time.deltaTime / SHRINK_TIME;
        float t               = Mathf.Clamp01(_scaleVel);
        transform.localScale  = Vector3.Lerp(_startScale, Vector3.zero, t);

        if (t >= 1f) Destroy(gameObject);
    }

    public void MarkConsumed(HoleBase hole)
    {
        if (IsConsumed) return;
        IsConsumed = true;
        GameManager.Instance.AllObjects.Remove(this);

        var collapse = GetComponent<BuildingCollapse>();
        if (collapse != null)
        {
            collapse.Collapse(hole.transform.position, hole.Radius);
            return;
        }

        _holeTransform = hole.transform;

        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        _startScale = transform.localScale;
        _falling    = true;
        _fallVel    = 0f;
        _spinVel    = (Random.value < 0.5f ? 1f : -1f) * Random.Range(90f, 260f);
        _scaleVel   = 0f;
    }

    void OnDestroy()
    {
        if (!IsConsumed && GameManager.Instance != null)
            GameManager.Instance.AllObjects.Remove(this);
    }
}
