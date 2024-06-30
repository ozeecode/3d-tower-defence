using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PanelUI), true)]
public class PanelUIEditor : Editor
{
    private PanelUI panel;
    private void OnEnable()
    {
        panel = (PanelUI)target;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Show"))
        {
            panel.Show();
        }
        if (GUILayout.Button("Hide"))
        {
            panel.Hide();
        }
        GUILayout.EndHorizontal();
    }
}
