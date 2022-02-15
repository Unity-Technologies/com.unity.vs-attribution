using NUnit.Framework;
using UnityEngine.Analytics;

namespace UnityEngine.VspAttribution.Tests
{
    public class VspAttributionTests
    {
        [Test]
        public void SendAttributionEvent_Returns_AnalyticsDisabled()
        {
            AnalyticsResult result = VspAttribution.SendAttributionEvent("testAction", "testPartner", "testCustomerUid", false);
            Assert.AreEqual(AnalyticsResult.AnalyticsDisabled, result);
        }
    }
}