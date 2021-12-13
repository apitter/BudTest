using BudTest.Api.DTOs;
using BudTest.Application.Interface.Exceptions;
using BudTest.Application.Interface.Models;
using BudTest.Application.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudTest.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class WorldBankController : ControllerBase
    {
        //private readonly ILogger<WorldBankController> _logger;
        private readonly ICountryService _countryService;

        public WorldBankController(
            //ILogger<WorldBankController> logger,
            ICountryService countryService)
        {
            //_logger = logger;
            _countryService = countryService;
        }

        [HttpGet("alpha2code/{alpha2code}")]
        public async Task<ActionResult> Get(string alpha2code)
        {
            try
            {
                //_logger.LogInformation($"WorldBankController.Get/{alpha2code}");
                var country = await _countryService.GetAsync(alpha2code);

                return StatusCode(200, CreateDto(country));
            }
            catch (WebResponseException wEx)
            {
                return StatusCode(wEx.StatusCode, wEx.Message);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return StatusCode(500, "An unexpected error has occured");
            }
        }

        #region mappers

        private CountryDto CreateDto(ICountry country) => 
            new CountryDto
            {
                Id = country.Id,
                Iso2Code = country.Iso2Code,
                Name = country.Name,
                Region = country.Region == null ? null : CreateRegionDto(country.Region),
                AdminRegion = country.AdminRegion == null ? null : CreateRegionDto(country.AdminRegion),
                IncomeLevel = country.IncomeLevel == null ? null : CreateRegionDto(country.IncomeLevel),
                LendingType = country.LendingType == null ? null : CreateRegionDto(country.LendingType),
                CapitalCity = country.CapitalCity,
                Longitude = country.Longitude,
                Latitude = country.Latitude
            };

        private RegionDto CreateRegionDto(IRegion region) =>
            new RegionDto
            {
                Id = region.Id,
                Iso2Code = region.Iso2Code,
                Value = region.Value,
            };

        #endregion

    }

}
