using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Localization;

namespace Kit.Settings
{
	public enum ScreenMode
	{
		Fullscreen = 0,
		Windowed = 1
	}

    [CreateAssetMenu(menuName = "Kit/Settings/ScreenModeSetting")]
    public class ScreenModeSetting : ChoosableSetting
    {
        public override TextData CurrentValueTextData()
        {
            return new TextData(((ScreenMode)CurrentValue).ToString());
        }

        public override void LoadValue()
        {
            int enumValue = SettingsScript.Instance.SettingsCustomPrefs.GetInt(SettingName);

            if (enumValue >= System.Enum.GetValues(typeof(ScreenMode)).Length)
            {
                Debug.LogError("Incorrect load value for " + SettingName);
                return;
            }

            CurrentValue = enumValue;

            Apply();
        }

        public override void Apply()
        {
            ScreenMode screenMode = (ScreenMode)CurrentValue;

            if (screenMode == ScreenMode.Fullscreen)
            {
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
            }
            else if (screenMode == ScreenMode.Windowed)
            {
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed);
            }
        }

        public override void SaveValue(int enumValue)
        {
            CurrentValue = enumValue;
            SettingsScript.Instance.SettingsCustomPrefs.SetInt(SettingName, enumValue);
        }
    }
}
