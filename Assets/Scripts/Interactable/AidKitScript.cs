using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidKitScript : TouchableObjectScript
{
    [SerializeField]
    private float _healthAmount = 60f;

    protected override void Action()
    {
        StartCoroutine(DestroyCoroutine());

        PlayerScript.Instance.Heal(_healthAmount);
        CompanionScript.Instance.Heal(_healthAmount);
    }
}
