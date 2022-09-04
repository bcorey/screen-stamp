using System;
using UnityEditor;
using UnityEngine;

namespace VirtualEscapes.Common
{
    public class EditorExtended : UnityEditor.Editor
    {
        protected void BeginDisableGroupIfPropertyIsFalse(string propertyName)
        {
            SerializedProperty serializedProperty = serializedObject.FindProperty(propertyName);
            EditorGUI.BeginDisabledGroup(!serializedProperty.boolValue);
        }

        protected void AddMaterialArray(string arrayName, SerializedObject obj)
        {
            int liCount = obj.FindProperty(arrayName + ".Array.size").intValue;
            for (int i = 0; i < liCount; i++)
            {
                SerializedProperty remappedMaterialProperty = obj.FindProperty(string.Format("{0}.Array.data[{1}]", arrayName, i));
                GUIContent labelContent = new GUIContent(remappedMaterialProperty.FindPropertyRelative("name").stringValue);
                EditorGUILayout.PropertyField(remappedMaterialProperty.FindPropertyRelative("material"), labelContent);
            }
        }

        protected void AddContent(string label, string propertyName)
        {
            GUIContent content = new GUIContent(label);
            SerializedProperty serializedProperty = serializedObject.FindProperty(propertyName);
            EditorGUILayout.PropertyField(serializedProperty, content);
        }

        protected void AddIntSlider(SerializedProperty serializedProperty, string label, int leftValue, int rightValue)
        {
            Rect ourRect = EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginProperty(ourRect, GUIContent.none, serializedProperty);
            EditorGUI.BeginChangeCheck();

            int selectionFromInspector = serializedProperty.intValue;
            int selectedValue = EditorGUILayout.IntSlider(label, selectionFromInspector, leftValue, rightValue);
            serializedProperty.intValue = selectedValue;

            EditorGUI.EndProperty();
            EditorGUILayout.EndHorizontal();
        }

        protected void AddPopup(SerializedProperty serializedProperty, GUIContent content, Type typeOfEnum, params GUILayoutOption[] options)
        {
            AddPopup(serializedProperty, content, stringArrayToGUIContentArray(Enum.GetNames(typeOfEnum)), options);
        }

        protected void AddPopup(SerializedProperty serializedProperty, string label, Type typeOfEnum, params GUILayoutOption[] options)
        {
            AddPopup(serializedProperty, label, Enum.GetNames(typeOfEnum), options);
        }

        protected void AddPopup(SerializedProperty serializedProperty, string label, string[] enumNames, params GUILayoutOption[] options)
        {
            AddPopup(serializedProperty, new GUIContent(label), stringArrayToGUIContentArray(enumNames), options);
        }

        private GUIContent[] stringArrayToGUIContentArray(string[] strings)
        {
            GUIContent[] laContents = new GUIContent[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                laContents[i] = new GUIContent(strings[i]);
            }
            return laContents;
        }

        protected void AddPopup(SerializedProperty serializedProperty, GUIContent content, GUIContent[] enumNames, params GUILayoutOption[] options)
        {
            Rect ourRect = EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginProperty(ourRect, GUIContent.none, serializedProperty);

            int selectionFromInspector = serializedProperty.intValue;

#if UNITY_2017_1_OR_NEWER
            int selectedValue = EditorGUILayout.Popup(content, selectionFromInspector, enumNames, options);
#else
            int[] laValues = new int[enumNames.Length];
            for (int i = 0; i < laValues.Length; i++)
            {
                laValues[i] = i;
            }
            EditorGUILayout.PrefixLabel(content);
            int selectedValue = EditorGUILayout.IntPopup(selectionFromInspector, enumNames, laValues, options);
#endif

            serializedProperty.intValue = selectedValue;

            EditorGUI.EndProperty();
            EditorGUILayout.EndHorizontal();
        }

        protected int AddPopup(GUIContent content, int value, string[] enumNames, params GUILayoutOption[] options)
        {
            return AddPopup(content, value, stringArrayToGUIContentArray(enumNames), options);
        }

        protected int AddPopup(GUIContent content, int value, GUIContent[] enumNames, params GUILayoutOption[] options)
        {
#if UNITY_2017_1_OR_NEWER
            int selectedValue = EditorGUILayout.Popup(content, value, enumNames, options);
#else
            int[] laValues = new int[enumNames.Length];
            for (int i = 0; i < laValues.Length; i++)
            {
                laValues[i] = i;
            }
            EditorGUILayout.PrefixLabel(content);
            int selectedValue = EditorGUILayout.IntPopup(value, enumNames, laValues, options);
#endif

            return selectedValue;
        }
    }
}