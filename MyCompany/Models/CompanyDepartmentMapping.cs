using System;
using System.Collections.Generic;

namespace MyCompany.Models;

public partial class CompanyDepartmentMapping
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public int? DepartmentId { get; set; }

    public DateTime? CreatedDate { get; set; }
}
