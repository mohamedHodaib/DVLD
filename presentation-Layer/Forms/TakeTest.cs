using DVLD.Properties;
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
    public partial class TakeTest : Form
    {
        int AppointmentID = 0;
         clsTestTypes.enTestType   TestTypeID = clsTestTypes.enTestType.VisionTest;
        clsAppointments Appointment = new clsAppointments();
        public TakeTest(int AppointmentID,clsTestTypes.enTestType TestTypeID)
        {
            InitializeComponent();
            this.AppointmentID = AppointmentID;
            this.TestTypeID = TestTypeID;
            _FillTheFormWithData();
        }
        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }
        private void _CheckForSystemError()
        {
            if (clsGlobal.IsSystemError)
                _ShowSystemError();
        }

        private void _FillRetakeTestInfo()
        {
            lblTitle.Text = "Schedule Retake Test";
        }

        private void _FillTheTestResult()
        {
            clsTests Test = clsTests.GetTest(AppointmentID);
            if (Test != null)
            {
                lblTestID.Text = Test.TestID.ToString();
                rdbFail.Visible = false;
                rdbPass.Visible = false;
                lblResult.Visible = true;
                if (Test.TestResult)
                {
                    lblResult.Visible = true;
                    lblResult.Text = "Pass";
                    lblResult.ForeColor = Color.Green;
                }
                else
                {
                    lblResult.Text = "Fail";
                    lblResult.ForeColor = Color.Red;
                }

            }
            else
                _ShowSystemError();

        }
        private void _FillUniqueFieldsInTest()
        {
            string TestName = "", TestTitle = "";
            Image TestImage = null;
            double TestFees = 0;
            if (TestTypeID == clsTestTypes.enTestType.VisionTest)
            {
                TestName = "VisionTest";
                TestTitle = "Schedule VisionTest";
                TestImage = Resources.Vision_5122;
                TestFees = clsTestTypes.GetTestTypeFees(clsTestTypes.enTestType.VisionTest);
            }
            else if(TestTypeID == clsTestTypes.enTestType.WrittenTest)
            {
                TestName = "WrittenTest";
                TestTitle = "Schedule Written Test";
                TestImage = Resources.Written_Test_512;
                TestFees = clsTestTypes.GetTestTypeFees(clsTestTypes.enTestType.WrittenTest);
            }
            else
            {
                TestName = "StreetTest";
                TestTitle = "Schedule Street Test";
                TestImage = Resources.Street_Test_321;
                TestFees = clsTestTypes.GetTestTypeFees(clsTestTypes.enTestType.StreetTest);
            }

            lblTestFees.Text = TestFees.ToString();
            lblTitle.Text = TestTitle;
            groupBox1.Text = TestName;
            pictureBox1.Image = TestImage;
        }
        private void _FillTheFormWithData()
        {
          _FillUniqueFieldsInTest();
            Appointment = clsAppointments.Find(AppointmentID);
            lblDLAppID.Text = Appointment.LocalDrivingLicenseApplicationID.ToString();
            lblDClass.Text = clsDrivingLicenseClass.FindClassNameByClassID(clsGlobal.LDLApp.LicenseClassID);
            _CheckForSystemError();
            clsPerson P = clsPerson.FindByPersonID(clsGlobal.App.ApplicantPersonID).Result;
            _CheckForSystemError();
            lblName.Text = P.FName + " " + P.SName + " " + P.TName + " " + P.LName;
            lblTrial.Text = clsTests.GetTrials(clsGlobal.LDLApp.LocalDrivingLicenseApplicationID,
                (clsTestTypes.enTestType) TestTypeID).ToString();
            _CheckForSystemError();
            lblDate.Text = 
                Appointment.AppointmentDate.ToString("dd/MMM/yyyy");
            if (lblTrial.Text != "0")
            {
                _FillRetakeTestInfo();
            }
            if (clsAppointments.IsAppointmentLocked(AppointmentID))
            {
                _FillTheTestResult();
                lblMessage.Visible = true;
                txtNotes.Enabled = false;
                btnSave.Enabled = false;
            }
            else if(clsGlobal.IsSystemError)
            {
                _ShowSystemError();
                this.Close();
            }

        }
        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _LockTheAppointment()
        {
          if( ! clsAppointments.LockTheAppointment(AppointmentID))
            {
               _ShowSystemError();
                this.Close();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure to save this Result","Confirm"
                ,MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
            clsTests Test = new clsTests();
            Test.TestAppointmentID = AppointmentID;
            Test.TestResult = rdbPass.Checked;
            Test.Notes = txtNotes.Text;
            Test.UserID = clsGlobal.CurrentUserID;
            if(Test.Save())
            {
                lblTestID.Text = Test.TestID.ToString();
                    _LockTheAppointment();
                MessageBox.Show("Data Saved Successfully","Success",MessageBoxButtons.OK
                    ,MessageBoxIcon.Information);
                    this.Close();
            }
            else
                {
                    MessageBox.Show("Saving Failed,something go wrong,may be a " +
                  "system error try again later!",
                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
