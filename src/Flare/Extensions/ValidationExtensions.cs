namespace Flare.Extensions;

using Model;

public static class ValidationExtensions
{
    public static IReadOnlyList<Error> ValidateNullableIdType(this string identifier, IdentifierType? identifierType,
        Func<IdentifierType?, bool> isIdTypeMissing)
    {
        var errors = new List<Error>();
        if (identifierType.HasValue && isIdTypeMissing(identifierType))
            errors.Add(Errors.Create(ErrorType.IdentifierTypeInvalidWithinContext,
                $"{identifierType.ToString()} is not a valid identifier type in the current context."));

        if (!identifierType.HasValue)
            errors.Add(Errors.Create(ErrorType.IdentifierTypeMissing, "Identifier type is missing."));

        bool isGuid = Guid.TryParse(identifier, out _);

        if (isGuid && identifierType != IdentifierType.Id
            || (!isGuid && !string.IsNullOrWhiteSpace(identifier) &&
                identifierType is not (IdentifierType.Name or IdentifierType.Alias)))
            errors.Add(Errors.Create(ErrorType.IdentifierTypeIncompatible,
                "Identifier type is not compatible with identifier."));

        if (string.IsNullOrWhiteSpace(identifier) || identifier.Equals(Guid.Empty.ToString()))
            errors.Add(Errors.Create(ErrorType.IdentifierInvalid, "Identifier is invalid."));

        return errors;
    }

    public static IReadOnlyList<Error> ValidateIdType(this string identifier, IdentifierType identifierType,
        Func<IdentifierType, bool> isIdTypeMissing)
    {
        var errors = new List<Error>();
        if (isIdTypeMissing(identifierType))
            errors.Add(Errors.Create(ErrorType.IdentifierTypeInvalidWithinContext,
                $"{identifierType.ToString()} is not a valid identifier type in the current context."));

        bool isGuid = Guid.TryParse(identifier, out _);

        if (isGuid && identifierType != IdentifierType.Id
            || (!isGuid && !string.IsNullOrWhiteSpace(identifier) &&
                identifierType is not (IdentifierType.Name or IdentifierType.Alias)))
            errors.Add(Errors.Create(ErrorType.IdentifierTypeIncompatible,
                "Identifier type is not compatible with identifier."));

        if (string.IsNullOrWhiteSpace(identifier) || identifier.Equals(Guid.Empty.ToString()))
            errors.Add(Errors.Create(ErrorType.IdentifierInvalid, "Identifier is invalid."));

        return errors;
    }
}