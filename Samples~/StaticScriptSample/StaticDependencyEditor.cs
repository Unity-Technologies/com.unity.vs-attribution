using UnityEditor;
using UnityEngine;
using UnityEngine.VspAttribution;

public class StaticScriptSample : EditorWindow
{
    static readonly Vector2 s_WindowSize = new Vector2(320,330);
    
    public string actionName;
    public string partnerName;
    public string customerUid;

    [MenuItem("VSP-Samples/Static Script Information")]
    public static void Initialize()
    {
        var window = GetWindow<StaticScriptSample>();
        
        window.titleContent = new GUIContent("Static Script Information!");
        window.minSize = s_WindowSize;
        window.maxSize = s_WindowSize;
    }

    public void OnGUI()
    {
        // Implementation information
        DrawLine(Color.gray);
        
        DrawHelpBox("<b><size=11>To implement VSP Attribution service:</size></b>\n" +
                    "1. Import <color=#ff00ffff>VSPAttribution</color> script into your package\n\n" +
                    "2. Change the namespace to something else, like:\n" +
                    "namespace <color=#ff00ffff>UnityEngine.VspAttribution.PartnerName</color> \n\n" +
                    "3. Add <color=#ff00ffff>SendAttributionEvent</color> call to your login\n\n" +
                    "or initialization method.", 7);

        DrawLine(Color.gray);
        
        // VSP Attribution API
        GUILayout.Label("VSP Attribution API", EditorStyles.boldLabel);
        
        actionName = EditorGUILayout.TextField("Action Name", actionName);
        partnerName = EditorGUILayout.TextField("VSP Partner Name", partnerName);
        customerUid = EditorGUILayout.TextField("VSP Customer UID", customerUid);

        GUILayout.Space(20f);

        if(GUILayout.Button("Send Attribution Event"))
        {
            var result = VspAttribution.SendAttributionEvent(actionName, partnerName, customerUid);
            Debug.Log($"[VSP Attribution] Attribution Event returned status: {result}!");
        }

        DrawHelpBox("Calls VspAttribution.SendAttributionEvent(string actionName, \n" +
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
