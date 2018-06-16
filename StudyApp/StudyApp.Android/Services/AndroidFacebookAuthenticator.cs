using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;

namespace StudyApp.Services
{
    public class AndroidFacebookAuthenticator : IFacebookAuthenticator
    {
        public async Task Authenticate(Action<string> cb)
        {
            var auth = new OAuth2Authenticator(
                clientId: "226251828179258",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"),
                isUsingNativeUI: false);

            auth.Completed += async (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    // Use eventArgs.Account to do wonderful things
                    var request = new OAuth2Request(
                        "GET",
                        new Uri("https://graph.facebook.com/me"),
                        null,
                        eventArgs.Account
                        );

                    var fbResult = await request.GetResponseAsync();
                    var fbUser = fbResult.GetResponseText();

                    cb(fbUser);
                }
            };

            Application.Context.StartActivity(auth.GetUI(Application.Context));
        }
    }
}