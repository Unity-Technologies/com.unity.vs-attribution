using System;
using UnityEngine.Analytics;

namespace UnityEditor.VspAnalytics
{
	public static class VspAnalytics
	{
		const int k_MaxEventsPerHour = 1000;
		const int k_MaxNumberOfElements = 1000;

		const string k_VendorKey = "unity.vsp-analytics";

		static bool RegisterEvent(string eventName)
		{
			AnalyticsResult result =
				EditorAnalytics.RegisterEventWithLimit(eventName, k_MaxEventsPerHour, k_MaxNumberOfElements, k_VendorKey);

			bool isResultOk = result == AnalyticsResult.Ok;
			VspDebug.Log($"RegisterEvent's AnalyticsResult is {result}");

			return isResultOk;
		}

		[System.Serializable]
		struct VspAnalyticsData
		{
			public string eventName;
			public string partnerName;
			public string customerUid;
			public string extra;
		}

		/// <summary>
		/// Registers and attempts to send a VSP Analytics event.
		/// </summary>
		/// <param name="eventName">Name of the event, identifying a place this analytics event was called from.</param>
		/// <param name="partnerName">Identifiable Verified Solutions Partner name.</param>
		/// <param name="customerUid">Unique identifier of the customer using Partner's Verified Solution.</param>
		public static void SendAnalyticsEvent(string eventName, string partnerName, string customerUid)
		{
			try
			{
				VspDebug.Log($"SendAnalyticsEvent invoked with parameters: {eventName}, {partnerName}, {customerUid}");
				
				// Are Editor Analytics enabled ? (Preferences)
				// The event shouldn't be able to report if this is disabled but if we know we're not going to report
				// Lets early out and not waste time gathering all the data
				bool isEditorAnalyticsEnabled = EditorAnalytics.enabled;
				VspDebug.Log($"EditorAnalytics.enabled: {isEditorAnalyticsEnabled}");
				if (!isEditorAnalyticsEnabled)
					return;

				// Can an event be registered?
				bool isEventRegistered = RegisterEvent(eventName);
				VspDebug.Log($"RegisterEvent(eventName): {isEventRegistered}");
				if (!isEventRegistered)
					return;

				// Create an expected data object
				var eventData = new VspAnalyticsData
				{
					eventName = eventName,
					partnerName = partnerName,
					customerUid = customerUid,
					extra = "{}"
				};

				// Send the Event and get the result
				AnalyticsResult result = EditorAnalytics.SendEventWithLimit(eventName, eventData);

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