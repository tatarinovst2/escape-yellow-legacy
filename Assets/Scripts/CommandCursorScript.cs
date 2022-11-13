using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Kit.Controls;

public class CommandCursorScript : MonoBehaviour
{
    private Transform _transform;

    [SerializeField]
    private GameObject _commandCounter;

    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _slowMoTimeScale = 0.1f;

    [SerializeField]
    private GameObject _actionIconObject;
    private SpriteRenderer _actionIconRenderer;

    [SerializeField]
    private Sprite _defaultSprite;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _actionIconRenderer = _actionIconObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void UpdatePosition()
    {
        _transform.position = new Vector3(Camera.main.ScreenToWorldPoint(ControlsScript.MousePosition()).x,
            Camera.main.ScreenToWorldPoint(ControlsScript.MousePosition()).y, _transform.position.z);
    }

    private void CheckClickableObjects()
    {
        if (PlayerScript.Instance.AvailableCommandCount == 0)
        {
            return;
        }

        _actionIconRenderer.sprite = _defaultSprite;

        foreach (ClickableObjectScript clicableObjectScript in ClickableObjectScript.All)
        {
            if (clicableObjectScript.MousePositionInside() == true)
            {
                _actionIconRenderer.sprite = clicableObjectScript.IconSprite;

                if (ControlsScript.InGameControls.BindWithName("Command").Up)
                {
                    if (clicableObjectScript.TryGetComponent<CompanionScript>(out _))
                    {
                        return;
                    }

                    CompanionScript.Instance.Command(clicableObjectScript.CreateCommand());
                    PlayerScript.Instance.AvailableCommandCount -= 1;
                    return;
                }
            }
        }

        if (ControlsScript.InGameControls.BindWithName("Command").Up)
        {
            CompanionScript.Instance.Command(new Command(CompanionState.Hold, 10f, Camera.main.ScreenToWorldPoint(ControlsScript.MousePosition())));
            PlayerScript.Instance.AvailableCommandCount -= 1;
        }
    }

    private void Update()
    {
        if ((ControlsScript.InGameControls.BindWithName("Command").Hold == false) && (ControlsScript.InGameControls.BindWithName("Command").Up == false))
        {
            Time.timeScale = 1f;
            _spriteRenderer.enabled = false;
            _commandCounter.SetActive(false);
            _actionIconObject.SetActive(false);
            return;
        }

        _spriteRenderer.enabled = true;
        _commandCounter.SetActive(true);
        _actionIconObject.SetActive(true);
        Time.timeScale = _slowMoTimeScale;

        CheckClickableObjects();

        UpdatePosition();
    }
}
