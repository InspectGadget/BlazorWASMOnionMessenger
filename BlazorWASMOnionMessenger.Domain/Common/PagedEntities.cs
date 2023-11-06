namespace BlazorWASMOnionMessenger.Domain.Common
{
    public class PagedEntities<T> : ResponseDto where T : class
    {
        public int Quantity { get; set; }
        public int Pages { get; set; }
        public List<T> Entities { get; } = new List<T>();

        public PagedEntities(List<T> entities)
        {
            Entities.AddRange(entities);
        }
    }
}
