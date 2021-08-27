using UnityEditor;
using UnityEngine;

namespace UnityEngine.VspAnalytics
{
    internal static class VspDebug
    {
#if VSP_ANALYTICS_DEBUG
        private static readonly string k_VspPrefix = "[VSP Analytics] ";
#endif
        public static void Log(string message)
        {
#if VSP_ANALYTICS_DEBUG
            string finalMessage = k_VspPrefix + message;
            Debug.Log(finalMessage);
#endif
        }
        
        public static void LogWarning(string message)
        {
#if VSP_ANALYTICS_DEBUG
            string finalMessage = k_VspPrefix + message;
            Debug.LogWarning(finalMessage);
#endif
        }
        
        public static void LogError(string message)
        {
#if VSP_ANALYTICS_DEBUG
            string finalMessage = k_VspPrefix + message;
            Debug.LogError(finalMessage);
#endif
        }
    }
}