namespace Flare.Alert.Tests;

using Flare.Model;
using Model;

[TestFixture]
public class UpdateAlertPriorityTests :
    FlareApiTesting
{
    [Test]
    public async Task Test()
    {
        var result = await GetContainerBuilder("TestData/StandardResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .UpdatePriority(NewId.NextGuid().ToString(), IdentifierType.Id, AlertPriority.P4);

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.Result!.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.333f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }
}