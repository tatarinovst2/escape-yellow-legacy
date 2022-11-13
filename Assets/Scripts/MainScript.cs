using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    public static MainScript Instance = null;

    private EnergyDevice _currentEnergyDevice = null;
    public EnergyDevice CurrentEnergyDevice { get { return _currentEnergyDevice; } set { _currentEnergyDevice = value; } }

    public static List<Transform> EnemiesList = new List<Transform>();

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _spriteRenderer.gameObject.SetActive(true);
        StartCoroutine(RevealCoroutine());
    }

    private IEnumerator RevealCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f);

        while (_spriteRenderer.color.a > 0f)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a - Time.deltaTime);
            yield return null;
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public static bool IsVisibilityBlocked(Vector2 position1, Vector2 position2)
    {
        /*
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        */

        int layerWalls = 6;

        int layerMask = 1 << layerWalls;

        RaycastHit2D hit = Physics2D.Raycast(position1,
                          position2 - position1,
                          Vector2.Distance(position1, position2), layerMask);

        if (hit.collider == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
