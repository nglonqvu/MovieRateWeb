using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public partial class Person
    {
        public Person()
        {
            Rates = new HashSet<Rate>();
        }

		public int PersonId { get; set; }
		[Required(ErrorMessage = "Username is not empty.")]
		public string Fullname { get; set; }
		[Required(ErrorMessage = "Gender is not empty.")]
		public string Gender { get; set; }
		[Required(ErrorMessage = "Email is not empty.")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is not empty.")]
		public string Password { get; set; }
		public int? Type { get; set; }
		public bool? IsActive { get; set; }

		public virtual ICollection<Rate> Rates { get; set; }
    }
}
