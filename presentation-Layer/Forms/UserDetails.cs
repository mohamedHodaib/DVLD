using DVLD.MyControls;
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

namespace DVLD.Forms
{
    public partial class UserDetails : Form
    {
        int _UserID = -1;
        


        public UserDetails(int UserID)
        {
            InitializeComponent();
           _UserID = UserID;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UserDetails_Load(object sender, EventArgs e)
        {
            ctrUserInfo2.LoadUserInfo(_UserID);
        }
    }
}
