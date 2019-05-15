using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NewtonVR
{
    public class NVRUnityXRIntegration : NVRIntegration
    {

        public override void Initialize(NVRPlayer player)
        {
            Player = player;
            // UnityEngine.XR.InputTracking.Recenter();
        }

        private Vector3 PlayspaceBounds = Vector3.one;
        public override Vector3 GetPlayspaceBounds()
        {
            return PlayspaceBounds;
        }

        public override bool IsHmdPresent()
        {
            return UnityEngine.XR.XRDevice.isPresent;
        }

    }
}