using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CORE.ApplicationCommon.Helpers
{
    public static class SaveImageProcess
    {

        public static FTPInformation GetFTPInformation(string _admin)
        {
            FTPInformation ftpInfo = new FTPInformation();
            switch (_admin)
            {
                case "Admin":
                    ftpInfo.Url = "ftp://uploads.gazetekapi.com//uploads/images";
                    ftpInfo.UserName = "sysuser_8";
                    ftpInfo.Password = "1g1*j0Ld";
                    break;

                default:
                    break;
            }
            return ftpInfo;
        }

        public static string ImageInsert(IFormFile _file, string _admin)
        {

            try
            {

                FTPInformation fTPInformation = GetFTPInformation(_admin);
                var uploadurl = fTPInformation.Url;
                var username = fTPInformation.UserName;
                var password = fTPInformation.Password;

                string uploadfilename = Path.GetFileNameWithoutExtension(_file.FileName);
                //string extension = Path.GetExtension(_file.FileName);
                uploadfilename = uploadfilename + DateTime.Now.ToString("yymmssfff") + ".webp"; /* + extension;*/
                Stream streamObj = _file.OpenReadStream();
                byte[] buffer = new byte[_file.Length];
                streamObj.Read(buffer, 0, buffer.Length);
                streamObj.Close();
                string ftpurl = string.Format("{0}/{1}", uploadurl, uploadfilename);
                var requestObj = WebRequest.Create(ftpurl) as FtpWebRequest;
                requestObj.Method = WebRequestMethods.Ftp.UploadFile;
                requestObj.Credentials = new NetworkCredential(username, password);
                Stream requestStream = requestObj.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Flush();
                requestStream.Close();

                return uploadfilename;
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;
                return null;
            }
        }
      
    }

    public class FTPInformation
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
    }
}
