using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnInfo
{
    [SerializeField]
    private GameObject _entityToSpawn;
    public GameObject EntityToSpawn { get { return _entityToSpawn; } }

    [SerializeField]
    private int _entityAmount;
    public int EntityAmount { get { return _entityAmount; } }
}

public class NestScript : MonoBehaviour
{
    [SerializeField]
    private float _health = 100f;
    [SerializeField]
    private List<SpawnInfo> _spawnInfos;
    [SerializeField]
    private float _explosionRadius = 1f;
    [SerializeField]
    private float _collidingDamage = 10f;

    public void GetHit(float damage)
    {
        _health -= damage;

        if (_health <= 0f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        foreach (SpawnInfo spawnInfo in _spawnInfos)
        {
            for (int i = 0; i < spawnInfo.EntityAmount; i++)
            {
                GameObject entityObject = Instantiate<GameObject>(spawnInfo.EntityToSpawn);

                entityObject.transform.position = new Vector3(transform.position.x + Random.Range(-_explosionRadius, _explosionRadius),
                        Random.Range(transform.position.y + -_explosionRadius, _explosionRadius), entityObject.transform.position.z);
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player & Companion"))
        {
            GetHit(_collidingDamage);
        }
    }
}
