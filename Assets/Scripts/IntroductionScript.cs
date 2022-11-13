using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Kit.Controls;

public class IntroductionScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textMeshPro;

    [SerializeField]
    private float _fadeInSpeed;

    [SerializeField]
    private float _fadeOutSpeed;

    [SerializeField]
    private string _sceneToLoad;

    private void Start()
    {
        StartCoroutine(FadeInAndOutCoroutine());
    }

    private IEnumerator FadeInAndOutCoroutine()
    {
        yield return new WaitForSeconds(2f);

        while (_textMeshPro.color.a < 1f)
        {
            _textMeshPro.color = new Color(_textMeshPro.color.r, _textMeshPro.color.g, _textMeshPro.color.b, _textMeshPro.color.a + Time.deltaTime * _fadeInSpeed);

            yield return null;
        }

        yield return new WaitForSeconds(15f);

        while (_textMeshPro.alpha > 0f)
        {
            _textMeshPro.color = new Color(_textMeshPro.color.r, _textMeshPro.color.g, _textMeshPro.color.b, _textMeshPro.color.a - Time.deltaTime * _fadeOutSpeed);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(_sceneToLoad);
    }

    private void Update()
    {
        if (ControlsScript.UIControls.BindWithName("Proceed").Down)
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
