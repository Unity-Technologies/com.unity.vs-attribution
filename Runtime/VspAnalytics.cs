using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

public static class VspAnalytics
{
	const int k_MaxEventsPerHour = 1000;
	const int k_MaxNumberOfElements = 1000;

	const string k_VendorKey = "unity.vsp-analytics";

	static bool RegisterEvent(string eventName)
	{
		AnalyticsResult result =
			EditorAnalytics.RegisterEventWithLimit(eventName, k_MaxEventsPerHour, k_MaxNumberOfElements, k_VendorKey);

		if (result == AnalyticsResult.Ok)
			return true;

		return false;
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
	/// Registers and attemps to send a VSP Analytics event.
	/// </summary>
	/// <param name="eventName">Name of the event, identifying a place this analytics event was called from</param>
	/// <param name="partnerName">Identifiable Verified Solutions Partner name</param>
	/// <param name="customerUid">Verified Solutions Partner's customer unique identifier</param>
	public static void SendAnalyticsEvent(string eventName, string partnerName, string customerUid = null)
	{
		// Are Editor Analytics enabled ? (Preferences)
		// The event shouldn't be able to report if this is disabled but if we know we're not going to report
		// Lets early out and not waste time gathering all the data
		if (!EditorAnalytics.enabled)
			return;

		if (!RegisterEvent(eventName))
			return;

		// Create an expected data object
		var eventData = new VspAnalyticsData
		{
			eventName = eventName,
			partnerName = partnerName,
			customerUid = customerUid,
			extra = "{}"
		};

		// Set the EndPoint
		EditorAnalytics.SendEventWithLimit(eventName, eventData);

		// Send the Event and get the result
		AnalyticsResult result = EditorAnalytics.SendEventWithLimit(eventName, eventData, 1);

		// Fail/succeed silently as we are not handling results
	}
}