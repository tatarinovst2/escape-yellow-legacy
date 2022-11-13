using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;
using Kit.Localization;

namespace Kit.Settings
{
	[CreateAssetMenu(menuName = "Kit/Settings/BindSetting")]
	public class BindSetting : Setting
	{
        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("BindName")]
        private string _bindName;

        public Bind Bind()
        {
            if (ControlsScript.InGameControls.Contains(_bindName))
            {
                return ControlsScript.InGameControls.BindWithName(_bindName);
            }
            else if (ControlsScript.UIControls.Contains(_bindName))
            {
                return ControlsScript.UIControls.BindWithName(_bindName);
            }

            Debug.LogError("No bind with name: " + _bindName);
            return null;
        }

        public override TextData CurrentValueTextData()
        {
            return new TextData(((KeyboardAndMouseBindKey)CurrentValue).ToString());
        }

        public override void LoadValue()
        {
            int enumValue = SettingsScript.Instance.SettingsCustomPrefs.GetInt(_bindName);

            if (enumValue >= System.Enum.GetValues(typeof(KeyboardAndMouseBindKey)).Length)
            {
                Debug.LogError("Incorrect load value for " + _bindName);
                return;
            }

            CurrentValue = enumValue;

            if (ControlsScript.InGameControls.Contains(_bindName))
            {
                ControlsScript.InGameControls.BindWithName(_bindName).SetKeyboardBind((KeyboardAndMouseBindKey)CurrentValue);
            }
            else
            {
                ControlsScript.UIControls.BindWithName(_bindName).SetKeyboardBind((KeyboardAndMouseBindKey)CurrentValue);
            }
        }

        public override int DefaultValue()
        {
            if (ControlsScript.InGameControls.Contains(_bindName))
            {
                return (int)ControlsScript.InGameControls.BindWithName(_bindName).KeyboardAndMouseBindKey;
            }
            else
            {
                return (int)ControlsScript.UIControls.BindWithName(_bindName).KeyboardAndMouseBindKey;
            }
        }

        public override void SaveValue(int enumValue)
        {
            CurrentValue = enumValue;
            SettingsScript.Instance.SettingsCustomPrefs.SetInt(_bindName, enumValue);
        }
    }
}
