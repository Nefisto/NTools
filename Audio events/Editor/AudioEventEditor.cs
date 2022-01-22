using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NTools
{
#if UNITY_EDITOR
    [CustomEditor(typeof(AudioEvent), true)]
    public class AudioEventEditor : Editor
    {
        [SerializeField]
        private AudioSource _previewer;

        protected void OnEnable()
        {
            _previewer = EditorUtility
                .CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource))
                .GetComponent<AudioSource>();
        }

        protected void OnDisable()
        {
            DestroyImmediate(_previewer.gameObject);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
            if (GUILayout.Button("Preview"))
            {
                ((AudioEvent)target).Play(_previewer);
            }

            EditorGUI.EndDisabledGroup();
        }
    }
#endif
}