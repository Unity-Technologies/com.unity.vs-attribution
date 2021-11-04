using System;
using UnityEditor;
using UnityEngine.Analytics;

namespace UnityEngine.VspAttribution
{
	public static class VspAttribution
	{
		const int k_MaxEventsPerHour = 1000;
		const int k_MaxNumberOfElements = 1000;

		const string k_VendorKey = "unity.vsp-attribution";
		const string k_EventName = "vspAttribution";

		static bool RegisterEvent()
		{
			AnalyticsResult result = EditorAnalytics.RegisterEventWithLimit(k_EventName, k_MaxEventsPerHour,
				k_MaxNumberOfElements, k_VendorKey);

			bool isResultOk = result == AnalyticsResult.Ok;
			return isResultOk;
		}

		[Serializable]
		struct VspAttributionData
		{
			public string actionName;
			public string partnerName;
			public string customerUid;
			public string extra;
		}

		/// <summary>
		/// Registers and attempts to send a VSP Attribution event.
		/// </summary>
		/// <param name="actionName">Name of the action, identifying a place this event was called from.</param>
		/// <param name="partnerName">Identifiable Verified Solutions Partner name.</param>
		/// <param name="customerUid">Unique identifier of the customer using Partner's Verified Solution.</param>
		public static AnalyticsResult SendAttributionEvent(string actionName, string partnerName, string customerUid)
		{
			try
			{
				// Are Editor Analytics enabled ? (Preferences)
				// The event shouldn't be able to report if this is disabled but if we know we're not going to report
				// lets early out and not spend time gathering all the data
				bool isEditorAnalyticsEnabled = EditorAnalytics.enabled;

				if (!isEditorAnalyticsEnabled)
					return AnalyticsResult.AnalyticsDisabled;

				// Can an event be registered?
				bool isEventRegistered = RegisterEvent();
				
				if (!isEventRegistered)
					return AnalyticsResult.InvalidData;

				// Create an expected data object
				var eventData = new VspAttributionData
				{
					actionName = actionName,
					partnerName = partnerName,
					customerUid = customerUid,
					extra = "{}"
				};

				// Send the Attribution Event
				var eventResult = EditorAnalytics.SendEventWithLimit(k_EventName, eventData);
				return eventResult;
			}
			catch
			{
				// Fail silently
				return AnalyticsResult.AnalyticsDisabled;
			}
		}
	}
}