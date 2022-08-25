using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Generator))]
public class GeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        Generator mapGen = (Generator) target;

        if (DrawDefaultInspector()) {
            if (mapGen.autoUpdate) {
                mapGen.Generate();
            }
        }

        if (GUILayout.Button("Generate")) {
            mapGen.Generate();
        }
    }
}
