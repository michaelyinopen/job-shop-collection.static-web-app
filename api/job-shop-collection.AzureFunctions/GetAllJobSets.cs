using AutoMapper;
using AutoMapper.QueryableExtensions;
using job_shop_collection.AzureFunctions.Data;
using job_shop_collection.AzureFunctions.Data.Models;
using job_shop_collection.AzureFunctions.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace job_shop_collection.AzureFunctions
{
    public class GetAllJobSets
    {
        private readonly JobShopCollectionDbContext _jobShopCollectionDbContext;
        private readonly IMapper _mapper;

        public GetAllJobSets(
            JobShopCollectionDbContext jobShopCollectionDbContext,
            IMapper mapper)
        {
            _jobShopCollectionDbContext = jobShopCollectionDbContext;
            _mapper = mapper;
        }

        [FunctionName("GetAllJobSets")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "pageToken", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The **PageToken** parameter")]
        [OpenApiParameter(name: "limit", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The **Limit** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JobSetHeadersDto), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "job-sets")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processing request GetAllJobSets.");

            int? pageToken = int.TryParse(req.Query["pageToken"], out int pageTokenResult) ? pageTokenResult : default(int?);

            int limitDefault = 100;
            int limit = int.TryParse(req.Query["limit"], out int limitResult) ? limitResult : limitDefault;

            IQueryable<JobSet> dataQuery = _jobShopCollectionDbContext.JobSet;
            if (pageToken.HasValue)
            {
                dataQuery = dataQuery.Where(j => j.Id < pageToken);
            }
            var data = await dataQuery
                .OrderByDescending(j => j.Id)
                .Take(limit)
                .ProjectTo<JobSetHeaderDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            int? nextPageToken = data.Count == limit ? data[^1].Id : default(int?);

            var result = new JobSetHeadersDto
            {
                Data = data,
                NextPageToken = nextPageToken
            };

            return new OkObjectResult(result);
        }
    }
}

