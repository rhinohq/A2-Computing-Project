using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Code_College.Models
{
    public class Account
    {
        private UserDBEntities DB = new UserDBEntities();

        public string HashCredentials(string Salt, string Password)
        {
            SHA512 Hash = SHA512.Create();

            string Salted = Password + Salt;
            string HashedCredentials = Convert.ToBase64String(Hash.ComputeHash(Encoding.UTF8.GetBytes(Salted)));

            return HashedCredentials;
        }

        public void CreateNewUser(string Name, string Email, string Username, string Password, string DOB, char Gender, Bitmap PP, HttpResponse Response)
        {
            User user = DB.Users.Where(x => x.Username == Username).FirstOrDefault();

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
                    
                }

                NewUser.UserScore = 0;
                NewUser.UserLevel = 0;

                DB.Users.Add(NewUser);
                DB.SaveChangesAsync();

                AddCookie(NewUser.Username, Response, false);
            }
            else
            {
                Response.Write("Account taken");
            }
        }

        public bool VerifyUser(string Username, string Password)
        {
            User user = DB.Users.Where(x => x.Username == Username).FirstOrDefault();

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

        public static void AddCookie(string Username, HttpResponse Response, bool RememberUser)
        {
            HttpCookie LoginCookie = new HttpCookie("UserAuthentication");
            LoginCookie["User"] = Username;

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

        public void DeleteUser(string Username, string Password)
        {
            User user = DB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user != null)
            {
                DB.Users.Remove(user);
                DB.SaveChangesAsync();
            }
        }

        public void ChangePassword(string Email, string Username, string NewPassword)
        {
            User user = DB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user != null)
            {
                user.PasswordHash = HashCredentials(Email, NewPassword);
                DB.SaveChangesAsync();
            }
        }

        public void ChangePP(string Username, Bitmap NewPP)
        {
            User user = DB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user != null)
            {
                user.ProfilePicture = NewPP;
                DB.SaveChangesAsync();
            }
        }

        public Bitmap ProcessImage(Bitmap UploadedPicture)
        {
            Bitmap Image;
            Graphics GImage = Graphics.FromImage(UploadedPicture);

            GImage.InterpolationMode = InterpolationMode.HighQualityBicubic;
            GImage.SmoothingMode = SmoothingMode.HighQuality;
            GImage.PixelOffsetMode = PixelOffsetMode.HighQuality;
            GImage.CompositingQuality = CompositingQuality.HighQuality;

            return Image;
        }

        public void RemoveCookie(HttpRequest Request)
        {
            HttpCookie Cookie = Request.Cookies["UserAuthentication"];

            if (Cookie != null)
            {
                Cookie.Expires.AddYears(-2);
            }
        }

        public bool Validation(string Data, char FieldType)
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
                    Regex UsernameValidator = new Regex("[a-zA-Z0-9'-_]", RegexOptions.Compiled);

                    Validated = UsernameValidator.IsMatch(Data);

                    break;
            }

            return Validated;
        }
    }
}