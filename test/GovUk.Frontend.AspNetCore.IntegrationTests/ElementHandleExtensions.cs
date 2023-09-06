using System.Threading.Tasks;
using Microsoft.Playwright;

namespace GovUk.Frontend.AspNetCore.IntegrationTests;

public static class ElementHandleExtensions
{
    public static async Task<string[]> GetClassListAsync(this IElementHandle element)
    {
        var classes = await element.GetAttributeAsync("class") ?? string.Empty;
        return classes.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
    }
}
