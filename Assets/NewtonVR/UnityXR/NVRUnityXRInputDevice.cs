using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.XR;

namespace NewtonVR
{
    public class NVRUnityXRInputDevice : NVRInputDevice
    {
        private XRNode Controller;
        private Dictionary<NVRButtons, string> TouchMapping = new Dictionary<NVRButtons, string>(new NVRButtonsComparer());
        private Dictionary<NVRButtons, string> NearTouchMapping = new Dictionary<NVRButtons, string>(new NVRButtonsComparer());
        private Dictionary<NVRButtons, string> ButtonMapping = new Dictionary<NVRButtons, string>(new NVRButtonsComparer());
        private Dictionary<NVRButtons, string> TriggerMapping = new Dictionary<NVRButtons, string>(new NVRButtonsComparer());
        private Dictionary<NVRButtons, string> StickMapping = new Dictionary<NVRButtons, string>(new NVRButtonsComparer());

        public override void Initialize(NVRHand hand)
        {
            base.Initialize(hand);
            if(hand.IsLeft) {
                Controller = XRNode.LeftHand;
                SetupButtonMappingLeft();
            }
            else if(hand.IsRight) {
                Controller = XRNode.RightHand;
                SetupButtonMappingRight();
            }
            else {
                Debug.LogError("Hand must be either a left hand or a right hand! What is this?", hand);
            }

        }
        
        protected virtual void SetupButtonMappingLeft() {
            TriggerMapping.Add(NVRButtons.Trigger, "Axis9");
            ButtonMapping.Add(NVRButtons.Trigger, "Button14");

            TriggerMapping.Add(NVRButtons.Grip, "Axis11");
            ButtonMapping.Add(NVRButtons.Grip, "Button4");
        }
        protected virtual void SetupButtonMappingRight() {
            TriggerMapping.Add(NVRButtons.Trigger, "Axis10");
            ButtonMapping.Add(NVRButtons.Trigger, "Button15");

            TriggerMapping.Add(NVRButtons.Grip, "Axis12");
            ButtonMapping.Add(NVRButtons.Grip, "Button5");

        }
            // ButtonMapping.Add(NVRButtons.A, OVRInput.Button.One);
            // ButtonMapping.Add(NVRButtons.B, OVRInput.Button.Two);
            // ButtonMapping.Add(NVRButtons.X, OVRInput.Button.One);
            // ButtonMapping.Add(NVRButtons.Y, OVRInput.Button.Two);
            // ButtonMapping.Add(NVRButtons.Touchpad, OVRInput.Button.PrimaryThumbstick);
            // ButtonMapping.Add(NVRButtons.DPad_Up, OVRInput.Button.DpadUp);
            // ButtonMapping.Add(NVRButtons.DPad_Down, OVRInput.Button.DpadDown);
            // ButtonMapping.Add(NVRButtons.DPad_Left, OVRInput.Button.DpadLeft);
            // ButtonMapping.Add(NVRButtons.DPad_Right, OVRInput.Button.DpadRight);
            // ButtonMapping.Add(NVRButtons.Trigger, OVRInput.Button.PrimaryIndexTrigger);
            // ButtonMapping.Add(NVRButtons.Grip, OVRInput.Button.PrimaryHandTrigger);
            // ButtonMapping.Add(NVRButtons.System, OVRInput.Button.Back);
            // ButtonMapping.Add(NVRButtons.ApplicationMenu, OVRInput.Button.Start);
            
            // TouchMapping.Add(NVRButtons.A, OVRInput.Touch.One);
            // TouchMapping.Add(NVRButtons.B, OVRInput.Touch.Two);
            // TouchMapping.Add(NVRButtons.X, OVRInput.Touch.One);
            // TouchMapping.Add(NVRButtons.Y, OVRInput.Touch.Two);
            // TouchMapping.Add(NVRButtons.Touchpad, OVRInput.Touch.PrimaryThumbstick);
            // TouchMapping.Add(NVRButtons.Trigger, OVRInput.Touch.PrimaryIndexTrigger);
            
            // NearTouchMapping.Add(NVRButtons.Touchpad, OVRInput.NearTouch.PrimaryThumbButtons);
            // NearTouchMapping.Add(NVRButtons.Trigger, OVRInput.NearTouch.PrimaryIndexTrigger);

            // TriggerMapping.Add(NVRButtons.Grip, OVRInput.Axis1D.PrimaryHandTrigger);
            // TriggerMapping.Add(NVRButtons.Trigger, OVRInput.Axis1D.PrimaryIndexTrigger);

            // StickMapping.Add(NVRButtons.Touchpad, OVRInput.Axis2D.PrimaryThumbstick);
        

        private string GetButtonMap(NVRButtons button)
        {
            if (ButtonMapping.ContainsKey(button) == false)
            {
                //Debug.LogError("No Oculus button configured for: " + button.ToString());
                return "";
            }
            return ButtonMapping[button];
        }

