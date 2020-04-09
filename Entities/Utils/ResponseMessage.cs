using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Utils
{
    /// <summary>
    /// Clase que sirve para respuestas de las peticiones
    /// </summary>
    public class ResponseMessage
    {
        public string Message { get; set; }

        public int Code { get; set; }
    }
}
