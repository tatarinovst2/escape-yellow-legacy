using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;

[RequireComponent(typeof(SpriteRenderer))]
public class ClickableObjectScript : MonoBehaviour
{
    public static List<ClickableObjectScript> All = new List<ClickableObjectScript>();

    protected SpriteRenderer _spriteRenderer;
    protected Transform _transform;

    [SerializeField]
    protected float _extraClickingArea = 0.05f;

    [Header("Command")]
    [SerializeField]
    protected CompanionState _companionState;
    [SerializeField]
    protected float _duration;
    [SerializeField]
    protected Transform _commandTransform;
    [SerializeField]
    protected Sprite _iconSprite;
    public Sprite IconSprite { get { return _iconSprite; } }

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _transform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        All.Add(this);
    }

    private void OnDisable()
    {
        All.Remove(this);
    }

    public bool MousePositionInside()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(ControlsScript.MousePosition());

        if (mousePosition.x > _transform.position.x + _spriteRenderer.bounds.extents.x + _extraClickingArea)
        {
            return false;
        }

        if (mousePosition.x < _transform.position.x - _spriteRenderer.bounds.extents.x - _extraClickingArea)
        {
            return false;
        }

        if (mousePosition.y > _transform.position.y + _spriteRenderer.bounds.extents.y + _extraClickingArea)
        {
            return false;
        }

        if (mousePosition.y < _transform.position.y - _spriteRenderer.bounds.extents.y - _extraClickingArea)
        {
            return false;
        }

        return true;
    }

    public virtual Command CreateCommand()
    {
        return new Command(_companionState, _duration, _commandTransform.position);
    }
}
