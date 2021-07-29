using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.VspAnalytics;

public class StaticDependencyEditor : EditorWindow
{
    static readonly Vector2 s_WindowSize = new Vector2(300, 500);
    static readonly string s_VspAnalyticsAssetPath = "Packages/com.unity.vsp-analytics";
    
    static List<string> s_AssetPaths = new List<string> {"Assets"};
    
    public string eventName;
    public string partnerName;
    public string customerUid;

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
        #region Data Preparation
        bool vspAnalyticsFound = Directory.Exists(s_VspAnalyticsAssetPath);

        if (vspAnalyticsFound)
        {
            if (!s_AssetPaths.Contains(s_VspAnalyticsAssetPath))
                s_AssetPaths.Add(s_VspAnalyticsAssetPath);
        }
        else
        {
            if (s_AssetPaths.Contains(s_VspAnalyticsAssetPath))
                s_AssetPaths.Remove(s_VspAnalyticsAssetPath);
        }
        #endregion
        
        #region Exporting GUI
        DrawLine(Color.gray);
        
        DrawHelpBox("This Sample exports everything from 'Assets' and " +
                    "'Packages/VSP Analytics' folders.\n\n" +
                    "Custom exporting (or A$ Tools) is needed to include VSP " +
                    "Analytics package, since Unity's default exporter " +
                    "does not handle package dependencies.\n\n" +
                    "Exported .unitypackage will appear in the project's " +
                    "directory.", 9);
        
        GUILayout.Space(10f);
        
        GUILayout.Label("VSP Analytics found:", EditorStyles.boldLabel);
        GUILayout.Label(vspAnalyticsFound.ToString());
        
        GUILayout.Space(10f);
        
        GUILayout.Label($"Exporting:\n{string.Join( "\n", s_AssetPaths.ToArray())}");
        
        GUILayout.Space(10f);
        
        if(GUILayout.Button("Export"))
        {
            Export();
        }
        
        DrawLine(Color.gray);
        #endregion
        
        #region VSP Analytics API
        if (!vspAnalyticsFound)
            return;

        GUILayout.Label("VSP Analytics API", EditorStyles.boldLabel);
        
        eventName = EditorGUILayout.TextField("Event Name", eventName);
        partnerName = EditorGUILayout.TextField("VSP Partner Name", partnerName);
        customerUid = EditorGUILayout.TextField("VSP Customer UID", customerUid);

        GUILayout.Space(20f);

        if(GUILayout.Button("Send Analytics Event"))
        {
            VspAnalytics.SendAnalyticsEvent(eventName, partnerName, customerUid);
        }

        DrawHelpBox("Calls VspAnalytics.SendAnalyticsEvent(string eventName, \n" +
                    "string partnerName, string customerUid)\n\n" +
                    "Include this call in your Login / Initialization code.", 4);
        
        #endregion
    }
    #endregion

    #region Custom Exporting
    private void Export()
    {
        // Change "Assets" in s_AssetPaths to your SDK path
        // Export your SDK with VSP Analytics dependencies into a .unitypackage
        // If VSP Analytics are found within the Packages folder
        AssetDatabase.ExportPackage(s_AssetPaths.ToArray(), "VspAnalyticsDependency.unitypackage",
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
