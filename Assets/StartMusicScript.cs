using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusicScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player & Companion"))
        {
            _audioSource.enabled = true;
        }
    }
}
