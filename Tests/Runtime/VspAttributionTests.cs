using NUnit.Framework;
using UnityEngine.Analytics;

namespace UnityEngine.VspAttribution.Tests
{
    public class VspAttributionTests
    {
        [SetUp]
        public void Setup()
        {
            Analytics.Analytics.enabled = true;
            Analytics.Analytics.limitUserTracking = false;
            Analytics.Analytics.ResumeInitialization();
        }
        
        [Test]
        public void SendAttributionEvent_Returns_Ok()
        {
            AnalyticsResult result = VspAttribution.SendAttributionEvent("testAction", "testPartner", "testCustomerUid");
            Assert.AreEqual(AnalyticsResult.Ok, result);
        }
    }
}