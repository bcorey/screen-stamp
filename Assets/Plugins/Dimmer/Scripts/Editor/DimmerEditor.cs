using UnityEngine;
using UnityEditor;

namespace VirtualEscapes.Common
{
    [CustomEditor(typeof(Dimmer))]
    public class DimmerEditor : EditorExtended
    {
        private const int VERSION_MAJOR = 1;
        private const int VERSION_MINOR = 4;
        private const int VERSION_REVISION = 0;
        private const string URL_VIRTUAL_ESCAPES = "http://www.virtualescapes.no";

        private SerializedProperty mSetInitialBrightnessProperty;
        private SerializedProperty mBrightnessProperty;

        private SerializedProperty mOnDisableActionProperty;

        private string[] maOnPauseOrDisableOptions = { "Keep App Brightness", "Restore System Brightness" };

#if UNITY_IOS

        private SerializedProperty mPauseActionProperty;
        private SerializedProperty mResumeActionProperty;
        private SerializedProperty mSystemBrightnessChangeEventProperty;

        private GUIContent mPauseActionContent;
        private GUIContent mResumeActionContent;

        private string[] maOnResumeOptions = { "Restore App Brightness", "Keep System Brightness" };

#endif

        private GUIContent mContentFooter;
        private GUIContent mHyperlink;
        private GUIContent mOnDisableActionContent;
        private GUIContent mOverrideInitialBrightnessContent;
        private GUIContent mInitialBrightnessContent;

        private string[] maInitialBrightnessOptions = { "Get Initial Brightness From Device", "Set Initial Brightness From Slider" };

        protected virtual void OnEnable()
        {
            SerializedProperty lDimmerDataProperty = serializedObject.FindProperty("mDimmerData");
            mSetInitialBrightnessProperty = lDimmerDataProperty.FindPropertyRelative("overrideInitialBrightness");
            mOnDisableActionProperty = lDimmerDataProperty.FindPropertyRelative("restoreIOSBrightnessOnDisable");

#if UNITY_IOS

            mPauseActionProperty = lDimmerDataProperty.FindPropertyRelative("restoreIOSBrightnessOnPause");
            mResumeActionProperty = lDimmerDataProperty.FindPropertyRelative("keepIOSBrightnessOnResume");
            mSystemBrightnessChangeEventProperty = serializedObject.FindProperty("systemBrightnessChangedEvent");

            mPauseActionContent = new GUIContent("On Pause Action", "Determines how Dimmer behaves when the application is paused (iOS only).");
            mResumeActionContent = new GUIContent("On Resume Action", "Determines how Dimmer behaves when the application is resumed after being paused (iOS only).");

#endif

            mBrightnessProperty = serializedObject.FindProperty("brightness");

            mContentFooter = new GUIContent(string.Format("Dimmer v{0}.{1}{2} © 2019",
                VERSION_MAJOR,
                VERSION_MINOR,
                VERSION_REVISION));
            mHyperlink = new GUIContent("Virtual Escapes");

            mOnDisableActionContent = new GUIContent("OnDisable Action", "Determines how Dimmer behaves when the component is disabled, for example when switching scenes.");
            mOverrideInitialBrightnessContent = new GUIContent("Awake Action", "Determines how Dimmer behaves when it first awakes");
            mInitialBrightnessContent = new GUIContent("Initial Brightness", "The initial brightness you want the screen to be.");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            bool lbSetInitialBrightness = popUpForBoolProperty(mSetInitialBrightnessProperty, mOverrideInitialBrightnessContent, maInitialBrightnessOptions);

            if (lbSetInitialBrightness)
            {
                EditorGUILayout.Slider(mBrightnessProperty, 0, 1, mInitialBrightnessContent);
            }
            popUpForBoolProperty(mOnDisableActionProperty, mOnDisableActionContent, maOnPauseOrDisableOptions);

#if UNITY_IOS

            EditorGUILayout.Space();
            GUILayout.Label("iOS", EditorStyles.miniLabel);
            popUpForBoolProperty(mPauseActionProperty, mPauseActionContent, maOnPauseOrDisableOptions);
            if (popUpForBoolProperty(mResumeActionProperty, mResumeActionContent, maOnResumeOptions))
            {
                EditorGUILayout.PropertyField(mSystemBrightnessChangeEventProperty);
            }

#endif

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUIStyle lMiniLabelStyle = EditorStyles.miniLabel;
            buttonFixedWidth(mContentFooter, lMiniLabelStyle);
            lMiniLabelStyle.normal.textColor = Color.gray;

            GUIStyle lMiniURLstyle = new GUIStyle(EditorStyles.miniLabel);
            lMiniURLstyle.normal.textColor = Color.blue;
            if (buttonFixedWidth(mHyperlink, lMiniURLstyle))
            {
                Application.OpenURL(URL_VIRTUAL_ESCAPES);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        private bool buttonFixedWidth(GUIContent content, GUIStyle style)
        {
            return GUILayout.Button(content, style, GUILayout.Width(style.CalcSize(content).x + 9 * EditorGUI.indentLevel));
        }

        private bool popUpForBoolProperty(SerializedProperty property, GUIContent content, string[] popupOptions)
        {
            Rect lRect = EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginProperty(lRect, GUIContent.none, property);
            bool lbResult = AddPopup(content, property.boolValue ? 1 : 0, popupOptions) != 0;
            property.boolValue = lbResult;
            EditorGUI.EndProperty();
            EditorGUILayout.EndHorizontal();

            return lbResult;
        }
    }
}