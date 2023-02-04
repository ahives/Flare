using Flare.Model;

namespace Flare;

public interface Alert :
    FlareAPI
{
    Task<Result> Create(Action<AlertDefinition> action, CancellationToken cancellationToken = default);
    
    Task<Result> Create(CreateAlertRequest request, CancellationToken cancellationToken = default);
}