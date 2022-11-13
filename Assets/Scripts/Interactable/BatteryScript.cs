using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryScript : TouchableObjectScript
{
    [SerializeField]
    private float _energyAmount = 60f;

    protected override void Action()
    {
        StartCoroutine(DestroyCoroutine());

        MainScript.Instance.CurrentEnergyDevice.TryReplenishEnergy(_energyAmount);
    }
}
