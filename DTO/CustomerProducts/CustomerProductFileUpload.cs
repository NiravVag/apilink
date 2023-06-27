using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CustomerProducts
{
    public class CustomerProductFileUpload
    {
        [Required]
        public string ProductReferance { get; set; }
        [Required]

        public int FileType { get; set; }
        [Required]

        public IFormFile File { get; set; }
    }
}
