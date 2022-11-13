using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Controls;
using Kit.Localization;

namespace Kit.Settings
{
    [CreateAssetMenu(menuName = "Kit/Settings/LayoutSetting")]
    public class LayoutSetting : ChoosableSetting
    {
        public override TextData CurrentValueTextData()
        {
            return new TextData(((ControllerLayout)CurrentValue).ToString());
        }

        public override void LoadValue()
        {
            int enumValue = SettingsScript.Instance.SettingsCustomPrefs.GetInt(SettingName);

            if (enumValue >= System.Enum.GetValues(typeof(ControllerLayout)).Length)
            {
                Debug.LogError("Incorrect load value for " + SettingName);
                return;
            }

            CurrentValue = enumValue;
        }

        public override void SaveValue(int enumValue)
        {
            CurrentValue = enumValue;
            SettingsScript.Instance.SettingsCustomPrefs.SetInt(SettingName, enumValue);
        }
    }
}