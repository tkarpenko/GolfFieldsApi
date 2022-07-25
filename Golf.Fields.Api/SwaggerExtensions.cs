using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace Golf.Fields.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class RemoveVersionFromParameter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "version");
            if (versionParameter != null)
                operation.Parameters.Remove(versionParameter);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc == null)
                throw new ArgumentNullException(nameof(swaggerDoc));
            var replacements = new OpenApiPaths();
            foreach (var (key, value) in swaggerDoc.Paths)
            {
                replacements.Add(key.Replace("v{version}", swaggerDoc.Info.Version, StringComparison.InvariantCulture), value);
            }

            swaggerDoc.Paths = replacements;
        }

    }
}

