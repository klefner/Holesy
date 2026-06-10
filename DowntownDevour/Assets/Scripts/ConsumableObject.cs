using UnityEngine;

public enum ObjectCategory { Person, Car, Tree, Building, Prop }

public class ConsumableObject : MonoBehaviour
{
    public float          Size       { get; private set; }
    public int            Tier       { get; private set; }
    public float          Value      { get; private set; }
    public ObjectCategory Category   { get; private set; }
    public bool           IsConsumed { get; private set; }

    private bool    _falling;
    private float   _spinVel;
    private float   _scaleVel;
    private Vector3 _startScale;
    private Vector3 _holePos;   // world-space hole center captured at consumption time

    const float SHRINK_TIME = 0.5f;

    public void Init(float size, int tier, float value, ObjectCategory category)
    {
        Size        = size;
        Tier        = tier;
        Value       = value;
        Category    = category;
        _startScale = transform.localScale;
    }

    void Update()
    {
        if (!_falling) return;

        _scaleVel += Time.deltaTime / SHRINK_TIME;
        float t = Mathf.Clamp01(_scaleVel);

        // Spin accelerates as the object gets sucked in
        transform.Rotate(Vector3.up, _spinVel * (1f + t * 3f) * Time.deltaTime, Space.World);

        // Pull toward the hole center and below ground (into the dark void)
        // Speed ramps from 8 → 40 u/s so objects visibly accelerate as they vanish
        Vector3 target = new Vector3(_holePos.x, -2f, _holePos.z);
        float   speed  = Mathf.Lerp(8f, 40f, t);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, t);

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
            collapse.Collapse(hole.transform.position);
            return;
        }

        // Stop physics so the script controls movement from here
        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        _holePos    = hole.transform.position;
        _holePos.y  = 0f;
        _startScale = transform.localScale;
        _falling    = true;
        _spinVel    = (Random.value < 0.5f ? 1f : -1f) * Random.Range(200f, 520f);
        _scaleVel   = 0f;
    }

    void OnDestroy()
    {
        if (!IsConsumed && GameManager.Instance != null)
            GameManager.Instance.AllObjects.Remove(this);
    }
}
