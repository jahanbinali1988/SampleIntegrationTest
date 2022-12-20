namespace SampleIntegrationTest.Application.Contract.Shared
{
    public abstract class GetListQueryBase
    {
        public GetListQueryBase(int offset, int count)
        {
            Offset = offset;
            Count = count;
        }

        public virtual int Offset { get; set; }
        public virtual int Count { get; set; }
    }
}
