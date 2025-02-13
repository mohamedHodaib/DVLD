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
    public partial class ShowPersonDetails : Form
    {
        public delegate void DataBackEventHandler(string FullName);
        public event DataBackEventHandler DataBack;
        public ShowPersonDetails(int PersonID)
        {
            InitializeComponent();
           ctrPersonCard1.FillPersonDetails(PersonID);
               

        }

        private void ShowPersonDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(DataBack != null) 
                DataBack.Invoke(ctrPersonCard._Person.FName + " " +
           ctrPersonCard._Person.SName + " " + ctrPersonCard._Person.TName
           + " " + ctrPersonCard._Person.LName);
        }
    }
}
