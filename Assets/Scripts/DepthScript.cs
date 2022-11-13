using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthScript : MonoBehaviour
{
    private Transform _transform;

    [SerializeField]
    private float _extra = 0f;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        if (gameObject.isStatic == true)
        {
            _transform.position = new Vector3(_transform.position.x, _transform.position.y, _transform.position.y * 0.01f + _extra);

            enabled = false;
        }
    }

    private void Update()
    {
        _transform.position = new Vector3(_transform.position.x, _transform.position.y, _transform.position.y * 0.01f + _extra);
    }
}
