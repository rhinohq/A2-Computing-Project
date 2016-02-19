﻿using Code_College.Models;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Code_College.Controllers
{
    public class CodeController : ApiController
    {
        private static UserDBEntities UserDB = new UserDBEntities();
        private static ExDBEntities ExDB = new ExDBEntities();
        private string URL;

        // GET api/<controller>/5
        [HttpGet]
        public string GetSubmission(Submission Submission)
        {
            if (!Authenticate(Submission.username, Submission.password))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            Exercise CurrentExercise = ExDB.Exercises.Where(x => x.ExID == Submission.id).FirstOrDefault();

            if (CurrentExercise == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            string console = ExerciseController.SubmitCode(Submission.code, CurrentExercise, Submission.username);

            return console;
        }
        
        // POST api/<controller>
        [HttpPost]
        public string PostSubmission(Submission Submission)
        {
            if (!Authenticate(Submission.username, Submission.password))
                throw new HttpResponseException(HttpStatusCode.Unauthorized);

            Exercise CurrentExercise = ExDB.Exercises.Where(x => x.ExID == Submission.id).FirstOrDefault();

            if (CurrentExercise == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            string console = ExerciseController.SubmitCode(Submission.code, CurrentExercise, Submission.username);

            return console;
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
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set;}
        public string code { get; set; }
    }
}