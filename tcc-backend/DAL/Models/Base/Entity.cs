using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models.Base
{
    public class Entity : Entity<int>
    {

    }
    public class Entity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
