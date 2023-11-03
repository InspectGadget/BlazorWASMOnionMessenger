namespace BlazorWASMOnionMessenger.Domain.Common
{
    public class PagedEntities<T> where T : class
    {
        public int Quantity { get; set; }
        public int Pages { get; set; }
        public List<T> Entities { get; } = new List<T>();

        public PagedEntities(IEnumerable<T> entities)
        {
            Entities.AddRange(entities);
        }
    }
}
