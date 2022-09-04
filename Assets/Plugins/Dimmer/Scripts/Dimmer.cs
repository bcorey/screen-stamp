#if !UNITY_EDITOR
#if UNITY_ANDROID
#define COMPILE_ANDROID_PLUGIN
#elif UNITY_IOS
#define COMPILE_IOS_PLUGIN
#endif
#endif

using System;
using UnityEngine;
using UnityEngine.Events;

#if COMPILE_ANDROID_PLUGIN

using VirtualEscapes.Common.Android;

#elif COMPILE_IOS_PLUGIN

using System.Runtime.InteropServices;

#endif

namespace VirtualEscapes.Common
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Virtual Escapes/Dimmer")]
    public class Dimmer : MonoBehaviour
    {
        public float brightness;

        [Serializable]
        public class DimmerData
        {
            public bool overrideInitialBrightness;
            public bool restoreIOSBrightnessOnPause;
            public bool keepIOSBrightnessOnResume;
            public bool restoreIOSBrightnessOnDisable;
        }

        public UnityEvent systemBrightnessChangedEvent;

#if COMPILE_ANDROID_PLUGIN

        private AndroidJavaClass mScreenClass;

#elif COMPILE_IOS_PLUGIN

        [DllImport("__Internal")]
        private static extern float getScreenBrightness();
        [DllImport("__Internal")]
        private static extern void setScreenBrightness(float value);

#endif

        [SerializeField]
        private DimmerData mDimmerData = null;

        #region property getters/setters

        private DimmerData dimmerData
        {
            get
            {
                if (mDimmerData == null)
                {
                    mDimmerData = new DimmerData();
                }
                return mDimmerData;
            }
        }

        public bool overrideInitialBrightness
        {
            get { return dimmerData.overrideInitialBrightness; }
            set { dimmerData.overrideInitialBrightness = value; }
        }

        #endregion

#if !UNITY_EDITOR

        private float mfDeviceBrightness;
        private float mfBrightnessCache = -1;

        void Awake()
        {

#if COMPILE_ANDROID_PLUGIN

            mScreenClass = new AndroidJavaClass("com.virtualescapes.androidplugins.Screen");

#endif

            //read brightness

#if COMPILE_ANDROID_PLUGIN

                int liDeviceBrightness = getDeviceSettingsBrightness();
                mfDeviceBrightness = liDeviceBrightness / 255.0f;

#elif COMPILE_IOS_PLUGIN

                mfDeviceBrightness = getWindowBrightness();
#endif

            if (!dimmerData.overrideInitialBrightness)
            {
                 mfBrightnessCache = brightness = mfDeviceBrightness;
            }
        }

        void Update()
        {
            if (!Mathf.Approximately(brightness, mfBrightnessCache))
            {
                setWindowBrightness(brightness);
            }

            mfBrightnessCache = brightness;
        }

        void OnDisable()
        {
            if (dimmerData.restoreIOSBrightnessOnDisable)
            {
                setWindowBrightness(mfDeviceBrightness);
            }
        }


#if COMPILE_IOS_PLUGIN

        void OnApplicationPause(bool pauseState)
        {
            if (pauseState) //pausing
            {
                if (dimmerData.restoreIOSBrightnessOnPause)
                {
                    setWindowBrightness(mfDeviceBrightness);
                }
            }
            else //resuming
            {
                mfDeviceBrightness = getWindowBrightness();
                mfBrightnessCache = - 1; //force set brightness in update

                if (dimmerData.keepIOSBrightnessOnResume)
                {
                    brightness = mfDeviceBrightness; //update the app brightness too

                    if (systemBrightnessChangedEvent != null)
                    {
                        systemBrightnessChangedEvent.Invoke();
                    }
                }
             }
         }
#endif


        private float getWindowBrightness()
        {

#if COMPILE_ANDROID_PLUGIN

            return mScreenClass.CallStatic<float>("GetWindowBrightness", AndroidHelper.GetCurrentActivity());

#elif COMPILE_IOS_PLUGIN

            return getScreenBrightness();

#else

            return 0;

#endif

        }

#if COMPILE_ANDROID_PLUGIN

        private int getDeviceSettingsBrightness()
        {
            return mScreenClass.CallStatic<int>("GetDeviceBrightness", AndroidHelper.GetCurrentActivity());
        }

#endif

        /// <summary>
        /// Sets the window brightness.
        /// </summary>
        /// <param name="value">Value.</param>
        private void setWindowBrightness(float value)
        {

#if COMPILE_ANDROID_PLUGIN

            mScreenClass.CallStatic("SetWindowBrightness", AndroidHelper.GetCurrentActivity(), value);

#elif COMPILE_IOS_PLUGIN

            setScreenBrightness(value);

#endif

        }

#endif

    }
}
