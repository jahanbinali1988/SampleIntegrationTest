namespace SampleIntegrationTest.Tests.Setup
{
    public class FakeApiFactory
    {
        public static BaseFakeApi GetApi(FakeApiType fakeApiType)
        {
            switch (fakeApiType)
            {
                case FakeApiType.Minimal:
                    return new MinimalFakeApiBuilder();
                case FakeApiType.Maximal:
                    return new MaximalFakeApiBuilder();
                default:
                    break;
            }
            return null;
        }
    }
}
