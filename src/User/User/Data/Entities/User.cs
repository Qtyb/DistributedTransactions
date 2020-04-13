using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Data.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}