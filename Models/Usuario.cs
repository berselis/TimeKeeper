

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeKeeper.Models
{
    public class Usuario : IdentityUser
    {
        [Required, MaxLength(20)]
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }


    [NotMapped]
    public class UserCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}
