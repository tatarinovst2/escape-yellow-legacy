using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Localization;

namespace Kit.Settings
{
    public enum Resolution
    {
        Low = 0,
        Standard = 1,
        High = 2,
        Ultra = 3
    }

    [CreateAssetMenu(menuName = "Kit/Settings/ResolutionSetting")]
    public class ResolutionSetting : ChoosableSetting
    {
        [SerializeField]
        private bool allowUltraWide = false;

        public override TextData CurrentValueTextData()
        {
            return new TextData(((Resolution)CurrentValue).ToString());
        }

        public override void LoadValue()
        {
            int enumValue = SettingsScript.Instance.SettingsCustomPrefs.GetInt(SettingName);

            if (enumValue >= System.Enum.GetValues(typeof(Resolution)).Length)
            {
                Debug.LogError("Incorrect load value for " + SettingName);
                return;
            }

            CurrentValue = enumValue;

            Apply();
        }

        public override void Apply()
        {
            Resolution resolution = (Resolution)CurrentValue;

            int height = 1080;

            switch (resolution)
            {
                case Resolution.Low:
                    height = 720;
                    break;

                case Resolution.Standard:
                    height = 1080;
                    break;

                case Resolution.High:
                    height = 1440;
                    break;

                case Resolution.Ultra:
                    height = 2160;
                    break;
            }

            float ratio = Screen.width / Screen.height;

            int width = 1920;

            if ((ratio > 2.3f) && (ratio < 2.4f) && (allowUltraWide)) // 21:9
            {
                if (height == 720)
                {
                    width = 1680;
                }
                else if (height == 1080)
                {
                    width = 2560;
                }
                else if (height == 1440)
                {
                    width = 3440;
                }
                else if (height == 2160)
                {
                    if (Screen.width == 5160)
                    {
                        width = 5160;
                    }
                    else
                    {
                        width = 5120;
                    }
                }
            }
            else
            {
                if (height == 720)
                {
                    width = 1280;
                }
                else if (height == 1080)
                {
                    width = 1920;
                }
                else if (height == 1440)
                {
                    width = 2560;
                }
                else if (height == 2160)
                {
                    width = 3840;
                }
            }

            Screen.SetResolution(width, height, Screen.fullScreenMode);
        }

        public override int DefaultValue()
        {
            int height = Screen.height;

            if (height == 720)
            {
                return (int)Resolution.Low;
            }
            else if (height == 1080)
            {
                return (int)Resolution.Standard;
            }
            else if (height == 1440)
            {
                return (int)Resolution.High;
            }
            else if (height == 2160)
            {
                return (int)Resolution.Ultra;
            }
            else
            {
                return (int)Resolution.Standard;
            }
        }

        public override void SaveValue(int enumValue)
        {
            CurrentValue = enumValue;
            SettingsScript.Instance.SettingsCustomPrefs.SetInt(SettingName, enumValue);
        }
    }
}