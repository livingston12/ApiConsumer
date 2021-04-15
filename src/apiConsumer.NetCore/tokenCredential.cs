using System.Net.Http;
using src.apiConsumer.NetCore.Identity;

namespace src.apiConsumer.NetCore
{
    public class tokenCredential
    {
        public string accessToken { get; set; }

        private tokenCredential GetToken(apiCredential apiCredential, loginViewModel credentialLogin)
        {
            ApiConsumer api = new ApiConsumer(apiCredential);
            HttpResponseMessage Response = ApiConsumer.PostApiAsync<loginViewModel>(credentialLogin);
            return parameters.convertJsonToClass<tokenCredential>(Response);
        }
    }

}