using System;
using System.IO;

using SQLite;

namespace Project_4.Helpers
{
    public sealed class UserManagert
    {
        private readonly string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "project4.sqlite3");

        public bool IsUserLoggedIn { get; private set; }
        public string UserUsername { get; private set; }
        public string Name { get; private set; }
        public string UserunitNummer { get; private set; }

        private SQLiteConnection dbConn;
        private static UserManagert instance = null;
        private static readonly object padlock = new object();

        UserManagert()
        {
            try
            {
                dbConn = new SQLiteConnection(dbPath);
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public static UserManagert Instance
        {
            get
            {
                lock (padlock)
                {
                    if(instance == null)
                    {
                        instance = new UserManagert();
                    }
                    return instance;
                }
            }
        }
        
        public bool LogUserIn(string username, string password)
        {
            var contentsOfDb = dbConn.Query<Users>("SELECT Username, Password, Name, UnitNumber FROM users where Username = ?", username);
            foreach (var s in contentsOfDb)
            {
                if (s.Username == username)
                {
                    if (s.Password == password)
                    {
                        IsUserLoggedIn = true;
                        UserUsername = s.Username;
                        Name = s.Name;
                        UserunitNummer = s.UnitNumber;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public bool RegisterUser(string username, string password, string name ,string unitnumber)
        {
            Users user = new Users
            {
                Username = username,
                Password = password,
                Name = name,
                UnitNumber = unitnumber
            };

            //Checking if the user exists before adding it to the database to prevent double logins.
            if (DoesUserExist(username))
            {
                return false;
            }
            else
            {
                dbConn.Insert(user);
                return true;
            }
        }

        public bool DoesUserExist(string username)
        {
            if (dbConn.Query<Users>("SELECT * FROM users WHERE Username = '" + username + "'").Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool LogUserOut()
        {
            if(IsUserLoggedIn)
            {
                IsUserLoggedIn = false;
                UserUsername = "";
                Name = "";
                UserunitNummer = "";
                return true;
            }
            return false;
        }
    }
}
