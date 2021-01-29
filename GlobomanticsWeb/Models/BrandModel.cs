using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobomanticsWeb.Models
{
    public class BrandModel
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime InceptionDate { get; set; }

    }
}
