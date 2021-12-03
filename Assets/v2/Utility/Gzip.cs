using System.IO;
using System.IO.Compression;
using System.Text;

public class GZip
{
    // Given by William at Tier 9 Studios (Auto Battles Online)

    public static byte[] Zip(byte[] bytes)
    {
        byte[] zippedBytes = null;

        using (MemoryStream msi = new MemoryStream(bytes))
        {
            using (MemoryStream mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }

                zippedBytes = mso.ToArray();

            }
        }
        return zippedBytes;
    }

    public static byte[] Zip(string str)
    {
        return Zip(Encoding.UTF8.GetBytes(str));
    }

    public static string Unzip(byte[] bytes)
    {
        string unzippedStr = null;

        using (MemoryStream msi = new MemoryStream(bytes))
        {
            using (MemoryStream mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }

                unzippedStr = Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        return unzippedStr;
    }
}