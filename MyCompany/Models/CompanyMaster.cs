using System;
using System.Collections.Generic;

namespace MyCompany.Models;

public partial class CompanyMaster
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? RegistrationNo { get; set; }

    public DateTime? FoundedDate { get; set; }

    public string? Website { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastUpdated { get; set; }
}
