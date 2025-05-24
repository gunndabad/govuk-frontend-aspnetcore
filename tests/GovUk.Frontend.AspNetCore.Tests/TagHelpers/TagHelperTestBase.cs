using System.Linq.Expressions;
using System.Text.Encodings.Web;
using GovUk.Frontend.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit.Sdk;

namespace GovUk.Frontend.AspNetCore.Tests.TagHelpers;

public abstract class TagHelperTestBase(string tagName)
{
    protected string TagName { get; } = tagName;

    protected TagHelperContext CreateTagHelperContext(string? tagName = null, params object[] contexts)
    {
        var items = contexts.ToDictionary(object (c) => c.GetType(), c => c);

        return new TagHelperContext(
            tagName ?? TagName,
            allAttributes: new TagHelperAttributeList(),
            items,
            uniqueId: "test");
    }

    protected TagHelperOutput CreateTagHelperOutput(
        string? tagName = null,
        Func<bool, HtmlEncoder, Task<TagHelperContent>>? getChildContentAsync = null)
    {
        return new TagHelperOutput(
            tagName ?? TagName,
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: getChildContentAsync ?? ((usedCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            }));
    }

    protected ViewContext CreateViewContext() =>
        new ViewContext() { HttpContext = new DefaultHttpContext() };

    protected (IComponentGenerator ComponentGenerator, Func<TOptions> GetActualOptions) CreateComponentGenerator<TOptions>(
        string generateMethodName)
        where TOptions : class
    {
        var componentGenerator = new Mock<DefaultComponentGenerator>() { CallBase = true };

        Expression<Func<DefaultComponentGenerator, ValueTask<IHtmlContent>>> CreateExpression()
        {
            var generatorParameter = Expression.Parameter(typeof(DefaultComponentGenerator));

            var method = typeof(DefaultComponentGenerator).GetMethod(generateMethodName)!;

            var optionsArg = Expression.Call(
                instance: null,
                typeof(It).GetMethod("IsAny")!.MakeGenericMethod(typeof(TOptions)));

            return (Expression<Func<DefaultComponentGenerator, ValueTask<IHtmlContent>>>)Expression.Lambda(
                Expression.Call(
                    generatorParameter,
                    method,
                    optionsArg),
                generatorParameter);
        }

        TOptions? actualOptions = null;
        componentGenerator.Setup(CreateExpression()).Callback<TOptions>(o => actualOptions = o);

        return (componentGenerator.Object, () => actualOptions ?? throw new XunitException("ComponentGenerator method was not invoked."));
    }
}
