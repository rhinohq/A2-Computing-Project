using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Code_College.Models
{
    public static class Account
    {
        private static UserDBEntities UserDB = new UserDBEntities();

        public static void AddCookie(string Username, string Password, HttpResponseBase Response)
        {
            HttpCookie LoginCookie = new HttpCookie("CCUserAuth");
            LoginCookie.Values["Username"] = Username;
            LoginCookie.Values["Password"] = Cryptography.Encrypt(Password, Username);

            LoginCookie.Expires = DateTime.Now.AddYears(1);

            Response.Cookies.Add(LoginCookie);
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

        public static void CreateNewUser(string Name, string Email, string Username, string Password, HttpResponseBase Response)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user == null)
            {
                User NewUser = new User();
                NewUser.Name = Name;
                NewUser.Email = Email;
                NewUser.Username = Username;
                NewUser.PasswordHash = HashCredentials(Email, Password);

                NewUser.UserLevel = 1;

                UserDB.Users.Add(NewUser);
                UserDB.SaveChangesAsync();

                AddCookie(NewUser.Username, Password, Response);
            }
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

        public static string HashCredentials(string Salt, string Password)
        {
            SHA512 Hash = SHA512.Create();

            string Salted = Password + Salt;
            string HashedCredentials = Convert.ToBase64String(Hash.ComputeHash(Encoding.UTF8.GetBytes(Salted)));

            return HashedCredentials;
        }

        public static void LevelUp(string Username, Exercise Exercise)
        {
            User User = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (User.UserLevel <= Exercise.ExID)
            {
                User.UserLevel++;

                UserDB.SaveChangesAsync();
            }
        }

        public static void RemoveCookie(HttpRequest Request)
        {
            HttpCookie Cookie = Request.Cookies["CCUserAuth"];

            if (Cookie != null)
                Cookie.Expires.AddYears(-2);
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

        public static bool VerifyCookie(HttpCookie Cookie)
        {
            string Username = Convert.ToString(Cookie.Values["Username"]);
            string Password = Convert.ToString(Cookie.Values["Password"]);
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user == null)
                return false;
            else if (user.PasswordHash == HashCredentials(user.Email, Cryptography.Decrypt(Password, Username)))
                return true;
            else
                return false;
        }

        public static bool VerifyUser(string Username, string Password)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user == null)
                return false;
            else if (user.PasswordHash == HashCredentials(user.Email, Password))
                return true;
            else
                return false;
        }

        public static class Cryptography
        {
            private const int DerivationIterations = 1000;
            private const int Keysize = 256;

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