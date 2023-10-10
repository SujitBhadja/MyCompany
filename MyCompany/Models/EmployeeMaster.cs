using System;
using System.Collections.Generic;

namespace MyCompany.Models;

public partial class EmployeeMaster
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public DateTime? Dob { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Designation { get; set; }

    public DateTime? JoiningDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastUpdated { get; set; }
}
