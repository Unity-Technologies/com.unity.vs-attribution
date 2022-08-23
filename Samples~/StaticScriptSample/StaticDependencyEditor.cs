using UnityEditor;
using UnityEngine;
using UnityEngine.VSAttribution;

public class StaticScriptSample : EditorWindow
{
    static readonly Vector2 s_WindowSize = new Vector2(320,330);
    
    public string actionName;
    public string partnerName;
    public string customerUid;

    [MenuItem("VS-Samples/Static Script Information")]
    public static void Initialize()
    {
        var window = GetWindow<StaticScriptSample>();
        
        window.titleContent = new GUIContent("Static Script Information");
        window.minSize = s_WindowSize;
        window.maxSize = s_WindowSize;
    }

    public void OnGUI()
    {
        // Implementation information
        DrawLine(Color.gray);
        
        DrawHelpBox("<b><size=11>To implement VS Attribution service:</size></b>\n" +
                    "1. Import <b>VSAttribution</b> script into your package\n\n" +
                    "2. Change the namespace to something else, like:\n" +
                    "namespace <b>UnityEngine.VSAttribution.PartnerName</b> \n\n" +
                    "3. Add <b>SendAttributionEvent</b> call to your login\n" +
                    "or initialization method.", 8);

        DrawLine(Color.gray);
        
        // VS Attribution API
        GUILayout.Label("VS Attribution API", EditorStyles.boldLabel);
        
        actionName = EditorGUILayout.TextField("Action Name", actionName);
        partnerName = EditorGUILayout.TextField("VS Partner Name", partnerName);
        customerUid = EditorGUILayout.TextField("VS Customer UID", customerUid);

        GUILayout.Space(20f);

        if (GUILayout.Button("Send Attribution Event"))
        {
            var result = VSAttribution.SendAttributionEvent(actionName, partnerName, customerUid);
            Debug.Log($"[VS Attribution] Attribution Event returned status: {result}!");
        }

        DrawHelpBox("Calls VSAttribution.SendAttributionEvent(string actionName, \n" +
                    "string partnerName, string customerUid)\n\n" +
                    "Include this call in your Login / Initialization code.", 4);
        
    }

    #region Utilities
    private void DrawHelpBox(string text, int linesCount)
    {
        GUIStyle helpBox = new GUIStyle(EditorStyles.helpBox) {richText = true};
        EditorGUILayout.SelectableLabel(text, helpBox, GUILayout.Height(linesCount*14f));
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