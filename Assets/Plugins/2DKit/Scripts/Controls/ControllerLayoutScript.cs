using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kit.Controls
{
    public enum ControllerLayout
    {
        KeyboardAndMouse,
        XboxLayout,
        PlaystationLayout,
        SwitchLayout
    }

    [DefaultExecutionOrder(-95)]
    public class ControllerLayoutScript : MonoBehaviour
    {
        public static ControllerLayout ControllerLayout;

        private void Update()
        {
            if (Keyboard.current != null)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    ControllerLayout = ControllerLayout.KeyboardAndMouse;
                    return;
                }
            }

            if (Mouse.current != null)
            {
                if ((Mouse.current.leftButton.wasPressedThisFrame) || (Mouse.current.rightButton.wasPressedThisFrame))
                {
                    ControllerLayout = ControllerLayout.KeyboardAndMouse;
                    return;
                }
            }

            if (Gamepad.current != null)
            {
                if ((Gamepad.current.buttonEast.wasPressedThisFrame) || (Gamepad.current.buttonWest.wasPressedThisFrame) ||
                    (Gamepad.current.buttonNorth.wasPressedThisFrame) || (Gamepad.current.buttonSouth.wasPressedThisFrame) ||
                    (Gamepad.current.startButton.wasPressedThisFrame) || (Gamepad.current.selectButton.wasPressedThisFrame) ||
                    (Gamepad.current.rightStickButton.wasPressedThisFrame) || (Gamepad.current.leftStickButton.wasPressedThisFrame) ||
                    (Gamepad.current.rightShoulder.wasPressedThisFrame) || (Gamepad.current.leftShoulder.wasPressedThisFrame) ||
                    (Gamepad.current.leftTrigger.wasPressedThisFrame) || (Gamepad.current.rightTrigger.wasPressedThisFrame) ||
                    (Gamepad.current.dpad.up.wasPressedThisFrame) || (Gamepad.current.dpad.down.wasPressedThisFrame) ||
                    (Gamepad.current.dpad.left.wasPressedThisFrame) || (Gamepad.current.dpad.right.wasPressedThisFrame))
                {
                    string gamepadName = Gamepad.current.name;

                    if ((gamepadName.ToLower().Contains("playstation")) || (gamepadName.ToLower().Contains("dualshock")) || (gamepadName.ToLower().Contains("dualsense")) || (gamepadName.ToLower().Contains("sony")))
                    {
                        ControllerLayout = ControllerLayout.PlaystationLayout;
                    }
                    else
                    {
                        ControllerLayout = ControllerLayout.XboxLayout;
                    }
                }
            }
        }
    }
}
