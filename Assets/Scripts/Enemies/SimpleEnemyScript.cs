using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyState
{
    Idle,
    Action
}

[RequireComponent(typeof(Seeker))]
public class SimpleEnemyScript : MoveableCharacterScript
{
    public static List<MonoBehaviour> TargetScripts = new List<MonoBehaviour>();

    private EnemyState _enemyState = EnemyState.Idle;

    [SerializeField]
    private float _visionDistance = 3f;
    [SerializeField]
    private float _followDistance = 4f;
    [SerializeField]
    private float _damage = 50f;
    [SerializeField]
    private float _inflationWhenDamagedFactor = 0.5f;
    [SerializeField]
    private List<GameObject> _splashObjects = new List<GameObject>();

    private Transform _targetTransform;

    private Seeker _seeker;

    private Path _path;
    private float _nextWaypointDistance = 3;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath;

    protected override void Awake()
    {
        _seeker = GetComponent<Seeker>();

        base.Awake();
    }

    private void OnEnable()
    {
        MainScript.EnemiesList.Add(transform);
    }

    private void OnDisable()
    {
        MainScript.EnemiesList.Remove(transform);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;

            // Reset the waypoint counter so that we start to move towards the first point in the path
            _currentWaypoint = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player & Companion"))
        {
            collision.gameObject.GetComponent<MoveableCharacterScript>().GetHit(_damage);

            Die();
        }
    }

    protected override void Die()
    {
        if (_splashObjects.Count > 0)
        {
            int randomSplashNumber = Random.Range(0, _splashObjects.Count);

            GameObject splashObject = Instantiate<GameObject>(_splashObjects[randomSplashNumber]);
            splashObject.transform.position = new Vector3(transform.position.x, transform.position.y, splashObject.transform.position.z);
        }

        base.Die();
    }

    private void CheckForNearbyEnemies()
    {
        float minDistance = float.PositiveInfinity;

        foreach (MonoBehaviour targetScript in TargetScripts)
        {
            float distance = Vector2.Distance(transform.position, targetScript.transform.position);

            if (distance > _visionDistance)
            {
                continue;
            }

            if (MainScript.IsVisibilityBlocked(transform.position, targetScript.transform.position) == true)
            {
                continue;
            }

            if (distance < minDistance)
            {
                _targetTransform = targetScript.transform;
                _enemyState = EnemyState.Action;

                minDistance = distance;
            }
        }
    }

    private void FollowEnemy()
    {
        if (Vector2.Distance(transform.position, _targetTransform.position) > _followDistance)
        {
            _path = null;

            return;
        }

        _seeker.StartPath(transform.position, _targetTransform.position, OnPathComplete);
    }

    private void EnemyAI()
    {
        if (_enemyState == EnemyState.Idle)
        {
            CheckForNearbyEnemies();
        }
        else if (_enemyState == EnemyState.Action)
        {
            FollowEnemy();
            UpdateDirectionForMoving();
            Move(_movementDirection);
        }
    }

    private void UpdateDirectionForMoving()
    {
        if (_path == null)
        {
            _movementDirection = new Vector2();

            return;
        }

        _reachedEndOfPath = false;

        float distanceToWaypoint;

        while (true)
        {
            distanceToWaypoint = Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]);

            if (distanceToWaypoint < _nextWaypointDistance)
            {
                if (_currentWaypoint + 1 < _path.vectorPath.Count)
                {
                    _currentWaypoint++;
                }
                else
                {
                    _reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        float speedFactor = _reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / _nextWaypointDistance) : 1f;

        //_movementDirection = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;

        if (_path.vectorPath.Count > 1)
        {
            _movementDirection = (_path.vectorPath[1] - transform.position).normalized;
        }
        else
        {
            _movementDirection = (_path.vectorPath[0] - transform.position).normalized;
        }
    }

    private void UpdateScale()
    {
        transform.localScale = new Vector3(1f + ((_maxHealth - _health) * (_inflationWhenDamagedFactor / _maxHealth)), 1f + ((_maxHealth - _health) * (_inflationWhenDamagedFactor / _maxHealth)), 1f);
    }

    protected override void Update()
    {
        base.Update();

        EnemyAI();
        UpdateScale();
    }
}
