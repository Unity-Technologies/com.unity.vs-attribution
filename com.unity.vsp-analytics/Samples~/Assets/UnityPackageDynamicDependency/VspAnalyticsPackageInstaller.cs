using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

#if !VSP_ANALYTICS_ENABLED
public class VspAnalyticsPackageInstaller
{
    private static AddRequest addRequest;
 
    private static readonly string VSP_ANALYTICS_GIT_URL = "https://github.com/Unity-Technologies/com.unity.vsp-analytics.git";
    
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        addRequest = Client.Add(VSP_ANALYTICS_GIT_URL);
        EditorApplication.update += OnEditorUpdateForAddRequest;
    }
    
    private static void OnEditorUpdateForAddRequest()
    {
        if (addRequest.IsCompleted)
        {
            if (addRequest.Status == StatusCode.Success)
            {
                // Success, package has been registered and should be downloaded
            }
            EditorApplication.update -= OnEditorUpdateForAddRequest;
        }
    }
}
#endif

