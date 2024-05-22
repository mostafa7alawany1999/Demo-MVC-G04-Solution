using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Demo.PL.Models
{
    public class UserViewModel
    {
        public string  Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }


        public IEnumerable<string> Roles { get; set; }

        //public string ImageName { get; set; }

        //public IFormFile Image { get; set; }
    }
}
