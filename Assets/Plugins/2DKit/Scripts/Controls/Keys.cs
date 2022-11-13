using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kit.Controls
{
    public enum KeyboardAndMouseStickKey
    {
        Missing,
        WASD,
        KeyboardArraws,
        MouseMovement
    }

    public enum GamepadStickKey
    {
        Missing,
        DPad,
        LeftStick,
        RightStick
    }

    public enum KeyboardAndMouseBindKey
    {
        Missing,
        Q,
        W,
        E,
        R,
        T,
        Y,
        U,
        I,
        O,
        P,
        A,
        S,
        D,
        F,
        G,
        H,
        J,
        K,
        L,
        Z,
        X,
        C,
        V,
        B,
        N,
        M,
        Tab,
        Shift,
        Ctrl,
        Alt,
        Space,
        Number1,
        Number2,
        Number3,
        Number4,
        Number5,
        Number6,
        Number7,
        Number8,
        Number9,
        Number0,
        Enter,
        Escape,
        Minus,
        Equal,
        MouseLeft,
        MouseRight
    }

    public enum GamepadBindKey
    {
        Missing,
        North,
        East,
        South,
        West,
        UpArrow,
        RightArrow,
        DownArrow,
        LeftArrow,
        LeftTrigger,
        RightTrigger,
        LeftShoulder,
        RightShoulder,
        LeftStickButton,
        RightStickButton,
        Start,
        Select
    }

    public static class KeyToInputControl
    {
        public static UnityEngine.InputSystem.Controls.Vector2Control VectorControlFromKey(GamepadStickKey gamepadStickKey)
        {
            if (Gamepad.current == null)
            {
                return null;
            }

            switch (gamepadStickKey)
            {
                case GamepadStickKey.LeftStick:
                    return Gamepad.current.leftStick;

                case GamepadStickKey.RightStick:
                    return Gamepad.current.rightStick;

                case GamepadStickKey.DPad:
                    return Gamepad.current.dpad;

                default:
                    return null;
            }
        }

        public static UnityEngine.InputSystem.Controls.Vector2Control VectorControlFromKey(KeyboardAndMouseStickKey keyboardAndMouseStickKey)
        {
            if (Keyboard.current == null)
            {
                return null;
            }

            if (Mouse.current == null)
            {
                return null;
            }

            switch (keyboardAndMouseStickKey)
            {
                case KeyboardAndMouseStickKey.MouseMovement:
                    return Mouse.current.delta;

                default:
                    return null;
            }
        }

        public static void ButtonControlsFromKey(KeyboardAndMouseStickKey keyboardAndMouseStickKey, out UnityEngine.InputSystem.Controls.ButtonControl buttonControlNorth,
            out UnityEngine.InputSystem.Controls.ButtonControl buttonControlEast, out UnityEngine.InputSystem.Controls.ButtonControl buttonControlSouth, out UnityEngine.InputSystem.Controls.ButtonControl buttonControlWest)
        {
            buttonControlNorth = null;
            buttonControlEast = null;
            buttonControlSouth = null;
            buttonControlWest = null;

            if (Keyboard.current == null)
            {
                return;
            }

            if (Mouse.current == null)
            {
                return;
            }

            switch (keyboardAndMouseStickKey)
            {
                case KeyboardAndMouseStickKey.WASD:
                    buttonControlNorth = Keyboard.current.wKey;
                    buttonControlEast = Keyboard.current.dKey;
                    buttonControlSouth = Keyboard.current.sKey;
                    buttonControlWest = Keyboard.current.aKey;
                    break;

                case KeyboardAndMouseStickKey.KeyboardArraws:
                    buttonControlNorth = Keyboard.current.upArrowKey;
                    buttonControlEast = Keyboard.current.rightArrowKey;
                    buttonControlSouth = Keyboard.current.downArrowKey;
                    buttonControlWest = Keyboard.current.leftArrowKey;
                    break;

                default:
                    break;
            }
        }

        public static UnityEngine.InputSystem.Controls.ButtonControl ButtonControlFromKey(KeyboardAndMouseBindKey keyboardAndMouseBindKey)
        {
            if (Keyboard.current == null)
            {
                return null;
            }

            if (Mouse.current == null)
            {
                return null;
            }

            switch (keyboardAndMouseBindKey)
            {
                case KeyboardAndMouseBindKey.Q:
                    return Keyboard.current.qKey;

                case KeyboardAndMouseBindKey.W:
                    return Keyboard.current.wKey;

                case KeyboardAndMouseBindKey.E:
                    return Keyboard.current.eKey;

                case KeyboardAndMouseBindKey.R:
                    return Keyboard.current.rKey;

                case KeyboardAndMouseBindKey.T:
                    return Keyboard.current.tKey;

                case KeyboardAndMouseBindKey.Y:
                    return Keyboard.current.yKey;

                case KeyboardAndMouseBindKey.U:
                    return Keyboard.current.uKey;

                case KeyboardAndMouseBindKey.I:
                    return Keyboard.current.iKey;

                case KeyboardAndMouseBindKey.O:
                    return Keyboard.current.oKey;

                case KeyboardAndMouseBindKey.P:
                    return Keyboard.current.pKey;

                case KeyboardAndMouseBindKey.A:
                    return Keyboard.current.aKey;

                case KeyboardAndMouseBindKey.S:
                    return Keyboard.current.sKey;

                case KeyboardAndMouseBindKey.D:
                    return Keyboard.current.dKey;

                case KeyboardAndMouseBindKey.F:
                    return Keyboard.current.fKey;

                case KeyboardAndMouseBindKey.G:
                    return Keyboard.current.gKey;

                case KeyboardAndMouseBindKey.H:
                    return Keyboard.current.hKey;

                case KeyboardAndMouseBindKey.J:
                    return Keyboard.current.jKey;

                case KeyboardAndMouseBindKey.K:
                    return Keyboard.current.kKey;

                case KeyboardAndMouseBindKey.L:
                    return Keyboard.current.lKey;

                case KeyboardAndMouseBindKey.Z:
                    return Keyboard.current.zKey;

                case KeyboardAndMouseBindKey.X:
                    return Keyboard.current.xKey;

                case KeyboardAndMouseBindKey.C:
                    return Keyboard.current.cKey;

                case KeyboardAndMouseBindKey.V:
                    return Keyboard.current.vKey;

                case KeyboardAndMouseBindKey.B:
                    return Keyboard.current.bKey;

                case KeyboardAndMouseBindKey.N:
                    return Keyboard.current.nKey;

                case KeyboardAndMouseBindKey.M:
                    return Keyboard.current.mKey;

                case KeyboardAndMouseBindKey.Alt:
                    return Keyboard.current.altKey;

                case KeyboardAndMouseBindKey.Ctrl:
                    return Keyboard.current.ctrlKey;

                case KeyboardAndMouseBindKey.Enter:
                    return Keyboard.current.enterKey;

                case KeyboardAndMouseBindKey.Equal:
                    return Keyboard.current.equalsKey;

                case KeyboardAndMouseBindKey.Escape:
                    return Keyboard.current.escapeKey;

                case KeyboardAndMouseBindKey.Minus:
                    return Keyboard.current.minusKey;

                case KeyboardAndMouseBindKey.Number0:
                    return Keyboard.current.digit0Key;

                case KeyboardAndMouseBindKey.Number1:
                    return Keyboard.current.digit1Key;

                case KeyboardAndMouseBindKey.Number2:
                    return Keyboard.current.digit2Key;

                case KeyboardAndMouseBindKey.Number3:
                    return Keyboard.current.digit3Key;

                case KeyboardAndMouseBindKey.Number4:
                    return Keyboard.current.digit4Key;

                case KeyboardAndMouseBindKey.Number5:
                    return Keyboard.current.digit5Key;

                case KeyboardAndMouseBindKey.Number6:
                    return Keyboard.current.digit6Key;

                case KeyboardAndMouseBindKey.Number7:
                    return Keyboard.current.digit7Key;

                case KeyboardAndMouseBindKey.Number8:
                    return Keyboard.current.digit8Key;

                case KeyboardAndMouseBindKey.Number9:
                    return Keyboard.current.digit9Key;

                case KeyboardAndMouseBindKey.Shift:
                    return Keyboard.current.shiftKey;

                case KeyboardAndMouseBindKey.Space:
                    return Keyboard.current.spaceKey;

                case KeyboardAndMouseBindKey.Tab:
                    return Keyboard.current.tabKey;

                case KeyboardAndMouseBindKey.MouseLeft:
                    return Mouse.current.leftButton;

                case KeyboardAndMouseBindKey.MouseRight:
                    return Mouse.current.rightButton;

                default:
                    return null;
            }
        }

        public static UnityEngine.InputSystem.Controls.ButtonControl ButtonControlFromKey(GamepadBindKey gamepadBindKey)
        {
            if (Gamepad.current == null)
            {
                return null;
            }

            switch (gamepadBindKey)
            {
                case GamepadBindKey.North:
                    return Gamepad.current.buttonNorth;

                case GamepadBindKey.East:
                    return Gamepad.current.buttonEast;

                case GamepadBindKey.South:
                    return Gamepad.current.buttonSouth;

                case GamepadBindKey.West:
                    return Gamepad.current.buttonWest;

                case GamepadBindKey.UpArrow:
                    return Gamepad.current.dpad.up;

                case GamepadBindKey.RightArrow:
                    return Gamepad.current.dpad.right;

                case GamepadBindKey.DownArrow:
                    return Gamepad.current.dpad.down;

                case GamepadBindKey.LeftArrow:
                    return Gamepad.current.dpad.left;

                case GamepadBindKey.LeftTrigger:
                    return Gamepad.current.leftTrigger;

                case GamepadBindKey.RightTrigger:
                    return Gamepad.current.rightTrigger;

                case GamepadBindKey.LeftShoulder:
                    return Gamepad.current.leftShoulder;

                case GamepadBindKey.RightShoulder:
                    return Gamepad.current.rightShoulder;

                case GamepadBindKey.LeftStickButton:
                    return Gamepad.current.leftStickButton;

                case GamepadBindKey.RightStickButton:
                    return Gamepad.current.rightStickButton;

                case GamepadBindKey.Start:
                    return Gamepad.current.startButton;

                case GamepadBindKey.Select:
                    return Gamepad.current.selectButton;

                default:
                    return null;
            }
        }
    }

    public static class KeyToHelper
    {
        public static string KeyToString(GamepadBindKey gamepadBindKey, ControllerLayout controllerLayout)
        {
            if (Gamepad.current == null)
            {
                return "";
            }

            switch (controllerLayout) // TODO
            {
                case ControllerLayout.PlaystationLayout:
                    switch (gamepadBindKey)
                    {
                        case GamepadBindKey.LeftTrigger:
                            return "L2";

                        case GamepadBindKey.RightTrigger:
                            return "R2";

                        case GamepadBindKey.LeftShoulder:
                            return "L1";

                        case GamepadBindKey.RightShoulder:
                            return "R1";

                        case GamepadBindKey.LeftStickButton:
                            return "L3";

                        case GamepadBindKey.RightStickButton:
                            return "R3";

                        case GamepadBindKey.Start:
                            return "Options";

                        case GamepadBindKey.Select:
                            return "Share";

                        default:
                            return "";
                    }

                case ControllerLayout.XboxLayout:
                    switch (gamepadBindKey)
                    {
                        case GamepadBindKey.LeftTrigger:
                            return "LT";

                        case GamepadBindKey.RightTrigger:
                            return "RT";

                        case GamepadBindKey.LeftShoulder:
                            return "LB";

                        case GamepadBindKey.RightShoulder:
                            return "RB";

                        case GamepadBindKey.LeftStickButton:
                            return "L";

                        case GamepadBindKey.RightStickButton:
                            return "R";

                        case GamepadBindKey.Start:
                            return "Start";

                        case GamepadBindKey.Select:
                            return "Select";

                        case GamepadBindKey.North:
                            return "Y";

                        case GamepadBindKey.East:
                            return "B";

                        case GamepadBindKey.South:
                            return "A";

                        case GamepadBindKey.West:
                            return "X";

                        default:
                            return "";
                    }

                case ControllerLayout.SwitchLayout:
                    switch (gamepadBindKey)
                    {
                        case GamepadBindKey.LeftTrigger:
                            return "ZL";

                        case GamepadBindKey.RightTrigger:
                            return "ZR";

                        case GamepadBindKey.LeftShoulder:
                            return "L";

                        case GamepadBindKey.RightShoulder:
                            return "R";

                        case GamepadBindKey.LeftStickButton:
                            return "Left Stick";

                        case GamepadBindKey.RightStickButton:
                            return "Right Stick";

                        case GamepadBindKey.Start:
                            return "+";

                        case GamepadBindKey.Select:
                            return "-";

                        case GamepadBindKey.North:
                            return "X";

                        case GamepadBindKey.East:
                            return "A";

                        case GamepadBindKey.South:
                            return "B";

                        case GamepadBindKey.West:
                            return "Y";

                        default:
                            return "";
                    }

                default:
                    switch (gamepadBindKey)
                    {
                        case GamepadBindKey.North:
                            return "";

                        case GamepadBindKey.East:
                            return "";

                        case GamepadBindKey.South:
                            return "";

                        case GamepadBindKey.West:
                            return "";

                        case GamepadBindKey.UpArrow:
                            return "";

                        case GamepadBindKey.RightArrow:
                            return "";

                        case GamepadBindKey.DownArrow:
                            return "";

                        case GamepadBindKey.LeftArrow:
                            return "";

                        case GamepadBindKey.LeftTrigger:
                            return "";

                        case GamepadBindKey.RightTrigger:
                            return "";

                        case GamepadBindKey.LeftShoulder:
                            return "";

                        case GamepadBindKey.RightShoulder:
                            return "";

                        case GamepadBindKey.LeftStickButton:
                            return "";

                        case GamepadBindKey.RightStickButton:
                            return "";

                        case GamepadBindKey.Start:
                            return "";

                        case GamepadBindKey.Select:
                            return "";

                        default:
                            return "";
                    }
            }
        }

        public static Sprite KeyToImage(GamepadBindKey gamepadBindKey, ControllerLayout controllerLayout)
        {
            if (Gamepad.current == null)
            {
                return null;
            }

            switch (controllerLayout)
            {
                case ControllerLayout.PlaystationLayout:
                    switch (gamepadBindKey)
                    {
                        case GamepadBindKey.North:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/Triangle");

                        case GamepadBindKey.East:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/Circle");

                        case GamepadBindKey.South:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/X");

                        case GamepadBindKey.West:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/Square");

                        case GamepadBindKey.UpArrow:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/North");

                        case GamepadBindKey.RightArrow:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/East");

                        case GamepadBindKey.DownArrow:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/South");

                        case GamepadBindKey.LeftArrow:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/Wesr");

                        default:
                            return null;
                    }

                default:
                    switch (gamepadBindKey)
                    {
                        case GamepadBindKey.UpArrow:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/North");

                        case GamepadBindKey.RightArrow:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/East");

                        case GamepadBindKey.DownArrow:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/South");

                        case GamepadBindKey.LeftArrow:
                            return Resources.Load<Sprite>("UI/Helpers/Gamepad/West");

                        default:
                            return null;
                    }
            }
        }
    }
}
