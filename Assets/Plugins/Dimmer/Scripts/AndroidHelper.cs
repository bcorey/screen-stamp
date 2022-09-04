using UnityEngine;

namespace VirtualEscapes.Common.Android
{
    public static class AndroidHelper
    {
        private static AndroidJavaObject sCurrentActivity = null;

        public static AndroidJavaObject GetCurrentActivity()
        {
            if (sCurrentActivity == null)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                sCurrentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return sCurrentActivity;
        }
    }
}
