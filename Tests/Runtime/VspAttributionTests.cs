using NUnit.Framework;
using UnityEngine.Analytics;

namespace UnityEngine.VspAttribution.Tests
{
    public class VspAttributionTests
    {
        [Test]
        public void SendAttributionEvent_Returns_Ok()
        {
            AnalyticsResult result = VspAttribution.SendAttributionEvent("testAction", "testPartner", "testCustomerUid");
            Assert.AreEqual(result, AnalyticsResult.Ok);
        }
    }
}