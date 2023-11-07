namespace codingtest.DTOs
{
    public class OrganizationSummaryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int BlacklistTotal { get; set; }
        public int TotalCount { get; set; }
        public List<UserSummaryDto> Users { get; set; }
    }
}