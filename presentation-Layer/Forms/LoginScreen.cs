using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BussinessLayer;
using Microsoft.Win32;
using System.Configuration;
using System.Xml.Linq;
using System.Xml;


namespace DVLD.Forms
{
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
        }
       public static string []Result;
        public static string path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Credentials";
        private void _ManageRemembering()
        {
            if (chxRememberMe.Checked)
            {
                if (Result != null)
                {
                    if (txtUserName.Text == Result[0]) return;
                }
               if(! clsLogin.WriteToRegistery(txtUserName.Text, txtPassword.Text, path))

                {
                    MessageBox.Show("There is an error in Remeber your UserName and Password , try to check Remember me again!",
                        "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("There is an error in Remeber your UserName and Password ", EventLogEntryType.Error);
                    chxRememberMe.Checked = false;
                }
            }
            else
            {
                if(Result[0] == txtUserName.Text) 
                  clsLogin.DeletedCurrentCredentials(path);
               
            }

        }
  

       private void timer1_Tick(object sender, EventArgs e)
        {
            if (clsLogin._CurrentWait == 0)
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
                btnLogin.Enabled = true;
                chxRememberMe.Enabled = true;
                timer1.Enabled = false;
                lblLoginFailed.Visible = false;
                clsLogin.IncreaseCountOfLogin();
                clsLogin.CalculateCurrentWait();
                clsLogin.AssignCurrentWaitToPrevWait();
                return;
            }
            lblLoginFailed.Text = "Please Wait " + clsLogin._CurrentWait + " Seconds";
        

            clsLogin. DecreaseTheCurrentWait();
        }

        private bool _GetStoredCredential()
        {
            
                try
                {
                   string value = Registry.GetValue(path, "DVLD_Credentials", null) as string ;
                  if(value == null) return false ;
                  Result = value?.Split(new string[] { "#//#" }, StringSplitOptions.None);
                   return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                        "Error In Reading Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  clsValidations.LogTheError("Error in reading Credentials", EventLogEntryType.Error);

                    return false;
                }
            
        }
        private void LoginScreen_Load(object sender, EventArgs e)
        {
            if (_GetStoredCredential())
            {
              
                    txtUserName.Text = Result[0];
                    txtPassword.Text = clsUser.Decrypt(Result[1], ConfigurationManager.AppSettings["Key"]);
                    chxRememberMe.Checked = true;
            }
            else
                chxRememberMe.Checked = false;


            //get settings from config
            if(ConfigurationManager.AppSettings["Color"] == "CornflowerBlue")
                rdbCornflowerBlue.Checked = true;
            else if(ConfigurationManager.AppSettings["Color"] == "Red")
                rdbRed.Checked = true;
            else
                rdbLime.Checked = true;

        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            if (clsLogin.Login(txtUserName.Text,txtPassword.Text))
            {
                if (clsGlobal.CurrentUser.IsActive == 0)
                {
                    MessageBox.Show("Account is not activated ,Contact Your Admin!",
                        "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsValidations.LogTheError("Account is not activated ,Contact Your Admin!", EventLogEntryType.Error);

                    return;
                }
                clsValidations.LogTheError("Login Successfully", EventLogEntryType.Information);
                _ManageRemembering();
                this.Hide();
                MainForm frmMainForm = new MainForm( this);
                frmMainForm.ShowDialog();
            }
            else
            {
                if (clsGlobal.IsSystemError)
                {
                    MessageBox.Show("There is a system error in Login in ,we will solve it, Try Again Later or call support",
                        "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsGlobal.IsSystemError = false;
                    return;
                }
               
                lblLoginFailed.Visible = true;
                lblLoginFailed.Text = "Login Failed," + "You have only" + (3 - clsLogin.FaildLoginCount) + " attempts.";
                if (clsLogin.FaildLoginCount == 3)
                {
                    txtUserName.Enabled = false;
                    txtPassword.Enabled = false;
                    btnLogin.Enabled = false;
                    chxRememberMe.Enabled = false;
                    timer1.Enabled = true;
                    clsValidations.LogTheError("warning,there is more tries to log to your account", EventLogEntryType.Warning);
                }

            }
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public  void UpdateAppSetting(string key, string value)
        {
            string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFile);

            XmlNode node = xmlDoc.SelectSingleNode("//appSettings");
            if (node != null)
            {
                XmlElement element = (XmlElement)node.SelectSingleNode($"add[@key='{key}']");
                if (element != null) element.SetAttribute("value", value);
                xmlDoc.Save(configFile);
            }
        }

        private void ChangeColor(RadioButton rbColor)
        {
            if (rbColor.Checked)
            {
                Color color = ColorTranslator.FromHtml(rbColor.Tag.ToString());
                this.BackColor = color;
                UpdateAppSetting("Color", rbColor.Tag.ToString());
                splitContainer1.Panel2.BackColor = color;
                splitContainer1.Panel1.BackColor = color;
                splitContainer1.BackColor = color;
            }
        }
        private void rdbColor_CheckedChanged(object sender, EventArgs e)
        {
            ChangeColor((RadioButton)sender);
        }
    }
}
