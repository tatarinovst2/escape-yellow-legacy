using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandCountScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _commandCounterText;
    [SerializeField]
    private TextMeshProUGUI _commandCounterTextUI;

    private void Update()
    {
        if (_commandCounterText)
        {
            _commandCounterText.text = PlayerScript.Instance.AvailableCommandCount.ToString();
        }
        else if (_commandCounterTextUI)
        {
            _commandCounterTextUI.text = PlayerScript.Instance.AvailableCommandCount.ToString();
        }
    }
}
