using System.ComponentModel.DataAnnotations;

namespace MyCompany.ViewModel
{
    public class Department_VM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid phone number.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Established Date is required.")]
        public DateTime? EstablishedDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
