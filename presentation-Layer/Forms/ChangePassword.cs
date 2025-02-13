using DVLD_BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Forms
{
    public partial class ChangePassword : Form
    {
        private int _UserID = -1;
        private clsUser _User = new clsUser();

        public ChangePassword(int UserID)
        {
            InitializeComponent();
           _UserID = UserID;
        }
        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {

            if (txtNewPassword.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "Don't leave Password blank");
            }
            else if (txtNewPassword.Text.Length < 8)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "please enter at Least 8 characters : [a-z or A-Z] at least 2, or [0-9] at least 4,or special Characters[&*$%#@!....] at least 2");
            }
            else if (Regex.IsMatch(txtNewPassword.Text, @"^(.*[a-zA-Z].*){2,}$") &&
                Regex.IsMatch(txtNewPassword.Text, @"^(.*[0-9].*){4,}$") && Regex.IsMatch(txtNewPassword.Text, @"^(.*[^a-zA-Z0-9\s].*){2,}$"))
            {
                errorProvider1.SetError(txtNewPassword, null);
            }
            else
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "please enter [a-z or A-Z ] at least 2, or [0-9] at least 4,or special Characters[&*$%#@!....] at least 2");
            }

        }

        private void txtConfirmNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmNewPassword.Text != txtNewPassword.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmNewPassword, "Confirm password doesn't match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmNewPassword, null);
            }
        }

     

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("Some fileds are not valide", EventLogEntryType.Error);

                return;

            }
            if (!_User.SavingNewPassword(txtNewPassword.Text))
            {
                MessageBox.Show("Changing  Password Faild,There is a system error we will solve try Again Later!", "Changing Password Failed",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("Changing  Password Faild", EventLogEntryType.Error);
                this.Close();
            }
            else
            {
                MessageBox.Show("Password Changed Successfully", "Changing Password success",
                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                //log the changepassword event success to the event log in your windows  
                clsValidations.LogTheError("Password Changed Successfully", EventLogEntryType.Information);
                if (LoginScreen.Result[0] == _User.UserName)
                    clsLogin.WriteToRegistery(_User.UserName,txtNewPassword.Text,LoginScreen.path);
            }
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Please Enter The Current Password");
            }
           else if (_User.Password != clsUser.ComputeHash(txtCurrentPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "The Current Password Is Not Correct");
            }
           else
           {
               errorProvider1.SetError(txtCurrentPassword,null);
           }
        }

        

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            _User = clsUser.FindByUserID(_UserID).Result;
            if (_User == null)
            {
                MessageBox.Show($"No User with ID:{_User.UserID}", "User Not Found", MessageBoxButtons.OK
                  , MessageBoxIcon.Error);
                clsValidations.LogTheError("User Not Found", EventLogEntryType.Error);
                this.Close();
                return;
            }
            ctrUserInfo2.LoadUserInfo(_UserID);
        }

        private void txtConfirmNewPassword_Enter(object sender, EventArgs e)
        {
            if (txtNewPassword.Text == "")
                txtNewPassword.Focus();
            else
                btnSave.Enabled = true;
        }
    }
}
