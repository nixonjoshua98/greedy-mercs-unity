using System.IO;

namespace GM
{
    public static class MemoryStream_Extensions
    {
        public static string ReadToEnd(this MemoryStream stream)
        {
            stream.Position = 0;

            StreamReader R = new StreamReader(stream);

            return R.ReadToEnd();
        }
    }
}
