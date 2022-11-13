using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShuttleScript : TouchableObjectScript
{
    [SerializeField]
    private SpriteRenderer _blackRenderer;

    protected override void Action()
    {
        StartCoroutine(LoadEndingCoroutine());
    }

    private IEnumerator LoadEndingCoroutine()
    {
        while (_blackRenderer.color.a < 1f)
        {
            _blackRenderer.color = new Color(_blackRenderer.color.r, _blackRenderer.color.g, _blackRenderer.color.b, _blackRenderer.color.a + Time.deltaTime);

            yield return null;
        }

        SceneManager.LoadScene("EndingScene");
    }
}
