namespace GM.HTTP
{
    public struct HTTPServerConfig
    {
        public string Address;
        public int Port;

        public string Url => string.Format("http://{0}:{1}/api", Address, Port);
    }
}
