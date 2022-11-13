using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Localization;

namespace Kit.Settings
{
    public class ChoosableSetting : Setting
    {
        [SerializeField]
        private List<TextData> _optionsTextDatas;
        public List<TextData> OptionsTextData { get { return _optionsTextDatas; } }

        public override TextData CurrentValueTextData()
        {
            throw new System.NotImplementedException();
        }

        public override void LoadValue()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveValue(int enumValue)
        {
            throw new System.NotImplementedException();
        }
    }
}