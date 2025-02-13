using DVLD_BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Forms
{
    public partial class UpdateTestType : Form
    {

        clsTestTypes TestType = new clsTestTypes();
        public UpdateTestType(int ID)
        {
            InitializeComponent();
            TestType = clsTestTypes.GetTestType(ID);
            if(TestType != null) 
            _FillTheFormWithTestTypeData();
            else
            {
                MessageBox.Show("Couldn't Find This Test Type","Not Found"
                    ,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void _FillTheFormWithTestTypeData()
        {
            lblTestTypeID.Text = TestType.ID.ToString();
            txtTestTitle.Text = TestType.Title;
            txtTestTypeDescription.Text = TestType.Description;
            txtTestFees.Text = TestType.Fees.ToString();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTestTitle_Validating(object sender, CancelEventArgs e)
        {

            if (txtTestTitle.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTitle, "Don't leave Test Type Title blank");
            }
            else if (Regex.IsMatch(txtTestTitle.Text, @"^(.*[a-zA-Z].*){2,}$"))
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTestTitle, "");
            }
            else
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestTitle, "please enter at Least 2 alphabetical characters [a-z or A-Z] ");
            }
        }

        private void txtTestFees_Validating(object sender, CancelEventArgs e)
        {
            if (txtTestFees.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestFees, "Don't leave Test Type Title blank");
            }
            else if (clsValidations.IsNumber(txtTestFees.Text))
            {
                e.Cancel = false;
                errorProvider1.SetError(txtTestFees, "");
            }
            else
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTestFees, "Please enter at least one number from [0-9]");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            TestType.Title = txtTestTitle.Text.Trim();
            TestType.Description = txtTestTypeDescription.Text.Trim();
            TestType.Fees = Convert.ToSingle( txtTestFees.Text);
            if (!TestType.Save())
            {
                MessageBox.Show("The is a system error We will solve Try Again Now or Later,The Window will " +
                      "Close!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                MessageBox.Show("Saved Successfully"
                    , "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
