using DVLD.MyControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace DVLD.Forms
{
    public partial class frmDriverInternationalLicenseInfo : Form
    {
        public frmDriverInternationalLicenseInfo(int ILID, bool IsWithLocalLicenseCard)
        {
            InitializeComponent();
            if (!driverInternationalLicenseInfo1.Fill(ILID))
            {
                MessageBox.Show("The is a system error We will solve Try Again Now or Later,The Window will " +
                 "Close!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("There is a system error in Login in ,we will solve it", EventLogEntryType.Error);


            this.Close();
            }
        }
    }
}
