using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Code_College.Models
{
    public static class Account
    {
        private static UserDBEntities UserDB = new UserDBEntities();

        public static string HashCredentials(string Salt, string Password)
        {
            SHA512 Hash = SHA512.Create();

            string Salted = Password + Salt;
            string HashedCredentials = Convert.ToBase64String(Hash.ComputeHash(Encoding.UTF8.GetBytes(Salted)));

            return HashedCredentials;
        }

        public static void CreateNewUser(string Name, string Email, string Username, string Password, string DOB, char Gender, Bitmap PP, HttpResponseBase Response)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user == null)
            {
                User NewUser = new User();
                NewUser.Name = Name;
                NewUser.Email = Email;
                NewUser.Username = Username;
                NewUser.PasswordHash = HashCredentials(Email, Password);
                NewUser.DOB = DOB;
                NewUser.Gender = Gender;

                if (PP == null)
                {
                    NewUser.ProfilePicture = ProcessImage(Properties.Resources.DefaultPP);
                }
                else if (PP.Height == 512 && PP.Width == 512)
                    NewUser.ProfilePicture = PP;
                else
                    NewUser.ProfilePicture = ProcessImage(PP);

                NewUser.UserScore = 0;
                NewUser.UserLevel = 0;

                UserDB.Users.Add(NewUser);
                UserDB.SaveChangesAsync();

                AddCookie(NewUser.Username, Password, Response, false);
            }
            else
            {
                Response.Write("Account taken");
            }
        }

        public static bool VerifyUser(string Username, string Password)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user == null)
            {
                return false;
            }
            else if (user.PasswordHash == HashCredentials(Username, HashCredentials(Username, Password)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void AddCookie(string Username, string Password, HttpResponseBase Response, bool RememberUser)
        {
            HttpCookie LoginCookie = new HttpCookie("UserAuth");
            LoginCookie["Username"] = Username;
            LoginCookie["Password"] = Cryptography.Encrypt(Password, Username);

            if (RememberUser)
            {
                LoginCookie.Expires = DateTime.Now.AddYears(1);
            }
            else
            {
                LoginCookie.Expires = DateTime.Now.AddDays(1);
            }

            Response.Cookies.Add(LoginCookie);
        }

        public static void DeleteUser(string Username, string Password)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user != null)
            {
                UserDB.Users.Remove(user);
                UserDB.SaveChangesAsync();
            }
        }

        public static void ChangePassword(string Email, string Username, string NewPassword)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user != null)
            {
                user.PasswordHash = HashCredentials(Email, NewPassword);
                UserDB.SaveChangesAsync();
            }
        }

        public static void ChangePP(string Username, Bitmap NewPP)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user != null)
            {
                user.ProfilePicture = NewPP;
                UserDB.SaveChangesAsync();
            }
        }

        public static Bitmap ProcessImage(Image UploadedPicture)
        {
            var destRect = new Rectangle(0, 0, 512, 512);
            var destImage = new Bitmap(512, 512);

            destImage.SetResolution(UploadedPicture.HorizontalResolution, UploadedPicture.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(UploadedPicture, destRect, 0, 0, UploadedPicture.Width, UploadedPicture.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static void RemoveCookie(HttpRequest Request)
        {
            HttpCookie Cookie = Request.Cookies["UserAuth"];

            if (Cookie != null)
            {
                Cookie.Expires.AddYears(-2);
            }
        }

        public static bool Validation(string Data, char FieldType)
        {
            bool Validated = false;

            switch (FieldType)
            {
                case 'n':
                    Regex NameValidator = new Regex("[a-zA-Z'-]", RegexOptions.Compiled);

                    Validated = NameValidator.IsMatch(Data);

                    break;

                case 'e':
                    Regex EmailValidator = new Regex("[a-zA-Z0-9'-@.]", RegexOptions.Compiled);

                    Validated = EmailValidator.IsMatch(Data);

                    break;

                case 'u':
                    Regex UsernameValidator = new Regex("[a-zA-Z0-9'-_.]", RegexOptions.Compiled);

                    Validated = UsernameValidator.IsMatch(Data);

                    break;
            }

            return Validated;
        }

        public static bool VerifyCookie(HttpRequest Request, HttpCookie Cookie)
        {
            User user = UserDB.Users.Where(x => x.Username == Cookie["Username"]).FirstOrDefault();

            if (user == null)
                return false;
            else if (user.PasswordHash == HashCredentials(Cookie["Username"], Cryptography.Decrypt(Cookie["Password"], Cookie["Username"])))
                return true;
            else
                return false;
        }

        public static string GetCookieUsername(HttpRequest Request, HttpCookie Cookie)
        {
            User user = UserDB.Users.Where(x => x.Username == Cookie["Username"]).FirstOrDefault();

            if (user == null)
                return null;
            else if (Cookie["Username"] != null)
                return Cookie["Username"];
            else
                return null;
        }

        public static void LevelUp(string Username)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            user.UserLevel++;

            UserDB.SaveChangesAsync();
        }

        public class Cryptography
        {
            private const int Keysize = 256;
            private const int DerivationIterations = 1000;

            public static string Encrypt(string PlainText, string PassPhrase)
            {
                byte[] SaltStringBytes = Generate256BitsOfRandomEntropy();
                byte[] IVStringBytes = Generate256BitsOfRandomEntropy();
                byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);

                using (Rfc2898DeriveBytes Password = new Rfc2898DeriveBytes(PassPhrase, SaltStringBytes, DerivationIterations))
                {
                    byte[] KeyBytes = Password.GetBytes(Keysize / 8);

                    using (RijndaelManaged SymmetricKey = new RijndaelManaged())
                    {
                        SymmetricKey.BlockSize = 256;
                        SymmetricKey.Mode = CipherMode.CBC;
                        SymmetricKey.Padding = PaddingMode.PKCS7;

                        using (var Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, IVStringBytes))
                        {
                            using (MemoryStream MemoryStream = new MemoryStream())
                            {
                                using (CryptoStream CryptoStream = new CryptoStream(MemoryStream, Encryptor, CryptoStreamMode.Write))
                                {
                                    CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                                    CryptoStream.FlushFinalBlock();

                                    byte[] CipherTextBytes = SaltStringBytes;

                                    CipherTextBytes = CipherTextBytes.Concat(IVStringBytes).ToArray();
                                    CipherTextBytes = CipherTextBytes.Concat(MemoryStream.ToArray()).ToArray();

                                    MemoryStream.Close();
                                    CryptoStream.Close();

                                    return Convert.ToBase64String(CipherTextBytes);
                                }
                            }
                        }
                    }
                }
            }

            public static string Decrypt(string CipheredText, string PassPhrase)
            {
                byte[] cipherTextBytesWithSaltAndIv = Convert.FromBase64String(CipheredText);
                byte[] saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
                byte[] ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
                byte[] CipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

                using (Rfc2898DeriveBytes Password = new Rfc2898DeriveBytes(PassPhrase, saltStringBytes, DerivationIterations))
                {
                    byte[] KeyBytes = Password.GetBytes(Keysize / 8);

                    using (RijndaelManaged SymmetricKey = new RijndaelManaged())
                    {
                        SymmetricKey.BlockSize = 256;
                        SymmetricKey.Mode = CipherMode.CBC;
                        SymmetricKey.Padding = PaddingMode.PKCS7;

                        using (var Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, ivStringBytes))
                        {
                            using (MemoryStream MemoryStream = new MemoryStream(CipherTextBytes))
                            {
                                using (CryptoStream CryptoStream = new CryptoStream(MemoryStream, Decryptor, CryptoStreamMode.Read))
                                {
                                    var PlainTextBytes = new byte[CipherTextBytes.Length];
                                    var DecryptedByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);

                                    MemoryStream.Close();
                                    CryptoStream.Close();

                                    return Encoding.UTF8.GetString(PlainTextBytes, 0, DecryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }

            private static byte[] Generate256BitsOfRandomEntropy()
            {
                byte[] RandomBytes = new byte[32];

                using (RNGCryptoServiceProvider RNGCSP = new RNGCryptoServiceProvider())
                    RNGCSP.GetBytes(RandomBytes);

                return RandomBytes;
            }
        }
    }
}