namespace Flare.Alert.Tests;

using Flare.Model;

[TestFixture]
public class EscalateAlertTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        var result = await GetContainerBuilder("TestData/EscalateAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Escalate(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.Escalation(e => e.Id(NewId.NextGuid()));
                x.User("Flare");
                x.Source("Flare");
                x.Note("");
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            Assert.That(result.Result.Result, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result.Took, Is.EqualTo(0.456f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("43a29c5c-3dbf-4fa4-9c26-f4f71023e120")));
        });
    }
}