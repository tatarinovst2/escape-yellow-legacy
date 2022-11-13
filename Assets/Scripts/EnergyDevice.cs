using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDevice : MonoBehaviour
{
    protected float _energy;
    [SerializeField]
    protected float _maxEnergy = 100f;

    public void TryReplenishEnergy(float amount)
    {
        if (_energy + amount > _maxEnergy)
        {
            _energy = _maxEnergy;
        }
        else
        {
            _energy += amount;
        }
    }
}
