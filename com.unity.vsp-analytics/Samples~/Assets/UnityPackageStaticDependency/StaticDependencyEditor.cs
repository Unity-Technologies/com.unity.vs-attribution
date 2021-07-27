using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StaticDependencyEditor : EditorWindow
{
    static readonly Vector2 s_WindowSize = new Vector2(300, 200);

    #region GUI
    [MenuItem("VSP-Samples/Static Dependency Example")]
    public static void Initialize()
    {
        var window = GetWindow<StaticDependencyEditor>();
        
        window.titleContent = new GUIContent("Static Dependency Sample!");
        window.minSize = s_WindowSize;
        window.maxSize = s_WindowSize;
    }

    public void OnGUI()
    {
        DrawLine(Color.gray);
        
        DrawHelpBox("This Sample exports everything from 'Assets' and " +
                    "'Packages/VSP Analytics' folders.\n\n" +
                    "Custom exporting (or A$ Tools) is needed to include VSP " +
                    "Analytics package, since Unity's default exporter " +
                    "does not handle package dependencies.\n\n" +
                    "Exported .unitypackage will appear in the project's " +
                    "directory.", 9);
        
        GUILayout.Space(20f);
        
        if(GUILayout.Button("Export with VSP Analytics"))
        {
            Export();
        }
    }
    #endregion

    #region Custom Exporting
    private void Export()
    {
        // TODO: Change "Assets" to your SDK path
        string[] assetPaths = {"Assets", "Packages/com.unity.vsp-analytics" };

        // Export your SDK with VSP Analytics dependencies into a .unitypackage
        AssetDatabase.ExportPackage(assetPaths, "VspAnalyticsDependency.unitypackage",
            ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
    }
    #endregion
    
    #region Utilities
    private void DrawHelpBox(string text, int linesCount)
    {
        EditorGUILayout.SelectableLabel(text, EditorStyles.helpBox, GUILayout.Height(linesCount*14f));
    }

    private void DrawLine(Color color, int thickness = 2, int padding = 7)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding*2+thickness));
        r.height = thickness;
        r.y+=padding;
        r.x-=2;
        r.width +=6;
        EditorGUI.DrawRect(r, color);
    }
    #endregion
}
