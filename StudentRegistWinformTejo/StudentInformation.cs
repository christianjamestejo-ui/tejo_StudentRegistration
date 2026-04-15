using Microsoft.VisualBasic.Devices;
using StudentRegistWinformTejo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using static StudentRegistWinformTejo.Menu;
using static StudentRegistWinformTejo.StudentList;
using static StudentRegistWinformTejo.StudentRegistration;
using StudentRegistWinformTejo.Models;
using Newtonsoft.Json;

namespace StudentRegistWinformTejo
{
    public partial class StudentInformation : Form
    {
        public Student? selectedStudent;

        public StudentInformation(Student? student)
        {
            InitializeComponent();
            selectedStudent = student;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Menu regForm = new Menu();
            regForm.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EditStudentInformation regForm = new EditStudentInformation(selectedStudent!, null!);
            regForm.Show();
            this.Hide();
        }

        private void StudentInformation_Load(object sender, EventArgs e)
        {
            string fullPath = Path.Combine(System.Windows.Forms.Application.StartupPath, selectedStudent.ImagePath!);

            txtName.Text = $"{selectedStudent!.LastName}, {selectedStudent!.FirstName}";
            dtpBirthdate.Value = selectedStudent.Birthdate!.Value;
            cmbGender.Text = $"{selectedStudent.Gender!}";
            txtAddress.Text = selectedStudent.Address;
            cmbCourse.Text = $"{selectedStudent.Course!}";
            txtyear.Text = selectedStudent.Year!.ToString();
            if (File.Exists(fullPath))
            {
                ofdProfilePic.Image = Image.FromFile(fullPath);
            }
        }

        private void ofdImageBase64_Click(object sender, EventArgs e)
        {

        }
    }

    public static class JsonHelper
    {
        private static string filePath = "Students.json";

        public static List<Student> LoadStudents()
        {
            if (!File.Exists(filePath))
                return new List<Student>();

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Student>>(json)!;
        }

    }
}
