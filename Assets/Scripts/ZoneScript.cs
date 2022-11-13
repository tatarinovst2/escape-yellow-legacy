using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ZoneScript : MonoBehaviour
{
    public static ZoneScript CurrentZoneScript = null;

    private bool _alreadybeenCurrent = false;

    [SerializeField]
    private float _characterSpeedMultiplier = 1f;
    public float CharacterSpeedMultiplier { get { return _characterSpeedMultiplier; } }
    [SerializeField]
    private float _enemiesSpeedMultiplier = 1f;
    public float EnemiesSpeedMultiplier { get { return _enemiesSpeedMultiplier; } }

    [SerializeField]
    private Transform _exitPoint;
    public Transform ExitPoint { get { return _exitPoint; } }
    [SerializeField]
    private DoorScript _doorScript;
    public DoorScript DoorScript { get { return _doorScript; } }
    [SerializeField]
    private List<Transform> _scoutPoints = new List<Transform>();
    public List<Transform> ScoutPoints { get { return _scoutPoints; } }

    private List<Transform> _usedPoints = new List<Transform>();
    public List<Transform> UsedPoints { get { return _usedPoints; } }

    private List<GameObject> _enemyObjects = new List<GameObject>();

    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void UsePoint(Transform transform)
    {
        if (_usedPoints.Contains(transform))
        {
            return;
        }

        _usedPoints.Add(transform);
    }

    public List<Transform> UnusedPoints()
    {
        List<Transform> unusedPoints = new List<Transform>(_scoutPoints);
        unusedPoints.RemoveAll(item => _usedPoints.Contains(item));

        return unusedPoints;
    }

    public Transform ClosestUnusedPoint(Vector2 vector2)
    {
        List<Transform> unusedPoints = UnusedPoints();

        float minDistance = float.PositiveInfinity;
        Transform closestTransform = null;

        foreach (Transform transform in unusedPoints)
        {
            float distance = Vector2.Distance(transform.position, vector2);

            if (distance < minDistance)
            {
                closestTransform = transform;
                minDistance = distance;
            }
        }

        return closestTransform;
    }

    public Vector2 RandomPointInside()
    {
        return (Vector2)_collider.bounds.center +
            new Vector2(Random.Range(-_collider.bounds.extents.x, _collider.bounds.extents.x), Random.Range(-_collider.bounds.extents.y, _collider.bounds.extents.y));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_alreadybeenCurrent == true)
        {
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player & Companion"))
        {
            CurrentZoneScript = this;
            _alreadybeenCurrent = true;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            _enemyObjects.Add(collision.gameObject);
        }
    }

    private void Update()
    {
        for (int i = _enemyObjects.Count - 1; i >= 0; i--)
        {
            if (_enemyObjects[i] == null)
            {
                _enemyObjects.RemoveAt(i);
            }
        }
    }
}
