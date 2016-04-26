using Code_College.Models;
using Language.Lua;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Code_College.Controllers
{
    public class CodeController : ApiController
    {
        private static ExDBEntities ExDB = new ExDBEntities();
        private static UserDBEntities UserDB = new UserDBEntities();

        // POST api/<controller>
        [HttpPost]
        public string PostSubmission(Submission Submission)
        {
            // Double checks the user that uploaded the code is a valid user
            if (!Authenticate(Submission.username, Submission.password))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            Exercise CurrentExercise = ExDB.Exercises.Where(x => x.ExID == Submission.id).FirstOrDefault();

            // Checks to see if the exercise is a valid exercise and throws a HTTP 404 if not
            if (CurrentExercise == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // Returns output of the program
            return SubmitCode(Submission.code, CurrentExercise, Submission.username);
        }

        // Authenticates the user
        private bool Authenticate(string Username, string Password)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user.PasswordHash == Account.HashCredentials(user.Email, Account.Cryptography.Decrypt(Password, Username)))
                return true;
            else
                return false;
        }

        // Handles interpretation of code and returns the output of the code
        public static string SubmitCode(string Code, Exercise CurrentExercise, string Username)
        {
            bool Correct;
            string ConsoleOutput = "Sorry, that was incorrect. Please, read the task and try again.";

            Code += CurrentExercise.ExAppendCode ?? "";

            Marker.Marker.MarkScheme = CurrentExercise.ExMarkScheme;

            try
            {
                LuaInterpreter.RunCode(Code);
                Correct = Marker.Marker.FullMark();
            }
            catch (Exception ex)
            {
                Correct = false;

                ConsoleOutput = ex.Message;
            }

            if (Correct)
            {
                Account.LevelUp(Username, CurrentExercise);
                ConsoleOutput = LuaInterpreter.CodeReport.Output;

                return ConsoleOutput;
            }
            else
                return ConsoleOutput;
        }
    }

    public class Submission
    {
        public string code { get; set; }
        public int id { get; set; }
        public string password { get; set; }
        public string username { get; set; }
    }
}