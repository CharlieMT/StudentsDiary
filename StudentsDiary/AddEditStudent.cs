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
    public partial class AddEditStudent : Form
    {

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        private int _studentID;

        private Student _student;

        public AddEditStudent(int id = 0)
        {

            _studentID = id;

            InitializeComponent();

            GetStudentData();

            tbFirstName.Select();
        }

        private void GetStudentData()
        {
            if (_studentID != 0)
            {
                Text = "Edytowamnie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentID);

                if (_student == null)
                {
                    throw new Exception("Brak ucznia o podanym ID");
                }

                FillTextBoxes();
            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            richTextBox1.Text = _student.Comments;
            tbMath.Text = _student.Math;
            tbTechnology.Text = _student.Technology;
            tbPhysics.Text = _student.Physics;
            tbPolishLanguage.Text = _student.PolishLanguage;
            tbForeignLanguage.Text = _student.ForeignLanguage;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentID != 0)
                students.RemoveAll(x => x.Id == _studentID);
            else
                AssigneIdToNewStudent(students);

            AddNewStudentToList(students);

            _fileHelper.SerializeToFile(students);

            Close();
        }

        private void AddNewStudentToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentID,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = richTextBox1.Text,
                Math = tbMath.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhysics.Text,
                PolishLanguage = tbPolishLanguage.Text,
                ForeignLanguage = tbForeignLanguage.Text

            };

            students.Add(student);
        }

        private void AssigneIdToNewStudent(List<Student> students)
        {
            var studentWithHighestID = students.OrderByDescending(x => x.Id).FirstOrDefault();

            //var studentId = 0;

            //if(studentWithHighestID == null)
            //{
            //    studentId = 1;
            //}
            //else
            //{
            //    studentId = studentWithHighestID.Id + 1;
            //}

            _studentID = studentWithHighestID == null ? 1 : studentWithHighestID.Id + 1;
        }

        private void AddEditStudent_Load(object sender, EventArgs e)
        {

        }
    }
}
