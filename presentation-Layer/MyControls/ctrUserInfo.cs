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
    public partial class ctrUserInfo : UserControl
    {
        public ctrUserInfo()
        {
            InitializeComponent();
        }

        private clsUser _User = new clsUser();

        private void _FillUserInfo()
        {
            ctrPersonCard1.FillPersonDetails(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName;
            if (_User.IsActive == 1)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";
        }

        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.FindByUserID(UserID).Result;
            if (_User == null)
            {
                MessageBox.Show($"No User with ID:{_User.UserID}", "User Not Found", MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
            else
                _FillUserInfo();
        }

    }
}
