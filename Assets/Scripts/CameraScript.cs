using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private float interpolationFactor = 5f;

    private void Update()
    {
        Vector2 interpolatedPosition = Vector2.Lerp(gameObject.transform.position, PlayerScript.Instance.transform.position, interpolationFactor * Time.deltaTime);

        gameObject.transform.position = new Vector3(interpolatedPosition.x, interpolatedPosition.y, gameObject.transform.position.z);
    }
}
