using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SearchQuery
    {
        [Required]
        public string SearchPhrase { get; set; }
        public string[] Markets { get; set; }
        public int Limit { get; set; } = 25;
    }
}