using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kit.Controls
{
    public enum ControlsType
    {
        InGame,
        UI,
        Other
    }

    [System.Serializable]
    public class Bind
    {
        [SerializeField]
        private string _bindName;
        public string BindName { get { return _bindName; } }
        [SerializeField]
        private KeyboardAndMouseBindKey _keyboardAndMouseBindKey;
        public KeyboardAndMouseBindKey KeyboardAndMouseBindKey { get { return _keyboardAndMouseBindKey; } }
        [SerializeField]
        private GamepadBindKey _gamepadBindKey;
        public GamepadBindKey GamepadBindKey { get { return _gamepadBindKey; } }

        private bool _up;
        public bool Up { get { return _up; } }
        private bool _down;
        public bool Down { get { return _down; } }
        private bool _hold;
        public bool Hold { get { return _hold; } }

        private float _timeSinceDown;
        public float TimeSinceDown
        {
            get { return _timeSinceDown; }
        }

        private UnityEngine.InputSystem.Controls.ButtonControl _gamepadButtonControl;
        private UnityEngine.InputSystem.Controls.ButtonControl _keyboardAndMouseKeyControl;

        private void UpdateKeyboardAndMouseValue()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            if (Mouse.current == null)
            {
                return;
            }

            if (_keyboardAndMouseKeyControl == null)
            {
                return;
            }

            _hold = _keyboardAndMouseKeyControl.isPressed;
            _down = _keyboardAndMouseKeyControl.wasPressedThisFrame;
            _up = _keyboardAndMouseKeyControl.wasReleasedThisFrame;
        }

        private void UpdateGamepadValue()
        {
            if (Gamepad.current == null)
            {
                return;
            }

            if (_gamepadButtonControl == null)
            {
                return;
            }

            _hold = _gamepadButtonControl.isPressed;
            _down = _gamepadButtonControl.wasPressedThisFrame;
            _up = _gamepadButtonControl.wasReleasedThisFrame;
        }

        public void SetGamepadBind(GamepadBindKey gamepadBindKey)
        {
            _gamepadBindKey = gamepadBindKey;
            UpdateControl();
        }

        public void SetKeyboardBind(KeyboardAndMouseBindKey keyboardAndMouseBindKey)
        {
            _keyboardAndMouseBindKey = keyboardAndMouseBindKey;
            UpdateControl();
        }

        private void UpdateControl()
        {
            _gamepadButtonControl = KeyToInputControl.ButtonControlFromKey(_gamepadBindKey);
            _keyboardAndMouseKeyControl = KeyToInputControl.ButtonControlFromKey(_keyboardAndMouseBindKey);
        }

        public void Start()
        {
            UpdateControl();
        }

        public void Update()
        {
            if (ControllerLayoutScript.ControllerLayout == ControllerLayout.KeyboardAndMouse)
            {
                UpdateKeyboardAndMouseValue();
            }
            else
            {
                UpdateGamepadValue();
            }

            if ((Hold == false) && (Up == false))
            {
                _timeSinceDown = 0f;
            }
            else
            {
                _timeSinceDown += Time.deltaTime;
            }
        }

        public void ResetValue()
        {
            _hold = false;
            _down = false;
            _up = false;
        }
    }

    public class StickBind
    {
        private bool _up;
        public bool Up { get { return _up; } }
        private bool _down;
        public bool Down { get { return _down; } }
        private bool _hold;
        public bool Hold { get { return _hold; } }

        private float _timeSinceDown;
        public float TimeSinceDown { get { return _timeSinceDown; } }

        public void SetValue(UnityEngine.InputSystem.Controls.ButtonControl keyControl)
        {
            _up = keyControl.wasReleasedThisFrame;
            _down = keyControl.wasPressedThisFrame;
            _hold = keyControl.isPressed;
        }

        public void SetValue(bool isActive)
        {
            if (isActive == true)
            {
                if ((_down == false) && (_hold == false))
                {
                    _down = true;
                }
                else if (_down == true)
                {
                    _down = false;
                    _hold = true;
                }
                else
                {
                    _hold = true;
                }
            }
            else
            {
                if (_hold == true)
                {
                    _hold = false;
                    _up = true;
                }
                else
                {
                    _up = false;
                    _hold = false;
                    _down = false;
                }
            }
        }

        public void Update()
        {
            if ((Hold == false) && (Up == false))
            {
                _timeSinceDown = 0f;
            }
            else
            {
                _timeSinceDown += Time.deltaTime;
            }
        }

        public void ResetValue()
        {
            _hold = false;
            _down = false;
            _up = false;
        }
    }

    [System.Serializable]
    public class Stick
    {
        [SerializeField]
        private string _stickName;
        public string StickName { get { return _stickName; } }
        [SerializeField]
        private KeyboardAndMouseStickKey _keyboardAndMouseStickKey;
        [SerializeField]
        private GamepadStickKey _gamepadStickKey;

        [Header("Optional")]
        [SerializeField]
        private bool allowDiagonalMovement = true; // TODO
        [SerializeField]
        private float deadRadius = 0.1f;
        [SerializeField]
        private bool _shouldClampVelocity = false;

        private StickBind _north = new StickBind();
        public StickBind North { get { return _north; } }
        private StickBind _east = new StickBind();
        public StickBind East { get { return _east; } }
        private StickBind _south = new StickBind();
        public StickBind South { get { return _south; } }
        private StickBind _west = new StickBind();
        public StickBind West { get { return _west; } }

        private Vector2 _vector2;
        public Vector2 Vector2 { get { return _vector2; } }

        private UnityEngine.InputSystem.Controls.Vector2Control _gamepadVectorControl;
        private UnityEngine.InputSystem.Controls.ButtonControl _gamepadButtonControlNorth;
        private UnityEngine.InputSystem.Controls.ButtonControl _gamepadButtonControlEast;
        private UnityEngine.InputSystem.Controls.ButtonControl _gamepadButtonControlSouth;
        private UnityEngine.InputSystem.Controls.ButtonControl _gamepadButtonControlWest;

        private UnityEngine.InputSystem.Controls.Vector2Control _keyboardAndMouseVectorControl;
        private UnityEngine.InputSystem.Controls.ButtonControl _keyboardAndMouseButtonControlNorth;
        private UnityEngine.InputSystem.Controls.ButtonControl _keyboardAndMouseButtonControlEast;
        private UnityEngine.InputSystem.Controls.ButtonControl _keyboardAndMouseButtonControlSouth;
        private UnityEngine.InputSystem.Controls.ButtonControl _keyboardAndMouseButtonControlWest;

        private void UpdateValue(UnityEngine.InputSystem.Controls.Vector2Control vector2Control)
        {
            if (vector2Control == null)
            {
                return;
            }

            _vector2 = new Vector2(vector2Control.x.ReadValue(), vector2Control.y.ReadValue());

            if (_shouldClampVelocity == true)
            {
                _vector2 = Vector2.ClampMagnitude(_vector2, 1f);
            }

            if (allowDiagonalMovement == true)
            {
                if (_vector2.x > deadRadius)
                {
                    _east.SetValue(true);
                }
                else
                {
                    _east.SetValue(false);
                }

                if (_vector2.x < -deadRadius)
                {
                    _west.SetValue(true);
                }
                else
                {
                    _west.SetValue(false);
                }

                if (_vector2.y > deadRadius)
                {
                    _north.SetValue(true);
                }
                else
                {
                    _north.SetValue(false);
                }

                if (_vector2.y < -deadRadius)
                {
                    _south.SetValue(true);
                }
                else
                {
                    _south.SetValue(false);
                }
            }
            else
            {
                if ((Mathf.Sqrt(_vector2.x * _vector2.x)) + (Mathf.Sqrt(_vector2.y * _vector2.y)) < (deadRadius * deadRadius))
                {
                    _north.SetValue(false);
                    _east.SetValue(false);
                    _south.SetValue(false);
                    _west.SetValue(false);
                }
                else
                {
                    float angle = Angle(_vector2);

                    if ((angle > 315f) || (angle < 45f))
                    {
                        _north.SetValue(true);
                        _east.SetValue(false);
                        _south.SetValue(false);
                        _west.SetValue(false);
                    }
                    else if ((angle > 45f) && (angle < 135f))
                    {
                        _north.SetValue(false);
                        _east.SetValue(true);
                        _south.SetValue(false);
                        _west.SetValue(false);
                    }
                    else if ((angle > 135f) && (angle < 225f))
                    {
                        _north.SetValue(false);
                        _east.SetValue(false);
                        _south.SetValue(true);
                        _west.SetValue(false);
                    }
                    else
                    {
                        _north.SetValue(false);
                        _east.SetValue(false);
                        _south.SetValue(false);
                        _west.SetValue(true);
                    }
                }
            }

            float Angle(Vector2 vector2)
            {
                if (vector2.x < 0)
                {
                    return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
                }
                else
                {
                    return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
                }
            }
        }

        private void UpdateValue(UnityEngine.InputSystem.Controls.ButtonControl north, UnityEngine.InputSystem.Controls.ButtonControl east,
            UnityEngine.InputSystem.Controls.ButtonControl south, UnityEngine.InputSystem.Controls.ButtonControl west)
        {
            if ((north == null) || (east == null) || (south == null) || (west == null))
            {
                return;
            }

            _north.SetValue(north);
            _east.SetValue(east);
            _south.SetValue(south);
            _west.SetValue(west);

            float x = 0f;
            if ((_east.Hold == true) || (_east.Down == true))
            {
                x = 1f;
            }
            else if ((_west.Hold == true) || (_west.Down == true))
            {
                x = -1f;
            }

            float y = 0f;
            if ((_north.Hold == true) || (_north.Down == true))
            {
                y = 1f;
            }
            else if ((_south.Hold == true) || (_south.Down == true))
            {
                y = -1f;
            }

            _vector2 = new Vector2(x, y);

            if (_shouldClampVelocity == true)
            {
                _vector2 = Vector2.ClampMagnitude(_vector2, 1f);
            }
        }

        public void SetGamepadBind(GamepadStickKey gamepadStickKey)
        {
            _gamepadStickKey = gamepadStickKey;
            UpdateControl();
        }

        public void SetKeyboardBind(KeyboardAndMouseStickKey keyboardAndMouseStickKey)
        {
            _keyboardAndMouseStickKey = keyboardAndMouseStickKey;
            UpdateControl();
        }

        private void UpdateControl()
        {
            _gamepadVectorControl = KeyToInputControl.VectorControlFromKey(_gamepadStickKey);

            if (_keyboardAndMouseStickKey == KeyboardAndMouseStickKey.MouseMovement)
            {
                _keyboardAndMouseVectorControl = KeyToInputControl.VectorControlFromKey(_keyboardAndMouseStickKey);
            }
            else
            {
                KeyToInputControl.ButtonControlsFromKey(_keyboardAndMouseStickKey, out var buttonControlNorth, out var buttonControlEast, out var buttonControlSouth, out var buttonControlWest);

                _keyboardAndMouseButtonControlNorth = buttonControlNorth;
                _keyboardAndMouseButtonControlEast = buttonControlEast;
                _keyboardAndMouseButtonControlSouth = buttonControlSouth;
                _keyboardAndMouseButtonControlWest = buttonControlWest;
            }
        }

        public void Start()
        {
            UpdateControl();
        }

        private void UpdateGamepadValue()
        {
            UpdateValue(_gamepadVectorControl);
        }

        private void UpdateKeyboardAndMouseValue()
        {
            if (_keyboardAndMouseStickKey == KeyboardAndMouseStickKey.MouseMovement)
            {
                UpdateValue(_keyboardAndMouseVectorControl);
            }
            else
            {
                UpdateValue(_keyboardAndMouseButtonControlNorth, _keyboardAndMouseButtonControlEast, _keyboardAndMouseButtonControlSouth, _keyboardAndMouseButtonControlWest);
            }
        }

        public void Update()
        {
            if (ControllerLayoutScript.ControllerLayout == ControllerLayout.KeyboardAndMouse)
            {
                UpdateKeyboardAndMouseValue();
            }
            else
            {
                UpdateGamepadValue();
            }
        }

        public void ResetValue()
        {
            _north.ResetValue();
            _east.ResetValue();
            _south.ResetValue();
            _west.ResetValue();
            _vector2 = new Vector2();
        }
    }

    [DefaultExecutionOrder(-90)]
    public class ControlsScript : MonoBehaviour
    {
        [SerializeField]
        private ControlsType _controlsType = ControlsType.InGame;
        [SerializeField]
        private List<Bind> _binds = new List<Bind>();
        [SerializeField]
        private List<Stick> _sticks = new List<Stick>();

        public static ControlsScript InGameControls = null;
        public static ControlsScript UIControls = null;

        public static Vector2 MousePosition()
        {
            if (Mouse.current == null)
            {
                return new Vector2();
            }

            return Mouse.current.position.ReadValue();
        }

        public static Vector2 MouseMovement()
        {
            if (Mouse.current == null)
            {
                return new Vector2();
            }

            return Mouse.current.delta.ReadValue();
        }

        private void Awake()
        {
            if (_controlsType == ControlsType.InGame)
            {
                InGameControls = this;
            }
            else if (_controlsType == ControlsType.UI)
            {
                UIControls = this;
            }
        }

        public bool Contains(string bindName)
        {
            for (int i = 0; i < _binds.Count; i++)
            {
                if (_binds[i].BindName == bindName)
                {
                    return true;
                }
            }

            return false;
        }

        public Bind BindWithName(string bindName)
        {
            for (int i = 0; i < _binds.Count; i++)
            {
                if (_binds[i].BindName == bindName)
                {
                    return _binds[i];
                }
            }

            Debug.LogError("No bind for bindName: " + bindName);
            return new Bind();
        }

        public Stick StickWithName(string stickName)
        {
            for (int i = 0; i < _sticks.Count; i++)
            {
                if (_sticks[i].StickName == stickName)
                {
                    return _sticks[i];
                }
            }

            Debug.LogError("No bind for stickName: " + stickName);
            return new Stick();
        }

        private void Start()
        {
            Initialize();
        }

        private void OnDisable()
        {
            for (int i = 0; i < _binds.Count; i++)
            {
                _binds[i].ResetValue();
            }

            for (int i = 0; i < _sticks.Count; i++)
            {
                _sticks[i].ResetValue();
            }
        }

        private void Initialize()
        {
            for (int i = 0; i < _binds.Count; i++)
            {
                _binds[i].Start();
            }

            for (int i = 0; i < _sticks.Count; i++)
            {
                _sticks[i].Start();
            }
        }

        private void Update()
        {
            for (int i = 0; i < _binds.Count; i++)
            {
                _binds[i].Update();
            }

            for (int i = 0; i < _sticks.Count; i++)
            {
                _sticks[i].Update();
            }
        }
    }
}
