using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace KXP.EGB.Tools
{
    public class CheckVersion
    {
        private static readonly string ExpressDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                                        "Kofax");

        public static string GetExpressDatabasePath()
        {
            if (!File.Exists(Path.Combine(GetExpressLicensingPath(), "KofaxLicenseManager.xml")))
                throw new FileNotFoundException(string.Format("{0} not found.", Path.Combine(GetExpressLicensingPath(), "KofaxLicenseManager.xml")));

            string versionFolder = GetFolderName(GetVersion(Path.Combine(GetExpressLicensingPath(), "KofaxLicenseManager.xml")));

            if (string.IsNullOrEmpty(versionFolder))
                throw new Exception("Invalid Kofax Express Folder");

            return Path.Combine(ExpressDataFolder, versionFolder);
        }

        private static string GetFolderName(string version)
        {
            Version expressVersion = new Version(version);

            if (expressVersion.Major == 1 && expressVersion.Minor == 0)
                return "Kofax Express";

            if (expressVersion.Major == 1 && expressVersion.Minor == 1)
                return "Kofax Express 1.1";

            if (expressVersion.Major == 2 && expressVersion.Minor == 0)
                return "Kofax Express 2.0";

            return string.Empty;
        }

        private static string GetVersion(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlElement element = doc.DocumentElement;

            XmlNode version = element.SelectSingleNode("//*/ProductVersion");

            return version.InnerText;
        }

        private static string GetExpressLicensingPath()
        {
            return Path.Combine(ExpressDataFolder, @"Kofax Express Licensing");
        }

        public static string GetExpressVersion()
        {
            return GetVersion(Path.Combine(GetExpressLicensingPath(), "KofaxLicenseManager.xml"));
        }
    }

}
