using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Rates = new HashSet<Rate>();
        }

        public int MovieId { get; set; }
		[Required(ErrorMessage = "Title is not empty.")]
		public string Title { get; set; }
		[Required(ErrorMessage = "Year is not empty.")]
		public int Year { get; set; }
		[Required(ErrorMessage = "Image is not empty.")]
		public string Image { get; set; }
		[Required(ErrorMessage = "Description is not empty.")]
		public string Description { get; set; }
		[Required(ErrorMessage = "GenreId is not empty.")]
		public int GenreId { get; set; }

        public virtual Genre? Genre { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }

		public double AverageRate()
		{
			double? avg = 0.0;
			int i = 0;
			foreach (var rate in Rates)
			{
				i++;
				avg += rate.NumericRating;
			}
			return (double)(avg / i);
		}
	}
}
