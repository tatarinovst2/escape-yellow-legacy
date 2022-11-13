using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevicePickUpScript : TouchableObjectScript
{
    [SerializeField]
    private CompanionScript _companionScript;

    [SerializeField]
    private DeviceScript _deviceScript;

    [SerializeField]
    private GameObject _shieldObject;

    protected override void Action()
    {
        StartCoroutine(DestroyCoroutine());

        _companionScript.enabled = true;
        _deviceScript.gameObject.SetActive(true);
        _shieldObject.SetActive(true);
    }
}
