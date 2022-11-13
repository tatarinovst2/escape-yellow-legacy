using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class TouchableObjectScript : MonoBehaviour
{
    public static List<TouchableObjectScript> All = new List<TouchableObjectScript>();

    private void OnEnable()
    {
        All.Add(this);
    }

    private void OnDisable()
    {
        All.Remove(this);
    }

    protected SpriteRenderer _spriteRenderer;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player & Companion"))
        {
            Action();
        }
    }

    protected IEnumerator DestroyCoroutine()
    {
        while (true)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a - Time.deltaTime);

            if (_spriteRenderer.color.a <= 0f)
            {
                break;
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    private float _maxSpeed = 0.2f;
    private float _speed = 0f;
    private float _jumpFactor = 0.75f;
    private bool _positiveDirection = true;

    private void Update()
    {
        if (_speed > _maxSpeed)
        {
            _positiveDirection = false;
        }
        else if (_speed < -_maxSpeed)
        {
            _positiveDirection = true;
        }

        if (_positiveDirection)
        {
            _speed += _jumpFactor * Time.deltaTime;
        }
        else
        {
            _speed -= _jumpFactor * Time.deltaTime;
        }

        transform.localPosition += new Vector3(0, _speed * Time.deltaTime, 0);
    }

    protected virtual void Action()
    {

    }
}
