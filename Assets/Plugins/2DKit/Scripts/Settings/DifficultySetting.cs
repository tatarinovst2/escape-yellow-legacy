using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Localization;

namespace Kit.Settings
{
	public enum Difficulty
	{
		Standard = 0,
		StoryMode = 1
	}

    [CreateAssetMenu(menuName = "Kit/Settings/DifficultySetting")]
    public class DifficultySetting : ChoosableSetting
    {
        public override TextData CurrentValueTextData()
        {
            return new TextData(((Difficulty)CurrentValue).ToString());
        }

        public override void LoadValue()
        {
            int enumValue = SettingsScript.Instance.SettingsCustomPrefs.GetInt(SettingName);

            if (enumValue >= System.Enum.GetValues(typeof(Difficulty)).Length)
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

