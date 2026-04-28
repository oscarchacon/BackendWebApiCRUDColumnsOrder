using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils
{
    /// <summary>
    /// Class used for request responses.
    /// </summary>
    public class ResponseMessage
    {
        public string Message { get; set; }

        public int Code { get; set; }
    }
}
