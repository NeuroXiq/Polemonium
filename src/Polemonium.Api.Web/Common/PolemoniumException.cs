using System;

namespace Polemonium.Api.Web.Common
{
    public class PolemoniumException : Exception
    {
        public PolemoniumException() : base() { }
        public PolemoniumException(string message) : base(message) { }
    }
}
