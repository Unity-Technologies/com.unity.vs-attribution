using UnityEditor;
using UnityEngine;

#if VSP_ANALYTICS_ENABLED
using UnityEditor.VspAnalytics;
#endif

public class DynamicDependencyEditor : EditorWindow
{
    public string eventName;
    public string partnerName;
    public string customerUid;
    
    static readonly Vector2 s_WindowSize = new Vector2(300, 400);

    #region GUI
    [MenuItem("VSP-Samples/Dynamic Dependency Example")]
    public static void Initialize()
    {
        var window = GetWindow<DynamicDependencyEditor>();
        
        window.titleContent = new GUIContent("Dynamic Dependency Sample!");
        window.minSize = s_WindowSize;
        window.maxSize = s_WindowSize;
    }
    
    private void OnGUI()
    {
        #region VSP_ANALYTICS_ENABLED define
        
#if VSP_ANALYTICS_ENABLED
        bool isUsingVspAnalyticsEnabled = true;
#else
        bool isUsingVspAnalyticsEnabled = false;
#endif// VSP_ANALYTICS_ENABLED
        
        #endregion

        #region Data Preparation
        
        // Is VSP Analytics package in the project?
        string isDownloadedText = DynamicPackageManagerUtility.vspAnalyticsPackageFound ? "Found!" : "Not Found!";
        
        // Is VSP_ANALYTICS_ENABLED define is enabled
        string defineText = isUsingVspAnalyticsEnabled ? "Enabled!" : "Disabled!";
        
        #endregion

        #region Custom Editor GUI
        
        DrawLine(Color.gray);
        
        GUILayout.Label("VSP Analytics package status:", EditorStyles.boldLabel);
        GUILayout.Label(isDownloadedText);
        DrawHelpBox("The package should be installed by " +
                    "VspAnalyticsPackageInstaller automatically.", 2);
        
        DrawLine(Color.gray);
        
        GUILayout.Label("VSP_ANALYTICS_ENABLED status:", EditorStyles.boldLabel);
        GUILayout.Label($"{defineText}");
        DrawHelpBox("For this to be Enabled, two conditions need to be fullfilled.\n" +
                    "1.Assembly Definition - reference to VSP Analytics and a Version Define\n" +
                    "2.VSP Analytics installed (VspAnalyticsPackageInstaller)", 4);
        
        DrawLine(Color.gray);

        // If VSP Analytics not downloaded - no option to send Analytics
        if (!DynamicPackageManagerUtility.vspAnalyticsPackageFound)
            return;
        
        GUILayout.Label("VSP Analytics API", EditorStyles.boldLabel);
        
        eventName = EditorGUILayout.TextField("Event Name", eventName);
        partnerName = EditorGUILayout.TextField("VSP Partner Name", partnerName);
        customerUid = EditorGUILayout.TextField("VSP Customer UID", customerUid);

        GUILayout.Space(20f);
        
        #endregion

        #region VSP Analytics API
        
        if(GUILayout.Button("Send Analytics Event"))
        {
#if VSP_ANALYTICS_ENABLED
            VspAnalytics.SendAnalyticsEvent(eventName, partnerName, customerUid);
#endif // VSP_ANALYTICS_ENABLED
        }

        DrawHelpBox("Calls VspAnalytics.SendAnalyticsEvent(string eventName, \n" +
                    "string partnerName, string customerUid)\n\n" +
                    "Include this call in your Login / Initialization code.", 4);
        
        #endregion
    }
    #endregion
    
    #region Utilities
    
    private void DrawHelpBox(string text, int linesCount)
    {
        EditorGUILayout.SelectableLabel(text, EditorStyles.helpBox, GUILayout.Height(linesCount*15f));
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
