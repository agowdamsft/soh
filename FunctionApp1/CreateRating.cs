using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;

namespace FunctionApp1
{
    public static class CreateRating
    {
        [FunctionName("CreateRating")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            //Initiatlize classes
            FullCreateRatingObj fo = new FullCreateRatingObj();

            DocumentDBRepository<FullCreateRatingObj>.Initialize();

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
                //Check if these are valid entries
                var client = new RestClient("https://serverlessohlondonproduct.azurewebsites.net/");
                // client.Authenticator = new HttpBasicAuthenticator(username, password);

                var request = new RestRequest("api/GetProduct", Method.GET);
                request.AddParameter("productid", e.productid); // adds to POST or URL querystring based on Method
                //request.AddUrlSegment("", ); // replaces matching token in request.Resource

                // easily add HTTP Headers
               // request.AddHeader("header", "value");

                // add files to upload (works with compatible verbs)
                // request.AddFile(path);

                // execute the request
                IRestResponse response = client.Execute(request);
                var content = response.Content; // raw content as string

                if(content.Contains("does not"))
                    {
                    req.CreateResponse(HttpStatusCode.OK, "Product does not exist");
                }
                else
                {
                    var client2 = new RestClient("https://serverlessohlondonuser.azurewebsites.net/");
                    // client.Authenticator = new HttpBasicAuthenticator(username, password);

                    var request2 = new RestRequest("api/GetUser", Method.GET);
                    request2.AddParameter("userid", e.userid); // adds to POST or URL querystring based on Method
                    IRestResponse response2 = client2.Execute(request2);
                    var content2 = response2.Content;
                    if (content2.Contains("does not exist"))
                    {
                        req.CreateResponse(HttpStatusCode.OK, "The user does not exist"); 
                    }

                    else
                    {
                       
                        Guid g;
                        // Create and display the value of two GUIDs.
                        g = Guid.NewGuid();
                        fo.id = g.ToString();
                        fo.locationname = e.locationname;
                        fo.productid = e.productid;
                        fo.timestamp = DateTime.Now.ToUniversalTime().ToString();
                        fo.userid = e.userid;
                        fo.usernotes = e.usernotes;

                        //Add Cosmos DB Write
                        
                        await DocumentDBRepository<FullCreateRatingObj>.CreateItemAsync(fo);

                        req.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(fo));


                    }

                    req.CreateResponse(HttpStatusCode.OK, "Cosmos Entry did not work");

                }

                // rSeturn req.CreateResponse(HttpStatusCode.OK, JsonConvert.erializeObject(e));
            }

          

                return name != null
                    ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                    : req.CreateResponse(HttpStatusCode.OK, fo);

            //req.CreateResponse(HttpStatusCode.OK, "All executed well");
        }
    }
}
