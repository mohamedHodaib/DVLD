using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DVLD_BussinessLayer;
using DVLD.Properties;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;
using DVLD_DataAccessLayer;
using System.IO;
using System.Net.Mail;
using System.Diagnostics;
using DVLD.MyControls;
namespace DVLD.Forms
{
    public partial class frmAddorUpdatePerson : Form
    {
        public delegate bool DataBackEventHandler(clsPerson person);
        public event DataBackEventHandler DataBack;
        private clsPerson _Person = new clsPerson() ;
        enum enMode {AddNew = 1,Update = 2};
        enMode _Mode = enMode.AddNew;

        Stopwatch _Stopwatch = new Stopwatch();

        public frmAddorUpdatePerson(int PersonID)
        {
            _Stopwatch.Start();
            InitializeComponent();
            _Mode = enMode.Update;
            _LoadDataToPersonObject(PersonID);

        }


        public frmAddorUpdatePerson(clsPerson person)
        {
            InitializeComponent();
            _Person = person;
            _Mode = enMode.Update;
        }

        private async void _LoadDataToPersonObject(int personID)
        {
            _Person = await clsPerson.FindByPersonID(personID);
            if (_Person == null)
            {
                MessageBox.Show("This Person Is not exist!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("This Person Is not exist!", EventLogEntryType.Error);
                return;
            }
            _FillTheFormWithData(_Person);
        }
        private void _FillTheFormWithData(clsPerson Person)
        {
            lblTitle.Text = "Update Person";
            lblID.Text = Person.ID.ToString();
            txtFName.Text = Person.FName;
            txtSName.Text = Person.SName;
            txtTName.Text = Person.TName;
            txtLastName.Text = Person.LName;
            txtNationalNo.Text = Person.NationalNo;
            dtpDateOfBirth.Value = Person.DateOfBirth;
            txtPhone.Text = Person.Phone;
            txtEmail.Text = Person.Email;
            txtAddress.Text = Person.Address;
            cbxCountry.SelectedItem = clsCountry.FindCountryName(Person.CountryID);
            if(_Person.Gender == 0)rdMale.Checked = true;
            else rdFemale.Checked = true;
            if (_Person.ImagePath != "")
            {
                pbxImage.ImageLocation = _Person.ImagePath;
                llRemoveImage.Visible = true;
            }
        }
       
        public frmAddorUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
        private bool _CheckForSystemError()
        {
            if (clsGlobal.IsSystemError)
            {
                MessageBox.Show("There is a system error we will solve try Again Later!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsGlobal.IsSystemError = false ;
                return true;
            }
            return false;
        }

        private void ValidateName(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
            if (textBox.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "Don't leave " + textBox.Name + " blank");
            }
            if (!Regex.IsMatch(textBox.Text, @"^([a-zA-Z]){2,}$"))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "please enter at Least 2 alphabetical characters [a-z or A-Z] ");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox, "");
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if(txtEmail.Text == "")
            {
                errorProvider1.SetError(txtEmail,null);
                return;
            }
            try
            {
                // This will throw a FormatException if the email is not valid
                var mailAddress = new MailAddress(txtEmail.Text);
                errorProvider1.SetError(txtEmail,null);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, 
                    ex.Message);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (txtNationalNo.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "Don't leave " + txtNationalNo.Name + " blank");
            }
            else if (!Regex.IsMatch(txtNationalNo.Text, @"^([0-9]){14,}$"))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "Invalid National Number!");
            }
            else if (clsPerson.IsNationalNumberExist(txtNationalNo.Text))
            {
                e.Cancel= true;
                errorProvider1.SetError(txtNationalNo, "The national number is already exist enter another one");
            }
            else
            {
                if(_CheckForSystemError())
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtNationalNo, "there is a system error try again later !");
                    return;
                }
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        private void FillCountriesInCbx()
        {
            DataTable dt = clsCountry.ListCountries();
            if(clsGlobal.IsSystemError)
            {
                MessageBox.Show("There is a system error we will solve try Again Later!", "Error",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsGlobal.IsSystemError = false;
                return;
            }
            foreach (DataRow dr in dt.Rows)
            {
                cbxCountry.Items.Add(dr["CountryName"].ToString());
            }
        }
        private void frmAddorUpdatePerson_Load(object sender, EventArgs e)
        {
            _Stopwatch.Stop();

            MessageBox.Show(_Stopwatch.ElapsedMilliseconds.ToString(), "success",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
            FillCountriesInCbx();
            if(_Mode == enMode.AddNew)
            {
                this.Text = "Add New Person";
            rdMale.Checked = true;
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;
            cbxCountry.SelectedItem = "Egypt";
            }
            else
            {
                this.Text = "Update Person";
                
            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (txtPhone.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Don't leave " + txtPhone.Name + " blank");
            }
            else if (!Regex.IsMatch(txtPhone.Text, @"^([0-9]){11,11}$")|| (!Regex.IsMatch(txtPhone.Text, @"^010([0-9]){8,8}$")&& 
                !Regex.IsMatch(txtPhone.Text, @"^011([0-9]){8,8}$")&&
                !Regex.IsMatch(txtPhone.Text, @"^012([0-9]){8,8}$") 
                && !Regex.IsMatch(txtPhone.Text, @"^015([0-9]){8,8}$")))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Invalid Phone Number,Phone Number start with 010 and contain 14 digit [0-9!");
            }
            else if (clsPerson.IsPhoneNumberExist(txtPhone.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "The Phone number is already exist enter another one");
            }
            else
            {
                if (_CheckForSystemError())
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtPhone, "there is a system error try again later !");
                    return;
                }
                errorProvider1.SetError(txtPhone, null);
            }
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.InitialDirectory = "C:\\Users\\DELL\\Pictures";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string SelectedPath = openFileDialog1.FileName;
                pbxImage.Load(SelectedPath);
                llRemoveImage.Visible = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbxImage.ImageLocation = null;
            if (rdFemale.Checked) pbxImage.Image = Resources.Female_512;
            else pbxImage.Image = Resources.Male_512;
            llRemoveImage.Visible = false;
        }

        private bool _HandlePersonImage()
        {
            if(_Person.ImagePath != pbxImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch(Exception ex) 
                    {
                        MessageBox.Show("Delete old Image Path Failed", "Save Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                        clsValidations.LogTheError(ex.Message, EventLogEntryType.Error);

                        return false;
                    }
                }
                if(pbxImage.ImageLocation != null)
                {
                    string SourceFile = pbxImage.ImageLocation.ToString();  
                    if(clsUtil.CopyImageToProjectImagesFolder(ref SourceFile))
                    {
                        pbxImage.ImageLocation = SourceFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Copy Image To Images Folder Failed", "Save Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clsValidations.LogTheError("Copy Image To Images Folder Failed", EventLogEntryType.Error);

                        return false;
                    }
                }
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
                if(!_HandlePersonImage())
                    return ;
                _Person.CountryID = clsCountry.FindCountryID(cbxCountry.Text);
                if(_Person.CountryID == -1)
                {
                    MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ;
                }
                 if (pbxImage.ImageLocation != null)
                     _Person.ImagePath = pbxImage.ImageLocation;
                 else
                     _Person.ImagePath = "";
                _Person.FName = txtFName.Text;
                _Person.SName = txtSName.Text;
                _Person.TName = txtTName.Text;
                _Person.LName = txtLastName.Text;
                _Person.NationalNo = txtNationalNo.Text;
                _Person.Email = txtEmail.Text;
                _Person.Phone = txtPhone.Text;
                _Person.Address = txtAddress.Text;
                if (rdMale.Checked) _Person.Gender = 0;
                else _Person.Gender = 1;
                _Person.DateOfBirth = dtpDateOfBirth.Value;
                if (_Person.Save())
                {
                lblTitle.Text = "Update Person";
                lblID.Text = _Person.ID.ToString();
                this.Text = "Update Person";
                _Mode = enMode.Update;
                MessageBox.Show("Saved Successfully", "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DataBack?.Invoke(_Person);
                clsValidations.LogTheError("Saved Successfully", EventLogEntryType.Error);


                }
            else
            {
                    MessageBox.Show("Saving Failed,something go wrong!", "Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("Saving Failed", EventLogEntryType.Error);
            }


        }

        private void rdMale_Click(object sender, EventArgs e)
        {
            if (pbxImage.ImageLocation == null)
                pbxImage.Image = Resources.Male_512;
        }

        private void rdFemale_Click(object sender, EventArgs e)
        {
            if (pbxImage.ImageLocation == null)
                pbxImage.Image = Resources.Female_512;
        }
    }
}
