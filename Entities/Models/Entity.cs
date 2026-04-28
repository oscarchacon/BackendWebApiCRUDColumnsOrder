using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    /// <summary>
    /// Entity class to be used as a model.
    /// </summary>
    [Table("Entity")]
    public class Entity : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string Description { get; set; }

        [Column]
        public DateTime RegisterDate { get; set; }
    }
}
