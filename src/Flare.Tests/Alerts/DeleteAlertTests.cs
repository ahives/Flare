namespace Flare.Tests.Alerts;

using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

public class DeleteAlertTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        var result = await GetContainerBuilder("TestData/DeleteAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Delete(NewId.NextGuid(), IdentifierType.AlertId, x =>
            {
                x.Source("");
                x.User("");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.204f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
            // var request = result.DebugInfo.Request.ToObject<CreateAlertRequest>();
        });
    }
}