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
    public partial class HumanForm : Form
    {
        
        internal Models.Human human;

        public HumanForm()
        {
            InitializeComponent();
        }

        protected virtual void Compress()
        {
            human = new Models.Human
                (
                    Convert.ToInt32(labelID.Text == "" ? "0" : labelID.Text.Split(':').Last()),
                    textBoxLastName.Text,
                    textBoxFirstName.Text,
                    textBoxMidleName.Text,
                    dtpBirthDate.Value.ToString("yyyy-MM-dd"),
                    textBoxEmail.Text,
                    textBoxPhone.Text,
                    pictureBoxPhoto.Image
                );
        }




        protected virtual void Extract()
        {
            labelID.Text = $"ID:{human.id}";
            textBoxLastName.Text = human.last_name;
            textBoxFirstName.Text = human.first_name;
            textBoxMidleName.Text = human.middle_name;
            dtpBirthDate.Value = Convert.ToDateTime(human.birth_date);
            textBoxEmail.Text = human.email;
            textBoxPhone.Text = human.phone;
        }


        //обработчик клика по ОК
        protected virtual void buttonОК_Click(object sender, EventArgs e)
        {
            Compress();
        }



        private void HumanForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }


    }
}
