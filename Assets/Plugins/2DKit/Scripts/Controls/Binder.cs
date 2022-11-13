using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kit.Controls
{
    [CreateAssetMenu(menuName = "Kit/Controls/Binder")]
    public class Binder : ScriptableObject
    {
        [SerializeField]
        private string _bindName;
        public string BindName { get { return _bindName; } }
    }
}
