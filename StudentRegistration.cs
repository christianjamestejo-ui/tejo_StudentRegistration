using Microsoft.VisualBasic;
using Newtonsoft.Json;
using StudentRegistWinformTejo.Models;
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
using static StudentRegistWinformTejo.Menu;
using static StudentRegistWinformTejo.StudentList;
using static System.Net.Mime.MediaTypeNames;

namespace StudentRegistWinformTejo
{
    public partial class StudentRegistration : Form
    {

        public string selectedImagePath;

        public StudentRegistration()
        {
            InitializeComponent();
        }

        public void StudentRegistration_Load(object sender, EventArgs e)
        {
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
            StudentList.LoadJson();

            dtpBirthdate.Format = DateTimePickerFormat.Custom;
            dtpBirthdate.CustomFormat = "yyyy-MM-dd";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StudentRegistration regForm = new StudentRegistration();
            regForm.Show();
        }

        private void SavejSON()
        {
            string json = JsonConvert.SerializeObject(Menu.students.ToList(), Formatting.Indented);
            File.WriteAllText("Students.json", json);
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
                if (!int.TryParse(txtYear.Text, out year) && year <= 4 && year >= 1)
                {
                        MessageBox.Show("Invalid, Please enter year level.");
                        return;
                }

                string folder = Path.Combine(System.Windows.Forms.Application.StartupPath, "Images");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Path.GetFileName(selectedImagePath);
                string destinationPath = Path.Combine(folder, fileName);

                File.Copy(selectedImagePath, destinationPath, true);

                Menu.students.Add(new Student
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    MiddleInitial = txtMiddleInitial.Text.ToUpper(),
                    Gender = cmbGender.SelectedItem.ToString(),
                    Birthdate = dtpBirthdate.Value.Date,
                    Address = txtAddress.Text,
                    Course = cmbCourse.SelectedItem.ToString(),
                    Year = year,
                    ImagePath = "Images/" + fileName
                });

                SavejSON();

                MessageBox.Show("Student saved");

                button1.Enabled = false;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Menu regForm = new Menu();
            regForm.Show();
            this.Close();
        }
    }
}
