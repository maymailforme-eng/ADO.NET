using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Academy.Models
{
    internal class Teacher: Human
    {
        internal string work_since;
        internal decimal rate;


        //конструктор класса
        public Teacher(int id, string first_name, string last_name, string middle_name, string birth_date,
            string email, string phone, Image photo, string work_since, decimal rate) :
            base(id, last_name, first_name, middle_name, birth_date, email, phone, photo)
        { 
            this.work_since = work_since;
            this.rate = rate;            
        }


        public Teacher(Human other, string work_since, decimal rate) : base(other)
        {
            this.work_since = work_since;
            this.rate = rate;
        }



        public override string GetNames()
        {
            return base.GetNames() + ",work_sinec,rate";
        }

        public override string GetValues()
        {
            return base.GetValues() + $",N'{work_since}',{rate}";
        }


        public override string GetUpdateString()
        {
            return base.GetUpdateString() + $",work_since=N'{work_since}',{rate}";
        }
        
    }
}
