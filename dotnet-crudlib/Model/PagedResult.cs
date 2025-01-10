namespace NetCrudStarter.Model
{

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = []; 
        public int TotalItems { get; set; }
    }
}