namespace septa.Auth.Domain.Entities
{
    public class ApiResourceSecret : Secret
    {
        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }








}
