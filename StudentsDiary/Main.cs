using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class dgvDiary : Form
    {

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        private AdditionalResourcesHelper additionalResourcesHelper = new AdditionalResourcesHelper();

        private List<string> studentsGroups;

        public dgvDiary()
        {
            InitializeComponent();

            SetStudentGroupComboBox();

            RefreshDiary();

            SetColumnsHeader();

        }

        private void SetStudentGroupComboBox()
        {
            studentsGroups = new List<string>(additionalResourcesHelper.StudentGroupList);
            studentsGroups.Insert(0,"Wszystkie");
            cbxStudentGroupFilter.DataSource = studentsGroups;
        }

        private void RefreshDiary()
        {

            var choosenGroup = cbxStudentGroupFilter.SelectedItem.ToString();

            var students = _fileHelper.DeserializeFromFile();

            students = students.OrderBy(x => x.Id).OrderBy(x => x.GroupNumber).ToList();

            StudentIDCleaner(students);

            if (choosenGroup == "Wszystkie")
            {
                dgvDiary1.DataSource = students;
            }
            else
            {
                var filetedStudentList = students.Where(x => x.GroupNumber == choosenGroup).Select(x => x).ToList();
                dgvDiary1.DataSource = filetedStudentList;
            }

        }

        private void StudentIDCleaner(List<Student> students )
        {
            var counter = 0;
            while(counter < studentsGroups.Count)
            {
                if (studentsGroups[counter] == "Wszystkie")
                {
                    counter++;
                    continue;
                }

                var lowestNumber = 1;

                foreach (var item in students.Where(x => x.GroupNumber == studentsGroups[counter]))
                {
                    if (item.Id != lowestNumber)
                        item.Id = lowestNumber;

                    lowestNumber++;
                }

                counter++;

            }
            _fileHelper.SerializeToFile(students);
        }


        private void SetColumnsHeader()
        {
            dgvDiary1.Columns[0].HeaderText = "Numer Grupy";
            dgvDiary1.Columns[1].HeaderText = "Numer";
            dgvDiary1.Columns[2].HeaderText = "Imie";
            dgvDiary1.Columns[3].HeaderText = "Nazwisko";
            dgvDiary1.Columns[4].HeaderText = "Uwagi";
            dgvDiary1.Columns[5].HeaderText = "Matematyka";
            dgvDiary1.Columns[6].HeaderText = "Technologia";
            dgvDiary1.Columns[7].HeaderText = "Fizyka";
            dgvDiary1.Columns[8].HeaderText = "Jezyk Polski";
            dgvDiary1.Columns[9].HeaderText = "Jezyk Obcy";
            dgvDiary1.Columns[10].HeaderText = "Zajecia Dodatkowe";
            dgvDiary1.Columns[10].ReadOnly = true;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
            addEditStudent.FormClosing -= AddEditStudent_FormClosing;
        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia ktorego dane chcesz edytowac");
                return;
            }

            var addEditStudent = new AddEditStudent(Convert.ToInt32(dgvDiary1.SelectedRows[0].Cells[1].Value));
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
            addEditStudent.FormClosing -= AddEditStudent_FormClosing;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia ktorego dane chcesz usunac");
                return;
            }

            var selectedStudent = dgvDiary1.SelectedRows[0];

            var confirmDeleteStudent = MessageBox.Show($"Czy na pewno chcesz usunac ucznia {selectedStudent.Cells[2].Value.ToString() + " " + selectedStudent.Cells[3].Value.ToString().Trim()}", "Usuwanie ucznia,", MessageBoxButtons.OKCancel);

            if (confirmDeleteStudent == DialogResult.OK)
            {
                DeleteStudent((selectedStudent.Cells[0].Value).ToString(), Convert.ToInt32(selectedStudent.Cells[1].Value));
            }
        }

        private void DeleteStudent(string groupNumber, int id)
        {
            var students = _fileHelper.DeserializeFromFile();

            StudentIDCleaner(students);

            students.RemoveAll(x => x.Id == id && x.GroupNumber == groupNumber);

            _fileHelper.SerializeToFile(students);

            RefreshDiary();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

    }
}
