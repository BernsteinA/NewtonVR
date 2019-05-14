using UnityEngine;
using System.Collections;
using NewtonVR;
using System.Collections.Generic;
using Valve.VR;
using System;
#if UNITY_PS4
using UnityEngine.PS4;
using UnityEngine.PS4.VR;
#endif

namespace NewtonVR {
    public class NVRPSVRInputDevice : NVRInputDevice {

#if UNITY_PS4
        PlayStationVRTrackingType trackingType = PlayStationVRTrackingType.Absolute;
        PlayStationVRTrackerUsage trackerUsageType = PlayStationVRTrackerUsage.OptimizedForHmdUser;
        PlayStationVRTrackingColor trackedColor;
        int handle;
        PlayStationVRTrackingStatus trackingStatus;
        Vector3 position;
        Quaternion orientation;

        private Dictionary<NVRButtons, string> ButtonMapping = new Dictionary<NVRButtons, string>(new NVRButtonsComparer());

        private IEnumerator vibratingCoroutine;


        private int whichController { get { return Hand.IsLeft ? 1 : 0; } }

        public override void Initialize(NVRHand hand) {
            base.Initialize(hand);
            SetupButtonMapping();
            trackingStatus = new PlayStationVRTrackingStatus();
            trackedColor = new PlayStationVRTrackingColor();

            if (hand.IsRight)
                StartCoroutine(RegisterHand(true));

            if (hand.IsLeft)
                StartCoroutine(RegisterHand(false));

        }

        protected virtual void SetupButtonMapping() {
            /*  ButtonMapping.Add(NVRButtons.Trigger, "TButton");
             ButtonMapping.Add(NVRButtons.PSMove, "MoveButton");
             ButtonMapping.Add(NVRButtons.triangle, "Fire4");
             ButtonMapping.Add(NVRButtons.X, "Fire1"); */

            ButtonMapping.Add(NVRButtons.X, "Button0");
            ButtonMapping.Add(NVRButtons.circle, "Button1");
            ButtonMapping.Add(NVRButtons.square, "Button2");
            ButtonMapping.Add(NVRButtons.triangle, "Button3");
            ButtonMapping.Add(NVRButtons.Trigger, "Button4");
            ButtonMapping.Add(NVRButtons.PSMove, "Button5");
            ButtonMapping.Add(NVRButtons.ApplicationMenu, "Button7");
        }

        private void Update() {
            if (handle >= 0) {
                Tracker.GetTrackedDeviceStatus(handle, out trackingStatus);

                if (Hand.transform) {
                    if (Tracker.GetTrackedDevicePosition(handle, out position) == PlayStationVRResult.Ok)
                        Hand.transform.localPosition = position;

                    if (Tracker.GetTrackedDeviceOrientation(handle, out orientation) == PlayStationVRResult.Ok)
                        Hand.transform.localRotation = orientation;
                }

            }
        }


        private IEnumerator RegisterHand(bool isPrimaryHand) {
            // right is zero left is one
            // primary hand is right
            yield return new WaitUntil(()=> PS4Input.MoveIsConnected(0, isPrimaryHand?0:1));

            var primaryhandles = new int[1];
            var secondaryhandles = new int[1];
            PS4Input.MoveGetUsersMoveHandles(1, primaryhandles, secondaryhandles);

            handle = isPrimaryHand ? primaryhandles[0] : secondaryhandles[0];

            var result = Tracker.RegisterTrackedDevice(PlayStationVRTrackedDevice.DeviceMove, handle, trackingType, trackerUsageType);

            if (result == PlayStationVRResult.Ok) {
                //TODO: May have to have a separate tracking status for every hand separately?
                //Look at psvr Unity example
                // var trackingStatus = new PlayStationVRTrackingStatus(); 
                while (trackingStatus == PlayStationVRTrackingStatus.NotStarted) {
                    Tracker.GetTrackedDeviceStatus(handle, out trackingStatus);
                    yield return null;
                }

                Tracker.GetTrackedDeviceLedColor(handle, out trackedColor);
            }
            else {
                Debug.LogError("Tracking failed for DeviceMove! This may be because you're trying to register too many devices at once.");
            }
        }


        public override bool IsCurrentlyTracked {
            get {
                return trackingStatus == PlayStationVRTrackingStatus.Tracking;
            }
        }

        private string GetButton(NVRButtons button) {
            if (ButtonMapping.ContainsKey(button) == false) {
                return "";
                //Debug.LogError("No SteamVR button configured for: " + button.ToString());
            }
            return ButtonMapping[button];
        }

        public override float GetAxis1D(NVRButtons button) {
            return 0f;
        }

        public override Vector2 GetAxis2D(NVRButtons button) {
            return Vector2.zero;
        }

        public override string GetDeviceName() {
            return trackedColor.ToString();
        }

        public override bool GetNearTouch(NVRButtons button) {
            return false;
        }

        public override bool GetNearTouchDown(NVRButtons button) {
            return false;
        }

        public override bool GetNearTouchUp(NVRButtons button) {
            return false;
        }

