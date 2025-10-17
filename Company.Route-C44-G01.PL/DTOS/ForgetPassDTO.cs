using System.ComponentModel.DataAnnotations;

namespace Company.Route_C44_G01.PL.DTOS
{
    public class ForgetPassDTO
    {
        [Required(ErrorMessage = "Email is Required !!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}
