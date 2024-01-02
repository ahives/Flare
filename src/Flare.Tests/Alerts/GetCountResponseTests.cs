namespace Flare.Tests.Alerts;

using Microsoft.Extensions.DependencyInjection;

[TestFixture]
public class GetCountResponseTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        var result = await GetContainerBuilder("TestData/GetCountResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .Count(x =>
            {
                x.SearchIdentifier(NewId.NextGuid());
                x.SearchIdentifierType(IdentifierType.Id);
            });
        
        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Result.Data.Count, Is.EqualTo(7));
            Assert.That(result.Result.Took, Is.EqualTo(0.051f));
            Assert.That(result.Result.RequestId, Is.EqualTo(Guid.Parse("9ae63dd7-ed00-4c81-86f0-c4ffd33142c9")));
        });
    }
}