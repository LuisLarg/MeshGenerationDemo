using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshCreator))]
public class MeshCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshCreator meshCreator = target as MeshCreator;

        meshCreator.width = EditorGUILayout.FloatField("Width", meshCreator.width);
        meshCreator.height = EditorGUILayout.FloatField("Height", meshCreator.height);
        meshCreator.length = EditorGUILayout.FloatField("Length", meshCreator.length);

        if (GUILayout.Button("Generate Quad"))
            meshCreator.CreateQuadMesh();

        if (GUILayout.Button("Generate Cube"))
            meshCreator.CreateCubeMesh();
    }
}
