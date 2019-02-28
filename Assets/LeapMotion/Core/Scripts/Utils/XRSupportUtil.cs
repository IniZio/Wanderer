/******************************************************************************
 * Copyright (C) Leap Motion, Inc. 2011-2018.                                 *
 * Leap Motion proprietary and confidential.                                  *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Leap Motion and you, your company or other organization.           *
 ******************************************************************************/

using UnityEngine;

#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR;
#else
using UnityEngine.VR;
#endif

namespace Leap.Unity {

  /// <summary>
  /// Wraps various (but not all) "XR" calls with Unity 5.6-supporting "VR" calls
  /// via #ifdefs.
  /// </summary>
  public static class XRSupportUtil {

    public static bool IsXREnabled() {
      #if UNITY_2017_2_OR_NEWER
      return XRSettings.enabled;
      #else
      return UnityEngine.XR.XRSettings.enabled;
      #endif
    }

    public static bool IsXRDevicePresent() {
      #if UNITY_2017_2_OR_NEWER
      return XRDevice.isPresent;
      #else
      return UnityEngine.XR.XRDevice.isPresent;
      #endif
    }

    static bool outputPresenceWarning = false;
    public static bool IsUserPresent(bool defaultPresence = true) {
      #if UNITY_2017_2_OR_NEWER
      var userPresence = XRDevice.userPresence;
      if (userPresence == UserPresenceState.Present) {
        return true;
      } else if (!outputPresenceWarning && userPresence == UserPresenceState.Unsupported) {
        Debug.LogWarning("XR UserPresenceState unsupported (XR support is probably disabled).");
        outputPresenceWarning = true;
      }
      #else
      if (!outputPresenceWarning){
        Debug.LogWarning("XR UserPresenceState is only supported in 2017.2 and newer.");
        outputPresenceWarning = true;
      }
      #endif
      return defaultPresence;
    }

    public static Vector3 GetXRNodeCenterEyeLocalPosition() {
      #if UNITY_2017_2_OR_NEWER
      return InputTracking.GetLocalPosition(XRNode.CenterEye);
      #else
      return UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.CenterEye);
      #endif
    }

    public static Quaternion GetXRNodeCenterEyeLocalRotation() {
      #if UNITY_2017_2_OR_NEWER
      return InputTracking.GetLocalRotation(XRNode.CenterEye);
      #else
      return UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye);
      #endif
    }

    public static Vector3 GetXRNodeHeadLocalPosition() {
      #if UNITY_2017_2_OR_NEWER
      return InputTracking.GetLocalPosition(XRNode.Head);
      #else
      return UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.Head);
      #endif
    }

    public static Quaternion GetXRNodeHeadLocalRotation() {
      #if UNITY_2017_2_OR_NEWER
      return InputTracking.GetLocalRotation(XRNode.Head);
      #else
      return UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head);
      #endif
    }

    public static void Recenter() {
      UnityEngine.XR.InputTracking.Recenter();
    }

    public static string GetLoadedDeviceName() {
      #if UNITY_2017_2_OR_NEWER
      return XRSettings.loadedDeviceName;
      #else
      return UnityEngine.XR.XRSettings.loadedDeviceName;
      #endif
    }

    public static bool IsRoomScale() {
      #if UNITY_2017_2_OR_NEWER
      return XRDevice.GetTrackingSpaceType() == TrackingSpaceType.RoomScale;
      #else
      return UnityEngine.XR.XRDevice.GetTrackingSpaceType() == UnityEngine.XR.TrackingSpaceType.RoomScale;
      #endif
    }

    public static float GetGPUTime() {
      float gpuTime = 0f;
      #if UNITY_5_6_OR_NEWER
      #if UNITY_2017_2_OR_NEWER
      UnityEngine.XR.XRStats.TryGetGPUTimeLastFrame(out gpuTime);
      #else
      UnityEngine.VR.VRStats.TryGetGPUTimeLastFrame(out gpuTime);
      #endif
      #else
      gpuTime = UnityEngine.XR.XRStats.gpuTimeLastFrame;
      #endif
      return gpuTime;
    }

  }

}
