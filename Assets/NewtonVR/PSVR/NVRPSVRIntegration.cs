#if UNITY_PS4
using UnityEngine.PS4.VR;
using UnityEngine.PS4;
#endif
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

#if NVR_PSVR
namespace NewtonVR {
    public class NVRPSVRIntegration : NVRIntegration {

        public override void Initialize(NVRPlayer player) {
            Player = player;
            UnityEngine.XR.InputTracking.Recenter();

        }

        private Vector3 PlayspaceBounds = Vector3.zero;
        public override Vector3 GetPlayspaceBounds() {
            PlayspaceBounds.y = 1;
            PlayspaceBounds.z = 3;
            PlayspaceBounds.x = 2;
            Debug.Log("Integration is PSVR BOUNDS");
            return PlayspaceBounds;
        }

        public override bool IsHmdPresent() {
            Debug.Log("is HMD getting calld?");
            return UnityEngine.XR.XRDevice.isPresent;
            //  return PlayStationVR.hmdMount != VRHmdMountStatus.NotInitialized;

        }

    

    }
}
#else
namespace NewtonVR
{
    public class NVRPSVRIntegration : NVRIntegration
    {
        public override void Initialize(NVRPlayer player)
        {
        }

        public override Vector3 GetPlayspaceBounds()
        {
            return Vector3.zero;
        }

        public override bool IsHmdPresent()
        {
            return false;
        }
    }
}
#endif