namespace Flare.Alert.Tests;

using Flare.Model;

[TestFixture]
public class GetLogsTests :
    FlareApiTesting
{
    [Test]
    public async Task Test()
    {
        int offset = 5;
        int limit = 100;
        var orderType = OrderType.Asc;
        var result = await GetContainerBuilder("TestData/GetLogsResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .GetLogs(NewId.NextGuid().ToString(), IdentifierType.Id, x =>
            {
                x.Direction(PageDirection.Next);
                x.PaginationOffset(offset);
                x.PaginationLimit(limit);
                x.OrderBy(orderType);
            });

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