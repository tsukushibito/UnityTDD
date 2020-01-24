using UnityEngine;

namespace App
{
    public class VirtualGamepad : MonoBehaviour
    {
        [SerializeField]
        private Camera virtualGamepadCamera = null;

        [SerializeField]
        private VirtualStick leftStick = null;

        [SerializeField]
        private VirtualButton button0 = null;

        [SerializeField]
        private VirtualButton button1 = null;

        [SerializeField]
        private VirtualButton button2 = null;

        [SerializeField]
        private VirtualButton button3 = null;

        public bool GetKey(VirtualKeyCode code)
        {
            bool result = false;
            switch (code)
            {
                case VirtualKeyCode.Button0:
                    result = button0.GetButton();
                    break;
                case VirtualKeyCode.Button1:
                    result = button1.GetButton();
                    break;
                case VirtualKeyCode.Button2:
                    result = button2.GetButton();
                    break;
                case VirtualKeyCode.Button3:
                    result = button3.GetButton();
                    break;
                default:
                    break;
            }
            return result;
        }

        public bool GetKeyDown(VirtualKeyCode code)
        {
            bool result = false;
            switch (code)
            {
                case VirtualKeyCode.Button0:
                    result = button0.GetButtonDown();
                    break;
                case VirtualKeyCode.Button1:
                    result = button1.GetButtonDown();
                    break;
                case VirtualKeyCode.Button2:
                    result = button2.GetButtonDown();
                    break;
                case VirtualKeyCode.Button3:
                    result = button3.GetButtonDown();
                    break;
                default:
                    break;
            }
            return result;
        }

        public bool GetKeyUp(VirtualKeyCode code)
        {
            bool result = false;
            switch (code)
            {
                case VirtualKeyCode.Button0:
                    result = button0.GetButtonUp();
                    break;
                case VirtualKeyCode.Button1:
                    result = button1.GetButtonUp();
                    break;
                case VirtualKeyCode.Button2:
                    result = button2.GetButtonUp();
                    break;
                case VirtualKeyCode.Button3:
                    result = button3.GetButtonUp();
                    break;
                default:
                    break;
            }
            return result;
        }

        public float GetAxis(VirtualAxisCode code)
        {
            if (leftStick == null)
            {
                return 0;
            }

            var axis = 0.0f;
            switch (code)
            {
                case VirtualAxisCode.Holizontal:
                    axis = leftStick.GetHolizontal();
                    break;
                case VirtualAxisCode.Vertical:
                    axis = leftStick.GetVertical();
                    break;
            }

            return axis;
        }

        private void Awake()
        {
            if (leftStick != null)
            {
                leftStick.UiCamera = virtualGamepadCamera;
            }
        }

        private void Update()
        {
            Debug.unityLogger.Log(string.Format("Holizontal: {0}, Vertical: {1}", GetAxis(VirtualAxisCode.Holizontal), GetAxis(VirtualAxisCode.Vertical)));
        }
    }
}