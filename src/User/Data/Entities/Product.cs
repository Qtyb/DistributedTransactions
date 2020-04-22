using Qtyb.Common.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Data.Entities
{
    public class Product : IGuidBasedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}