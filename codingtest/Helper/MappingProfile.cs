using AutoMapper;
using codingtest.DTOs;
using codingtest.Entities;

namespace codingtest.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Organization, OrganizationSummaryDto>();
            CreateMap<User, UserSummaryDto>();
        }
    }
}