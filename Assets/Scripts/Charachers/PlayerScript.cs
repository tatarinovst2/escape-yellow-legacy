using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;
using UnityEngine.SceneManagement;

public class PlayerScript : MoveableCharacterScript
{
    public static PlayerScript Instance = null;

    protected override void Awake()
    {
        Instance = this;

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

    protected override void Die()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void UpdateShieldPosition() // Depricated
    {
        /**
        return;
        Vector2 vectorToCursor = Vector2.ClampMagnitude(Camera.main.ScreenToWorldPoint(ControlsScript.MousePosition()) - transform.position, _shieldDistance);

        _shieldScript.transform.localPosition = vectorToCursor;
        _shieldScript.transform.up = vectorToCursor;
        **/
    }

    private int _availableCommandCount;
    public int AvailableCommandCount { get { return _availableCommandCount; } set { _availableCommandCount = value; } }
    [SerializeField]
    private int _maxCommandCount = 3;

    [SerializeField]
    private float _timeToGainCommand = 10f;

    private float _timeSinceGainedCommand = 0f;

    private void UpdateCommandCount()
    {
        if (_availableCommandCount == _maxCommandCount)
        {
            _timeSinceGainedCommand = 0f;
            return;
        }

        _timeSinceGainedCommand += Time.deltaTime;

        if (_timeSinceGainedCommand >= _timeToGainCommand)
        {
            _availableCommandCount += 1;
            _timeSinceGainedCommand = 0f;
        }
    }

    private void UpdateInputMovement()
    {
        _movementDirection = ControlsScript.InGameControls.StickWithName("Movement").Vector2;
    }

    protected override void Update()
    {
        base.Update();

        UpdateCommandCount();
        UpdateInputMovement();
        Move(_movementDirection);
    }
}
