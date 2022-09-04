using UnityEngine;
using UnityEngine.UI;

namespace VirtualEscapes.Common.Examples
{
    [RequireComponent(typeof(Dimmer))]
    public class DimmerUIExample : MonoBehaviour
    {
        public Text descriptionText;
        public Slider slider;
        public Text valueText;

        private float mfSliderValueCache;

        void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

#if !UNITY_EDITOR && !UNITY_STANDALONE
            string lsDevice;
    #if UNITY_IOS
                lsDevice = "iOS";
    #elif UNITY_ANDROID
                lsDevice = "Android";
    #endif
            // descriptionText.text = "Adjust the screen brightness of your " + lsDevice + " device using the slider.";
#else
            Debug.LogWarning("This example won't do anything in the Editor. To see Dimmer in action, it must be built to a mobile device.");
#endif

            //Dimmer will either set the initial screen brightness from Dimmer.initialBrightness, or it will read the brightness from the device
            //either way, we read this value back and assign it to our slider
            float lfBrightness = GetComponent<Dimmer>().brightness;
            slider.value = lfBrightness;
            mfSliderValueCache = -1;
        }

        void Update()
        {
            //set brightness using Dimmer.brightness
            if (!Mathf.Approximately(slider.value, mfSliderValueCache))
            {
                GetComponent<Dimmer>().brightness = slider.value;
                //valueText.text = slider.value.ToString("0.#");
            }

            mfSliderValueCache = slider.value;
        }

        public void DeviceBrightnessChanged()
        {
            slider.value = GetComponent<Dimmer>().brightness;
        }
    }
}
