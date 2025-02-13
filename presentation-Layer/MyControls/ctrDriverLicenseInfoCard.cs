using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Properties;
using DVLD_BussinessLayer;
namespace DVLD.MyControls
{
    public partial class ctrDriverLicenseInfoCard : UserControl
    {
        public ctrDriverLicenseInfoCard()
        {
            InitializeComponent();
        }

       public  clsDriverLicenseInfo DriverLicenseInfo = new clsDriverLicenseInfo();
        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void _LoadImage()
        {
            if (DriverLicenseInfo.DriverInfo.PersonInfo.Gender == 0)
            {
                lblGender.Text = "Male";
                pbxGender.Image = Resources.Man_32;
                pbxPersonImage.Image = Resources.Male_512;
            }
            else
            {
                lblGender.Text = "Female";
                pbxGender.Image = Resources.Woman_32;
                pbxPersonImage.Image = Resources.Female_512;
            }
            if (DriverLicenseInfo.DriverInfo.PersonInfo.ImagePath != "")
                if (File.Exists(DriverLicenseInfo.DriverInfo.PersonInfo.ImagePath))
                    pbxPersonImage.Load(DriverLicenseInfo.DriverInfo.PersonInfo.ImagePath);
                else
                {
                    MessageBox.Show("Couldn't Find This Image with path " +
                        DriverLicenseInfo.DriverInfo.PersonInfo.ImagePath,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("Couldn't Find This Image with path " +
                        DriverLicenseInfo.DriverInfo.PersonInfo.ImagePath, EventLogEntryType.Error);
                }

        }
        public void FillTheDriverLicenseInfoCardWithData(clsDriverLicenseInfo DriverLicenseInfo)
        {
            lblLicenseID.Text = DriverLicenseInfo.LicenseID.ToString();
            lblLicenseClass.Text = clsDrivingLicenseClass.
                FindClassNameByClassID( DriverLicenseInfo.LicenseClass);
            lblName.Text = DriverLicenseInfo.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text = DriverLicenseInfo.DriverInfo.PersonInfo.NationalNo;
            lblNotes.Text = DriverLicenseInfo.Notes == "" ?"No Notes": DriverLicenseInfo.Notes;
            lblIssueDate.Text = DriverLicenseInfo.IssueDate.ToString("dd/MMM/yyyy");
            lblExpirationDate.Text = DriverLicenseInfo.ExpirationDate.ToString("dd/MMM/yyyy");
            lblGender.Text = (DriverLicenseInfo.DriverInfo.PersonInfo.Gender == 0) ? "Male" : "Female";
            lblIsActive.Text = DriverLicenseInfo.IsActive? "Yes": "No";
            lblIsDetained.Text = DriverLicenseInfo.IsDetaind? "Yes": "No";
            lblIssueReason.Text = DriverLicenseInfo.IssueReasonText;
            lblDateOFB.Text = DriverLicenseInfo.DriverInfo.PersonInfo.DateOfBirth.ToString("dd/MMM/yyyy");
            lblDriverID.Text = DriverLicenseInfo.DriverID.ToString();
            _LoadImage();

        }
        public bool Fill(int LocalLicenseID)
        {
            DriverLicenseInfo = clsDriverLicenseInfo.GetDriverLicenseInfo(LocalLicenseID);
            if(DriverLicenseInfo != null)
            {
                FillTheDriverLicenseInfoCardWithData(DriverLicenseInfo);
                return true;
            }
            else
            {
                _ShowSystemError();
                return false;
            }
        }
    }
}
