using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace GovUk.Frontend.AspNetCore.ModelBinding;

internal class GovUkFrontendAspNetCoreMetadataDetailsProvider : IMetadataDetailsProvider, IDisplayMetadataProvider
{
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        var dateOnlyMetadataAttribute = context.Attributes.OfType<DateInputAttribute>().FirstOrDefault();

        if (dateOnlyMetadataAttribute is not null)
        {
            var dateOnlyMetadata = new DateInputModelMetadata()
            {
                ErrorMessagePrefix = dateOnlyMetadataAttribute.ErrorMessagePrefix
            };

            context.DisplayMetadata.AdditionalValues.Add(typeof(DateInputModelMetadata), dateOnlyMetadata);
        }
    }
}
