namespace SampleIntegrationTest.Infrastructure.ThirdParty.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }

        public ApiResponseMeta Meta { get; set; }
    }
}
