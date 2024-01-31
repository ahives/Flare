namespace Flare.Alert.Tests;

using Flare.Model;

[TestFixture]
public class GetAllRecipientsTests :
    FlareApiTesting
{
    [Test]
    public async Task Test()
    {
        var result = await GetContainerBuilder("TestData/GetAllRecipientsResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .GetAllRecipients(NewId.NextGuid().ToString(), IdentifierType.Id);

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.HasFaulted, Is.False);
            // Assert.That(result.Result!.Data, Is.EqualTo("Request will be processed"));
            Assert.That(result.Result!.Took, Is.EqualTo(0.306f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("d90a7fb5-f5b6-4e1a-a1d1-0a37bded6a32")));
        });
    }
}