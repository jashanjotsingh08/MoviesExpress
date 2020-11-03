using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3_Arshdeep_Jashanjot_.Models
{
    public class MovieViewModel
    {
        public Movie Movie;
        public IEnumerable<Movie> MovieList { get; set; }
    }
}
