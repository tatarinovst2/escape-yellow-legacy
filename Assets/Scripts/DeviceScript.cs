using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;

public class DeviceScript : EnergyDevice
{
    [SerializeField]
    private GameObject _shieldObject;
    public GameObject ShieldObject { get { return _shieldObject; } }
    [SerializeField]
    private float _shieldDistance = 0.2f;

    private bool _isFiring = false;
    private AudioSource _audioSource;

    //private float _reloadSpeed = 0.5f;
    //private float _timeTillReload;

    [SerializeField]
    private float _energyUsedPerSecond = 20f;
    [SerializeField]
    private float _alwaysReplenishingEnergyLimit = 40f;
    [SerializeField]
    private float _alwaysReplenishSpeedPerSecond = 10f;
    [SerializeField]
    private float _timeoutTillReplenish = 2f;

    [SerializeField]
    private int _projectilesFiredPerSecond = 50;

    [Header("Optional")]
    [SerializeField]
    private InfoBarScript _energyBarScript;

    private float _timeSinceLastProjectile = 0f;
    private float _timeSinceLastProjectileIfShooting = 0f;

    private GameObject _projectileObject;

    private Sprite _horizontalShield;
    private Sprite _diagonalShield;
    private Sprite _verticalShield;
    private SpriteRenderer _shieldRenderer;

    private void Awake()
    {
        _energy = _maxEnergy;
        //_timeTillReload = _reloadSpeed;

        _projectileObject = Resources.Load<GameObject>("PREFABS/Projectile");

        _shieldRenderer = _shieldObject.GetComponent<SpriteRenderer>();
        _horizontalShield = Resources.Load<Sprite>("Weapons/Shield");
        _diagonalShield = Resources.Load<Sprite>("Weapons/Shield Diagonal");
        _verticalShield = Resources.Load<Sprite>("Weapons/Shield Vertical");

        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // TEMP

        MainScript.Instance.CurrentEnergyDevice = this;
    }

    private void ReplenishDefaultEnergy()
    {
        if (_timeSinceLastProjectile < _timeoutTillReplenish)
        {
            return;
        }

        if (_energy > _alwaysReplenishingEnergyLimit)
        {
            return;
        }

        float energyWithAddedReplenishingEnergy = _energy + _alwaysReplenishSpeedPerSecond * Time.deltaTime;

        if (energyWithAddedReplenishingEnergy > _alwaysReplenishingEnergyLimit)
        {
            _energy = _alwaysReplenishingEnergyLimit;
        }
        else
        {
            _energy = energyWithAddedReplenishingEnergy;
        }
    }

    private void UpdateShooting()
    {
        _timeSinceLastProjectile += Time.deltaTime;

        if (_isFiring == false)
        {
            _audioSource.enabled = false;
            return;
        }

        _audioSource.enabled = true;
        _energy -= _energyUsedPerSecond * Time.deltaTime;
        _timeSinceLastProjectileIfShooting += Time.deltaTime;

        while (_timeSinceLastProjectileIfShooting > (1f / _projectilesFiredPerSecond))
        {
            float randomMultiplier = Random.value;

            GameObject projectileObject = Instantiate<GameObject>(_projectileObject);

            projectileObject.transform.position = transform.position + ((_shieldObject.transform.position - transform.position) * randomMultiplier);

            _timeSinceLastProjectileIfShooting -= (1f / _projectilesFiredPerSecond);
            _timeSinceLastProjectile = 0f;
        }


        return;

        /**
        _timeTillReload -= Time.deltaTime;

        if (_timeTillReload <= 0f)
        {
            Fire();
            _timeTillReload = _reloadSpeed;
        }
        **/
    }

    private void Fire()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject projectileObject = Instantiate<GameObject>(_projectileObject);

            projectileObject.transform.position = transform.position + ((_shieldObject.transform.position - transform.position) * ((i + 1f) / 21f));
        }

        //projectileObject.transform.position = transform.position + (Vector3)Vector2.ClampMagnitude(_shieldScript.transform.position - transform.position, 0.2f);
    }

    private void UpdatePosition()
    {
        // Device

        transform.position = CompanionScript.Instance.transform.position;

        // Shield

        Vector2 vectorToDevice = (CompanionScript.Instance.transform.position - PlayerScript.Instance.transform.position).normalized;

        _shieldObject.transform.position = new Vector3(PlayerScript.Instance.transform.position.x + vectorToDevice.x * _shieldDistance,
            PlayerScript.Instance.transform.position.y + vectorToDevice.y * _shieldDistance, _shieldObject.transform.position.z);

        return;
        if (vectorToDevice.x > 0f)
        {
            _shieldObject.transform.position = new Vector3(_shieldDistance + PlayerScript.Instance.transform.position.x,
                vectorToDevice.y * 0.2f + PlayerScript.Instance.transform.position.y, _shieldObject.transform.position.z);
        }
        else if (vectorToDevice.x < 0f)
        {
            _shieldObject.transform.position = new Vector3(-_shieldDistance + PlayerScript.Instance.transform.position.x,
                vectorToDevice.y * 0.2f + PlayerScript.Instance.transform.position.y, _shieldObject.transform.position.z);
        }
    }

    private void UpdateSprites()
    {
        // Shield

        Vector2 vectorToDevice = (CompanionScript.Instance.transform.position - PlayerScript.Instance.transform.position).normalized;

        if (vectorToDevice.x > 0f)
        {
            _shieldRenderer.flipX = false;
        }
        else if (vectorToDevice.x < 0f)
        {
            _shieldRenderer.flipX = true;
        }

        if (vectorToDevice.y > 0.75f)
        {
            _shieldRenderer.sprite = _verticalShield;
            _shieldRenderer.flipY = false;
        }
        else if (vectorToDevice.y < -0.75f)
        {
            _shieldRenderer.sprite = _verticalShield;
            _shieldRenderer.flipY = true;
        }
        else if (vectorToDevice.y > 0.25f)
        {
            _shieldRenderer.sprite = _diagonalShield;
            _shieldRenderer.flipY = false;
        }
        else if (vectorToDevice.y < -0.25f)
        {
            _shieldRenderer.sprite = _diagonalShield;
            _shieldRenderer.flipY = true;
        }
        else
        {
            _shieldRenderer.sprite = _horizontalShield;
            _shieldRenderer.flipY = false;
        }
    }

    protected void UpdateEnergyBar()
    {
        if (_energyBarScript == null)
        {
            return;
        }

        _energyBarScript.UpdateValue(_energy);
    }

    private void Update()
    {
        UpdatePosition();
        UpdateSprites();

        if ((ControlsScript.InGameControls.BindWithName("Shoot").Hold) && (_energy > 0f))
        {
            _isFiring = true;
        }
        else
        {
            _isFiring = false;
        }

        UpdateShooting();
        UpdateEnergyBar();
        ReplenishDefaultEnergy();
    }
}
