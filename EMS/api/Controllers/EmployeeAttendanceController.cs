﻿using api.Dto.EmployeeAttendance;
using api.Dto.Employees;
using api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/employee-attendance")]
    [Controller]
    public class EmployeeAttendanceController(IEmployeeAttendanceService context):ControllerBase
    {
        private readonly IEmployeeAttendanceService _context = context;

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.GetAllAttendanceAsync());
        }

        //[HttpGet]
        //[Route("get")]
        //public async Task<IActionResult> Get(int id)
        //{
        //    return Ok(await _context.GetEmployeeWithAttendanceAsync(id));
        //}

        
        [HttpPost]
        [Route("add")]
        public async Task<OkResult> Add([FromBody] EmployeeAttendanceCreateDto dto)
        {
            await _context.AddAttendanceAsync(dto);
            return Ok();
        }

        [HttpPatch]
        [Route("update")]
        public async Task<OkResult> Update(int attendanceId,[FromBody] EmployeeAttendanceUpdateDto dto)
        {
            await _context.UpdateAttendanceAsync(attendanceId,dto);
            return Ok();
        }


    }
}