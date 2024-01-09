namespace Flare.Alert.Tests;

using Flare.Model;
using MassTransit;
using Model;

[TestFixture]
public class GetAllAlertTests :
    FlareApiTesting
{
    [Test]
    public async Task Test1()
    {
        int offset = 5;
        int limit = 100;
        var orderType = OrderType.Asc;
        var sortField = SortableFields.Status;
        string identifier = NewId.NextGuid().ToString();
        var identifierType = IdentifierType.Id;
        var result = await GetContainerBuilder("TestData/GetAllAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .GetAll(x =>
            {
                x.PaginationOffset(offset);
                x.PaginationLimit(limit);
                x.OrderBy(orderType);
                x.SortBy(sortField);
                x.SearchIdentifier(identifier);
                x.SearchIdentifierType(identifierType);
            });

        var expected1 = new AlertData
        {
            Id = Guid.Parse("70413a06-38d6-4c85-92b8-5ebc900d42e2"),
            TinyId = "1791",
            Alias = "event_573",
            Message = "Our servers are in danger",
            Status = AlertStatus.Closed,
            Acknowledged = false,
            IsSeen = true,
            Snoozed = true,
            SnoozedUntil = DateTimeOffset.Parse("2017-04-03T20:32:35.143Z"),
            LastOccurredAt = DateTimeOffset.Parse("2017-04-03T20:05:50.894Z"),
            CreatedAt = DateTimeOffset.Parse("2017-03-21T20:32:52.353Z"),
            UpdatedAt = DateTimeOffset.Parse("2017-04-03T20:32:57.301Z"),
            Count = 79,
            Source = "Isengard",
            Owner = "morpheus@opsgenie.com",
            Priority = AlertPriority.P4,
            Integration = new ApiIntegration
            {
                Id = Guid.Parse("4513b7ea-3b91-438f-b7e4-e3e54af9147c"),
                Name = "Nebuchadnezzar",
                Type = ApiIntegrationType.API
            },
            Report = new AlertReport
            {
                AckTime = 15702,
                CloseTime = 60503,
                AcknowledgedBy = "agent_smith@opsgenie.com",
                ClosedBy = "neo@opsgenie.com"
            },
            Responders = new List<Recipient>()
        };

        var expected2 = new AlertData
        {
            Id = Guid.Parse("70413a06-38d6-4c85-92b8-5ebc900d42e2"),
            TinyId = "1791",
            Alias = "event_573",
            Message = "Sample Message",
            Status = AlertStatus.Open,
            Acknowledged = false,
            IsSeen = false,
            Snoozed = false,
            LastOccurredAt = DateTimeOffset.Parse("2017-03-21T20:32:52.353Z"),
            CreatedAt = DateTimeOffset.Parse("2017-03-21T20:32:52.353Z"),
            UpdatedAt = DateTimeOffset.Parse("2017-04-03T20:32:57.301Z"),
            Count = 1,
            Source = "Zion",
            Priority = AlertPriority.P5,
            Integration = new ApiIntegration
            {
                Id = Guid.Parse("4513b7ea-3b91-b7e4-438f-e3e54af9147c"),
                Name = "My_Lovely_Amazon",
                Type = ApiIntegrationType.CloudWatch
            }
        };

        string searchIdentifierType = identifierType switch
        {
            IdentifierType.Id => "id",
            IdentifierType.Name => "name",
            _ => string.Empty
        };

        string sortFieldString = sortField switch
        {
            SortableFields.CreatedAt => "createdAt",
            SortableFields.UpdatedAt => "updatedAt",
            SortableFields.TinyId => "tinyId",
            SortableFields.Alias => "alias",
            SortableFields.Message => "message",
            SortableFields.Status => "status",
            SortableFields.Acknowledged => "acknowledged",
            SortableFields.IsSeen => "isSeen",
            SortableFields.Snoozed => "snoozed",
            SortableFields.SnoozedUntil => "snoozedUntil",
            SortableFields.Count => "count",
            SortableFields.LastOccurredAt => "lastOccurredAt",
            SortableFields.Source => "source",
            SortableFields.Owner => "owner",
            SortableFields.ReportClosedBy => "report.closedBy",
            SortableFields.ReportAcknowledgedBy => "report.acknowledgedBy",
            SortableFields.ReportCloseTime => "report.closeTime",
            SortableFields.ReportAckTime => "report.ackTime",
            SortableFields.IntegrationType => "integration.type",
            SortableFields.IntegrationName => "integration.name",
            _ => string.Empty
        };

        string orderTypeString = orderType switch
        {
            OrderType.Desc => "desc",
            OrderType.Asc => "asc",
            _ => string.Empty
        };

        Console.WriteLine(result.DebugInfo.Response);
        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.DebugInfo.URL, Is.EqualTo($"alerts?searchIdentifier={identifier}&searchIdentifierType={searchIdentifierType}&offset={offset}&limit={limit}&sort={sortFieldString}&order={orderTypeString}"));
            Assert.That(result.Result.Paging.Last, Is.EqualTo("https://api.opsgenie.com/v2/alerts?query=status%3Aopen&offset=100&limit=10&sort=createdAt&order=desc"));
            Assert.That(result.Result.Paging.First, Is.EqualTo("https://api.opsgenie.com/v2/alerts?query=status%3Aopen&offset=0&limit=10&sort=createdAt&order=desc"));
            Assert.That(result.Result.Paging.Next, Is.EqualTo("https://api.opsgenie.com/v2/alerts?query=status%3Aopen&offset=20&limit=10&sort=createdAt&order=desc"));

            AssertData(result.Result.Data[0], expected1);
            AssertData(result.Result.Data[1], expected2);

            // Assert.That(result.Data.Data, Is.EqualTo(""));
            // Assert.That(result.Data.Data, Is.EqualTo(""));
            // Assert.That(result.Data.Data, Is.EqualTo(""));
            // Assert.That(result.Data.Data, Is.EqualTo(""));
            // Assert.That(result.Data.Data, Is.EqualTo(""));
        });
    }

    [Test]
    public async Task Test2()
    {
        int offset = 5;
        int limit = 100;
        var orderType = OrderType.Asc;
        var sortField = SortableFields.Status;
        var status = AlertStatus.Open;
        var result = await GetContainerBuilder("TestData/GetAllAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .GetAll(x =>
            {
                x.Query(c =>
                {
                    c.Status(status);
                });
                x.PaginationOffset(offset);
                x.PaginationLimit(limit);
                x.OrderBy(orderType);
                x.SortBy(sortField);
            });

        string sortFieldString = sortField switch
        {
            SortableFields.CreatedAt => "createdAt",
            SortableFields.UpdatedAt => "updatedAt",
            SortableFields.TinyId => "tinyId",
            SortableFields.Alias => "alias",
            SortableFields.Message => "message",
            SortableFields.Status => "status",
            SortableFields.Acknowledged => "acknowledged",
            SortableFields.IsSeen => "isSeen",
            SortableFields.Snoozed => "snoozed",
            SortableFields.SnoozedUntil => "snoozedUntil",
            SortableFields.Count => "count",
            SortableFields.LastOccurredAt => "lastOccurredAt",
            SortableFields.Source => "source",
            SortableFields.Owner => "owner",
            SortableFields.ReportClosedBy => "report.closedBy",
            SortableFields.ReportAcknowledgedBy => "report.acknowledgedBy",
            SortableFields.ReportCloseTime => "report.closeTime",
            SortableFields.ReportAckTime => "report.ackTime",
            SortableFields.IntegrationType => "integration.type",
            SortableFields.IntegrationName => "integration.name",
            _ => string.Empty
        };

        string orderTypeString = orderType switch
        {
            OrderType.Desc => "desc",
            OrderType.Asc => "asc",
            _ => string.Empty
        };

        string alertStatus = status switch
        {
            AlertStatus.Open => "open",
            AlertStatus.Closed => "close",
            _ => string.Empty
        };

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.DebugInfo.URL, Is.EqualTo($"alerts?query=status:{alertStatus}&offset={offset}&limit={limit}&sort={sortFieldString}&order={orderTypeString}"));
        });
    }

    [Test]
    public async Task Test3()
    {
        var result = await GetContainerBuilder("TestData/GetAllAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .GetAll(x =>
            {
                x.PaginationOffset(5);
                x.PaginationLimit(100);
                x.OrderBy(OrderType.Asc);
                x.SortBy(SortableFields.Status);
                x.SearchIdentifier(NewId.NextGuid().ToString());
                x.SearchIdentifierType(IdentifierType.Name);
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.Result, Is.Null);
            Assert.That(result.HasFaulted, Is.True);
        });
    }

    [Test]
    public async Task Test4()
    {
        var result = await GetContainerBuilder("TestData/GetAllAlertResponse.json")
            .BuildServiceProvider()
            .GetService<IFlareClient>()!
            .API<Alert>()
            .GetAll(x =>
            {
                x.PaginationOffset(-1);
                x.PaginationLimit(100);
                x.OrderBy(OrderType.Asc);
                x.SortBy(SortableFields.Status);
                x.SearchIdentifier(NewId.NextGuid().ToString());
                x.SearchIdentifierType(IdentifierType.Id);
            });

        Assert.Multiple(() =>
        {
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.Result, Is.Null);
            Assert.That(result.HasFaulted, Is.True);
        });
    }

    void AssertData(AlertData actual, AlertData expected)
    {
        Assert.That(actual.Id, Is.EqualTo(expected.Id));
        Assert.That(actual.TinyId, Is.EqualTo(expected.TinyId));
        Assert.That(actual.Alias, Is.EqualTo(expected.Alias));
        Assert.That(actual.Message, Is.EqualTo(expected.Message));
        Assert.That(actual.Status, Is.EqualTo(expected.Status));
        Assert.That(actual.Acknowledged, Is.EqualTo(expected.Acknowledged));
        Assert.That(actual.IsSeen, Is.EqualTo(expected.IsSeen));
        Assert.That(actual.Snoozed, Is.EqualTo(expected.Snoozed));
        Assert.That(actual.SnoozedUntil, Is.EqualTo(expected.SnoozedUntil));
        Assert.That(actual.LastOccurredAt, Is.EqualTo(expected.LastOccurredAt));
        Assert.That(actual.CreatedAt, Is.EqualTo(expected.CreatedAt));
        Assert.That(actual.UpdatedAt, Is.EqualTo(expected.UpdatedAt));
        Assert.That(actual.Count, Is.EqualTo(expected.Count));
        Assert.That(actual.Source, Is.EqualTo(expected.Source));
        Assert.That(actual.Owner, Is.EqualTo(expected.Owner).Or.Empty.Or.Null);
        Assert.That(actual.Priority, Is.EqualTo(expected.Priority));

        // for (int i = 0; i < expected.Responders.Count; i++)
        // {
        //     bool found = false;
        //     for (int j = 0; j < actual.Responders.Count; j++)
        //     {
        //         switch(actual.Responders[j])
        //         {
        //             case TeamIdResponder responder:
        //                 Assert.That(responder, Is.EqualTo(expected.Responders[i]).Using(new TeamIdResponderEqualityComparer()));
        //                 found = true;
        //                 break;
        //         }
        //         if (actual.Responders[j])
        //     }
        // }
        // Assert.That(actual.Responders, Is.EquivalentTo(expected.Responders));

        if (actual.Integration is not null)
        {
            Assert.That(actual.Integration.Id, Is.EqualTo(expected.Integration.Id));
            Assert.That(actual.Integration.Name, Is.EqualTo(expected.Integration.Name));
            Assert.That(actual.Integration.Type, Is.EqualTo(expected.Integration.Type));
        }

        if (actual.Report is not null)
        {
            Assert.That(actual.Report.AckTime, Is.EqualTo(expected.Report.AckTime));
            Assert.That(actual.Report.CloseTime, Is.EqualTo(expected.Report.CloseTime));
            Assert.That(actual.Report.AcknowledgedBy, Is.EqualTo(expected.Report.AcknowledgedBy));
            Assert.That(actual.Report.ClosedBy, Is.EqualTo(expected.Report.ClosedBy));
        }

        // if (actual.Responders is not null || actual.Responders.Count > 0)
        // {
        //     Assert.That(actual.Responders, Is.EquivalentTo(expected.Responders));
        // }
    }
}