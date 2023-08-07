using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Movies = new HashSet<Movie>();
        }

        public int? GenreId { get; set; }
		[Required(ErrorMessage = "Description is not empty.")]
		public string Description { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
