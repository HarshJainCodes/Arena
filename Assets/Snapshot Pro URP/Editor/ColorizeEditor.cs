namespace SnapshotShaders.URP
{
    using UnityEditor.Rendering;
    using UnityEngine.Rendering.Universal;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [VolumeComponentEditor(typeof(ColorizeSettings))]
    public class ColorizeEditor : VolumeComponentEditor
    {
        SerializedDataParameter tintColor;

        public override void OnEnable()
        {
            var o = new PropertyFetcher<ColorizeSettings>(serializedObject);
            tintColor = Unpack(o.Find(x => x.tintColor));
        }

        public override void OnInspectorGUI()
        {
            if (!SnapshotUtility.CheckEffectEnabled<Colorize>())
            {
                EditorGUILayout.HelpBox("The Colorize effect must be added to your renderer's Renderer Features list.", MessageType.Error);
                if (GUILayout.Button("Add Colorize Renderer Feature"))
                {
                    SnapshotUtility.AddEffectToPipelineAsset<Colorize>();
                }
            }

            PropertyField(tintColor);
        }

#if UNITY_2021_2_OR_NEWER
        public override GUIContent GetDisplayTitle()
        {
            return new GUIContent("Colorize");
        }
#else
    public override string GetDisplayTitle()
    {
        return "Colorize";
    }
#endif
    }
}
