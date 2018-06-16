using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudyApp.Services
{
    public interface IFacebookAuthenticator
    {
        Task Authenticate(Action<string> cb);
    }
}
