using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class AttachmentRule : System.Web.UI.Page
    {
        public static string Upload(string fileFullPath, string tempTableName, string storedProcedureName, int instanceId)
        {
            return AttachmentDao.Upload(fileFullPath, tempTableName, storedProcedureName, instanceId);
        }

        public static string FormatPath(string path, bool isFolder)
        {
            if (isFolder && !string.IsNullOrEmpty(path) && path.Replace("/", "\\").LastIndexOf("\\") < path.Length - 1)
            {
                return path + "\\";
            }

            return path;
        }

        public static string GetFullPath(string path, bool isFolder = false)
        {
            path = FormatPath(path, isFolder);
            if (!string.IsNullOrEmpty(path) && !path.Contains(":"))
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            return path;
        }

        public static string GetVirtualPath(string path, bool isFolder = false)
        {
            path = FormatPath(path, isFolder);
            if (!string.IsNullOrEmpty(path) && path.Contains(":"))
            {
                return path.Replace(AppDomain.CurrentDomain.BaseDirectory, string.Empty);
            }

            return path;
        }

#region common Utility
            public static string MimeType(string file)
            {
                string retval = string.Empty;
                switch (Path.GetExtension(file).ToUpperInvariant())
                {
                    case ".3DM":
                        retval = "x-world/x-3dmf";
                        break;
                    case ".3DMF":
                        retval = "x-world/x-3dmf";
                        break;
                    case ".A":
                        retval = "application/octet-stream";
                        break;
                    case ".AAB":
                        retval = "application/x-authorware-bin";
                        break;
                    case ".AAM":
                        retval = "application/x-authorware-map";
                        break;
                    case ".AAS":
                        retval = "application/x-authorware-seg";
                        break;
                    case ".ABC":
                        retval = "text/vnd.abc";
                        break;
                    case ".ACGI":
                        retval = "text/html";
                        break;
                    case ".AFL":
                        retval = "video/animaflex";
                        break;
                    case ".AI":
                        retval = "application/postscript";
                        break;
                    case ".AIF":
                        retval = "audio/aiff";
                        break;
                    case ".AIFC":
                        retval = "audio/aiff";
                        break;
                    case ".AIFF":
                        retval = "audio/aiff";
                        break;
                    case ".AIM":
                        retval = "application/x-aim";
                        break;
                    case ".AIP":
                        retval = "text/x-audiosoft-intra";
                        break;
                    case ".ANI":
                        retval = "application/x-navi-animation";
                        break;
                    case ".AOS":
                        retval = "application/x-nokia-9000-communicator-add-on-software";
                        break;
                    case ".APS":
                        retval = "application/mime";
                        break;
                    case ".ARC":
                        retval = "application/octet-stream";
                        break;
                    case ".ARJ":
                        retval = "application/arj";
                        break;
                    case ".ART":
                        retval = "image/x-jg";
                        break;
                    case ".ASF":
                        retval = "video/x-ms-asf";
                        break;
                    case ".ASM":
                        retval = "text/x-asm";
                        break;
                    case ".ASP":
                        retval = "text/asp";
                        break;
                    case ".ASX":
                        retval = "video/x-ms-asf";
                        break;
                    case ".AU":
                        retval = "audio/basic";
                        break;
                    case ".AVI":
                        retval = "video/avi";
                        break;
                    case ".AVS":
                        retval = "video/avs-video";
                        break;
                    case ".BCPIO":
                        retval = "application/x-bcpio";
                        break;
                    case ".BIN":
                        retval = "application/octet-stream";
                        break;
                    case ".BM":
                        retval = "image/bmp";
                        break;
                    case ".BMP":
                        retval = "image/bmp";
                        break;
                    case ".BOO":
                        retval = "application/book";
                        break;
                    case ".BOOK":
                        retval = "application/book";
                        break;
                    case ".BOZ":
                        retval = "application/x-bzip2";
                        break;
                    case ".BSH":
                        retval = "application/x-bsh";
                        break;
                    case ".BZ":
                        retval = "application/x-bzip";
                        break;
                    case ".BZ2":
                        retval = "application/x-bzip2";
                        break;
                    case ".C":
                        retval = "text/plain";
                        break;
                    case ".C++":
                        retval = "text/plain";
                        break;
                    case ".CAT":
                        retval = "application/vnd.ms-pki.seccat";
                        break;
                    case ".CC":
                        retval = "text/plain";
                        break;
                    case ".CCAD":
                        retval = "application/clariscad";
                        break;
                    case ".CCO":
                        retval = "application/x-cocoa";
                        break;
                    case ".CDF":
                        retval = "application/cdf";
                        break;
                    case ".CER":
                        retval = "application/pkix-cert";
                        break;
                    case ".CHA":
                        retval = "application/x-chat";
                        break;
                    case ".CHAT":
                        retval = "application/x-chat";
                        break;
                    case ".CLASS":
                        retval = "application/java";
                        break;
                    case ".COM":
                        retval = "application/octet-stream";
                        break;
                    case ".CONF":
                        retval = "text/plain";
                        break;
                    case ".CPIO":
                        retval = "application/x-cpio";
                        break;
                    case ".CPP":
                        retval = "text/x-c";
                        break;
                    case ".CPT":
                        retval = "application/x-cpt";
                        break;
                    case ".CRL":
                        retval = "application/pkcs-crl";
                        break;
                    case ".CRT":
                        retval = "application/pkix-cert";
                        break;
                    case ".CSH":
                        retval = "application/x-csh";
                        break;
                    case ".CSS":
                        retval = "text/css";
                        break;
                    case ".CXX":
                        retval = "text/plain";
                        break;
                    case ".DCR":
                        retval = "application/x-director";
                        break;
                    case ".DEEPV":
                        retval = "application/x-deepv";
                        break;
                    case ".DEF":
                        retval = "text/plain";
                        break;
                    case ".DER":
                        retval = "application/x-x509-ca-cert";
                        break;
                    case ".DIF":
                        retval = "video/x-dv";
                        break;
                    case ".DIR":
                        retval = "application/x-director";
                        break;
                    case ".DL":
                        retval = "video/dl";
                        break;
                    case ".DOC":
                        retval = "application/msword";
                        break;
                    case ".XLS":
                        retval = "application/vnd.ms-excel";
                        break;
                    case ".XLSX":
                        retval = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case ".DOCX":
                        retval = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".DOT":
                        retval = "application/msword";
                        break;
                    case ".DP":
                        retval = "application/commonground";
                        break;
                    case ".DRW":
                        retval = "application/drafting";
                        break;
                    case ".DUMP":
                        retval = "application/octet-stream";
                        break;
                    case ".DV":
                        retval = "video/x-dv";
                        break;
                    case ".DVI":
                        retval = "application/x-dvi";
                        break;
                    case ".DWF":
                        retval = "model/vnd.dwf";
                        break;
                    case ".DWG":
                        retval = "image/vnd.dwg";
                        break;
                    case ".DXF":
                        retval = "image/vnd.dwg";
                        break;
                    case ".DXR":
                        retval = "application/x-director";
                        break;
                    case ".EL":
                        retval = "text/x-script.elisp";
                        break;
                    case ".ELC":
                        retval = "application/x-elc";
                        break;
                    case ".ENV":
                        retval = "application/x-envoy";
                        break;
                    case ".EPS":
                        retval = "application/postscript";
                        break;
                    case ".ES":
                        retval = "application/x-esrehber";
                        break;
                    case ".ETX":
                        retval = "text/x-setext";
                        break;
                    case ".EVY":
                        retval = "application/envoy";
                        break;
                    case ".EXE":
                        retval = "application/octet-stream";
                        break;
                    case ".F":
                        retval = "text/plain";
                        break;
                    case ".F77":
                        retval = "text/x-fortran";
                        break;
                    case ".F90":
                        retval = "text/plain";
                        break;
                    case ".FDF":
                        retval = "application/vnd.fdf";
                        break;
                    case ".FIF":
                        retval = "image/fif";
                        break;
                    case ".FLI":
                        retval = "video/fli";
                        break;
                    case ".FLO":
                        retval = "image/florian";
                        break;
                    case ".FLX":
                        retval = "text/vnd.fmi.flexstor";
                        break;
                    case ".FMF":
                        retval = "video/x-atomic3d-feature";
                        break;
                    case ".FOR":
                        retval = "text/x-fortran";
                        break;
                    case ".FPX":
                        retval = "image/vnd.fpx";
                        break;
                    case ".FRL":
                        retval = "application/freeloader";
                        break;
                    case ".FUNK":
                        retval = "audio/make";
                        break;
                    case ".G":
                        retval = "text/plain";
                        break;
                    case ".G3":
                        retval = "image/g3fax";
                        break;
                    case ".GIF":
                        retval = "image/gif";
                        break;
                    case ".GL":
                        retval = "video/gl";
                        break;
                    case ".GSD":
                        retval = "audio/x-gsm";
                        break;
                    case ".GSM":
                        retval = "audio/x-gsm";
                        break;
                    case ".GSP":
                        retval = "application/x-gsp";
                        break;
                    case ".GSS":
                        retval = "application/x-gss";
                        break;
                    case ".GTAR":
                        retval = "application/x-gtar";
                        break;
                    case ".GZ":
                        retval = "application/x-gzip";
                        break;
                    case ".GZIP":
                        retval = "application/x-gzip";
                        break;
                    case ".H":
                        retval = "text/plain";
                        break;
                    case ".HDF":
                        retval = "application/x-hdf";
                        break;
                    case ".HELP":
                        retval = "application/x-helpfile";
                        break;
                    case ".HGL":
                        retval = "application/vnd.hp-hpgl";
                        break;
                    case ".HH":
                        retval = "text/plain";
                        break;
                    case ".HLB":
                        retval = "text/x-script";
                        break;
                    case ".HLP":
                        retval = "application/hlp";
                        break;
                    case ".HPG":
                        retval = "application/vnd.hp-hpgl";
                        break;
                    case ".HPGL":
                        retval = "application/vnd.hp-hpgl";
                        break;
                    case ".HQX":
                        retval = "application/binhex";
                        break;
                    case ".HTA":
                        retval = "application/hta";
                        break;
                    case ".HTC":
                        retval = "text/x-component";
                        break;
                    case ".HTM":
                        retval = "text/html";
                        break;
                    case ".HTML":
                        retval = "text/html";
                        break;
                    case ".HTMLS":
                        retval = "text/html";
                        break;
                    case ".HTT":
                        retval = "text/webviewhtml";
                        break;
                    case ".HTX":
                        retval = "text/html";
                        break;
                    case ".ICE":
                        retval = "x-conference/x-cooltalk";
                        break;
                    case ".ICO":
                        retval = "image/x-icon";
                        break;
                    case ".IDC":
                        retval = "text/plain";
                        break;
                    case ".IEF":
                        retval = "image/ief";
                        break;
                    case ".IEFS":
                        retval = "image/ief";
                        break;
                    case ".IGES":
                        retval = "application/iges";
                        break;
                    case ".IGS":
                        retval = "application/iges";
                        break;
                    case ".IMA":
                        retval = "application/x-ima";
                        break;
                    case ".IMAP":
                        retval = "application/x-httpd-imap";
                        break;
                    case ".INF":
                        retval = "application/inf";
                        break;
                    case ".INS":
                        retval = "application/x-internett-signup";
                        break;
                    case ".IP":
                        retval = "application/x-ip2";
                        break;
                    case ".ISU":
                        retval = "video/x-isvideo";
                        break;
                    case ".IT":
                        retval = "audio/it";
                        break;
                    case ".IV":
                        retval = "application/x-inventor";
                        break;
                    case ".IVR":
                        retval = "i-world/i-vrml";
                        break;
                    case ".IVY":
                        retval = "application/x-livescreen";
                        break;
                    case ".JAM":
                        retval = "audio/x-jam";
                        break;
                    case ".JAV":
                        retval = "text/plain";
                        break;
                    case ".JAVA":
                        retval = "text/plain";
                        break;
                    case ".JCM":
                        retval = "application/x-java-commerce";
                        break;
                    case ".JFIF":
                        retval = "image/jpeg";
                        break;
                    case ".JFIF-TBNL":
                        retval = "image/jpeg";
                        break;
                    case ".JPE":
                        retval = "image/jpeg";
                        break;
                    case ".JPEG":
                        retval = "image/jpeg";
                        break;
                    case ".JPG":
                        retval = "image/jpeg";
                        break;
                    case ".JPS":
                        retval = "image/x-jps";
                        break;
                    case ".JS":
                        retval = "application/x-javascript";
                        break;
                    case ".JUT":
                        retval = "image/jutvision";
                        break;
                    case ".KAR":
                        retval = "audio/midi";
                        break;
                    case ".KSH":
                        retval = "application/x-ksh";
                        break;
                    case ".ROFF":
                        retval = "application/x-troff";
                        break;
                    case ".RP":
                        retval = "image/vnd.rn-realpix";
                        break;
                    case ".RPM":
                        retval = "audio/x-pn-realaudio-plugin";
                        break;
                    case ".RT":
                        retval = "text/richtext";
                        break;
                    case ".RTF":
                        retval = "text/richtext";
                        break;
                    case ".RTX":
                        retval = "text/richtext";
                        break;
                    case ".RV":
                        retval = "video/vnd.rn-realvideo";
                        break;
                    case ".WAV":
                        retval = "audio/wav";
                        break;
                    case ".WBMP":
                        retval = "image/vnd.wap.wbmp";
                        break;
                    case ".WEB":
                        retval = "application/vnd.xara";
                        break;
                    case ".WMF":
                        retval = "windows/metafile";
                        break;
                    case ".WML":
                        retval = "text/vnd.wap.wml";
                        break;
                    case ".WMLC":
                        retval = "application/vnd.wap.wmlc";
                        break;
                    case ".WORD":
                        retval = "application/msword";
                        break;
                    case ".XML":
                        retval = "application/xml";
                        break;
                    case ".X-PNG":
                        retval = "image/png";
                        break;
                    case ".ZIP":
                        retval = "application/zip";
                        break;
                    default:
                        retval = "application/octet-stream";
                        break;
                }

                return retval;
            }

            public static decimal GetIisVersion(HttpRequest request)
            {
                if (request != null)
                {
                    string os = request.ServerVariables["SERVER_SOFTWARE"];
                    if (!string.IsNullOrEmpty(os))
                    {
                        int dash = os.LastIndexOf("/");
                        if (dash > 0)
                        {
                            decimal iisVer = 0M;
                            if (decimal.TryParse(os.Substring(dash + 1), out iisVer))
                            {
                                return iisVer;
                            }
                        }
                    }
                }

                decimal serverVersion = Environment.OSVersion.Version.Major + ((decimal)Environment.OSVersion.Version.MajorRevision / 10);
                if (serverVersion == 6.1M)
                {
                    return 7.5M;
                }

                if (serverVersion == 6.0M)
                {
                    return 7.0M;
                }

                if (serverVersion == 5.2M)
                {
                    return 6.0M;
                }

                if (serverVersion == 5.1M)
                {
                    return 5.1M;
                }

                if (serverVersion == 5.0M)
                {
                    return 5.0M;
                }

                return -1M;
            }

            public static string GetRootUrl()
            {
                string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
                if (port == null || port == "80" || port == "443")
                {
                    port = string.Empty;
                }
                else
                {
                    port = ":" + port;
                }

                string protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
                if (protocol == null || protocol == "0")
                {
                    protocol = "http://";
                }
                else
                {
                    protocol = "https://";
                }

                string rootUrl = protocol + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port + HttpContext.Current.Request.ApplicationPath;
                if (rootUrl.LastIndexOf("/") == rootUrl.Length - 1)
                {
                    rootUrl = rootUrl.Remove(rootUrl.Length - 1, 1);
                }

                return rootUrl;
            }

            public static string FilterChinese(string input)
            {
                var result = string.Empty;

                if (true == string.IsNullOrEmpty(input))
                {
                    return result;
                }

                var pattern = @"[\u4E00-\u9FFF]*";

                result = Regex.Replace(input, pattern, string.Empty);

                return result;
            }
#endregion

    }
}
