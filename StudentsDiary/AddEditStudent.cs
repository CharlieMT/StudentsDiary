using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        private int _studentID;

        private Student _student;

        private AdditionalResourcesHelper additionalResourcesHelper = new AdditionalResourcesHelper();

        private List<string> studentChoosenExercises;

        public AddEditStudent(int id = 0)
        {
            _studentID = id;
            var groupsList = additionalResourcesHelper.StudentGroupList;
            InitializeComponent();

            cbGroupNumber.DataSource = groupsList;

            GetStudentData();

            tbFirstName.Select();
        }


        private void SetAvailableAdditionalExercisesList()
        {
            var availabelExercisesList = new List<string>(additionalResourcesHelper.FullListOfAdditionalExercises);

            foreach (var item in studentChoosenExercises)
            {
                if (availabelExercisesList.Contains(item))
                    availabelExercisesList.Remove(item);
            }

            lbxAvailableExerciseList.DataSource = null;
            lbxAvailableExerciseList.DataSource = availabelExercisesList;

            lbxChoosenExercises.DataSource = null;
            lbxChoosenExercises.DataSource = studentChoosenExercises;

            if (checkIflbxChoosenExerciseIsNotNull())
                lbxChoosenExercises.SetSelected(0, true);
        }



        private void GetStudentData()
        {
            if (_studentID != 0)
            {
                Text = "Edytowamnie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentID);
                studentChoosenExercises = _student.AdditionalExercisesList;
                SetAvailabilityForAdditionalExercisesFields(_student.AdditionalExercises);
                if (_student == null)
                {
                    throw new Exception("Brak ucznia o podanym ID");
                }

                FillTextBoxes();

            }

            studentChoosenExercises = new List<string>();
            SetAvailabilityForAdditionalExercisesFields(false);

            SetAvailableAdditionalExercisesList();
        }

        private void FillTextBoxes()
        {
            cbGroupNumber.Text = _student.GroupNumber;
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            richTextBox1.Text = _student.Comments;
            tbMath.Text = _student.Math;
            tbTechnology.Text = _student.Technology;
            tbPhysics.Text = _student.Physics;
            tbPolishLanguage.Text = _student.PolishLanguage;
            tbForeignLanguage.Text = _student.ForeignLanguage;
            ckbAdditionalExercises.Checked = _student.AdditionalExercises;
            lbxChoosenExercises.DataSource = _student.AdditionalExercisesList;
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
                GroupNumber = cbGroupNumber.Text,
                Id = _studentID,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = richTextBox1.Text,
                Math = tbMath.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhysics.Text,
                PolishLanguage = tbPolishLanguage.Text,
                ForeignLanguage = tbForeignLanguage.Text,
                AdditionalExercises = ckbAdditionalExercises.Checked,
                AdditionalExercisesList = studentChoosenExercises

            };

            students.Add(student);
        }

        private void AssigneIdToNewStudent(List<Student> students)
        {
            var studentWithHighestID = students.Where(x => x.GroupNumber == cbGroupNumber.Text).OrderByDescending(x => x.Id).FirstOrDefault();

            _studentID = studentWithHighestID == null ? 1 : studentWithHighestID.Id + 1;
        }

        private void btnAddExercise_Click(object sender, EventArgs e)
        {
            studentChoosenExercises.Add(lbxAvailableExerciseList.SelectedItem.ToString());

            SetAvailableAdditionalExercisesList();
        }

        private void btnDeleteExercise_Click(object sender, EventArgs e)
        {
            if (checkIflbxChoosenExerciseIsNotNull() && studentChoosenExercises.Contains(lbxChoosenExercises.SelectedItem.ToString()))
                studentChoosenExercises.Remove(lbxChoosenExercises.SelectedItem.ToString());

            SetAvailableAdditionalExercisesList();
        }

        private bool checkIflbxChoosenExerciseIsNotNull()
        {
            if (lbxChoosenExercises.Items.Count >= 1)
                return true;

            return false;
        }

        private void ckbAdditionalExercises_Click(object sender, EventArgs e)
        {
            SetAvailabilityForAdditionalExercisesFields(ckbAdditionalExercises.Checked);
        }

        private void SetAvailabilityForAdditionalExercisesFields(bool status)
        {
            if (status)
            {
                lbxAvailableExerciseList.Enabled = true;
                lbxChoosenExercises.Enabled = true;
                btnAddExercise.Enabled = true;
                btnDeleteExercise.Enabled = true;
            }
            else
            {
                lbxAvailableExerciseList.Enabled = false;
                lbxChoosenExercises.Enabled = false;
                btnAddExercise.Enabled = false;
                btnDeleteExercise.Enabled = false;
            }
        }
    }
}
