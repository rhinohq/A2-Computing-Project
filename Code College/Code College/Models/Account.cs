﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Code_College.Models
{
    public class Account
    {
        private UserDBEntities UserDB = new UserDBEntities();

        public string HashCredentials(string Salt, string Password)
        {
            SHA512 Hash = SHA512.Create();

            string Salted = Password + Salt;
            string HashedCredentials = Convert.ToBase64String(Hash.ComputeHash(Encoding.UTF8.GetBytes(Salted)));

            return HashedCredentials;
        }

        public void CreateNewUser(string Name, string Email, string Username, string Password, string DOB, char Gender, Bitmap PP, HttpResponseBase Response)
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

        public bool VerifyUser(string Username, string Password)
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

        public void AddCookie(string Username, string Password, HttpResponseBase Response, bool RememberUser)
        {
            HttpCookie LoginCookie = new HttpCookie("UserAuthentication");
            LoginCookie["User"] = Username;
            LoginCookie["Password"] = Password;

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
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user != null)
            {
                UserDB.Users.Remove(user);
                UserDB.SaveChangesAsync();
            }
        }

        public void ChangePassword(string Email, string Username, string NewPassword)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user != null)
            {
                user.PasswordHash = HashCredentials(Email, NewPassword);
                UserDB.SaveChangesAsync();
            }
        }

        public void ChangePP(string Username, Bitmap NewPP)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user != null)
            {
                user.ProfilePicture = NewPP;
                UserDB.SaveChangesAsync();
            }
        }

        public Bitmap ProcessImage(Image UploadedPicture)
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
                    Regex UsernameValidator = new Regex("[a-zA-Z0-9'-_.]", RegexOptions.Compiled);

                    Validated = UsernameValidator.IsMatch(Data);

                    break;
            }

            return Validated;
        }

        public bool VerifyCookie(HttpRequest Request, HttpCookie Cookie)
        {
            User user = UserDB.Users.Where(x => x.Username == Cookie["Username"]).FirstOrDefault();

            if (user == null)
            {
                return false;
            }
            else if (user.PasswordHash == HashCredentials(Cookie["Username"], Cookie["Password"]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}