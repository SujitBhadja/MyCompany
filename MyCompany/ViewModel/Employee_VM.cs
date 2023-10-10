using System.ComponentModel.DataAnnotations;

namespace MyCompany.ViewModel
{
    public class Employee_VM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required.")]
        public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid phone number.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public string? Designation { get; set; }

        [Required(ErrorMessage = "Joining Date is required.")]
        public DateTime? JoiningDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
