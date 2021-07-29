using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

// This file should not end up in your SDK - only for demonstration purposes
[InitializeOnLoad]
public static class DynamicPackageManagerUtility
{
    private static Request s_CurrentRequest;
    
    private const string k_VspAnalyticsPackageName = "com.unity.vsp-analytics";
    
    public static bool vspAnalyticsPackageFound = false;

    static DynamicPackageManagerUtility()
    {
        OnEnable();
    }

    private static void OnEnable()
    {
        FindVspAnalyticsPackage();
    }

    static void FindVspAnalyticsPackage()
    {
        s_CurrentRequest = Client.List();
        EditorApplication.update += ListRequestProgress;
    }

    public static void UninstallAnalyticsPackage()
    {
        s_CurrentRequest = Client.Remove(k_VspAnalyticsPackageName);
        EditorApplication.update += RemoveRequestProgress;
    }
    
    static void ListRequestProgress()
    {
        if (s_CurrentRequest.IsCompleted)
        {
            if (s_CurrentRequest.Status == StatusCode.Success)
            {
                List<PackageInfo> allPackagesFound = ((ListRequest) s_CurrentRequest).Result.ToList();
                vspAnalyticsPackageFound = allPackagesFound.Exists(x => x.name == k_VspAnalyticsPackageName);
            }

            EditorApplication.update -= ListRequestProgress;
        }
    }

    static void RemoveRequestProgress()
    {
        if (s_CurrentRequest.IsCompleted)
        {
            EditorApplication.update -= RemoveRequestProgress;
        }
    }
}
