using Microsoft.AspNetCore.Mvc;
using codingtest.Entities;
using AutoMapper;
using codingtest.DTOs;

namespace codingtest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationSummaryController : ControllerBase
    {
        private readonly IMapper _mapper;

        public OrganizationSummaryController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationSummaryDto>>> GetOrganizationSummaries()
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://607a0575bd56a60017ba2618.mockapi.io/");

                var organizationsResponse = await httpClient.GetAsync("organization");
                organizationsResponse.EnsureSuccessStatusCode();
                var organizations = await organizationsResponse.Content.ReadFromJsonAsync<List<Organization>>();

                var summaries = new List<OrganizationSummaryDto>();

                foreach (var organization in organizations)
                {
                    var response = await httpClient.GetAsync($"organization/{organization.Id}/users");
                    response.EnsureSuccessStatusCode();
                    var users = await response.Content.ReadFromJsonAsync<List<User>>();

                    foreach (var user in users)
                    {
                        var phonesResponse = await httpClient.GetAsync($"organization/{organization.Id}/users/{user.Id}/phones");
                        phonesResponse.EnsureSuccessStatusCode();
                        var phones = await phonesResponse.Content.ReadFromJsonAsync<List<Phone>>();

                        user.PhoneCount = phones.Count;

                        user.BlacklistTotal = phones.Count(phone => phone.Blacklist = true);
                    }

                    organization.TotalCount = users.Sum(user => user.PhoneCount);
                    organization.BlacklistTotal = users.Sum(user => user.BlacklistTotal);

                    var organizationSummaryDto = _mapper.Map<OrganizationSummaryDto>(organization);
                    organizationSummaryDto.Users = _mapper.Map<List<UserSummaryDto>>(users);

                    summaries.Add(organizationSummaryDto);
                }

                return Ok(summaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}