using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD
{
    public class clsUtil
    {
       static  string DestinationFolder = @"F:\DVLD\Images\";

        private static  bool _CreateFolderIfNotExist()
        {
            if(!Directory.Exists(DestinationFolder))
            {
                try
                {
                    Directory.CreateDirectory(DestinationFolder);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        public static string GenerateGuid()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
        public static string _ReplaceFileNameWithGuid(string SourceFile)
        {
            FileInfo file = new FileInfo(SourceFile);
            string ext = file.Extension;
            return  GenerateGuid() + ext;
        }
        public static bool CopyImageToProjectImagesFolder(ref string SourceFile)
        {
            if(!_CreateFolderIfNotExist())
                return false;
           string DestinationFile = DestinationFolder + _ReplaceFileNameWithGuid(SourceFile);
            try
            {
                File.Copy(SourceFile, DestinationFile, true);
            }
            catch
            {
                return false;
            }
            SourceFile = DestinationFile;
                return true;
        }
    }
}
