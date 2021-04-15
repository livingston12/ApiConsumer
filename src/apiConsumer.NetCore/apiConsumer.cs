using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using src.apiConsumer.NetCore.Identity;

namespace src.apiConsumer.NetCore
{
    public class ApiConsumer
    {
        private static string _URI, _method, json = "{}", request = "", _token = "";
        private static int? _Port = null;
        private static bool _RequeridAuthorization = false;
        private const string contentType = "application/json";
        private static Task<HttpResponseMessage> responseTask = null;
        private static HttpResponseMessage result;
        private static HttpClient client;
        private static apiCredential apiCredential;
        public ApiConsumer(apiCredential apiCredential)
        {
            _URI = apiCredential.URI;
            _method = apiCredential.method;
            _Port = apiCredential.port;
            _RequeridAuthorization = apiCredential.RequeridAuthorization;
            _URI = apiCredential.FullUri;
            _token = apiCredential.token;
            result = new HttpResponseMessage();
            client = new HttpClient();
            client.BaseAddress = new Uri(_URI); // send Url To app
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType)); // Send content type to Api
            if (_RequeridAuthorization)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
        public static Tsource GetApi<Tsource>(List<parameters> parameters = null, bool useParameterName = false)
        where Tsource : class
        {
            List<string> allParameters = new List<string>();
            string parameterGeneral = "";

            using (WebClient _client = new WebClient())
            {
                _client.BaseAddress = _URI;
                _client.Headers.Add("Content-Type", contentType);

                if (_RequeridAuthorization)
                    _client.Headers.Add("Authorization", "Bearer " + _token);

                if (parameters != null)
                {

                    foreach (parameters param in parameters)
                    {
                        if (useParameterName)
                            allParameters.Add(string.Concat(param.parameterName, "=", param.parameterValue));
                        else
                            allParameters.Add(string.Concat(param.parameterValue));

                    }
                    parameterGeneral = useParameterName == true ? "?" + string.Join("&", allParameters.ToArray()) : "/" + string.Join("/", allParameters.ToArray());
                    request = _method + parameterGeneral;
                }
                else
                    request = _method;


                byte[] arr = _client.DownloadData(request);

                System.Text.UTF8Encoding codificador = new System.Text.UTF8Encoding();
                string Json = codificador.GetString(arr);

                return JsonConvert.DeserializeObject<Tsource>(Json);

            }
        }
        public static HttpResponseMessage getApiByparameter<Tsource>(List<parameters> parameters = null, bool useParameterByUrl = false)
         where Tsource : class
        {
            List<string> allParameters = new List<string>();
            string parameterGeneral = "";
            try
            {

                if (parameters != null)
                {

                    foreach (parameters param in parameters)
                    {
                        if (useParameterByUrl)
                            allParameters.Add(string.Concat(param.parameterName, "=", param.parameterValue));
                        else
                            allParameters.Add(string.Concat(param.parameterValue));

                    }
                    parameterGeneral = useParameterByUrl == true ? "?" + string.Join("&", allParameters.ToArray()) : "/" + string.Join("/", allParameters.ToArray());
                    request = _method + parameterGeneral;
                }

                else
                    request = _method;

                responseTask = client.GetAsync(request);
                responseTask.Wait();

                result = responseTask.Result;

            }
            catch (Exception ex)
            {
                result = apiCredential.HttpMessage(ex);
            }
            return result;

        }
        public static HttpResponseMessage DeleteApiAsync(string Id)
        {
            request = _method + "/" + Id;
            try
            {
                using (var client = new HttpClient())
                {
                    responseTask = client.DeleteAsync(request);
                    responseTask.Wait();
                    //await responseTask;
                    result = responseTask.Result;
                }
            }
            catch (Exception ex)
            {
                result = apiCredential.HttpMessage(ex);
            }
            return result;
        }
        public static HttpResponseMessage EditApiAsync(string Id)
        {
            try
            {
                request = _method + "/" + Id;
                responseTask = client.GetAsync(request);
                responseTask.Wait();
                responseTask = client.PutAsync(request, responseTask.Result.Content);
                responseTask.Wait();
                result = responseTask.Result;
            }
            catch (Exception ex)
            {

                result = apiCredential.HttpMessage(ex);
            }
            return result;
        }
        public static HttpResponseMessage PostApiAsync<Tsource>(Tsource Model)
         where Tsource : class
        {
            try
            {

                json = Newtonsoft.Json.JsonConvert.SerializeObject(Model);
                HttpContent content = new StringContent(json, Encoding.UTF8, contentType);

                responseTask = client.PostAsync(_method, content);
                responseTask.Wait();

                result = responseTask.Result;
            }
            catch (Exception ex)
            {
                result = apiCredential.HttpMessage(ex);

            }
            return result;
        }
    }

}
