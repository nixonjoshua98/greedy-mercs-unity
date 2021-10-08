namespace GM.HTTP.Requests
{
    public interface ILoginRequest : IServerRequest
    {
        string DeviceId { get; set; }
    }

    public class UserLoginRequest : ILoginRequest
    {
        public string DeviceId { get; set; }
    }


    public class UserLoginReponse : ServerResponse
    {
        public string UserId;
    }
}
