using Microsoft.AspNetCore.Mvc.Rendering;
using MyCompany.Models;
using System.ComponentModel.DataAnnotations;

namespace MyCompany.ViewModel
{
    public class Company_VM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please select a company type.")]
        public string? Type { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid phone number.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Registration Number is required.")]
        public string? RegistrationNo { get; set; }

        [Required(ErrorMessage = "Founded Date is required.")]
        public DateTime? FoundedDate { get; set; }

        [Url(ErrorMessage = "Invalid website URL.")]
        public string? Website { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        [Required(ErrorMessage = "Please select departments.")]
        public List<int>? SelectedDepIDs { get; set; }

        public CompanyDepartmentMapping? CD_Mapping_VM { get; set; }
    }
}
