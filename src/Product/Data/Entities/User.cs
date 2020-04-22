using Qtyb.Common.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi.Data.Entities
{
    public class User : IGuidBasedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}