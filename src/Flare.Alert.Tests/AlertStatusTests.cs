namespace Flare.Alert.Tests;

using MassTransit;
using Model;

[TestFixture]
public class AlertStatusTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        var result = await GetContainerBuilder("TestData/AlertStatusResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Status(NewId.NextGuid());

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.Data, Is.Not.Null);
            Assert.That(result.Result.Data.Success, Is.True);
            Assert.That(result.Result.Data.Action, Is.EqualTo(AlertAction.Create));
            Assert.That(result.Result.Data.ProcessedAt, Is.EqualTo(DateTimeOffset.Parse("2017-05-24T14:24:20.844Z")));
            Assert.That(result.Result.Data.IntegrationId, Is.EqualTo(Guid.Parse("c9cec2cb-e782-4ebb-bc1d-1b2fa703cf03")));
            Assert.That(result.Result.Data.IsSuccess, Is.True);
            Assert.That(result.Result.Data.Status, Is.EqualTo("Created alert"));
            Assert.That(result.Result.Data.AlertId, Is.EqualTo(Guid.Parse("8743a1b2-11da-480e-8493-744660987bef").ToString()));
            Assert.That(result.Result.Data.Alias, Is.EqualTo(Guid.Parse("8743a1b2-11da-480e-8493-744660987bef").ToString()));
            Assert.That(result.Result.Took, Is.EqualTo(0.022f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("ec7e1d8e-1c75-442e-a271-731070a7fa4d")));
        });
    }
}