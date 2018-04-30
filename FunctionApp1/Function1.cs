using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string productid = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "productid", true) == 0)
                .Value;

            if (productid == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                productid = data?.name;
            }

            return productid == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "The product name for your product id " + productid + "is Starfruit Explosion");
        }
    }
}
