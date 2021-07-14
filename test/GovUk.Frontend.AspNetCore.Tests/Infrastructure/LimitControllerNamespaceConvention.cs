using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace GovUk.Frontend.AspNetCore.Tests.Infrastructure
{
    public class LimitControllerNamespaceConvention : IApplicationModelConvention
    {
        private readonly string _namespace;

        public LimitControllerNamespaceConvention(string @namespace)
        {
            _namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
        }

        public void Apply(ApplicationModel application)
        {
            var controllers = application.Controllers;

            for (var i = 0; i < controllers.Count; i++)
            {
                if (!controllers[i].ControllerType.FullName.StartsWith(_namespace + "."))
                {
                    controllers.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
