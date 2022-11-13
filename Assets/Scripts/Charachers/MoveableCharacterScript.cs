using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class MoveableCharacterScript : MonoBehaviour
{
    protected float _health;
    [SerializeField]
    protected float _maxHealth = 100f;

    protected Rigidbody2D _rigidBody2D;
    protected SpriteRenderer _spriteRenderer;
    protected Transform _transform;
    protected Animator _animator;
    [SerializeField]
    protected float _baseSpeed = 1f;
    protected float _speed;

    protected Vector2 _movementDirection = new Vector2();

    [Header("Optional")]
    [SerializeField]
    private InfoBarScript _healthBarScript;

    protected AudioSource _audioSource;

    protected virtual void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();

        if (TryGetComponent<AudioSource>(out var audioSource))
        {
            _audioSource = audioSource;
        }

        if (TryGetComponent<Animator>(out var animator))
        {
            _animator = animator;
        }

        _speed = _baseSpeed;
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    public virtual void GetHit(float _hitPower)
    {
        _health -= _hitPower;

        if (_health <= 0f)
        {
            Die();

            if ((this is PlayerScript) || (this is CompanionScript))
            {
                MainScript.Instance.GameOver();
            }
        }
    }

    public virtual void Heal(float _healthAmount)
    {
        if (_health + _healthAmount > _maxHealth)
        {
            _health = _maxHealth;
        }
        else
        {
            _health += _healthAmount;
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void Move(Vector2 movementDirection)
    {
        float multiplier = 1f;

        if (ZoneScript.CurrentZoneScript)
        {
            if (this is SimpleEnemyScript)
            {
                multiplier = ZoneScript.CurrentZoneScript.EnemiesSpeedMultiplier;
            }
            else
            {
                multiplier = ZoneScript.CurrentZoneScript.CharacterSpeedMultiplier;
            }
        }

        _rigidBody2D.velocity = _speed * multiplier * movementDirection;
    }

    protected virtual void UpdateAnimation()
    {
        if (_rigidBody2D.velocity.x > 0.1f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_rigidBody2D.velocity.x < -0.1f)
        {
            _spriteRenderer.flipX = true;
        }

        if (_animator == null)
        {
            if (_audioSource != null)
            {
                _audioSource.enabled = false;
            }
            
            return;
        }

        if (Mathf.Abs(_rigidBody2D.velocity.magnitude) > 0.1f)
        {
            _animator.Play("Move");

            if (_audioSource != null)
            {
                _audioSource.enabled = true;
            }
        }
        else
        {
            if (_audioSource != null)
            {
                _audioSource.enabled = false;
            }
            
            _animator.Play("Idle");
        }
    }

    protected void UpdateHealthBar()
    {
        if (_healthBarScript == null)
        {
            return;
        }

        _healthBarScript.UpdateValue(_health);
    }

    protected virtual void Update()
    {
        UpdateAnimation();
        UpdateHealthBar();
    }

    public void SetPosition(Vector2 vector2)
    {
        _transform.position = new Vector3(vector2.x, vector2.y, _transform.position.z);
    }
}
