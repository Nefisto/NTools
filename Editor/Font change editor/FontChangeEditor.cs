using TMPro;
using UnityEditor;
using UnityEngine;

namespace NTools
{
    public class FontChangerEditor : EditorWindow
    {
        private TMP_FontAsset tmpFont;

        private void OnGUI()
        {
            GUILayout.Label("Change All Fonts in Scene", EditorStyles.boldLabel);

            tmpFont = (TMP_FontAsset)EditorGUILayout.ObjectField("TextMeshPro Font", tmpFont, typeof(TMP_FontAsset),
                false);

            if (GUILayout.Button("Apply Fonts"))
                ChangeFonts();
        }

        [MenuItem("Tools/NTools/Font Changer")]
        public static void ShowWindow() => GetWindow<FontChangerEditor>("Font Changer");

        private void ChangeFonts()
        {
            var unityCount = 0;
            var tmpCount = 0;

            foreach (var tmpText in FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if (tmpFont != null)
                {
                    tmpText.font = tmpFont;
                    tmpCount++;
                    EditorUtility.SetDirty(tmpText);
                }

            Debug.Log($"Changed {unityCount} Unity UI fonts and {tmpCount} TextMeshPro fonts.");
        }
    }
}