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
using DVLD.Properties;
using DVLD_BussinessLayer;
namespace DVLD.MyControls
{
    public partial class ManagePeople : UserControl
    {
        private DataTable _dtPeople = new DataTable();

        public ManagePeople()
        {
            InitializeComponent();
        }
        public event Action<int> OnClose;
        public virtual void Close(int num)
        {
            Action<int> handler = OnClose;
            if (handler != null)
                handler(num);
        }
        public void JustifyTheNameOfTheColumns()
        {
            if (dgvListPeople.Rows.Count > 0)
            {

                dgvListPeople.Columns[0].HeaderText = "Person ID";
                dgvListPeople.Columns[0].Width = 110;

                dgvListPeople.Columns[1].HeaderText = "National No.";
                dgvListPeople.Columns[1].Width = 120;


                dgvListPeople.Columns[2].HeaderText = "First Name";
                dgvListPeople.Columns[2].Width = 120;

                dgvListPeople.Columns[3].HeaderText = "Second Name";
                dgvListPeople.Columns[3].Width = 140;


                dgvListPeople.Columns[4].HeaderText = "Third Name";
                dgvListPeople.Columns[4].Width = 120;

                dgvListPeople.Columns[5].HeaderText = "Last Name";
                dgvListPeople.Columns[5].Width = 120;

                dgvListPeople.Columns[6].HeaderText = "Gendor";
                dgvListPeople.Columns[6].Width = 120;

                dgvListPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvListPeople.Columns[7].Width = 140;

                dgvListPeople.Columns[8].HeaderText = "Nationality";
                dgvListPeople.Columns[8].Width = 120;


                dgvListPeople.Columns[9].HeaderText = "Phone";
                dgvListPeople.Columns[9].Width = 120;


                dgvListPeople.Columns[10].HeaderText = "Email";
                dgvListPeople.Columns[10].Width = 170;
            }

        }
        public  void RefreshPeopleList()
        {
            _dtPeople = clsPerson.ListPeople();
            _dtPeople = _dtPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                          "FirstName", "SecondName", "ThirdName", "LastName",
                                                          "Gender", "DateOfBirth", "CountryName",
                                                          "Phone", "Email");
            dgvListPeople.DataSource = _dtPeople;
            JustifyTheNameOfTheColumns();
            lblRecords.Text = dgvListPeople.Rows.Count.ToString();
        }
        private void _ShowSystemError()
        {
            MessageBox.Show("There a system error we will solve try Again Later!", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            clsGlobal.IsSystemError = false;
        }
        private void _CheckForSystemError()
        {
            if (clsGlobal.IsSystemError)
                _ShowSystemError();
            else if(!clsGlobal.IsValidID)
            {
                MessageBox.Show("Invalid ID!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsGlobal.IsValidID = true;
            }
        }
        private void ManagePeople_Load(object sender, EventArgs e)
        {
            cbxFilter.SelectedIndex = 0;
           RefreshPeopleList();
            _CheckForSystemError();
        }

        private void btnClose2_Click(object sender, EventArgs e)
        {
            Close(2);
        }


        private void btnclose1_Click(object sender, EventArgs e)
        {
            Close(1);
        }
       
        private void FilterTheDataInTheGridView()
        {
            string FilterColumn = "";
                switch (cbxFilter.Text)
                {
                case "Person ID":
                   FilterColumn = "PersonID";
                   break;
                case "National No":
                   FilterColumn = "NationalNo";
                   break;
                case "Email":
                    FilterColumn = "Email";
                    break;
                case "Phone":
                    FilterColumn = "Phone";
                    break;
                case "First Name":
                    FilterColumn = "FirstName";
                    break;
                case "Second Name":
                    FilterColumn = "SecondName";
                    break;
                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;
                case "Last Name":
                    FilterColumn = "LastName";
                    break;
                case "Nationality":
                    FilterColumn = "Nationality";
                    break;
                case "Gender":
                    FilterColumn = "Gender";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            _CheckForSystemError();
           if(txtFilter.Text.Trim() == "" || FilterColumn == "None")
                _dtPeople.DefaultView.RowFilter = "";
          else if(FilterColumn == "PersonID")
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}",FilterColumn,txtFilter.Text.Trim());
           else
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            lblRecords.Text = dgvListPeople.Rows.Count.ToString();
        }
        private void cbxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            txtFilter.Visible = (cbxFilter.SelectedIndex != 0);
           if (txtFilter.Visible)txtFilter.Focus();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            FilterTheDataInTheGridView();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddorUpdatePerson AddOrUpdatePerson = new frmAddorUpdatePerson();
             AddOrUpdatePerson.ShowDialog();
            RefreshPeopleList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddorUpdatePerson AddOrUpdatePerson = new frmAddorUpdatePerson((int)dgvListPeople.CurrentRow.Cells[0].Value);
            AddOrUpdatePerson.ShowDialog();
            RefreshPeopleList();
        }

        private void showDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPersonDetails frmshowPersonDetails = new ShowPersonDetails((int)dgvListPeople.CurrentRow.Cells[0].Value);
            frmshowPersonDetails.ShowDialog();
           RefreshPeopleList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to Delete this Person?", "Confirm",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {
                if (!clsPerson.DeletePerson((int)dgvListPeople.CurrentRow.Cells[0].Value))
                {
                    if (clsGlobal.IsForeignKeyConstraintException)
                    {
                        MessageBox.Show("Person has other related data,we can't delete", "Delete faild!"
                            , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clsGlobal.IsForeignKeyConstraintException = false;
                    }
                    else
                        _ShowSystemError();

                }    
                    else
                    {
                       RefreshPeopleList();
                       MessageBox.Show("Deleted Successfully!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
            }
               
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbxFilter.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            //if it true it discard the key press
        }
    }
}
