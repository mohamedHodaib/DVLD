using DVLD.Forms;
using DVLD_BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DVLD.MyControls
{
    public partial class ApplicationInfo : UserControl
    {
        public ApplicationInfo()
        {
            InitializeComponent();
        }
        clsApplication _App = new clsApplication();

        int _LicenseID = 0;
        private void _FillApplicationBasicInfo(clsApplication App)
        {
            lblApplicationID.Text = App.ApplicationID.ToString();
            clsPerson P =   clsPerson.FindByPersonID(App.ApplicantPersonID).Result;
            lblApplicantFullName.Text = P.FName + " " + P.SName + " " + P.TName + " " + P.LName;
            lblAppDate.Text = App.ApplicationDate.ToString("dd/MMM/yyyy");
            lblApplicationType.Text = App.ApplicationTypeInfo.Title;
            lblFees.Text = App.PaidFees.ToString();
            lblStatus.Text = App.StatusText;
            lblStatusDate.Text = App.LastStatusDate.ToString("dd/MMM/yyyy");
            lblCreatedByUser.Text = App.CreatedByUserInfo.UserName; 
        }
     
        private void _FillLDLApplicationInfo(clsLocalDrivingLicenseApplication LDLApp)
        {
            _LicenseID = clsLocalDrivingLicenseApplication
                .GetLocalLicenseIDByLDLAppID(LDLApp.LocalDrivingLicenseApplicationID);
            llblShowLicenseInfo.Enabled =  _LicenseID != -1;
            lblLDLAppID.Text = LDLApp.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = clsDrivingLicenseClass.FindClassNameByClassID(LDLApp.LicenseClassID);
            lblPassedTests.Text = LDLApp.GetPassedTests().ToString() + "/3";
        }
     
        public void FillTheApplicationFormWithData(clsLocalDrivingLicenseApplication LDLApp,clsApplication App)
        {
            _App = App;
            _FillApplicationBasicInfo(App);
            _FillLDLApplicationInfo(LDLApp);
        }

        private void _UpdateFullName(string FullName)
        {
            lblApplicantFullName.Text = FullName;
        }
        private void lblViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowPersonDetails frmPersonDetails = new ShowPersonDetails(_App.ApplicantPersonID);
            frmPersonDetails.DataBack += _UpdateFullName;
            frmPersonDetails.ShowDialog();
        }

        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DriverLicenseInfo driverLicenseInfo = new DriverLicenseInfo(_LicenseID);
            driverLicenseInfo.ShowDialog();
        }
    }
}
