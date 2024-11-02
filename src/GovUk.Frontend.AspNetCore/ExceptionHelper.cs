using System;
using System.Collections.Generic;
using System.Linq;

namespace GovUk.Frontend.AspNetCore;

internal static class ExceptionHelper
{
    public static InvalidOperationException AChildElementMustBeProvided(string childElementTagName) =>
        AChildElementMustBeProvided([childElementTagName]);

    public static InvalidOperationException AChildElementMustBeProvided(
        IReadOnlyCollection<string> childElementTagNames
    ) => new($"A {JoinTagNamesWithConjunction(childElementTagNames)} element must be provided.");

    public static InvalidOperationException AtLeastOneOfAttributesMustBeProvided(params string[] attributeNames)
    {
        if (attributeNames.Length < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(attributeNames));
        }

        var quotedNames = attributeNames.Select(a => $"'{a}'").ToArray();

        var attrsList = string.Join(
            ", ",
            quotedNames.SkipLast(2).Append(string.Join(" and ", quotedNames.TakeLast(2)))
        );

        return new InvalidOperationException($"At least one of the {attrsList} attributes must be provided.");
    }

    public static InvalidOperationException ChildElementMustBeSpecifiedBefore(
        string childElementTagName,
        string beforeSiblingTagName
    ) => new($"<{childElementTagName}> must be specified before <{beforeSiblingTagName}>.");

    public static InvalidOperationException OnlyOneElementIsPermittedIn(
        string childElementTagName,
        string parentElementTagName
    ) => OnlyOneElementIsPermittedIn([childElementTagName], [parentElementTagName]);

    public static InvalidOperationException OnlyOneElementIsPermittedIn(
        IReadOnlyCollection<string> childElementTagNames,
        string parentElementTagName
    ) => OnlyOneElementIsPermittedIn(childElementTagNames, [parentElementTagName]);

    public static InvalidOperationException OnlyOneElementIsPermittedIn(
        IReadOnlyCollection<string> childElementTagNames,
        IReadOnlyCollection<string> parentElementTagNames
    ) =>
        new(
            $"Only one {JoinTagNamesWithConjunction(childElementTagNames)} element is permitted within each {JoinTagNamesWithConjunction(parentElementTagNames)}."
        );

    public static InvalidOperationException TheAttributeMustBeSpecified(string attributeName) =>
        new($"The '{attributeName}' attribute must be specified.");

    public static InvalidOperationException CannotBeNestedInsideAnother(
        string tagName,
        IReadOnlyCollection<string> cannotBeNestedWithinTagNames
    ) =>
        new(
            $"A <{tagName}> cannot be nested inside another {JoinTagNamesWithConjunction(cannotBeNestedWithinTagNames)}."
        );

    public static InvalidOperationException MustBeInside(
        string tagName,
        IReadOnlyCollection<string> mustBeInsideTagNames
    ) => new($"A <{tagName}> must be inside a {JoinTagNamesWithConjunction(mustBeInsideTagNames)}.");

    private static string JoinTagNamesWithConjunction(
        IReadOnlyCollection<string> tagNames,
        string conjunction = " or "
    ) =>
        tagNames.Count == 1
            ? $"<{tagNames.Single()}>"
            : string.Join(
                conjunction,
                [
                    string.Join(", ", tagNames.SkipLast(1).Select(t => $"<{t}>")),
                    tagNames.TakeLast(1).Select(t => $"<{t}>").Single(),
                ]
            );
}
