namespace FastFood.Core.Controllers
{
    using System;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using FastFood.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using ViewModels.Employees;

    public class EmployeesController : Controller
    {
        private readonly FastFoodContext _context;
        private readonly IMapper _mapper;

        public EmployeesController(FastFoodContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Register()
        {
            List< RegisterEmployeeViewModel> employees = await _context.Positions
                .AsNoTracking()
                .ProjectTo<RegisterEmployeeViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterEmployeeInputModel model)
        {
            var employee = _mapper.Map<Employee>(model);

            await _context.Employees.AddAsync(employee);

            await _context.SaveChangesAsync();

            return RedirectToAction("All", "Employees");
        }

        public async Task<IActionResult> All()
        {
            List<EmployeesAllViewModel> employeesView = await _context.Employees
                .ProjectTo<EmployeesAllViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return View(employeesView);
        }
    }
}
