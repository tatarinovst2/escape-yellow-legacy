using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kit.Saving;

namespace Kit.Settings
{
	public class SettingsScript : MonoBehaviour
	{
		public static SettingsScript Instance = null;

		[SerializeField]
		private List<Setting> _settings;

		private CustomPrefs _settingsCustomPrefs;
		public CustomPrefs SettingsCustomPrefs { get { return _settingsCustomPrefs; } }

        private void Awake()
        {
			Instance = this;
			_settingsCustomPrefs = new CustomPrefs("/Kit/Settings.txt");
		}

        private void Start()
        {
            for (int i = 0; i < _settings.Count; i++)
            {
				if (_settingsCustomPrefs.HasKey(_settings[i].SettingName))
                {
					_settings[i].LoadValue();
				}
				else
                {
					_settings[i].LoadDefaultValue();
				}
            }
        }

        public Setting SettingWithName(string settingName)
        {
			for (int i = 0; i < _settings.Count; i++)
            {
				if (settingName == _settings[i].SettingName)
                {
					return _settings[i];
                }
            }

			Debug.LogError("No setting with name: " + settingName);
			return null;
        }

		public void FlushSettings()
        {
			_settingsCustomPrefs.Flush();
        }

		public void ResetAndFlush()
        {
			for (int i = 0; i < _settings.Count; i++)
			{
				_settings[i].ResetValues();
				FlushSettings();
			}
		}
	}
}

