namespace Flare.Model;

public enum ErrorType
{
    EndTime,
    AlertStatusIncompatible,
    SortFieldMissing,
    PaginationOffsetOutOfRange,
    Message,
    StringLengthLimitExceeded,
    UserCharLimitExceeded,
    SourceCharLimitExceeded,
    TagsLimitExceeded,
    CustomPropertiesLimitExceeded,
    ActionsLimitExceeded,
    RespondersLimitExceeded,
    OwnerMissing,
    AlertEscalationMissing,
    IdentifierTypeInvalidWithinContext,
    IdentifierTypeIncompatible,
    IdentifierInvalid,
    IdentifierTypeMissing,
    AlertStatusMissing,
    AlertTeamMissing,
    AlertResponderMissing,
    AlertTagsMissing,
    PageDirectionMissing
}