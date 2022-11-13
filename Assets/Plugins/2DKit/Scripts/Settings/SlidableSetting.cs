using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kit.Localization;

namespace Kit.Settings
{
    public class SlidableSetting : Setting
    {
        [SerializeField]
        private int _maxValue;
        public int MaxValue { get { return _maxValue; } }

        public override TextData CurrentValueTextData()
        {
            throw new System.NotImplementedException();
        }

        public override void LoadValue()
        {
            throw new System.NotImplementedException();
        }

        public override int DefaultValue()
        {
            return _maxValue;
        }

        public override void SaveValue(int enumValue)
        {
            throw new System.NotImplementedException();
        }
    }
}