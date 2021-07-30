using System;
using UnityEngine.Analytics;

namespace UnityEditor.VspAnalytics
{
	public static class VspAnalytics
	{
		const int k_MaxEventsPerHour = 1000;
		const int k_MaxNumberOfElements = 1000;

		const string k_VendorKey = "unity.vsp-analytics";
		const string k_EventName = "vspAnalytics";

		static bool RegisterEvent()
		{
			AnalyticsResult result =
				EditorAnalytics.RegisterEventWithLimit(k_EventName, k_MaxEventsPerHour, k_MaxNumberOfElements, k_VendorKey);

			bool isResultOk = result == AnalyticsResult.Ok;
			return isResultOk;
		}

		[System.Serializable]
		struct VspAnalyticsData
		{
			public string actionName;
			public string partnerName;
			public string customerUid;
			public string extra;
		}

		/// <summary>
		/// Registers and attempts to send a VSP Analytics event.
		/// </summary>
		/// <param name="actionName">Name of the action, identifying a place this analytics event was called from.</param>
		/// <param name="partnerName">Identifiable Verified Solutions Partner name.</param>
		/// <param name="customerUid">Unique identifier of the customer using Partner's Verified Solution.</param>
		public static void SendAnalyticsEvent(string actionName, string partnerName, string customerUid)
		{
			try
			{
				VspDebug.Log($"SendAnalyticsEvent invoked with parameters: {actionName}, {partnerName}, {customerUid}");
				
				// Are Editor Analytics enabled ? (Preferences)
				// The event shouldn't be able to report if this is disabled but if we know we're not going to report
				// Lets early out and not waste time gathering all the data
				bool isEditorAnalyticsEnabled = EditorAnalytics.enabled;
				VspDebug.Log($"EditorAnalytics.enabled: {isEditorAnalyticsEnabled}");
				if (!isEditorAnalyticsEnabled)
					return;

				// Can an event be registered?
				bool isEventRegistered = RegisterEvent();
				VspDebug.Log($"RegisterEvent {actionName}: {isEventRegistered}");
				if (!isEventRegistered)
					return;

				// Create an expected data object
				var eventData = new VspAnalyticsData
				{
					actionName = actionName,
					partnerName = partnerName,
					customerUid = customerUid,
					extra = "{}"
				};

				// Send the Event and get the result
				AnalyticsResult result = EditorAnalytics.SendEventWithLimit(k_EventName, eventData);

				string logMessage = $"SendEventWithLimit returned result: {result}";
				if (result != AnalyticsResult.Ok)
					VspDebug.LogWarning(logMessage);
				else
					VspDebug.Log(logMessage);
			}
			catch (Exception e)
			{
				VspDebug.LogError($"Exception has occured in SendAnalytics\n{e}");
			}
		}
	}
}