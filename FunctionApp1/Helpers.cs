using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1
{
    class Helpers
    {
    }
    public class CreateRatingObj
    {
        public string userid { get; set; }
        public string productid { get; set; }
        public string timestamp { get; set; }
        public string locationname { get; set; }
        public string usernotes { get; set; }
    }

    public class FullCreateRatingObj
    {
        public string userid { get; set; }
        public string productid { get; set; }
        public string timestamp { get; set; }
        public string locationname { get; set; }
        public string usernotes { get; set; }
        public string id { get; set; }
    }

    public static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    //public static string DoRestMethod(string type, string fullurl, string josnbody)
    //{
    //    var client = new RestClient(fullurl);
    //    // client.Authenticator = new HttpBasicAuthenticator(username, password);

    //    var request = new RestRequest("resource/{id}", Method.POST);
    //    request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
    //    request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

    //    // easily add HTTP Headers
    //    request.AddHeader("header", "value");

    //    // add files to upload (works with compatible verbs)
    //   // request.AddFile(path);

    //    // execute the request
    //    IRestResponse response = client.Execute(request);
    //    var content = response.Content; // raw content as string

    //    // or automatically deserialize result
    //    // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
    //    // RestResponse<Person> response2

    //    return content;
    //}
}
