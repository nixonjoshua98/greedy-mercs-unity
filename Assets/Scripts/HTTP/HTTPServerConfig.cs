namespace GM.HTTP
{
    public struct HTTPServerConfig
    {       
        public readonly string BaseURL;

        public HTTPServerConfig(string baseUrl)
        {
            BaseURL = baseUrl;
        }
    }
}
