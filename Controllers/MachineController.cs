using AutoMapper;
using back_end.Mediators.Machines;
using back_end.Models;
using back_end.Repositories;
using back_end.Services.Location;
using back_end.Services.ZKEM_Machine;
using back_end.ViewModels.Machine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MachineController : ControllerBase
    {
        private IMachinesMediator _machinesMediator;

        public MachineController(IMachinesMediator machinesMediator)
        {
            _machinesMediator = machinesMediator;
        }

        [HttpGet("All")]
        public ActionResult GetAllMachines()
        {
            var machines = _machinesMediator.GetAllMachines();
            return Ok(machines);
        }

        [HttpGet("{machineCode}")]
        public ActionResult GetMachine(int machineCode)
        {
            var machine = _machinesMediator.GetMachineByCode(machineCode);

            if (machine == null)
            {
                return NotFound("Machine not found.");
            }

            return Ok(machine);
        }

        [HttpPost]
        public ActionResult CreateMachine(MachineViewModel machineViewModel)
        {
            var machine = _machinesMediator.CreateMachine(machineViewModel);

            return Ok(machine);
        }

        [HttpPost("Connect/{machineCode}")]
        public ActionResult ConnectToZkemDevice(int machineCode)
        {
            var success = _machinesMediator.ConnectToMachine(machineCode);

            if (success)
            {
                return Ok("Machine connected successfully.");
            }

            return BadRequest("Failed to connect to the machine.");
        }

        [HttpPost("Disconnect/{machineCode}")]
        public ActionResult DisconnectFromZkemDevice(int machineCode)
        {
            _machinesMediator.DisconnectFromMachine(machineCode);

            return Ok("Machine disconnected successfully");
        }

        [HttpPut("{machineCode}")]
        public ActionResult UpdateMachine(int machineCode, MachineViewModel machineViewModel)
        {
            var result = _machinesMediator.UpdateMachine(machineCode, machineViewModel);

            if (!result)
            {
                return NotFound("Machine not found for update.");
            }

            return Ok("Machine updated successfully.");
        }

        [HttpDelete("{machineCode}")]
        public ActionResult DeleteMachine(int machineCode)
        {
            var result = _machinesMediator.DeleteMachine(machineCode);

            if (!result)
            {
                return NotFound("Machine not found for delete.");
            }

            return Ok("Machine deleted successfully.");
        }
    }
}
