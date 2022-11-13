using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Localization;

namespace Kit.Settings
{
	[CreateAssetMenu(menuName = "Kit/Settings")]
	public abstract class Setting : ScriptableObject
	{
		[SerializeField]
		private string _settingName;
		public string SettingName { get { return _settingName; } }

		[SerializeField]
		private TextData _settingNameTextData;
		public TextData SettingNameTextData { get { return _settingNameTextData; } }

		private int _currentValue;
		public int CurrentValue
		{
			get { return _currentValue; }
			protected set { _currentValue = value; }
		}

		public abstract TextData CurrentValueTextData();

		public virtual int DefaultValue()
        {
			return 0;
        }

        public void LoadDefaultValue()
        {
			_currentValue = DefaultValue();
			Apply();
        }

		public abstract void LoadValue();

		public virtual void Apply()
		{

		}

		public virtual void SaveValue()
		{
			SettingsScript.Instance.SettingsCustomPrefs.SetInt(SettingName, CurrentValue);
		}

		public abstract void SaveValue(int enumValue);

		public void ResetValues()
		{
			_currentValue = DefaultValue();
        }
	}
}
