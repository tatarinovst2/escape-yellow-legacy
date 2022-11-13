using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class AppearingTextScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _textMesh;

    [SerializeField]
    private float _appearSpeed = 0.25f;
    [SerializeField]
    private float _opaqueTime = 5f;
    [SerializeField]
    private float _disappearSpeed = 0.5f;

    private bool _alreadyStarted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerScript>(out _))
        {
            if (_alreadyStarted == false)
            {
                _alreadyStarted = true;
                StartCoroutine(ShowTextCoroutine());
            }
        }
    }

    private IEnumerator ShowTextCoroutine()
    {
        while (_textMesh.color.a < 1f)
        {
            _textMesh.color = new Color(_textMesh.color.r, _textMesh.color.b, _textMesh.color.g, _textMesh.color.a + Time.deltaTime / _appearSpeed);

            yield return null;
        }

        yield return new WaitForSeconds(_opaqueTime);

        while (_textMesh.color.a > 0f)
        {
            _textMesh.color = new Color(_textMesh.color.r, _textMesh.color.b, _textMesh.color.g, _textMesh.color.a - Time.deltaTime / _disappearSpeed);

            yield return null;
        }

        Destroy(gameObject);
    }
}
