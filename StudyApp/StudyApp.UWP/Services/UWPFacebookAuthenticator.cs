using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace StudyApp.Services
{
    public class UWPFacebookAuthenticator : IFacebookAuthenticator
    {
        private readonly Uri _loginUri;
        private readonly Uri _callbackUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();
        private readonly string _facebookAppId = "226251828179258";
        private readonly string _facebookPermissions = "";

        public string AccessToken { get; private set; }

        public bool IsLoggedIn => !string.IsNullOrEmpty(AccessToken);

        public UWPFacebookAuthenticator()
        {
            var fb = new FacebookClient();

            _loginUri = fb.GetLoginUrl(new
            {
                client_id = _facebookAppId,
                redirect_uri = _callbackUri.AbsoluteUri,
                scope = _facebookPermissions,
                display = "popup",
                response_type = "token"
            });
        }
        public async Task Authenticate(Action<string> cb)
        {
            try
            {
                var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, _loginUri, _callbackUri);
                AccessToken = GetAccessToken(result);

                if (!IsLoggedIn)
                    return;

                var fb = new FacebookClient(AccessToken);
                var fbResult = (JsonObject)await fb.GetTaskAsync("me");

                cb(fbResult.ToString());
            }
            catch (Exception){}
        }

        private string GetAccessToken(WebAuthenticationResult result)
        {
            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                return new FacebookClient().ParseOAuthCallbackUrl(new Uri(result.ResponseData)).AccessToken;
            }
            return null;
        }
    }
}
