using DVLD.MyControls;
using DVLD_BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Forms
{
    public partial class AddOrUpdateUser : Form
    {

        enum enMode { AddNew = 1, Update = 2 };
        enMode _Mode {  get; set; }
        private clsUser _User = new clsUser();
        public AddOrUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public AddOrUpdateUser(int UserID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _LoadDataToUserObject(UserID);
           
        }


        private async  void _LoadDataToUserObject(int UserID)
        {
            _User =  await clsUser.FindByUserID(UserID);
            if (_User?.PersonInfo != null && _User != null)
                _LoadData();
            else
            {
                MessageBox.Show("There is a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void _LoadData()
        {
            personCardWithFilter1.FillThePersonCardWithFilter(_User.PersonInfo);
            lblTitle.Text = "Update User";
            personCardWithFilter1.StopTheFilteration();
            btnSave.Enabled = true;
            lblUserID.Text = Convert.ToString(_User.UserID);
            txtUserName.Text = _User.UserName;
            txtPassword.Text = txtConfirmPassword.Text = _User.Password;
            if (_User.IsActive == 1) chxIsActive.Checked = true;
            else chxIsActive.Checked = false;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tabLoginInfo.Enabled = true;
                tabInformation.SelectedTab = tabInformation.TabPages["tabLoginInfo"];
                return;
            }
            if(ctrPersonCard._Person.ID != -1 )
            {
                if(clsUser.IsPersonLinkedToUser(ctrPersonCard._Person.ID))
                {
                    MessageBox.Show("Person Is Already Linked to User", "Error"
                       , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("Person Is Already Linked to User", EventLogEntryType.Error);
                    personCardWithFilter1.FilterFocus();
                    return;
                }
                else
                {
                    btnSave.Enabled = true;
                    tabLoginInfo.Enabled = true;
                    tabInformation.SelectedTab = tabInformation.TabPages["tabLoginInfo"];
                }
            }

            else
            {
                MessageBox.Show("Please Select A Person", "Error"
                      , MessageBoxButtons.OK, MessageBoxIcon.Error);
                personCardWithFilter1.FilterFocus();
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (txtUserName.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Don't leave UserName blank");
            }
            else if(!Regex.IsMatch(txtUserName.Text, @"^(.*[a-zA-Z].*){2,}$"))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "please enter at Least 2 alphabetical characters [a-z or A-Z] ");
            }
          else
            {
                if(_Mode == enMode.AddNew)
                {
                    if(clsUser.IsUserNameUsed(txtUserName.Text))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "The UserName is Used enter another one!");
                    }
                }
                else
                {
                    if(txtUserName.Text != _User.UserName)
                    {
                        if(clsUser.IsUserNameUsed(txtUserName.Text))
                        {
                            e.Cancel = true;
                            errorProvider1.SetError(txtUserName, "The UserName is Used enter another one!");
                        }
                    }
                }
                errorProvider1.SetError(txtUserName, null);
            }
        }
        

        private void txtConfirmPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                errorProvider1.SetError(txtPassword, "please enter The password!");
                txtPassword.Focus();
            }
            else
                btnSave.Enabled = true;
        }

        public  void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtPassword.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Don't leave Password blank");
            }
            else if (txtPassword.Text.Length < 8)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "please enter at Least 8 characters : [a-z or A-Z] at least 2, or [0-9] at least 4,or special Characters[&*$%#@!....] at least 2");
            }
            else if (!Regex.IsMatch(txtPassword.Text, @"^(.*[a-zA-Z].*){2,}$") ||
                !Regex.IsMatch(txtPassword.Text, @"\d{4,}") || !Regex.IsMatch(txtPassword.Text, @"^(.*[^a-zA-Z0-9\s].*){2,}$"))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "please enter [a-z or A-Z ] at least 2, or [0-9] at least 4,or special Characters[&*$%#@!....] at least 2");
            }
            else
                errorProvider1.SetError(txtPassword, null);
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtConfirmPassword.Text != txtPassword.Text)
            {
                e.Cancel= true;
                errorProvider1.SetError(txtConfirmPassword, "Confirm password doesn't match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            _User.PersonID = ctrPersonCard._Person.ID;
            _User.UserName = txtUserName.Text;
            _User.Password = txtPassword.Text;
            _User.IsActive = Convert.ToByte(chxIsActive.Checked);
            if (_User.Save())
            {
            lblTitle.Text = "Update User";
            lblUserID.Text = _User.UserID.ToString();
                this.Text = "UpdateUser";
                _Mode = enMode.Update;
                MessageBox.Show("Saved Successfully", "success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                personCardWithFilter1.StopTheFilteration();
            }
            else
            {
                MessageBox.Show("Saving Failed,something go wrong,may be a " +
                    "system error try again later!", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("Saving Failed", EventLogEntryType.Error);

            }

        }

        private void AddOrUpdateUser_Activated(object sender, EventArgs e)
        {
            personCardWithFilter1.FilterFocus();
        }

        private void AddOrUpdateUser_Load(object sender, EventArgs e)
        {
        

            if (_Mode == enMode.AddNew)
            {
                tabLoginInfo.Enabled = false;
                this.Text = "Add New User";
            }
            else
            {
                tabLoginInfo.Enabled = true;
                btnSave.Enabled = true;
                this.Text = "Update User";
            }
        }
    }
}
