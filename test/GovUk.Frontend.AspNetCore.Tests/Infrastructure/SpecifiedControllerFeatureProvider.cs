using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace GovUk.Frontend.AspNetCore.Tests.Infrastructure
{
    public class SpecifiedControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly TypeInfo _controllerType;

        public SpecifiedControllerFeatureProvider(TypeInfo controllerType)
        {
            _controllerType = controllerType ?? throw new ArgumentNullException(nameof(controllerType));
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            feature.Controllers.Add(_controllerType);
        }
    }
}
