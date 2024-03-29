namespace FireManagerServer.Database.Entity
{
    public class BaseDate
    {
        public DateTime? DateCreate { get; set; } = DateTime.Now;
        public DateTime? DateUpdate { get; set; }
    }
}
