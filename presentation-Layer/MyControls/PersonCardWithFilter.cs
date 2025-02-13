using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Forms;
using DVLD_BussinessLayer;
namespace DVLD.MyControls
{
    public partial class PersonCardWithFilter : UserControl
    {
        public PersonCardWithFilter()
        {
            InitializeComponent();
        }
        public  clsPerson _P = new clsPerson();
        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public bool FillThePersonCardWithFilter(clsPerson P)
        {
            cbxFilter.SelectedIndex = 0;
            if(P != null) 
            txtFilter.Text = P.ID.ToString();
            return ctrPersonCard1.FillPersonCard(P);
        }
        private void FindPerson()
        {
            switch (cbxFilter.Text)
            {
                case "Person ID":
                    ctrPersonCard1.FillPersonDetails(int.Parse(txtFilter.Text));
                    break;

                case "National No.":
                    ctrPersonCard1.FillPersonCard(clsPerson.FindByNationalNumber(txtFilter.Text));
                    break;

                default:
                    break;
            }
          
        }
        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            if (txtFilter.Text == "")
            {
                MessageBox.Show("Don't leave The Filter Field blank!", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FindPerson();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddorUpdatePerson AddOrUpdatePerson = new frmAddorUpdatePerson();
            ctrPersonCard1.SendDatabackToPersonCard(AddOrUpdatePerson);
            txtFilter.Text = ctrPersonCard._Person.ID.ToString();
            cbxFilter.SelectedIndex = 0;
        }

        public void StopTheFilteration()
        {
            grbFilter.Enabled = false;
        }

        public void FilterFocus()
        {
            txtFilter.Focus();
        }
        private void PersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbxFilter.SelectedIndex = 1;
            FilterFocus();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                btnSearchPerson.PerformClick();
            }

            if(cbxFilter.Text == "PersonID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
