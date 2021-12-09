namespace GM.HTTP
{
    public struct HTTPServerConfig
    {
        public string Address;

        public int Port;

        public string Url_ => string.Format("http://{0}:{1}/api", Address, Port);

        public string Url(string endpoint) => string.Format("http://{0}:{1}/api/{2}", Address, Port, endpoint);
    }
}
