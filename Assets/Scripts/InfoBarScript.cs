using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBarScript : MonoBehaviour
{
    [SerializeField]
    private float _maxValue = 100f;
    private float _value;

    [SerializeField]
    private GameObject _bar;
    private SpriteRenderer _barRenderer;

    private void Awake()
    {
        _barRenderer = _bar.GetComponent<SpriteRenderer>();
    }

    public void UpdateValue(float value)
    {
        _value = value;
    }

    private void Update()
    {
        _bar.transform.localScale = new Vector3(_value / _maxValue, 1f, 1f);

        _barRenderer.color = Color.HSVToRGB(0.3f * (_value / _maxValue), 1f, 1f);
    }
}
