using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Code_College.Models
{
    public class Authentication
    {
        private UserDBEntities db = new UserDBEntities();

        public string HashCredentials(string Salt, string Password)
        {
            SHA512 Hash = SHA512.Create();

            string Salted = Password + Salt;
            string HashedCredentials = Convert.ToBase64String(Hash.ComputeHash(Encoding.UTF8.GetBytes(Salted ?? "")));

            return HashedCredentials;
        }

        public void CreateNewUser(string Name, string Email, string Username, string Password, string DOB, HttpResponse Response)
        {
            User user = db.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user == null)
            {
                User NewUser = new User();
                NewUser.Name = Name;
                NewUser.Email = Email;
                NewUser.Username = Username;
                NewUser.PasswordHash = HashCredentials(Email, Password);
                NewUser.DOB = DOB;
                NewUser.PlayerScore = 0;
                NewUser.PlayerLevel = 0;

                db.Users.Add(NewUser);
                db.SaveChangesAsync();

                AddCookie(NewUser.Username, Response, false);
            }
            else
            {
                Response.Write("Account taken");
            }
        }

        public bool VerifyUser(string Username, string Password)
        {
            User user = db.Users.Where(x => x.Username == Username).FirstOrDefault();

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
            User user = db.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user != null)
            {
                db.SaveChangesAsync();
            }
        }

        public void ChangePassword(string Username, string NewPassword)
        {
            User user = db.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user == null)
            {
            }
            else
            {
                db.SaveChangesAsync();
            }
        }
    }
}