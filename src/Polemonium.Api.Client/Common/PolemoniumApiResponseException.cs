using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polemonium.Api.Client.Common
{
    public class PolemoniumApiResponseException : Exception
    {
        public string Error { get; set; }
    }
}
