using AzmoonYarWeb.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Drawing;

namespace WebShahrdari.Areas.Admin3mill.Models
{
    public static class Tools
    {
        // agar Type true bashad yani (shura) ast dar gheyre in surat Channel ast
        public static bool IsLiked(int Id, string type, string IdString)
        {
            List<string> Ids = new List<string>();
            Ids = IdString.Split('-').ToList();
            if (type == "Post")
                return Ids.Exists(u => u == "PL" + Id);
            else if (type == "PostComment")
                return Ids.Exists(u => u == "PCL" + Id);
            else
                return Ids.Exists(u => u == "PCRL" + Id);
        }
        public static bool IsDisLiked(int Id, string type, string IdString)
        {
            List<string> Ids = new List<string>();
            Ids = IdString.Split('-').ToList();
            if (type == "Post")
                return Ids.Exists(u => u == "PD" + Id);
            else if (type == "PostComment")
                return Ids.Exists(u => u == "PCD" + Id);
            else
                return Ids.Exists(u => u == "PCRD" + Id);
        }
        public static string ConvertNativeDigits(this string text)
        {

            if (text == null)
                return null;
            if (text.Length == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (char character in text)
            {
                if (char.IsDigit(character))
                    sb.Append(char.GetNumericValue(character));
                else
                    sb.Append(character);
            }
            return sb.ToString();


        }
        private static readonly CultureInfo arabic = new CultureInfo("fa-IR");
        private static readonly CultureInfo latin = new CultureInfo("en-US");

        /// <summary>
        /// in tabe jahate tabdile zabane english be arabic ( ta hududi farsi ) estefade mishavad
        /// </summary>
        /// <param name="input">reshteye morede nazar baraye tabdil</param>
        /// <returns>
        /// string
        /// reshteye tabdil shode
        /// </returns>
        public static string ToArabic(string input)
        {
            var arabicDigits = arabic.NumberFormat.NativeDigits;
            for (int i = 0; i < arabicDigits.Length; i++)
            {
                input = input.Replace(i.ToString(), arabicDigits[i]);
            }
            return input;
        }

        public static string ToLatin(string input)
        {
            var latinDigits = latin.NumberFormat.NativeDigits;
            var arabicDigits = arabic.NumberFormat.NativeDigits;
            for (int i = 0; i < latinDigits.Length; i++)
            {
                input = input.Replace(arabicDigits[i], latinDigits[i]);
            }
            return input;
        }

        /// <summary>
        /// in tabe tarikhe miladi ra dar forme datetime gerefte va tarikhe jalali ra dar forme string baz migardanad
        /// </summary>
        /// <param name="date">tarikhe morede nazar jahate tabdil</param>
        /// <returns>
        /// string
        /// tarikhe tabdil shode be jalali
        /// </returns>
        public static string GetDateTimeReturnJalaliDate(DateTime date)
        {
            PersianCalendar p = new PersianCalendar();
            int Month = p.GetMonth(date);
            int Year = p.GetYear(date);
            int Day = p.GetDayOfMonth(date);
            int Hour = p.GetHour(date);
            int Minute = p.GetMinute(date);
            int Second = p.GetSecond(date);
            string result1 = "";
            string result = Tools.ToArabic(Year.ToString()) + '/';
            if (Month.ToString().Count() == 2)
                result += Tools.ToArabic(Month.ToString()) + '/';
            else
                result += '۰' + Tools.ToArabic(Month.ToString()) + '/';
            if (Day.ToString().Count() == 2)
                result += Tools.ToArabic(Day.ToString());
            else
                result += '۰' + Tools.ToArabic(Day.ToString());
            if (Hour.ToString().Count() == 2)
                result1 += Tools.ToArabic(Hour.ToString()) + ':';
            else
                result1 += '۰' + Tools.ToArabic(Hour.ToString()) + ':';
            if (Minute.ToString().Count() == 2)
                result1 += Tools.ToArabic(Minute.ToString()) + ':';
            else
                result1 += '۰' + Tools.ToArabic(Minute.ToString()) + ':';
            if (Second.ToString().Count() == 2)
                result1 += Tools.ToArabic(Second.ToString());
            else
                result1 += '۰' + Tools.ToArabic(Second.ToString());
            string FinalResult = result + " " + result1;

            return FinalResult;
        }
        public static string JalaliDateWithoutHour(DateTime date)
        {
            PersianCalendar p = new PersianCalendar();
            int Month = p.GetMonth(date);
            int Year = p.GetYear(date);
            int Day = p.GetDayOfMonth(date);
            string result = Tools.ToArabic(Year.ToString()) + '/';
            if (Month.ToString().Count() == 2)
                result += Tools.ToArabic(Month.ToString()) + '/';
            else
                result += '۰' + Tools.ToArabic(Month.ToString()) + '/';
            if (Day.ToString().Count() == 2)
                result += Tools.ToArabic(Day.ToString());
            else
                result += '۰' + Tools.ToArabic(Day.ToString());

            return result;
        }

        /// <summary>
        /// in tabe tarikhe jalali ra dar forme string gerefte va tarikhe miladi ra dar forme datetime baz migardanad
        /// </summary>
        /// <param name="date">tarikhe jalaliye morede nazar dar forme string</param>
        /// <param name="_Date"></param>
        /// <returns>
        ///  
        /// </returns>
        public static bool GetJalaliDateReturnDateTime(string date, out DateTime _Date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                Regex rex = new Regex(@"^[۰-۹0-9]{4}\/[۰-۹0-9]{2}\/[۰-۹0-9]{2} [۰-۹0-9]{2}:[۰-۹0-9]{2}:[۰-۹0-9]{2}$");
                if (rex.Match(date).Success)
                {
                    string firstpart = date.Substring(0, date.IndexOf(':') - 2);
                    string SecondPart = date.Substring(date.IndexOf(':') - 2);
                    string[] persianDatePartsStart = firstpart.Split('/');
                    string[] persianDatePartsStartHour = SecondPart.Split(':');


                    int persianYearStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[0]));
                    int persianMonthStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[1]));
                    int persianDayStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[2]));
                    int persianHourStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[0]));
                    int persianMinStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[1]));
                    int persianSecondStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[2]));

                    string datetimeString = string.Format("{0}-{1}-{2} {3}:{4}:{5}", persianYearStart, persianMonthStart, persianDayStart, persianHourStart, persianMinStart, persianSecondStart);

                    PersianCalendar pc = new PersianCalendar();
                    try
                    {
                        DateTime start = new DateTime(persianYearStart, persianMonthStart, persianDayStart, persianHourStart, persianMinStart, persianSecondStart, pc);
                        _Date = start;
                        return true; ;
                    }
                    catch
                    {
                        _Date = DateTime.Now;
                        return false;
                    }
                }
            }
            _Date = DateTime.Now;
            return false; ;
        }

        public static string ImageSave_MaintainAspect(HttpPostedFileBase Content_Two, string F_UserName, string PathForSave = "PostImagesPath")
        {
            string path = Tools.ReturnPathPhysicalMode(PathForSave, F_UserName, "ImageSave_MaintainAspect()");
            string extension = Path.GetExtension(Content_Two.FileName);
            string curFile = "";
            string RandomValueString;
            Random rnd = new Random();
            do
            {

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = path + "/" + RandomValueString + extension;  //Your path
            } while (File.Exists(curFile));

            string pic = System.IO.Path.GetFileName(RandomValueString + extension);
            WebImage img = new WebImage(Content_Two.InputStream);
            if (img.Width < 790 || img.Height < 460)
            {
                int wi;
                int hi;
                // maintain the aspect ratio despite the thumbnail size parameters
                if (img.Width > img.Height)
                {
                    wi = 790;
                    hi = (int)(img.Height * ((decimal)790 / img.Width));
                }
                else
                {
                    hi = 460;
                    wi = (int)(img.Width * ((decimal)460 / img.Height));
                }
                img.Resize(wi, hi);
            }
            img.Save(path + pic);
            return RandomValueString + extension;
        }



        public static string ImageSave_Gallery(HttpPostedFileBase Content_Two, string PathForSave)
        {

            string path = PathForSave;

            string extension = Path.GetExtension(Content_Two.FileName);
            string curFile = "";
            string RandomValueString;
            Random rnd = new Random();
            do
            {

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = path + "/" + RandomValueString + extension;  //Your path
            } while (File.Exists(curFile));



            //string pic = System.IO.Path.GetFileName(RandomValueString + extension);

            WebImage img = new WebImage(Content_Two.InputStream);
            string newextension = img.ImageFormat;
            if (newextension.ToLower() == "jpeg")
            {
                newextension = "jpg";
            }
            if (img.Width < 790 || img.Height < 460)
            {
                int wi;
                int hi;
                // maintain the aspect ratio despite the thumbnail size parameters
                if (img.Width > img.Height)
                {
                    wi = 790;
                    hi = (int)(img.Height * ((decimal)790 / img.Width));
                }
                else
                {
                    hi = 460;
                    wi = (int)(img.Width * ((decimal)460 / img.Height));
                }
                img.Resize(wi, hi);
            }
         img.Save(path + "/" + RandomValueString+"."+newextension);

            return RandomValueString +"." +newextension;
        }


        public static string ContentFour_Save(string ContentFour, int ID,string F_UserName)
        {

            string path = Tools.ReturnPathPhysicalMode("ContentFourPath", F_UserName, "ContentFour_Save()");
            string RandomValueString = "Description_" + ID;
            Random rnd = new Random();
            string extension = ".txt";
            string curFile = path + "/" + RandomValueString + extension;
            int i = 1;
            while (File.Exists(curFile))
            {

                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = path + "/" + RandomValueString + "_" + i + extension;  //Your path
                i++;
            }
            System.IO.File.WriteAllText(curFile, ContentFour);
            return RandomValueString + extension;
        }

        public static void ContentFour_Edit(string ContentFour,string F_UserName, string ContentFour_Path)
        {
            string path = Tools.ReturnPathPhysicalMode("ContentFourPath", F_UserName, "ContentFour_Edit()");
            path = path + "/" + ContentFour_Path;
            System.IO.File.WriteAllText(path, ContentFour);

        }
        public static string ContentFour_Get(string ContentFour, string profile)
        {
            string path = Tools.ReturnPathPhysicalMode("ContentFourPath", profile, "ContentFour_Get()");

                try
                {
                    return System.IO.File.ReadAllText(path + ContentFour);
                }
                catch
                {
                    return "خطا در عملیات پردازش متن";
                }
   

        }
        public static string F_UserId()
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                return System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
        }
        //public static int F_UserId(string name)
        //{
        //    using (WebShahrdari.Models.ShahrdarEntities db = new ShahrdarEntities())
        //    {
        //        var _profile = db.UserInformations.Where(u => u.Name == name).FirstOrDefault();
        //        if (_profile != null)
        //        {
        //            return _profile.ID;
        //        }
        //    }
        //    return -1;
        //}
        //public static string F_UserName(int F_UserId)
        //{
        //    using (WebShahrdari.Models.ShahrdarEntities db = new ShahrdarEntities())
        //    {
        //        var _profile = db.UserInformations.Where(u => u.ID == F_UserId).FirstOrDefault();
        //        if (_profile != null)
        //        {
        //            return _profile.Name;
        //        }
        //    }
        //    return "";
        //}
        //public static string F_UserName()
        //{
        //    using (WebShahrdari.Models.ShahrdarEntities db = new ShahrdarEntities())
        //    {
        //        string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
        //        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //        var currentUser = manager.FindById(userId);

        //        var _info = db.UserInformations.FirstOrDefault(u => u.ID == currentUser.F_UserAccount_Id);
        //        if (_info != null)
        //        {
        //            return _info.Name;
        //        }
        //    }
        //    return "";
        //}

        public static string[] PasswordGenerator(int size)
        {
            string[] pass = new string[size];
            Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Day * DateTime.Now.Second);
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < size; i++)
            {
                pass[i] = new string(Enumerable.Repeat(chars, 10)
                   .Select(s => s[rnd.Next(s.Length)]).ToArray());

            }
            return pass;
        }


        public static string ImageSave_Profile(HttpPostedFileBase Content_Two, int x, int y, string profile, string prefix)
        {
   
            string path = ReturnPathPhysicalMode("ProfileImageBasePath", profile, "ImageSave_Profile()");
            string extension = Path.GetExtension(Content_Two.FileName);
            string curFile = "";
            //string RandomValueString;
            //Random rnd = new Random();
            //do
            //{
            //    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //    RandomValueString = new string(Enumerable.Repeat(chars, 12)
            //     .Select(s => s[rnd.Next(s.Length)]).ToArray());
            curFile = path + "/" + prefix + "_" + profile + ".png";  //Your path
            //} while (File.Exists(curFile));
            //   string pic = System.IO.Path.GetFileName(RandomValueString + extension);

            WebImage img = new WebImage(Content_Two.InputStream);
            img.Resize(x, y);
            img.Save(curFile, "png",false);
            return prefix + "_" + profile + ".png";
        }




        //public static ContactManagementModel XMLToPersonalSetting(string profile)
        //{

        //    ContactManagementModel OBj = new ContactManagementModel();
        //    string path = ReturnPathPhysicalMode("ContactPath", profile, "ContactManagementModel()");



        //        var serializer = new XmlSerializer(typeof(ContactManagementModel));

        //        using (var reader = XmlReader.Create(path + "/" + profile + "_PersonalSetting" + ".xml"))
        //        {
        //            OBj = (ContactManagementModel)serializer.Deserialize(reader);
        //            return OBj;
        //        }
           
        
        //}




        public static string ReturnPath(string ConfigPath, string F_UserName,string Caller)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path= string.Format(section[ConfigPath], F_UserName);
            MakeVaidPath(Path, Caller,false,F_UserName);
            return Path;
        }
        public static string ReturnPathPhysicalMode(string ConfigPath, string F_UserName, string Caller)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = HttpContext.Current.Server.MapPath("~" + string.Format(section[ConfigPath], F_UserName));
            MakeVaidPath(Path, Caller,true,F_UserName);
            return Path;
        }

        public static void MakeVaidPath(string Path, string Caller, bool isPhysicalPath, string F_UserName)
        {
            if (!isPhysicalPath)
            {
                 Path = HttpContext.Current.Server.MapPath(Path);
            }
            if (!System.IO.Directory.Exists(Path))
            {
                System.IO.Directory.CreateDirectory(Path);
                PipoLog(string.Format("Log : Directory Not Exist <<{0}>> Called by {1} At Time {2} For User {3}", Path, Caller, DateTime.Now,F_UserName));
            }
        }
        public static void PipoLog(string Content)
        {
            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/App_Data/PipoLog/PipoLog.txt"), Content+Environment.NewLine);
        }


    }


    //public class PageTitle
    //{
    //    public PageTitle(string _Pagetitle, string _ActionName, string _ControllerName, string _HtmlAttribute, string _RouteValue)
    //    {
    //        Pagetitle = _Pagetitle;
    //        ActionName = _ActionName;
    //        ControllerName = _ControllerName;
    //        RouteValue = _RouteValue;
    //        HtmlAttribute = _HtmlAttribute;
    //    }
    //    public string Pagetitle { get; set; }
    //    public string ActionName { get; set; }
    //    public string ControllerName { get; set; }
    //    public string RouteValue { get; set; }
    //    public string HtmlAttribute { get; set; }
    //}
    public class SendingReciveingSMS
    {
        string IP = "http://193.104.22.14:2055/CPSMSService/Access";
        string Number = "10006020";
        string UserName = "ATSIGNCO9";
        string Password = "m@hfye4@5";
        string Company = "ATSIGNCO";

        /// <summary>
        /// ersale taki
        /// </summary>
        /// <param name="msg">matne payam</param>
        /// <param name="dest">adrese magsad</param>
        /// <returns>vaziyate payamake ersal shode ke dars surate ersale movafag true mibashad</returns>
        //public string[] Send_single(string msg, string dest)
        //{
        //    string[] status = new string[2];
        //    if (string.IsNullOrEmpty(msg))
        //    {
        //        status[0] = "message is empty";
        //        return status;
        //    }

        //    if (string.IsNullOrEmpty(dest))
        //    {
        //        status[0] = "destination is empty";
        //        return status;
        //    }

        //    int msg_part = (int)Math.Ceiling((double)msg.Length / 70);
        //    int sms_amount;
        //    if (int.TryParse(SMS_Amount(), out sms_amount))
        //    {
        //        if (sms_amount < msg_part)
        //            status[0] = "SMS amount is insufficient";
        //    }

        //    Cls_SMS.ClsSend sms_Single = new Cls_SMS.ClsSend();
        //    status = sms_Single.SendSMS_Single(msg, dest, Number, UserName, Password, IP, Company, false);
        //    return status;
        //}
        //public string SMS_Amount()
        //{
        //    Cls_SMS.ClsGetRemain sms = new Cls_SMS.ClsGetRemain();
        //    string amount = sms.GetRemainCredit(UserName, Password, Company, IP);
        //    //lblRemain.Text = rem + " عدد پيامك  ";
        //    return amount;
        //}
    }


    public class InitilizeUserRequirements
    {
        public string InitUserFolders(string profile)
        {

            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Result;
            try
            {
                foreach (string key in section)
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Format("~/"+section[key], profile)));
                }
                Result = "OK";
            }
            catch (Exception e)
            {
                Result = "NOK :  " + e.ToString();
            }
            return Result;

        }
        public string DestroyUserFolders(string profile)
        {

            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Result;
            try
            {
                foreach (string key in section)
                {
                    System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(string.Format(section[key], profile)),true);
                }
                Result = "OK";
            }
            catch (Exception e)
            {
                Result = "NOK :  " + e.ToString();
            }
            return Result;

        }
        public  void SavePasswordFor3mill(string pass,string profile,string Role)
        {
            string Path = HttpContext.Current.Server.MapPath("~/App_Data/UsersKey/"+profile+"/");
            if (!System.IO.Directory.Exists(Path))
            {
                System.IO.Directory.CreateDirectory(Path);
            }
            string curFile = Path + Role + "_Key.txt";
            string EncryptedPass = StringCipher.Encrypt(pass, "ParsaDorsa");
            System.IO.File.WriteAllText(curFile, EncryptedPass);
        }

        public  string GetPasswordFor3mill(string profile,string Role)
        {
            string pass = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/UsersKey/" + profile + "/" + Role + "_Key.txt"));
            return StringCipher.Decrypt(pass, "ParsaDorsa");
        }
    }

    public static class StringCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}