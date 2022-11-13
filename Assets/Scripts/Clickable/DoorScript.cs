using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : ClickableObjectScript
{
    [SerializeField]
    private Vector2 _openingVector;
    [SerializeField]
    private float _openingDuration = 1f;

    private float _timeLeftToOpen;

    private bool _isOpened = false;
    public bool IsOpened { get { return _isOpened; } }

    private bool _startedOpening = false;

    [SerializeField]
    private GameObject _collderObject;

    public void TryOpen()
    {
        if (_startedOpening == true)
        {
            return;
        }

        _startedOpening = true;
        _collderObject.SetActive(false);

         _timeLeftToOpen = _openingDuration;

        if (TryGetComponent<DepthScript>(out var depthScript))
        {
            depthScript.enabled = false;
        }

        StartCoroutine(OpenCoroutine());
    }

    private IEnumerator OpenCoroutine()
    {
        while (_timeLeftToOpen > 0f)
        {
            _transform.position += (Vector3)_openingVector * Time.deltaTime;

            _timeLeftToOpen -= Time.deltaTime;

            yield return null;
        }

        _isOpened = true;

        AstarPath.active.Scan();
    }
}
