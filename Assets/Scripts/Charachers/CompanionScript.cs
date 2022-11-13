using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Command
{
    private CompanionState _companionState;
    public CompanionState CompanionState { get { return _companionState; } }
    private float _timeLeftToObey = 10f;
    public float TimeLeftToObey { get { return _timeLeftToObey; } set { _timeLeftToObey = value; } }
    private Vector2 _targetPosition;
    public Vector2 TargetPosition { get { return _targetPosition; } }

    public Command(CompanionState companionState, float timeLeftToObey, Vector2 targetPosition)
    {
        _companionState = companionState;
        _timeLeftToObey = timeLeftToObey;
        _targetPosition = targetPosition;
    }
}

public enum CompanionState
{
    Hold,
    Flee,
    GoToPoint,
    Scout,
    OpeningDoor,
    ReturnToPlayer
}

public class CompanionScript : MoveableCharacterScript
{
    public static CompanionScript Instance = null;

    private CompanionState _companionState = CompanionState.Hold;

    private Vector2 _targetPosition = new Vector2();
    List<Command> _commands = new List<Command>();

    private Command _selfCommand = null;

    [SerializeField]
    private float _fleeThreshhold = 1f;
    private List<Transform> _enemiesInsideFleeThreshold = new List<Transform>();

    private List<BatteryScript> _knownBatteryScripts = new List<BatteryScript>();
    private List<AidKitScript> _knownAidKitScripts = new List<AidKitScript>();

    [SerializeField]
    private float _returnToPlayerThreshold = 5f;

    [SerializeField]
    private float _mapSuppliesThreshold = 5f;
    [SerializeField]
    private float _forgetSuppliesThreshold = 8f;

    protected override void Awake()
    {
        Instance = this;

        _seeker = GetComponent<Seeker>();

        base.Awake();
    }

    private void OnEnable()
    {
        SimpleEnemyScript.TargetScripts.Add(this);
    }

