namespace Flare.Model;

public enum ErrorType
{
    EndTime,
    AlertStatusIncompatible,
    SortField,
    PaginationOffset,
    Message,
    StringLengthLimitExceeded,
    UserCharLimitExceeded,
    SourceCharLimitExceeded,
    TagsLimitExceeded,
    CustomPropertiesLimitExceeded,
    ActionsLimitExceeded,
    RespondersLimitExceeded,
    OwnerMissing,
    EscalationMissing,
    IdentifierTypeInvalidWithinContext,
    IdentifierTypeIncompatible,
    IdentifierInvalid,
    IdentifierTypeMissing,
    AlertStatusMissing
}