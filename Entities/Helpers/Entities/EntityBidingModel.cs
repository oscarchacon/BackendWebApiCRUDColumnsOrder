using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Helpers.Entities
{
    public class EntityRegisterModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
