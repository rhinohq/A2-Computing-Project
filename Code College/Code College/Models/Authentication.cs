using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Code_College.Models
{
    public class Authentication
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
                    GenerateRandomImage(Gender);
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
                user.PP = NewPP;
                DB.SaveChangesAsync();
            }
        }

        public Bitmap GenerateRandomImage(char Gender)
        {
            Bitmap Pixel = new Bitmap(1, 1);
            Pixel.SetPixel(0, 0, Color.White);

            Bitmap Image = new Bitmap(Pixel, 400, 400);
            Random Rand = new Random();
            int ColoredBits = Rand.Next(1, 17);
            int i, j, count = 0;
            Color color = new Color();
            int[] UsedPixels;
            UsedPixels = new int[ColoredBits * 100];

            if (Gender == 'M')
            {
                color = Color.Blue;
            }
            else if (Gender == 'F')
            {
                color = Color.Pink;
            }
            else
            {
                color = Color.Black;
            }

            while (count < ColoredBits)
            {
                i = Rand.Next(1, 5) * 10;
                j = Rand.Next(1, 5) * 10;

                for (int index = 0; index <= 200; index++)
                {
                    Image.SetPixel(i, j, color);

                    ++i;
                    ++j;
                }

                count++;
            }

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
    }
}