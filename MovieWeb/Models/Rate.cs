﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	public partial class Rate
	{


		public int? MovieId { get; set; }
		public int PersonId { get; set; }
		[Required(ErrorMessage = "Comment is not empty.")]
		public string Comment { get; set; }
		[Range(0, 10, ErrorMessage = "Rating must be in range [0, 10].")]
		public double? NumericRating { get; set; }
		public DateTime? Time { get; set; }

		public virtual Movie? Movie { get; set; }
		public virtual Person? Person { get; set; }
	}
}
