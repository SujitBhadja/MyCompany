using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;
using MyCompany.Models;
using MyCompany.Repository;
using MyCompany.Repository.IRepository;
using MyCompany.ViewModel;
using System.Diagnostics;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Security.Policy;

namespace MyCompany.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<CompanyMaster> _companyContext;
        private readonly IRepository<DepartmentMaster> _departmentContext;
        private readonly IRepository<EmployeeMaster> _employeeContext;
        private readonly IRepository<CompanyDepartmentMapping> _cdMappingContext;

        public HomeController(IRepository<CompanyMaster> companyContext, IRepository<DepartmentMaster> departmentContext, IRepository<EmployeeMaster> employeeContext, IRepository<CompanyDepartmentMapping> cdMappingContext)
        {
            _companyContext = companyContext;
            _departmentContext = departmentContext;
            _employeeContext = employeeContext;
            _cdMappingContext = cdMappingContext;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CompanyMaster> companyMasters = await _companyContext.GetAllAsync();
            return View(companyMasters);
        }

        #region Company
        public async Task<IActionResult> AddCompany()
        {
            var model = new Company_VM();
            await BindDropdownLists(0);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompany(Company_VM companyVM)
        {
            await BindDropdownLists(0);
            if (ModelState.IsValid)
            {
                CompanyMaster companyMaster = new CompanyMaster
                {
                    Name = companyVM.Name,
                    Type = companyVM.Type,
                    Description = companyVM.Description,
                    Address = companyVM.Address,
                    Phone = companyVM.Phone,
                    Email = companyVM.Email,
                    RegistrationNo = companyVM.RegistrationNo,
                    FoundedDate = companyVM.FoundedDate,
                    Website = companyVM.Website,
                    CreatedDate = DateTime.Now
                };

                try
                {
                    await _companyContext.AddAsync(companyMaster);

                    for (int i = 0; i < companyVM.SelectedDepIDs.Count; i++)
                    {
                        CompanyDepartmentMapping csMapping = new CompanyDepartmentMapping
                        {
                            CompanyId = companyMaster.Id,
                            DepartmentId = companyVM.SelectedDepIDs[i],
                            CreatedDate = DateTime.Now
                        };
                        await _cdMappingContext.AddAsync(csMapping);
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            return View(companyVM);
        }

        public async Task<IActionResult> UpdateCompany(int id)
        {
            await BindDropdownLists(0);
            var companyModel = await _companyContext.GetByIdAsync(id);

            Company_VM companyVM = new Company_VM
            {
                Name = companyModel.Name,
                Type = companyModel.Type,
                Description = companyModel.Description,
                Address = companyModel.Address,
                Phone = companyModel.Phone,
                Email = companyModel.Email,
                RegistrationNo = companyModel.RegistrationNo,
                FoundedDate = companyModel.FoundedDate,
                Website = companyModel.Website,
            };

            var cdMappingModel = await _cdMappingContext.GetAllAsync();
            var cdMappingModelList = cdMappingModel.Where(d => d.CompanyId == id).ToList();
            List<int> depIDs = new List<int>();
            for (int i = 0; i < cdMappingModelList.Count; i++)
            {
                depIDs.Add(Convert.ToInt32(cdMappingModelList[i].DepartmentId));
            }
            companyVM.SelectedDepIDs = depIDs;

            if (companyModel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(companyVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCompany(Company_VM companyVM)
        {
            await BindDropdownLists(0);
            if (ModelState.IsValid)
            {
                try
                {
                    var companyMaster = await _companyContext.GetByIdAsync(companyVM.Id);
                    companyMaster.Name = companyVM.Name;
                    companyMaster.Type = companyVM.Type;
                    companyMaster.Description = companyVM.Description;
                    companyMaster.Address = companyVM.Address;
                    companyMaster.Phone = companyVM.Phone;
                    companyMaster.Email = companyVM.Email;
                    companyMaster.RegistrationNo = companyVM.RegistrationNo;
                    companyMaster.FoundedDate = companyVM.FoundedDate;
                    companyMaster.Website = companyVM.Website;
                    companyMaster.LastUpdated = DateTime.Now;

                    await _companyContext.UpdateAsync(companyMaster);

                    var cdMappingIDs = await _cdMappingContext.GetAllAsync();
                    var cMappingIDs = cdMappingIDs.Where(d => d.CompanyId == companyMaster.Id).ToList();
                    if (cMappingIDs.ToList().Count > 0)
                    {
                        await _cdMappingContext.DeleteListAsync(cMappingIDs.Select(d => d.Id).ToList());
                    }

                    for (int i = 0; i < companyVM.SelectedDepIDs.Count; i++)
                    {
                        CompanyDepartmentMapping cdMapping = new CompanyDepartmentMapping
                        {
                            CompanyId = companyMaster.Id,
                            DepartmentId = companyVM.SelectedDepIDs[i],
                            CreatedDate = DateTime.Now
                        };
                        await _cdMappingContext.AddAsync(cdMapping);
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            return View(companyVM);
        }

        public async Task BindDropdownLists(int value)
        {
            var types = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "--- Select company type ---" },
                new SelectListItem { Value = "Proprietorship", Text = "Proprietorship" },
                new SelectListItem { Value = "Partnership", Text = "Partnership" },
                new SelectListItem { Value = "MSME", Text = "MSME" },
                new SelectListItem { Value = "LLC", Text = "LLC" },
                new SelectListItem { Value = "Public Company", Text = "Public Company" },
                new SelectListItem { Value = "Private Company", Text = "Private Company" },
                new SelectListItem { Value = "Nonprofit Organization", Text = "Nonprofit Organization" }
            };
            ViewBag.Types = new SelectList(types, "Value", "Text", value);

            IEnumerable<DepartmentMaster> DepartmentNameList = await _departmentContext.GetAllAsync();
            List<SelectListItem> DepartmentListItems = DepartmentNameList.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).ToList();
            //var comVM = new Company_VM
            //{
            //    DepNameListItems = DepartmentListItems,
            //};
            ViewBag.DepartmentList = DepartmentListItems;
        }
        #endregion

        #region Department
        public async Task<IActionResult> Departments()
        {
            IEnumerable<DepartmentMaster> departmentMasters = await _departmentContext.GetAllAsync();
            return View(departmentMasters);
        }

        public IActionResult AddDepartment()
        {
            var model = new Department_VM();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment(Department_VM depVM)
        {
            if (ModelState.IsValid)
            {
                DepartmentMaster departmentMaster = new DepartmentMaster
                {
                    Name = depVM.Name,
                    Description = depVM.Description,
                    Phone = depVM.Phone,
                    Email = depVM.Email,
                    EstablishedDate = depVM.EstablishedDate,
                    CreatedDate = DateTime.Now
                };

                try
                {
                    await _departmentContext.AddAsync(departmentMaster);

                    return RedirectToAction("Departments", "Home");
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            return View(depVM);
        }

        public async Task<IActionResult> UpdateDepartment(int id)
        {
            var depModel = await _departmentContext.GetByIdAsync(id);

            Department_VM depVM = new Department_VM
            {
                Name = depModel.Name,
                Description = depModel.Description,
                Phone = depModel.Phone,
                Email = depModel.Email,
                EstablishedDate = depModel.EstablishedDate,
            };

            if (depModel == null)
            {
                return RedirectToAction("Departments", "Home");
            }
            return View(depVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDepartment(Department_VM depVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var depMaster = await _departmentContext.GetByIdAsync(depVM.Id);
                    depMaster.Name = depVM.Name;
                    depMaster.Description = depVM.Description;
                    depMaster.Phone = depVM.Phone;
                    depMaster.Email = depVM.Email;
                    depMaster.EstablishedDate = depVM.EstablishedDate;
                    depMaster.LastUpdated = DateTime.Now;

                    await _departmentContext.UpdateAsync(depMaster);

                    return RedirectToAction("Departments", "Home");
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            return View(depVM);
        }
        #endregion

        #region Employee
        public async Task<IActionResult> Employees()
        {
            IEnumerable<EmployeeMaster> empMasters = await _employeeContext.GetAllAsync();
            return View(empMasters);
        }

        public IActionResult AddEmployee()
        {
            var model = new Employee_VM();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(Employee_VM empVM)
        {
            if (ModelState.IsValid)
            {
                EmployeeMaster empMaster = new EmployeeMaster
                {
                    FirstName = empVM.FirstName,
                    LastName = empVM.LastName,
                    Gender = empVM.Gender,
                    Dob = empVM.Dob,
                    Address = empVM.Address,
                    Phone = empVM.Phone,
                    Email = empVM.Email,
                    Designation = empVM.Designation,
                    JoiningDate = empVM.JoiningDate,
                    CreatedDate = DateTime.Now
                };

                try
                {
                    await _employeeContext.AddAsync(empMaster);

                    return RedirectToAction("Employees", "Home");
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            return View(empVM);
        }

        public async Task<IActionResult> UpdateEmployee(int id)
        {
            var empModel = await _employeeContext.GetByIdAsync(id);

            Employee_VM empVM = new Employee_VM
            {
                FirstName = empModel.FirstName,
                LastName = empModel.LastName,
                Gender = empModel.Gender,
                Dob = empModel.Dob,
                Address = empModel.Address,
                Phone = empModel.Phone,
                Email = empModel.Email,
                Designation = empModel.Designation,
                JoiningDate = empModel.JoiningDate
            };

            if (empModel == null)
            {
                return RedirectToAction("Employees", "Home");
            }
            return View(empVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee(Employee_VM empVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var empMaster = await _employeeContext.GetByIdAsync(empVM.Id);
                    empMaster.FirstName = empVM.FirstName;
                    empMaster.LastName = empVM.LastName;
                    empMaster.Gender = empVM.Gender;
                    empMaster.Dob = empVM.Dob;
                    empMaster.Address = empVM.Address;
                    empMaster.Phone = empVM.Phone;
                    empMaster.Email = empVM.Email;
                    empMaster.Designation = empVM.Designation;
                    empMaster.JoiningDate = empVM.JoiningDate;
                    empMaster.LastUpdated = DateTime.Now;

                    await _employeeContext.UpdateAsync(empMaster);

                    return RedirectToAction("Employees", "Home");
                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            return View(empVM);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}