namespace AuthAPI.Contracts.Requests
{
    public class GetRolesRequest
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
