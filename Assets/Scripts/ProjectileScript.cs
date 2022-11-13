using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
//[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ProjectileScript : MonoBehaviour
{
    //private int _collisionsCount = 0;
    //[SerializeField]
    //private int _maxCollisions = 2;

    

    //private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;

    //[SerializeField]
    //private float _energy = 10f;

    [SerializeField]
    private float timeToExist = 0.5f;
    [SerializeField]
    private float _hitPower = 20f;

    private void Awake()
    {
        //_rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(FadeOutAndDestroyCoroutine());
    }

    private IEnumerator FadeOutAndDestroyCoroutine()
    {
        while (true)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.b, _spriteRenderer.color.g, _spriteRenderer.color.a - (Time.deltaTime / timeToExist));

            if (_spriteRenderer.color.a - (Time.deltaTime / timeToExist) < 0f)
            {
                Destroy(gameObject);
                break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (collision.gameObject.TryGetComponent<MoveableCharacterScript>(out var moveableCharacterScript))
            {
                moveableCharacterScript.GetHit(_hitPower);
            }
            else
            {
                Debug.LogError("Enemy not on Enemies layer!");
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Nests"))
        {
            if (collision.gameObject.TryGetComponent<NestScript>(out var nestScript))
            {
                nestScript.GetHit(_hitPower);
            }
        }

        return;
        /**
        _collisionsCount += 1;

        if (collision.gameObject.TryGetComponent<MoveableCharacterScript>(out var moveableCharacterScript))
        {
            moveableCharacterScript.GetHit(_hitPower);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Device"))
        {
            if (collision.gameObject.TryGetComponent<DeviceScript>(out var deviceScript))
            {
                deviceScript.ReplenishEnergy(_energy);
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Mirror"))
        {
            return;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Vector2 normal = collision.GetContact(0).normal;
            Vector2 velocity =_rigidBody.velocity;

            if ((Vector2.Angle(velocity, normal) > 60f) && (_collisionsCount < _maxCollisions))
            {
                return;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        Destroy(gameObject);
        **/
    }
}
