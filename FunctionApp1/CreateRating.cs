using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FunctionApp1
{
    public static class CreateRating
    {
        [FunctionName("CreateRating")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            if (name == null)
            {
                // Get request body
               // dynamic data = await req.Content.ReadAsAsync<object>();
                // crobj = JsonConvert.DeserializeObject<CreateRatingObj>(data);
                // name = data?.name;

                // public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, Crea.Converter.Settings);

                dynamic body = await req.Content.ReadAsStringAsync();
                var e = JsonConvert.DeserializeObject<CreateRatingObj>(body as string);
               // return req.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(e));
            }


        
            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
    }
}
