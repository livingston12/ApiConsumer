using System.Net.Http;
using Newtonsoft.Json;

namespace src.apiConsumer.NetCore
{
    public class parameters
    {
        public string parameterName { get; set; }
        public object parameterValue { get; set; }
        public static Tsource convertJsonToClass<Tsource>(HttpResponseMessage Response) 
        where Tsource: class       
        {
            var readTask = Response.Content.ReadAsStringAsync();
            readTask.Wait();
            return JsonConvert.DeserializeObject<Tsource>(readTask.Result);
        }
    }
}