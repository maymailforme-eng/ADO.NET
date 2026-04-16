using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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


        public HumanForm
            (int id, string lastName, string firstName, string middleName, 
            string birth_date)
        {
            InitializeComponent();

            labelID.Text = $"ID:{id}";

            //заполнение боксов
            textBoxLastName.Text = lastName;
            textBoxFirstName.Text = firstName;  
            textBoxMidleName.Text = middleName;
            textBoxEmail.Text = "email";
            textBoxPhone.Text = "phone";

            //преобразование строки в DateTime
            DateTime dt;
            if (DateTime.TryParse(birth_date, out dt))
            {
                dtpBirthDate.Value = dt;
            }
            else
            {
                MessageBox.Show("Неверный формат даты");
            }


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

        //клик по кнопке добавить 
        private void buttonBrowse_MouseClick(object sender, MouseEventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog()) //дополнительное высвобождение
            {
                dialog.Title = "Выберите фото";
                dialog.Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png";


                dialog.Multiselect = false; // можно выбрать только один файл

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    //переносим в кодировку байтами 
                    byte[] bytes = File.ReadAllBytes(dialog.FileName);

                    //выделяем поток обертку над байтами (не заблокирует изображение на компьютере)
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        Image img = Image.FromStream(ms);
                        pictureBoxPhoto.Image = img;
                    }
                }
            }
        }





    }
}
