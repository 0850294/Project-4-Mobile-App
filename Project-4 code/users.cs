using System;
using SQLite;

namespace Project_4
{
    [Table("users")]
    class Users
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(10)]

        public string Username { get; set; }
        [MaxLength(20)]

        public string Password { get; set; }

        public string Name { get; set; }

        public string UnitNumber { get; set; }

       

    }
}