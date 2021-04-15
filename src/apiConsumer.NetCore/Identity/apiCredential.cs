using System;
using System.Net;
using System.Net.Http;

namespace src.apiConsumer.NetCore.Identity
{
    public class apiCredential
    {
        public string URI { get; set; }
        public string method { get; set; }
        public int? port { get; set; }
        public string token { get; set; }
        public bool RequeridAuthorization
        {
            get { return !string.IsNullOrWhiteSpace(token); }
        }
        public string FullUri
        {
            get { return port == null || port == 0 ? URI : string.Concat(URI, ":", port); }
        }   
        private HttpResponseMessage _HttpMessage
        {
            get { return new HttpResponseMessage(HttpStatusCode.Conflict); }
        }
        public HttpResponseMessage HttpMessage(Exception errorMessage)
        {            
            _HttpMessage.Content = new StringContent(errorMessage is null ? "Error ocurred in consumer api Check fields" : errorMessage.Message);
            return _HttpMessage;
        }

    }
}