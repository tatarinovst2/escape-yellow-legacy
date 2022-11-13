using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Localization;

namespace Kit.Settings
{
    public enum Language
    {
        English = 0,
        Russian = 1
    }

    [CreateAssetMenu(menuName = "Kit/Settings/LanguageSetting")]
    public class LanguageSetting : ChoosableSetting
    {
        public override TextData CurrentValueTextData()
        {
            return new TextData(((Language)CurrentValue).ToString());
        }

        public override void LoadValue()
        {
            int enumValue = SettingsScript.Instance.SettingsCustomPrefs.GetInt(SettingName);

            if (enumValue >= System.Enum.GetValues(typeof(Language)).Length)
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