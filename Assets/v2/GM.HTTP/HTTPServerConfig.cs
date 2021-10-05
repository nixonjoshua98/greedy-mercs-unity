namespace GM.HTTP
{
    public struct HTTPServerConfig
    {
        public string Address;

        public int Port;

        public string UrlFor(string endpoint) => string.Format("http://{0}:{1}/api/{2}", Address, Port, endpoint);
    }
}
