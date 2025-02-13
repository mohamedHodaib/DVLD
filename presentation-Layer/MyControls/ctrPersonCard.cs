using DVLD.Forms;
using DVLD.Properties;
using DVLD_BussinessLayer;
using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.MyControls
{
    public partial class ctrPersonCard : UserControl
    {
        public ctrPersonCard()
        {
            InitializeComponent();
        }
       public static  clsPerson _Person = new clsPerson();
         int _PersonID = -1;
        public int PersonID { get { return _PersonID; } }
        public void FillPersonDetails (int PersonID)
        {
             _Person = clsPerson.FindByPersonID(PersonID).Result;
             FillPersonCard(_Person);
        }
       public bool FillPersonCard(clsPerson Person)
        {
            if(Person == null )
            {
                MessageBox.Show("Couldn't Find This Person  " ,
                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            _PersonID = Person.ID;
            lblID.Text = Person.ID.ToString();
            lblCountry.Text = clsCountry.FindCountryName(Person .CountryID);
            lblName.Text = Person.FullName;
            lblNationalNo.Text = Person.NationalNo;
            lblPhone.Text =     Person.Phone;
            lblEmail.Text = Person.Email;
            lblAddress.Text = Person.Address;
            lblDateOfBirth.Text = Person.DateOfBirth.ToString("dd/MM/yyyy");
            llblEdit.Enabled = true;
            _Person = Person;
            _LoadImage(Person);
            clsGlobal.IsPersonInformationFillThePersonCard = true;
            return true;
        }

        private void _LoadImage(clsPerson Person )
        {
            if (Person.Gender == 0)
            {
                lblGender.Text = "Male";
                pbxGender.Image = Resources.Man_32;
                pbxImage.Image = Resources.Male_512;
            }
            else
            {
                lblGender.Text = "Female";
                pbxGender.Image = Resources.Woman_32;
                pbxImage.Image = Resources.Female_512;
            }
            if (Person.ImagePath != "")
                if (File.Exists(Person.ImagePath))
                    pbxImage.Load(Person.ImagePath);
                else
                    MessageBox.Show("Couldn't Find This Image with path " + Person.ImagePath,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      
        }
        public void SendDatabackToPersonCard(frmAddorUpdatePerson AddOrUpdatePerson)
        {
    
            AddOrUpdatePerson.DataBack += FillPersonCard;
            AddOrUpdatePerson.ShowDialog();
        }

        private void llblEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddorUpdatePerson AddOrUpdatePerson = new frmAddorUpdatePerson(_Person);
            SendDatabackToPersonCard(AddOrUpdatePerson);
        }
    }
}
