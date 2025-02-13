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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Forms
{
    public partial class UpdateApplicationType : Form
    {
        clsApplicationType ApplicationType = new clsApplicationType();
        public UpdateApplicationType(int ID)
        {
            InitializeComponent();
          ApplicationType = clsApplicationType.GetApplicationType(ID);
            if (ApplicationType == null )
            {
                MessageBox.Show("The is a system error We will solve Try Again Now or Later,The Window will " +
                    "Close!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            _FillTheFormWithApplicationTypeData();
        }

        private void _FillTheFormWithApplicationTypeData()
        {
            lblApplicationTypeID.Text = ApplicationType.ID.ToString();
            txtApplicationTitle.Text = ApplicationType.Title;
            txtApplicationFees.Text = ApplicationType.Fees.ToString();

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public  void txtApplicationTitle_Validating(object sender, CancelEventArgs e)
        {
            if (txtApplicationTitle.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTitle, "Don't leave Application Type Title blank");
            }
            else if (Regex.IsMatch(txtApplicationTitle.Text, @"^(.*[a-zA-Z].*){2,}$"))
            {
                e.Cancel = false;
                errorProvider1.SetError(txtApplicationTitle, "");
            }
            else
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTitle, "please enter at Least 2 alphabetical characters [a-z or A-Z] ");
            }
        }

        private void txtApplicationFees_Validating(object sender, CancelEventArgs e)
        {
          if(txtApplicationFees.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationFees, "Don't leave Application Type Title blank");
            }    
            else if (clsValidations.IsNumber(txtApplicationFees.Text))
            {
                e.Cancel= false;
                errorProvider1.SetError(txtApplicationFees, "");
            }
            else
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationFees, "Please enter Valid Fees");
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
            ApplicationType.Title = txtApplicationTitle.Text;
            ApplicationType.Fees = Convert.ToSingle(txtApplicationFees.Text);
            if (!ApplicationType.Save()) 
            {
                MessageBox.Show("The is a system error We will solve Try Again Now or Later,The Window will " +
                      "Close!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
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
