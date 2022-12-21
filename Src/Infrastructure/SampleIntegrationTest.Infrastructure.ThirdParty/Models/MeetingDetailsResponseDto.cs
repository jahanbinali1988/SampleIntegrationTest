namespace SampleIntegrationTest.Infrastructure.ThirdParty.Models
{
    public class MeetingDetailsResponseDto
    {
        public MeetingDetailsResponseDto()
        {

        }
        public Guid Id { get; set; }
        public string HostFullName { get; set; }
        public string Address { get; set; }
    }
}
