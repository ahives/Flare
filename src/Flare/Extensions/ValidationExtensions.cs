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

        if (!IsIdCompatibleWithType())
            errors.Add(Errors.Create(ErrorType.IdentifierTypeIncompatible,
                "Identifier type is not compatible with identifier."));

        if (string.IsNullOrWhiteSpace(identifier) || identifier.Equals(Guid.Empty.ToString()))
            errors.Add(Errors.Create(ErrorType.IdentifierInvalid, "Identifier is invalid."));

        return errors;

        bool IsIdCompatibleWithType() =>
            identifierType switch
            {
                IdentifierType.Id => Guid.TryParse(identifier, out _),
                IdentifierType.TinyId => int.TryParse(identifier, out _),
                IdentifierType.Name or IdentifierType.Alias => !string.IsNullOrWhiteSpace(identifier),
                null => false,
                _ => true
            };
    }

    public static IReadOnlyList<Error> ValidateIdType(this string identifier, IdentifierType identifierType,
        Func<IdentifierType, bool> isIdTypeMissing)
    {
        var errors = new List<Error>();

        bool isMissing = isIdTypeMissing(identifierType);
        if (isMissing)
            errors.Add(Errors.Create(ErrorType.IdentifierTypeInvalidWithinContext,
                $"{identifierType.ToString()} is not a valid identifier type in the current context."));

        if (!IsIdCompatibleWithType())
            errors.Add(Errors.Create(ErrorType.IdentifierTypeIncompatible,
                "Identifier type is not compatible with identifier."));

        if (string.IsNullOrWhiteSpace(identifier) || identifier.Equals(Guid.Empty.ToString()))
            errors.Add(Errors.Create(ErrorType.IdentifierInvalid, "Identifier is invalid."));

        return errors;

        bool IsIdCompatibleWithType() =>
            identifierType switch
            {
                IdentifierType.Id => Guid.TryParse(identifier, out _),
                IdentifierType.TinyId => int.TryParse(identifier, out _),
                IdentifierType.Name or IdentifierType.Alias => !string.IsNullOrWhiteSpace(identifier),
                _ => true
            };
    }
}