        private string GetTouchMap(NVRButtons button)
        {
            if (TouchMapping.ContainsKey(button) == false)
            {
                //Debug.LogError("No Oculus touch map configured for: " + button.ToString());
                return null;
            }
            return TouchMapping[button];
        }

        private string GetNearTouchMap(NVRButtons button)
        {
            if (NearTouchMapping.ContainsKey(button) == false)
            {
                //Debug.LogError("No Oculus near touch map configured for: " + button.ToString());
                return null;
            }
            return NearTouchMapping[button];
        }

        private string GetTriggerMap(NVRButtons button)
        {
            if (TriggerMapping.ContainsKey(button) == false)
            {
                //Debug.LogError("No Oculus trigger map configured for: " + button.ToString());
                return null;
            }
            return TriggerMapping[button];
        }

        private string GetStickMap(NVRButtons button)
        {
            if (StickMapping.ContainsKey(button) == false)
            {
                //Debug.LogError("No Oculus stick map configured for: " + button.ToString());
                return null;
            }
            return StickMapping[button];
        }

        public override float GetAxis1D(NVRButtons button)
        {
                return Input.GetAxis(GetTriggerMap(button));
        }

        public override Vector2 GetAxis2D(NVRButtons button)
        {
                return Vector2.zero;
        }

        public override bool GetPressDown(NVRButtons button)
        {
                return Input.GetButtonDown(GetButtonMap(button));
        }

        public override bool GetPressUp(NVRButtons button)
        {
                return Input.GetButtonUp(GetButtonMap(button));
        }

        public override bool GetPress(NVRButtons button)
        {
                return Input.GetButton(GetButtonMap(button));
        }

        public override bool GetTouchDown(NVRButtons button)
        {
                return Input.GetButtonDown(GetTouchMap(button));
        }

        public override bool GetTouchUp(NVRButtons button)
        {
                return Input.GetButtonUp(GetTouchMap(button));
        }

        public override bool GetTouch(NVRButtons button)
        {
                return Input.GetButton(GetTouchMap(button));
        }

        public override bool GetNearTouchDown(NVRButtons button)
        {
                return Input.GetButtonDown(GetNearTouchMap(button));
        }

        public override bool GetNearTouchUp(NVRButtons button)
        {
                return Input.GetButtonUp(GetNearTouchMap(button));
        }

        public override bool GetNearTouch(NVRButtons button)
        {
                return Input.GetButton(GetNearTouchMap(button));
        }

        public override void TriggerHapticPulse(ushort durationMicroSec = 500, NVRButtons button = NVRButtons.Touchpad)
        {
            StartCoroutine(DoHapticPulse(durationMicroSec));
        }

        private IEnumerator DoHapticPulse(ushort durationMicroSec)
        {
            InputDevices.GetDeviceAtXRNode(Controller).SendHapticImpulse(0, 0.5f, durationMicroSec);
            float endTime = Time.time + ((float)durationMicroSec / 1000000);
            do {
                yield return null;
            } while (Time.time < endTime);
            InputDevices.GetDeviceAtXRNode(Controller).StopHaptics();
        }

        public override bool IsCurrentlyTracked
        {
            get
            {
                return InputDevices.GetDeviceAtXRNode(Controller).IsValid;
            }
        }


        public override GameObject SetupDefaultRenderModel()
        {
            return null;
        }

        public override bool ReadyToInitialize()
        {
            return true;
        }

        public override string GetDeviceName()
        {
            return InputDevices.GetDeviceAtXRNode(Controller).ToString();
        }

        public override Collider[] SetupDefaultPhysicalColliders(Transform ModelParent)
        {
            Collider[] Colliders = null;

            string name = "oculusTouch";
            if (Hand.IsLeft == true)
            {
                name += "Left";
            }
            else
            {
                name += "Right";
            }
            name += "Colliders";

            Transform touchColliders = ModelParent.transform.Find(name);
            if (touchColliders == null)
            {
                touchColliders = GameObject.Instantiate(Resources.Load<GameObject>("TouchControllers/" + name)).transform;
                touchColliders.parent = ModelParent.transform;
                touchColliders.localPosition = Vector3.zero;
                touchColliders.localRotation = Quaternion.identity;
                touchColliders.localScale = Vector3.one;
            }

            Colliders = touchColliders.GetComponentsInChildren<Collider>();

            return Colliders;
        }

        public override Collider[] SetupDefaultColliders()
        {
            Collider[] Colliders = null;
            
            SphereCollider OculusCollider = gameObject.AddComponent<SphereCollider>();
            OculusCollider.isTrigger = true;
            OculusCollider.radius = 0.15f;

            Colliders = new Collider[] { OculusCollider };

            return Colliders;
        }
        
    }
}
