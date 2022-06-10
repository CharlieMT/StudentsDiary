using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class dgvDiary : Form
    {
        //private string _filePath = $@"{Environment.CurrentDirectory}\students.txt";

        //private string _filePath = Path.Combine(Environment.CurrentDirectory, "students.txt");

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        public dgvDiary()
        {
            InitializeComponent();

            RefreshDiary();

            SetColumnsHeader();

        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();

            dgvDiary1.DataSource = students;
        }


        private void SetColumnsHeader()
        {
            dgvDiary1.Columns[0].HeaderText = "Numer";
            dgvDiary1.Columns[1].HeaderText = "Imie";
            dgvDiary1.Columns[2].HeaderText = "Nazwisko";
            dgvDiary1.Columns[3].HeaderText = "Uwagi";
            dgvDiary1.Columns[4].HeaderText = "Matematyka";
            dgvDiary1.Columns[5].HeaderText = "Technologia";
            dgvDiary1.Columns[6].HeaderText = "Fizyka";
            dgvDiary1.Columns[7].HeaderText = "Jezyk Polski";
            dgvDiary1.Columns[8].HeaderText = "Jezyk Obcy";
        }

        //public void SerializeToFile(List<Student> students)
        //{
        //    //var serializer = new XmlSerializer(typeof(List<Student>));
        //    //var streamWriter = new StreamWriter(_filePath);
        //    //serializer.Serialize(streamWriter,students);
        //    //streamWriter.Close();
        //    //streamWriter.Dispose();

        //    //var serializer = new XmlSerializer(typeof(List<Student>));
        //    //StreamWriter streamWriter = null;
        //    //try
        //    //{
        //    //    serializer = new XmlSerializer(typeof(List<Student>));
        //    //    streamWriter = new StreamWriter(_filePath);
        //    //    serializer.Serialize(streamWriter, students);
        //    //}
        //    //finally
        //    //{
        //    //    streamWriter.Close();
        //    //    streamWriter.Dispose();
        //    //}

        //    var serializer = new XmlSerializer(typeof(List<Student>));

        //    using (var streamWriter = new StreamWriter(_filePath))
        //    {
        //        serializer.Serialize(streamWriter, students);

        //        streamWriter.Close();

        //    }

        //}

        //public List<Student> DeserializeFromFile()
        //{
        //    if (!File.Exists(_filePath))
        //        return new List<Student>();

        //    var serializer = new XmlSerializer(typeof(List<Student>));

        //    using (var streamReader = new StreamReader(_filePath))
        //    {
        //        var students = (List<Student>) serializer.Deserialize(streamReader);

        //        streamReader.Close();
        //        return students;
        //    }
        //}

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.ShowDialog();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia ktorego dane chcesz edytowac");
                return;
            }

            var addEditStudent = new AddEditStudent(Convert.ToInt32(dgvDiary1.SelectedRows[0].Cells[0].Value));
            addEditStudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia ktorego dane chcesz usunac");
                return;
            }

            var selectedStudent = dgvDiary1.SelectedRows[0];

            var confirmDeleteStudent = MessageBox.Show($"Czy na pewno chcesz usunac ucznia {selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString().Trim()}", "Usuwanie ucznia,", MessageBoxButtons.OKCancel);

            if (confirmDeleteStudent == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));

                
            }
        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();

            students.RemoveAll(x => x.Id == id);

            _fileHelper.SerializeToFile(students);

            RefreshDiary();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }
    }
}
