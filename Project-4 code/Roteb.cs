using System;
using SQLite;

namespace Project_4
{
    [Table("roteb")]
    class Roteb
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(10)]

        //Not going to make this difficult for meself, just throwing it into a string instead of date
        public string DateOfReservation { get; set; }
    }
}