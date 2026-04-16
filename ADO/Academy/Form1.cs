using DBtools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Academy
{
    public partial class MainForm : Form
    {
        Connector connector;
        DataGridView[] tables;
        Query[] queries =
            {
                new Query
                (
                    "stud_id,last_name,first_name,middle_name,birth_date,group_name,direction_name",
                    "Students,Groups,Directions",
                    "[group]=group_id AND direction=direction_id"
                ),
                new Query(
                    "group_id,group_name,start_date,start_time,learning_days,direction_name, " +
                    "DATENAME(WEEKDAY, start_date) AS weekday_name",
                    "Groups,Directions",
                    "direction=direction_id"
                ),
                new Query("*", "Directions"),
                new Query("*", "Disciplines"),
                new Query("*", "Teachers")
            };
        public MainForm()
        {
            InitializeComponent();

            //список вкладок (таблицы)
            tables = new DataGridView[] { dgvStudents, dgvGroups, dgvDirections, dgvDisciplines, dgvTeachers };

            connector = new Connector(ConfigurationManager.ConnectionStrings["PV_522_Import"].ConnectionString);
            //dgvDirections.DataSource = connector.Select("SELECT * FROM Directions");
            tabControl_SelectedIndexChanged(tabControl, null);

            cbGroupsDirection.DataSource = connector.Load("SELECT * FROM Directions");
            cbGroupsDirection.DisplayMember = "direction_name";
            cbGroupsDirection.ValueMember = "direction_id";


            //заполнение комбобокса студенотов по ГРУППАМ
            cbStudents.DataSource = connector.Load("SELECT * FROM Groups");
            cbStudents.DisplayMember = "group_name";
            cbStudents.ValueMember = "group_id";


            //заполнение комбобокса студенотов по НАПРАВЛЕНИЯ
            cbStudentDirection.DataSource = connector.Load("SELECT * FROM Directions");
            cbStudentDirection.DisplayMember = "direction_name";
            cbStudentDirection.ValueMember = "direction_id";

        }

        //событие смена вкладки
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = (sender as TabControl).SelectedIndex;   //Получаем номер выбранной вкладки
            tables[i].DataSource = connector.Load(queries[i].ToString());
            //tables[i].DataSource = connector.Select("*", tabControl.SelectedTab.Text);
            toolStripStatusLabel.Text = $"Количество записей: {tables[i].RowCount - 1}";
        }

        //обновление визуала текущей вкладки tabControl после внесения изменений
        public void UpdateVisualCuerrentTab()
        {
            tabControl_SelectedIndexChanged(tabControl, EventArgs.Empty);
        }

        //событие смены фильтра во вкладке
        private void cbGroupsDirection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dgvGroups.DataSource = connector.Load
                (
                queries[1].ToString() + $" AND direction={cbGroupsDirection.SelectedValue}"
                );
            toolStripStatusLabel.Text = $"Количество записей: {dgvGroups.RowCount - 1}";
        }


        //смена фильтра студентов - группы
        private void cbStudents_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dgvStudents.DataSource = connector.Load
            (
            queries[0].ToString() + $" AND group_id={cbStudents.SelectedValue}"
            );
            toolStripStatusLabel.Text = $"Количество записей: {dgvStudents.RowCount - 1}";
        }

        //смена фильтра студентов - навправления
        private void cbStudentDirection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dgvStudents.DataSource = connector.Load
            (
            queries[1].ToString() + $" AND direction={cbStudentDirection.SelectedValue}  AND group_id={cbStudents.SelectedValue}"
            );
            toolStripStatusLabel.Text = $"Количество записей: {dgvStudents.RowCount - 1}";
        }

        //кнопка добавить
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            StudentForm student = new StudentForm();
            //назначаем родителя
            student.Owner = this;
            student.ShowDialog();
        }

        ////обработчик клика по ячейе в таблице студентов
        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvStudents.ClearSelection();
                dgvStudents.Rows[e.RowIndex].Selected = true;
            }
        }

        //двойной клик по ячейке таблицы студента
        private void dgvStudents_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            DataGridViewRow row = dgvStudents.Rows[indexRow];

            StudentForm student = new StudentForm
                (
                    Convert.ToInt32(row.Cells["stud_id"].Value),
                    row.Cells["last_name"].Value.ToString(),
                    row.Cells["first_name"].Value.ToString(),
                    row.Cells["middle_name"].Value.ToString(),
                    row.Cells["birth_date"].Value.ToString(),
                    row.Cells["group_name"].Value.ToString()



                );
            //назначаем родителя
            student.Owner = this;
            student.ShowDialog();
        }


        //private void dgvGroups_CellClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}
    }
}