using Microsoft.AspNetCore.Identity;

namespace GibJohnWebsite.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add any additional properties you want to include for your user
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // You can add more properties as needed
    }
}