        public override bool GetPress(NVRButtons button) {
            /* if (Hand.IsRight) {
                return MoveButtonHeld(PS4Input.MoveGetButtons(0, 0), GetButton(button));
            }
            else if (Hand.IsLeft) {
                return MoveButtonHeld(PS4Input.MoveGetButtons(0, 1), GetButton(button));
            }
            else {

                return false;
            } */
            // return Input.GetButton(GetButton(button));
            if (Hand.IsLeft) {
                return Input.GetKey((KeyCode) Enum.Parse(typeof(KeyCode), "Joystick" + 6 + GetButton(button), true));
            }
            else if (Hand.IsRight) {
                return Input.GetKey((KeyCode) Enum.Parse(typeof(KeyCode), "Joystick" + 5 + GetButton(button), true));
            }

            return false;

        }

        public override bool GetPressDown(NVRButtons button) {
            //Debug.Log(button.ToString());
            //return Input.GetButtonDown(ButtonMapIntToString(GetButton(button)));

            // return Input.GetButtonDown(GetButton(button));
            if (Hand.IsLeft) {
                return Input.GetKeyDown((KeyCode) Enum.Parse(typeof(KeyCode), "Joystick" + 6 + GetButton(button), true));
            }
            else if (Hand.IsRight) {
                return Input.GetKeyDown((KeyCode) Enum.Parse(typeof(KeyCode), "Joystick" + 5 + GetButton(button), true));
            }

            return false;


        }

        public override bool GetPressUp(NVRButtons button) {

            if (Hand.IsLeft) {
                return Input.GetKeyUp((KeyCode) Enum.Parse(typeof(KeyCode), "Joystick" + 6 + GetButton(button), true));
            }
            else if (Hand.IsRight) {
                return Input.GetKeyUp((KeyCode) Enum.Parse(typeof(KeyCode), "Joystick" + 5 + GetButton(button), true));
            }

            return false;

        }

        public override bool GetTouch(NVRButtons button) {
            return false;
        }

        public override bool GetTouchDown(NVRButtons button) {
            return false;
        }

        public override bool GetTouchUp(NVRButtons button) {
            return false;
        }

        public override bool ReadyToInitialize() {
            return true;
        }

        public override Collider[] SetupDefaultColliders() {
            Collider[] Colliders = null;

            SphereCollider Collider = Hand.gameObject.AddComponent<SphereCollider>();
            Collider.isTrigger = true;
            Collider.radius = 0.15f;

            Colliders = new Collider[] { Collider };

            return Colliders;
        }

        public override Collider[] SetupDefaultPhysicalColliders(Transform ModelParent) {
            return null;
        }

        public override GameObject SetupDefaultRenderModel() {
            return null;
        }

        /// <summary>
        /// Trigger haptic pulse different than vive, calling ps4 coroutine
        /// </summary>
        /// <param name="durationMicroSec"></param>
        /// <param name="button"></param>
        public override void TriggerHapticPulse(ushort durationMicroSec = 500, NVRButtons button = NVRButtons.Touchpad) {
            if (vibratingCoroutine != null) {
                StopCoroutine(vibratingCoroutine);
            }
            vibratingCoroutine = Vibrate(durationMicroSec);
            StartCoroutine(vibratingCoroutine);
            // PS4Input.MoveSetVibration(0,whichController, 128);
        }

        /// <summary>
        /// Coroutine called when calling Trigger Haptic pulse for the ps4
        /// </summary>
        /// <param name="durationMicroSec"></param>
        /// <param name="intensity"></param>
        /// <returns></returns>
        private IEnumerator Vibrate(float durationMicroSec, int intensity = 64) {
            //Changes the intensity value depending on the value passed
            //Instead of yielding between times, have a constant stream of vibration
            //Coroutine stops in TriggerHaptic pulse if it's not called
            //Working as intended, but not sure if it's the best way to approach it, different than the vive
            if (durationMicroSec <= 200) {
                intensity = 64;
            }
            else if (durationMicroSec <= 400) {
                intensity = 128;
            }
            else if (durationMicroSec <= 600) {
                intensity = 150;
            }
            else if (durationMicroSec > 600) {
                intensity = 255;
            }
            PS4Input.MoveSetVibration(0, whichController, intensity);
            // Debug.Log("Calling vibrate from PS4 move before yield");
            yield return null; 
            PS4Input.MoveSetVibration(0, whichController, 0); //Not sure if this is called
            // yield return new WaitForSeconds(durationInSeconds);
            // PS4Input.MoveSetVibration(0, whichController, 0);
            // Debug.Log("Calling vibrate from PS4 after yield");

        }

#else
        public override bool IsCurrentlyTracked {
            get {
                throw new NotImplementedException();
            }
        }

        public override float GetAxis1D(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override Vector2 GetAxis2D(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override string GetDeviceName() {
            throw new NotImplementedException();
        }

        public override bool GetNearTouch(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override bool GetNearTouchDown(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override bool GetNearTouchUp(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override bool GetPress(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override bool GetPressDown(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override bool GetPressUp(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override bool GetTouch(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override bool GetTouchDown(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override bool GetTouchUp(NVRButtons button) {
            throw new NotImplementedException();
        }

        public override bool ReadyToInitialize() {
            throw new NotImplementedException();
        }

        public override Collider[] SetupDefaultColliders() {
            throw new NotImplementedException();
        }

        public override Collider[] SetupDefaultPhysicalColliders(Transform ModelParent) {
            throw new NotImplementedException();
        }

        public override GameObject SetupDefaultRenderModel() {
            throw new NotImplementedException();
        }

        public override void TriggerHapticPulse(ushort durationMicroSec = 500, NVRButtons button = NVRButtons.Touchpad) {
            throw new NotImplementedException();
        }
#endif
    }
}