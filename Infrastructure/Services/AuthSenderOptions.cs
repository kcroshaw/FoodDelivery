using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthSenderOptions
    {

        //the name to show up in the FROM: on email
        private readonly string user = "Food Delivery Demo";
        private readonly string key = "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";
        public string SendGridUser { get { return user; } }
        public string SendGridKey { get { return key; } }
    }
}
