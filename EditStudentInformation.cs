using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using StudentRegistWinformTejo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StudentRegistWinformTejo.Menu;
using static StudentRegistWinformTejo.StudentList;
using static StudentRegistWinformTejo.StudentRegistration;
using static StudentRegistWinformTejo.StudentInformation;

namespace StudentRegistWinformTejo
{
    public partial class EditStudentInformation : Form
    {
        public Student? selectedStudent;
        public string filePathJSON = Path.Combine(Application.StartupPath, "Students.json");
        public string selectedImagePath;

        public EditStudentInformation(Student? student, object value)
        {
            InitializeComponent();
            selectedStudent = student;

            LoadStudentData();
        }

        private void LoadStudentData()
        {
            string fullPath = Path.Combine(System.Windows.Forms.Application.StartupPath, selectedStudent.ImagePath!);

            txtFirstName.Text = selectedStudent.FirstName;
            txtLastName.Text = selectedStudent.LastName;
            txtMiddleInitial.Text = selectedStudent.MiddleInitial.ToUpper();
            cmbGender.Text = selectedStudent.Gender;
            dtpBirthdate.Value = selectedStudent.Birthdate.Value;
            txtAddress.Text = selectedStudent.Address;
            cmbCourse.Text = selectedStudent.Course;
            txtyear.Text = selectedStudent.Year.ToString();
            if (File.Exists(fullPath))
            {
                ofdProfilePic.SizeMode = PictureBoxSizeMode.Zoom;
                ofdProfilePic.Image = Image.FromFile(fullPath);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Menu regForm = new Menu();
            regForm.Show();
        }

        private void EditStudentInformation_Load(object sender, EventArgs e)
        {
            LoadData(filePathJSON);
            cmbGender.Items.AddRange(new string[]
            {
                "Male",
                "Female"
            });

            cmbCourse.Items.AddRange(new string[]
            {
                "Bachelor of Science of Information Technology",
                "Bachelor of Science of Mechanical Engineering"
            });

            dtpBirthdate.Format = DateTimePickerFormat.Custom;
            dtpBirthdate.CustomFormat = "yyyy-MM-dd";
        }

        private void LoadData(string filePathJSON)
        {
            var list = LoadJson();
            students = new BindingList<Models.Student>(list);
        }

        public void SaveJsonFile(string filePathJSON, List<Models.Student> students)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePathJSON, json);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog image = new OpenFileDialog();

            ofdProfilePic.SizeMode = PictureBoxSizeMode.Zoom;

            image.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png";

            if (image.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = image.FileName;
                using (var stream = new FileStream(selectedImagePath, FileMode.Open, FileAccess.Read))
                {
                    ofdProfilePic.Image = new Bitmap(stream);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text)
                    || string.IsNullOrWhiteSpace(txtMiddleInitial.Text) || string.IsNullOrWhiteSpace(txtAddress.Text))
                {
                    MessageBox.Show("Invalid Please fill all fields.");
                    return;
                }


                if (txtMiddleInitial.TextLength > 1 && !txtMiddleInitial.Text.All(char.IsLetter))
                {
                    MessageBox.Show("Please enter 1 character. and Write a Letter");
                    return;
                }

                int year;
                if (!int.TryParse(txtyear.Text, out year) && year < 5 && year > 0)
                {
                    MessageBox.Show("Invalid, Please enter year level.");
                }

                string folder = Path.Combine(System.Windows.Forms.Application.StartupPath, "Images");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Path.GetFileName(selectedImagePath);
                string destinationPath = Path.Combine(folder, fileName);

                File.Copy(selectedImagePath, destinationPath, true);

                var studentToUpdate = students.ToList().FirstOrDefault(s => s.FirstName == selectedStudent.FirstName);

                if (studentToUpdate != null)
                {
                    studentToUpdate.FirstName = txtFirstName.Text;
                    studentToUpdate.LastName = txtLastName.Text;
                    studentToUpdate.MiddleInitial = txtMiddleInitial.Text;
                    studentToUpdate.Gender = cmbGender.Text;
                    studentToUpdate.Birthdate = dtpBirthdate.Value.Date;
                    studentToUpdate.Address = txtAddress.Text;
                    studentToUpdate.Course = cmbCourse.Text;
                    studentToUpdate.Year = int.Parse(txtyear.Text);
                    studentToUpdate.ImagePath = "Images/" + fileName;
                }

                SaveJsonFile(filePathJSON, Menu.students.ToList());
                MessageBox.Show("Successfully edited");

                button1.Enabled = false;

                StudentInformation regForm = new StudentInformation(selectedStudent);
                regForm.Show();
                this.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
