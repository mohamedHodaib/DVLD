using DVLD.MyControls;
using DVLD_BussinessLayer;
using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_BussinessLayer.clsGlobal;

namespace DVLD.Forms
{
    public partial class NewOrUpdateLocalDrivingLicense : Form
    {
       clsLocalDrivingLicenseApplication _LDLApp =new clsLocalDrivingLicenseApplication();
        clsApplication _App = new clsApplication();
        enum enMode { AddNew = 0, Update = 1 }
        enMode _Mode = enMode.AddNew;
        int _LDLAppID = 0;
        public NewOrUpdateLocalDrivingLicense()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public NewOrUpdateLocalDrivingLicense(int LDLAppID)
        {
            InitializeComponent();
            _LDLAppID = LDLAppID;
            _LDLApp = clsLocalDrivingLicenseApplication.FindLDLApplicationByID(LDLAppID);
            _App = clsApplication.FindApplicationByID(_LDLApp.ApplicationID);
            _Mode = enMode.Update;
        }

        
        private void _FillTheFormWithData()
        {
            personCardWithFilter1.StopTheFilteration();
            if(_LDLApp != null )
            {
            lblApplicationID.Text = _LDLApp.LocalDrivingLicenseApplicationID.ToString();
            cbxClasses.SelectedIndex = cbxClasses.FindString(clsDrivingLicenseClass.FindClassNameByClassID(_LDLApp.LicenseClassID));
            }
            else
            {
                MessageBox.Show($"Cannot Find The Local Driving License Application with ID = {_LDLAppID} ", "Error"
             , MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError($"Cannot Find The Local Driving License Application with ID = {_LDLAppID} ", EventLogEntryType.Error);
            }
            if(_App != null )
            {

            personCardWithFilter1.FillThePersonCardWithFilter(clsPerson.FindByPersonID(_App.ApplicantPersonID).Result);
            lblApplicationDate.Text = _App.ApplicationDate.ToString();
            lblUserName.Text = clsUser.FindByUserID(_App.UserID).Result.UserName;
                lblPaidFees.Text = _App.PaidFees.ToString();
            }
            else
            {
                MessageBox.Show($"Cannot Find The Base Application for Local Application with ID = {_LDLAppID} ", "Error"
             , MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError($"Cannot Find The Base Application for Local Application with ID = {_LDLAppID} ", EventLogEntryType.Error);
            }
        }
        private void NewLocalDrivingLicense_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsGlobal.MakeIsPersonFillThePersonCardFalse();
        }

        private void _FillTheClassesComboBox()
        {
            DataTable dt = clsDrivingLicenseClass.ListClasses();
            if (clsGlobal.IsSystemError)
            {
                MessageBox.Show("There is a system error we will solve try Again Later!", "Error",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("There a system error we will solve try Again Later! ", EventLogEntryType.Error);
                clsGlobal.IsSystemError = false;
                return;
            }
            foreach (DataRow dr in dt.Rows)
            {
                cbxClasses.Items.Add(dr["ClassName"].ToString());
            }
            cbxClasses.SelectedIndex = 2;
        }
        private void NewLocalDrivingLicense_Load(object sender, EventArgs e)
        {
            _FillTheClassesComboBox();
            if (_Mode == enMode.AddNew)
            {
                this.Text = "Add New Local Driving License";
                lblApplicationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblUserName.Text = clsGlobal.CurrentUser.UserName;
                lblPaidFees.Text = clsApplicationType.GetApplicationTypeFees(
                  clsApplication.enApplicationType.NewDrivingLicense).ToString();
                tabApplicationInfo.Enabled = false;
                personCardWithFilter1.FilterFocus();
                cbxClasses.SelectedIndex = 2;
            }
            else
            {
                this.Text = "Update Person";
                lblTitle.Text = "Update Local Driving License";
                _FillTheFormWithData();
                tabApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tabApplicationInfo.Enabled = true;
                tabInformation.SelectedTab = tabInformation.TabPages["tabApplicationInfo"];
                return;
            }


            //incase of add new mode.
            if (personCardWithFilter1.ctrPersonCard1.PersonID != -1)
            {

                btnSave.Enabled = true;
                tabApplicationInfo.Enabled = true;
                tabInformation.SelectedTab = tabInformation.TabPages["tabApplicationInfo"];

            }

            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                personCardWithFilter1.FilterFocus();
                clsValidations.LogTheError("Please Select a Person", EventLogEntryType.Error);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _CheckForSystemError()
        {
            if (clsGlobal.IsSystemError)
            {
                MessageBox.Show("There is a system Error,we will solve try Again Later!",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("There a system error we will solve try Again Later! ", EventLogEntryType.Error);
                this.Close();
            }
        }

        private void _ShowSavingFailedMessage()
        {
            MessageBox.Show("Saving Failed,something go wrong,may be a " +
                            "system error try again later!",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            clsValidations.LogTheError("Saving Failed,something go wrong,may be a " +
                            "system error try again later!", EventLogEntryType.Error);
            this.Close();
        }

        private void _ShowSavingSuccessMessage()
        {
            MessageBox.Show("Saved Successfully", "success",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
            clsValidations.LogTheError("Saved Successfully", EventLogEntryType.Error);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = clsDrivingLicenseClass.FindClassIDByClassName(cbxClasses.Text);
            int ApplicationID = clsApplication.IsPersonAnActiveApplicationInThisClass(ctrPersonCard._Person.ID,
                clsApplication.enApplicationType.NewDrivingLicense,
                LicenseClassID);
           if ( ApplicationID != -1)
            {
                MessageBox.Show($"Choose another License Class ,The Selected Person has already an active Application For the Selected Class with ID = {ApplicationID}",
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError($"Choose another License Class ,The Selected Person has already an active Application For the Selected Class with ID = {ApplicationID}", EventLogEntryType.Error);
            }
           else if(clsDriverLicenseInfo.IsLicenseExistByPersonID(ctrPersonCard._Person.ID,LicenseClassID))
            {
                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("Person already have a license with the same applied driving class, Choose diffrent driving class", EventLogEntryType.Error);
            }
           else
            {
              _CheckForSystemError();

                _LDLApp.LicenseClassID = clsDrivingLicenseClass.FindClassIDByClassName(cbxClasses.Text);
                if ( _Mode == enMode.AddNew)
                {
                    _App.ApplicantPersonID = ctrPersonCard._Person.ID;
                    _App.ApplicationDate = DateTime.ParseExact(lblApplicationDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    _App.ApplicationTypeID = 1;
                    _App.ApplicationStatus = enApplicationStatus.New;
                    _App.LastStatusDate = DateTime.Now;
                    _App.PaidFees = Convert.ToSingle(lblPaidFees.Text);
                    _App.UserID = CurrentUser.UserID;
                    if (_App.Save())
                    {
                        _ShowSavingSuccessMessage();
                    }
                    else
                    {
                        _ShowSavingFailedMessage();
                    }
                    _LDLApp.ApplicationID = _App.ApplicationID;
                }

                if (_LDLApp.Save())
                {
                    if(_Mode == enMode.Update)
                    _ShowSavingSuccessMessage();
                    lblTitle.Text = "Update Local Driving License";
                    personCardWithFilter1.StopTheFilteration();
                    lblApplicationID.Text = _LDLApp.LocalDrivingLicenseApplicationID.ToString();
                    this.Text = "Update Local Driving License";
                    _Mode = enMode.Update;
                }
                else
                {
                    _ShowSavingFailedMessage();
                }

            }


        }

        private void NewOrUpdateLocalDrivingLicense_Activated(object sender, EventArgs e)
        {
            personCardWithFilter1.FilterFocus();
        }

        private void tabApplicationInfo_Enter(object sender, EventArgs e)
        {
            btnNext_Click(null,null);
        }
    }
}
