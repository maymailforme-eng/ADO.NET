using Academy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Academy
{
    public partial class StudentForm : HumanForm
    {

        internal Models.Student student;





        public StudentForm()
        {
            InitializeComponent();

            cbStudentsGroup.DataSource = DataBase.Connector.Load("SELECT * FROM Groups");
            cbStudentsGroup.DisplayMember = "group_name";
            cbStudentsGroup.ValueMember = "group_id";
        }


        public StudentForm(int id,
            string lastName, string firstName, string middleName,
            string birth_date, string groupName) :
            base(id, lastName, firstName, middleName, birth_date)
        {

            InitializeComponent();

            cbStudentsGroup.DataSource = DataBase.Connector.Load("SELECT * FROM Groups");
            cbStudentsGroup.DisplayMember = "group_name";
            cbStudentsGroup.ValueMember = "group_id";

            //по названию ищем индекс и выбираем из комбо бокса по индексу
            int index = cbStudentsGroup.FindStringExact(groupName);
            if (index != -1)
            {
                cbStudentsGroup.SelectedIndex = index;
            }
        }





        protected override void buttonОК_Click(object sender, EventArgs e)
        {


          
                    base.buttonОК_Click(sender, e);
                    student = new Models.Student(human, (int)cbStudentsGroup.SelectedValue);
            if (student.id == 0) student.id =
                    Convert.ToInt32(DataBase.Connector.Scalar
                        (
                        $"INSERT Students({student.GetNames()}) VALUES({student.GetValues()});SELECT SCOPE_IDENTITY();"
                        ));

            else
            {
                // ОБНОВЛЕНИЕ
                DataBase.Connector.Update(
                    $"UPDATE Students SET {student.GetUpdateString()} WHERE stud_id = {student.id}"
                );

            }






            //доступ к методу родительского окна
            MainForm mainForm = this.Owner as MainForm;
            if (mainForm != null)
            {
                mainForm.UpdateVisualCuerrentTab();
            }
        }






    }
}