    private void OnDisable()
    {
        SimpleEnemyScript.TargetScripts.Remove(this);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;

            _currentWaypoint = 0;

            _targetPosition = _path.vectorPath[_path.vectorPath.Count - 1];
        }
    }

    // Commands

    public void Command(Command command)
    {
        _commands.Add(command);
    }

    // AI

    protected override void Die()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void UpdateState()
    {
        // Check if there is danger nearby

        _enemiesInsideFleeThreshold = new List<Transform>();

        foreach (Transform enemyTransform in MainScript.EnemiesList)
        {
            float distance = Vector2.Distance(transform.position, enemyTransform.position);

            if ((distance < _fleeThreshhold) && (MainScript.IsVisibilityBlocked(transform.position, enemyTransform.position) == false))
            {
                _enemiesInsideFleeThreshold.Add(enemyTransform);
            }
        }

        CheckForSupplies();

        //

        if (_enemiesInsideFleeThreshold.Count > 0)
        {
            _companionState = CompanionState.Flee;
            return;
        }

        Command command = null;

        if (_commands.Count > 0)
        {
            command = _commands[_commands.Count - 1];
        }
        else if (_selfCommand != null)
        {
            command = _selfCommand;
        }

        if (command != null)
        {
            _targetPosition = command.TargetPosition;
            _companionState = command.CompanionState;

            if (command.CompanionState == CompanionState.Hold)
            {
                command.TimeLeftToObey -= Time.deltaTime;

                if (command.TimeLeftToObey <= 0f)
                {
                    NextCommand();
                    return;
                }
            }
        }
        else if (Vector2.Distance(PlayerScript.Instance.transform.position, transform.position) > _returnToPlayerThreshold)
        {
            _companionState = CompanionState.ReturnToPlayer;
        }
        else if (ZoneScript.CurrentZoneScript.UnusedPoints().Count > 0)
        {
            _companionState = CompanionState.Scout;
        }
        else
        {
            _companionState = CompanionState.Hold;
        }
    }

    private void CheckForSupplies()
    {
        // Add new

        foreach (TouchableObjectScript touchableObjectScript in TouchableObjectScript.All)
        {
            if (Vector2.Distance(touchableObjectScript.transform.position, _transform.position) < _mapSuppliesThreshold)
            {
                if (MainScript.IsVisibilityBlocked(touchableObjectScript.transform.position, _transform.position) == false)
                {
                    TryAddToKnownSupplies(touchableObjectScript);
                }
            }
        }

        // Remove old ones

        for (int i = _knownBatteryScripts.Count - 1; i >= 0; i--)
        {
            if (_knownBatteryScripts[i] == null)
            {
                _knownBatteryScripts.RemoveAt(i);
                continue;
            }

            if (Vector2.Distance(_knownBatteryScripts[i].transform.position, _transform.position) > _forgetSuppliesThreshold)
            {
                _knownBatteryScripts.Remove(_knownBatteryScripts[i]);
            }
        }

        for (int i = _knownAidKitScripts.Count - 1; i >= 0; i--)
        {
            if (_knownAidKitScripts[i] == null)
            {
                _knownAidKitScripts.RemoveAt(i);
                continue;
            }

            if (Vector2.Distance(_knownAidKitScripts[i].transform.position, _transform.position) > _forgetSuppliesThreshold)
            {
                _knownAidKitScripts.Remove(_knownAidKitScripts[i]);
            }
        }
    }

    private void TryAddToKnownSupplies(TouchableObjectScript touchableObjectScript)
    {
        if (touchableObjectScript is BatteryScript)
        {
            if (_knownBatteryScripts.Contains((BatteryScript)touchableObjectScript) == false)
            {
                _knownBatteryScripts.Add((BatteryScript)touchableObjectScript);
            }
        }
        else if (touchableObjectScript is AidKitScript)
        {
            if (_knownAidKitScripts.Contains((AidKitScript)touchableObjectScript) == false)
            {
                _knownAidKitScripts.Add((AidKitScript)touchableObjectScript);
            }
        }
    }

    private void CompanionAI()
    {
        UpdateState();

        if (_companionState == CompanionState.Hold)
        {
            Hold();
        }
        else if (_companionState == CompanionState.Flee)
        {
            Flee();
        }
        else if (_companionState == CompanionState.Scout)
        {
            Scout();
        }
        else if (_companionState == CompanionState.GoToPoint)
        {
            GoToPoint();
        }
        else if (_companionState == CompanionState.ReturnToPlayer)
        {
            ReturnToPlayer();
        }
        else if (_companionState == CompanionState.OpeningDoor)
        {
            OpeningDoor();
        }
    }

    private void Hold()
    {
        if (Vector2.Distance(_transform.position, _targetPosition) < 0.05f)
        {
            _movementDirection = new Vector2();
            return;
        }

        _seeker.StartPath(transform.position, _targetPosition, OnPathComplete);

        UpdateDirectionForMoving();
    }

    private void Flee()
    {
        if (_enemiesInsideFleeThreshold.Count == 0)
        {
            Debug.LogError("No enemies to flee from");
            return;
        }

        List<float> anglesToEnemiesOrWalls = new List<float>();

        foreach (Transform enemyTransform in _enemiesInsideFleeThreshold)
        {
            if (enemyTransform == null)
            {
                continue;
            }

            anglesToEnemiesOrWalls.Add(Vector2.SignedAngle(enemyTransform.position - transform.position, Vector2.right));
        }

        anglesToEnemiesOrWalls.Sort();

        anglesToEnemiesOrWalls.Add(anglesToEnemiesOrWalls[0]);

        float firstAngle = 0f;
        float maxAngularDistance = 0f;

        for (int i = 0; i < anglesToEnemiesOrWalls.Count - 1; i++)
        {
            float angularDistance = Mathf.DeltaAngle(anglesToEnemiesOrWalls[i], anglesToEnemiesOrWalls[i + 1]);

            if (angularDistance < 0f)
            {
                angularDistance += 360f;
            }

            if (angularDistance > maxAngularDistance)
            {
                maxAngularDistance = angularDistance;
                firstAngle = anglesToEnemiesOrWalls[i];
            }
        }

        float angle = firstAngle + (maxAngularDistance / 2f);

        _movementDirection = Vector2.ClampMagnitude(new Vector2((float)Mathf.Cos(angle * Mathf.PI / 180), (float)Mathf.Sin(angle * Mathf.PI / 180)), 1f);
    }

    private void Scout()
    {
        if (ZoneScript.CurrentZoneScript.UnusedPoints().Count == 0)
        {
            Debug.LogError("No unsued points!");
            _movementDirection = new Vector2();
            return;
        }

        if (_targetPosition == new Vector2())
        {
            _targetPosition = ZoneScript.CurrentZoneScript.ClosestUnusedPoint(_transform.position).position;
        }
        else if (Vector2.Distance(_transform.position, _targetPosition) < 0.1f)
        {
            for (int i = 0; i < ZoneScript.CurrentZoneScript.ScoutPoints.Count; i++)
            {
                if (Vector2.Distance(ZoneScript.CurrentZoneScript.ScoutPoints[i].position, _transform.position) < 0.2f)
                {
                    ZoneScript.CurrentZoneScript.UsePoint(ZoneScript.CurrentZoneScript.ScoutPoints[i]);

                    if (ZoneScript.CurrentZoneScript.UnusedPoints().Count == 0)
                    {
                        _movementDirection = new Vector2();
                        return;
                    }

                    _targetPosition = ZoneScript.CurrentZoneScript.ClosestUnusedPoint(_transform.position).position;
                }
            }
        }

        _seeker.StartPath(transform.position, _targetPosition, OnPathComplete);

        UpdateDirectionForMoving();
    }

    private void GoToPoint()
    {
        if (Vector2.Distance(_transform.position, _targetPosition) < 0.05f)
        {
            NextCommand();
            return;
        }

        _seeker.StartPath(transform.position, _targetPosition, OnPathComplete);

        UpdateDirectionForMoving();
    }

    private void ReturnToPlayer()
    {
        _targetPosition = PlayerScript.Instance.transform.position;

        _seeker.StartPath(transform.position, _targetPosition, OnPathComplete);

        UpdateDirectionForMoving();
    }

    private void OpeningDoor()
    {
        if (Vector2.Distance(_transform.position, _targetPosition) > 0.05f)
        {
            _seeker.StartPath(transform.position, _targetPosition, OnPathComplete);
            
            UpdateDirectionForMoving();
            return;
        }

        ZoneScript.CurrentZoneScript.DoorScript.TryOpen();

        _movementDirection = new Vector2();
        _commands = new List<Command>();

        if (ZoneScript.CurrentZoneScript.ExitPoint)
        {
            _selfCommand = new Command(CompanionState.GoToPoint, 0f, ZoneScript.CurrentZoneScript.ExitPoint.position);
        }
        else
        {
            _companionState = CompanionState.Scout;
        }
    }

    private void NextCommand()
    {
        if ((_commands.Count == 0) && (_selfCommand != null))
        {
            _selfCommand = null;
            _targetPosition = new Vector2();
        }

        if (_commands.Count > 0)
        {
            _commands.RemoveAt(_commands.Count - 1);
        }
    }

    private Seeker _seeker;

    private Path _path;
    private float _nextWaypointDistance = 3;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath;

    private void UpdateDirectionForMoving()
    {
        if (_path == null)
        {
            _movementDirection = new Vector2();

            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        _reachedEndOfPath = false;

        // The distance to the next waypoint in the path
        float distanceToWaypoint;

        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]);

            if (distanceToWaypoint < _nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (_currentWaypoint + 1 < _path.vectorPath.Count)
                {
                    _currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    _reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        float speedFactor = _reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / _nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
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

    protected override void Update()
    {
        base.Update();

        CompanionAI();
        Move(_movementDirection);
    }
}
