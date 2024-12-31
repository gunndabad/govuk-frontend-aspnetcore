using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace GovUk.Frontend.AspNetCore.ComponentGeneration;

internal class EncodedAttributesDictionaryBuilder
{
    private readonly EncodedAttributesDictionary _dictionary;

    public EncodedAttributesDictionaryBuilder()
        : this(new())
    {
    }

    public EncodedAttributesDictionaryBuilder(EncodedAttributesDictionary dictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary);

        _dictionary = dictionary;
    }

    public EncodedAttributesDictionaryBuilder WithBoolean(string name)
    {
        _dictionary.AddBoolean(name);

        return this;
    }

    public EncodedAttributesDictionaryBuilder WithOther(EncodedAttributesDictionary? other)
    {
        if (other is not null)
        {
            _dictionary.Add(other);
        }

        return this;
    }

    public EncodedAttributesDictionaryBuilder With(string name, IHtmlContent value)
    {
        _dictionary.Add(name, value);

        return this;
    }

    public EncodedAttributesDictionaryBuilder With(string name, string value, bool encodeValue)
    {
        _dictionary.Add(name, value, encodeValue);

        return this;
    }

    public EncodedAttributesDictionaryBuilder WithCssClass(string className)
    {
        _dictionary.AddCssClass(className);

        return this;
    }

    public EncodedAttributesDictionaryBuilder WithCssClasses(IEnumerable<string> classNames)
    {
        _dictionary.AddCssClasses(classNames);

        return this;
    }

    internal EncodedAttributesDictionaryBuilder When(Func<bool> condition, Action<EncodedAttributesDictionaryBuilder> action)
    {
        if (condition())
        {
            action(this);
        }

        return this;
    }

    internal EncodedAttributesDictionaryBuilder When(bool condition, Action<EncodedAttributesDictionaryBuilder> action) =>
        When(() => condition, action);

    internal EncodedAttributesDictionaryBuilder WhenNotNull<T>(Func<T?> getValue, Action<T, EncodedAttributesDictionaryBuilder> action)
    {
        var value = getValue();
        if (value is not null)
        {
            action(value, this);
        }

        return this;
    }

    internal EncodedAttributesDictionaryBuilder WithWhenNotNull(Func<string?> getValue, string name, bool encodedValue) =>
        WhenNotNull(getValue(), (value, b) => b.With(name, value, encodedValue));

    internal EncodedAttributesDictionaryBuilder WithWhenNotNull(string? value, string name, bool encodedValue) =>
        WhenNotNull(value, (_, b) => b.With(name, value!, encodedValue));

    internal EncodedAttributesDictionaryBuilder WithWhenNotNull(Func<IHtmlContent?> getValue, string name) =>
        WhenNotNull(getValue(), (value, b) => b.With(name, value));

    internal EncodedAttributesDictionaryBuilder WithWhenNotNull(IHtmlContent? value, string name) =>
        WhenNotNull(value, (_, b) => b.With(name, value!));

    internal EncodedAttributesDictionaryBuilder WhenNotNull<T>(T? value, Action<T, EncodedAttributesDictionaryBuilder> action) =>
        WhenNotNull(() => value, action);

    public static implicit operator EncodedAttributesDictionary(EncodedAttributesDictionaryBuilder builder) => builder._dictionary;
}
