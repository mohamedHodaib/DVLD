using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Forms
{
    public partial class DriverLicenseInfo : Form
    {
        public DriverLicenseInfo(int LocalLicenseID)
        {
            InitializeComponent();
            if(!driverLicenseInfoCard1.Fill(LocalLicenseID))
            {
                this.Close();
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
