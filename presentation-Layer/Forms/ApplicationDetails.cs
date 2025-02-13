using DVLD_BussinessLayer;
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

namespace DVLD.Forms
{
    public partial class ApplicationDetails : Form
    {
        public ApplicationDetails(int LDLAppID)
        {
            InitializeComponent();
            _LoadDataToLDLAppObject(LDLAppID);
          
        }

        private  void _LoadDataToLDLAppObject(int LDLAppID)
        {
            clsGlobal.LDLApp =   clsLocalDrivingLicenseApplication.FindLDLApplicationByID(LDLAppID);
            if (clsGlobal.LDLApp != null)
            {
                clsGlobal.App = clsApplication.FindApplicationByID(clsGlobal.LDLApp.ApplicationID);
                if (clsGlobal.App != null)
                    ctrApplicationInfoCard1.FillTheApplicationFormWithData(clsGlobal.LDLApp, clsGlobal.App);
                else
                {
                    MessageBox.Show("Basic Application Info cannot be Find!", "Not Found"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("Basic Application Info cannot be Find!", EventLogEntryType.Error);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Local Application Info cannot be Find!", "Not Found"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsValidations.LogTheError("Local Application Info cannot be Find!", EventLogEntryType.Error);

                this.Close();
            }
        }

    }
}
