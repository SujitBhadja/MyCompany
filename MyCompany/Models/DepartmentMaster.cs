using System;
using System.Collections.Generic;

namespace MyCompany.Models;

public partial class DepartmentMaster
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime? EstablishedDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastUpdated { get; set; }
}
