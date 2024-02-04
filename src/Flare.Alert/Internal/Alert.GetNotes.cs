namespace Flare.Alert.Internal;

using Extensions;
using Flare.Model;
using Model;
using Serialization;

public partial class AlertImpl
{
    public async Task<Maybe<AlertNoteInfo>> GetNotes(string identifier, IdentifierType identifierType, Action<QueryAlertNotesCriteria> criteria,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var impl = new GetNotesImpl(identifier, identifierType);
        criteria?.Invoke(impl);

        var errors = impl.Validate();
        if (errors.Any())
            return Response.Failed<AlertNoteInfo>(Debug.WithErrors("alerts/{identifier}/notes?identifierType={identifierType}", errors));

        string url = $"alerts/{identifier}/notes{impl.GetQueryArguments().BuildQueryString()}";

        return await GetRequest<AlertNoteInfo>(url, Serializer.Options, cancellationToken);
    }

    
    class GetNotesImpl :
        QueryAlertNotesCriteria,
        IQueryCriteria,
        IValidator
    {
        IdentifierType _identifierType;
        string _identifier;
        int _offset;
        int _limit;
        OrderType _orderType;
        PageDirection _direction;

        public GetNotesImpl(string identifier, IdentifierType identifierType)
        {
            _identifier = identifier;
            _identifierType = identifierType;
        }

        public void PaginationOffset(int offset)
        {
            _offset = offset;
        }

        public void PaginationLimit(int limit)
        {
            _limit = limit;
        }

        public void Direction(PageDirection direction)
        {
            _direction = direction;
        }

        public void OrderBy(OrderType type)
        {
            _orderType = type;
        }

        public IReadOnlyList<Error> Validate()
        {
            var errors = new List<Error>();

            if (_offset < 0)
                errors.Add(Errors.Create(ErrorType.PaginationOffsetOutOfRange, "Pagination offset must be greater than or equal to 0."));

            bool isDirectionMissing = _direction switch
            {
                PageDirection.Next => false,
                PageDirection.Previous => false,
                _ => true
            };

            if (isDirectionMissing)
                errors.Add(Errors.Create(ErrorType.PageDirectionMissing, "Sortable field is not valid in the current context."));

            errors.AddRange(
                _identifier.ValidateIdType(_identifierType, t => t switch
                {
                    IdentifierType.Id => false,
                    IdentifierType.Name => false,
                    _ => true
                }));

            return errors;
        }

        public Dictionary<string, QueryArg> GetQueryArguments()
        {
            var arguments = new Dictionary<string, QueryArg>();

            string pageDirection = _direction switch
            {
                PageDirection.Next => "next",
                PageDirection.Previous => "prev",
                _ => string.Empty
            };

            if (!string.IsNullOrWhiteSpace(pageDirection))
                arguments.Add("direction", new QueryArg {Value = pageDirection});

            string orderType = _orderType switch
            {
                OrderType.Desc => "desc",
                OrderType.Asc => "asc",
                _ => string.Empty
            };

            arguments.Add("offset", new QueryArg{Value = _offset});
            arguments.Add("limit", new QueryArg{Value = _limit});

            if (!string.IsNullOrWhiteSpace(orderType))
                arguments.Add("order", new QueryArg{Value = orderType});

            return arguments;
        }
    }
}