using AutoMapper;
using back_end.Models;
using back_end.Repositories;
using back_end.Services.Location;
using back_end.ViewModels.CompanySetup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CompanySetupController : ControllerBase
    {
        private ILocationService _locationService;
        private IRepository<CompanySetup> _companySetupRepository;
        private IMapper _mapper;

        public CompanySetupController(ILocationService locationService, IRepository<CompanySetup> companySetupRepository, IMapper mapper)
        {
            _locationService = locationService;
            _companySetupRepository = companySetupRepository;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public ActionResult GetCompanySetups()
        {
            var currentCompanySetup = _companySetupRepository.GetAll().ToList();
            return Ok(currentCompanySetup);
        }

        [HttpGet("{companySetupId}")]
        public ActionResult GetCompanySetup(int companySetupId)
        {
            var currentCompanySetup = _companySetupRepository.GetByFilter(x => x.Id == companySetupId).FirstOrDefault();

            if (currentCompanySetup == null)
            {
                return NotFound("Company setup not found.");
            }

            return Ok(currentCompanySetup);
        }

        [HttpPost]
        public ActionResult CreateCompanySetup(CompanySetupCreateViewModel createCompanySetup)
        {
            var companySetup = _mapper.Map<CompanySetup>(createCompanySetup);
            _companySetupRepository.Add(companySetup);
            _companySetupRepository.SaveChanges();

            return Ok(companySetup);
        }

        [HttpPut("{companySetupId}")]
        public ActionResult UpdateCompanySetup(int companySetupId, CompanySetupUpdateViewModel updateCompanySetup)
        {
            var currentCompanySetup = _companySetupRepository.GetByFilter(x => x.Id == companySetupId).FirstOrDefault();
            currentCompanySetup = _mapper.Map(updateCompanySetup, currentCompanySetup);

            _companySetupRepository.Update(currentCompanySetup);
            _companySetupRepository.SaveChanges();

            _locationService.setCompanyCoordinates(currentCompanySetup);

            return Ok("Company setup updated successfully.");
        }

        //[HttpDelete("{companySetupId}")]
        //public ActionResult DeleteCompanySetup(int companySetupId)
        //{
        //    var companySetup = _companySetupRepository.GetByFilter(x => x.companyId == companySetupId).FirstOrDefault();

        //    if (companySetup == null)
        //    {
        //        return NotFound("Company setup not found.");
        //    }

        //    _companySetupRepository.Delete(companySetup);
        //    _companySetupRepository.SaveChanges();

        //    return Ok("Company setup deleted successfully.");
        //}
    }
}
