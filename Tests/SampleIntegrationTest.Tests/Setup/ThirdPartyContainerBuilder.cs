using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.VisualBasic;

namespace SampleIntegrationTest.Tests.Setup
{
    public class ThirdPartyContainerBuilder
    {
        private static TestcontainersContainer? _serviceContainer;
        private int _thirdPartyFakePort;
        public int ThirdPartyFakePort
        {
            get
            {
                if (_thirdPartyFakePort == 0)
                    GenerateRandomPort();

                return _thirdPartyFakePort;
            }
            set
            {
                _thirdPartyFakePort = value;
            }
        }

        public TestcontainersContainer Build()
        {
            GenerateRandomPort();
            TestcontainersContainer serviceContainer =
                new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("webapplication2:latest")
                .WithPortBinding(80, true)
                .WithExposedPort(80)
                .WithPortBinding(_thirdPartyFakePort, 80)
                .Build();

            _serviceContainer = serviceContainer;
            return serviceContainer;
        }
        public KeyValuePair<string, string>[] GetConfiguration()
        {
            if (_serviceContainer == null)
                Build();

            List<KeyValuePair<string, string>> configs = new List<KeyValuePair<string, string>>();

            var thirdPartyBaseUrl = $"{ConstantVariables.ThirdPartyServiceBaseUrl}{_thirdPartyFakePort}";
            var thirdPartyServiceAddress = thirdPartyBaseUrl + "/meeting/{0}/details";
            configs.Add(new KeyValuePair<string, string>("ThirdPartyConfiguration:GetMeetingDetailUrl", thirdPartyServiceAddress));

            return configs.ToArray();
        }

        private void GenerateRandomPort()
        {
            if (_thirdPartyFakePort == default)
            {
                Random rnd = new Random();
                var port = rnd.Next(1000, 5000);
                _thirdPartyFakePort = port;
            }
        }
    }
}
