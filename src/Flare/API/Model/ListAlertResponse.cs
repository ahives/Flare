namespace Flare.API.Model;

public record ListAlertResponse :
    AlertResponse<List<AbbreviatedAlertData>>
{
}