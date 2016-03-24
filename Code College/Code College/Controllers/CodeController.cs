using Code_College.Models;
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
            if (!Authenticate(Submission.username, Submission.password))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            Exercise CurrentExercise = ExDB.Exercises.Where(x => x.ExID == Submission.id).FirstOrDefault();

            if (CurrentExercise == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return ExerciseController.SubmitCode(Submission.code, CurrentExercise, Submission.username);
        }

        private bool Authenticate(string Username, string Password)
        {
            User user = UserDB.Users.Where(x => x.Username == Username).FirstOrDefault();

            if (user.PasswordHash == Account.HashCredentials(user.Email, Account.Cryptography.Decrypt(Password, Username)))
                return true;
            else
                return false;
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