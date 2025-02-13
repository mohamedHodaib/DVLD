using DVLD.Properties;
using DVLD_BussinessLayer;
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

namespace DVLD.MyControls
{
    public partial class ctrDriverInternationalLicenseInfoCard : UserControl
    {
        public ctrDriverInternationalLicenseInfoCard()
        {
            InitializeComponent();
        }
        clsDriverInternationalLicenseInfo DriverILicenseInfo = new clsDriverInternationalLicenseInfo();

        private void _LoadPersonImage()
        {
            if (DriverILicenseInfo.DriverInfo.PersonInfo.Gender == 0)
                pbxPersonImage.Image = Resources.Male_512;
            else
                pbxPersonImage.Image = Resources.Female_512;

            string ImagePath = DriverILicenseInfo.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbxPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        public bool Fill(int ILID)
        {
            DriverILicenseInfo = clsDriverInternationalLicenseInfo.GetDriverInternationalLicenseInfo(ILID);
            if(DriverILicenseInfo != null )
            {
            lblILicenseID.Text = DriverILicenseInfo.ILicenseID.ToString();
            lblApplicationID.Text = DriverILicenseInfo.ApplicationID.ToString();
            lblDateOFB.Text = DriverILicenseInfo.DriverInfo.PersonInfo.ID.ToString("dd/MMM/yyyy");
            lblDriverID.Text = DriverILicenseInfo.DriverID.ToString();
            lblExpirationDate.Text = DriverILicenseInfo.ExpirationDate.ToString("dd/MMM/yyyy");
            lblGender.Text = Convert.ToBoolean( DriverILicenseInfo.DriverInfo.PersonInfo.Gender )? "Female" : "Male";
            lblLLicenseID.Text = DriverILicenseInfo.LLicenseID.ToString();
            lblName.Text = DriverILicenseInfo.DriverInfo.PersonInfo.FullName.ToString();
            lblNationalNo.Text = DriverILicenseInfo.DriverInfo.PersonInfo.NationalNo.ToString();
            lblIssueDate.Text = DriverILicenseInfo.IssueDate.ToString("dd/MMM/yyyy");
            lblIsActive.Text = DriverILicenseInfo.IsActive ? "Yes" : "No";
                _LoadPersonImage();
                return true;
            }
            return false;

        }
    }
}
