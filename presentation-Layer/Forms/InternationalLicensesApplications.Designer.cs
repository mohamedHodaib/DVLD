namespace DVLD.Forms
{
    partial class InternationalLicensesApplications
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowPersonDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowLicenseDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowPersonLicenseHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.lblRecords = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.cbxFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvListIDLApplications = new System.Windows.Forms.DataGridView();
            this.cbxIsActive = new System.Windows.Forms.ComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnAddNewInternationalDrivingLicenseApplication = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnclose1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListIDLApplications)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowPersonDetails,
            this.ShowLicenseDetails,
            this.ShowPersonLicenseHistory});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(281, 146);
            // 
            // ShowPersonDetails
            // 
            this.ShowPersonDetails.Image = global::DVLD.Properties.Resources.PersonDetails_322;
            this.ShowPersonDetails.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ShowPersonDetails.Name = "ShowPersonDetails";
            this.ShowPersonDetails.Size = new System.Drawing.Size(280, 38);
            this.ShowPersonDetails.Text = "Show Person Details";
            this.ShowPersonDetails.Click += new System.EventHandler(this.ShowPersonDetails_Click);
            // 
            // ShowLicenseDetails
            // 
            this.ShowLicenseDetails.Image = global::DVLD.Properties.Resources.License_View_32;
            this.ShowLicenseDetails.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ShowLicenseDetails.Name = "ShowLicenseDetails";
            this.ShowLicenseDetails.Size = new System.Drawing.Size(280, 38);
            this.ShowLicenseDetails.Text = "Show License Details";
            this.ShowLicenseDetails.Click += new System.EventHandler(this.ShowLicenseDetails_Click);
            // 
            // ShowPersonLicenseHistory
            // 
            this.ShowPersonLicenseHistory.Image = global::DVLD.Properties.Resources.PersonLicenseHistory_32;
            this.ShowPersonLicenseHistory.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ShowPersonLicenseHistory.Name = "ShowPersonLicenseHistory";
            this.ShowPersonLicenseHistory.Size = new System.Drawing.Size(280, 38);
            this.ShowPersonLicenseHistory.Text = "Show Person License History";
            this.ShowPersonLicenseHistory.Click += new System.EventHandler(this.ShowPersonLicenseHistory_Click);
            // 
            // lblRecords
            // 
            this.lblRecords.AutoSize = true;
            this.lblRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecords.Location = new System.Drawing.Point(134, 682);
            this.lblRecords.Name = "lblRecords";
            this.lblRecords.Size = new System.Drawing.Size(18, 20);
            this.lblRecords.TabIndex = 35;
            this.lblRecords.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 677);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 25);
            this.label3.TabIndex = 34;
            this.label3.Text = "#Records:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 337);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 29);
            this.label2.TabIndex = 33;
            this.label2.Text = "Filter by:";
            // 
            // txtFilter
            // 
            this.txtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilter.Location = new System.Drawing.Point(498, 340);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(393, 30);
            this.txtFilter.TabIndex = 31;
            this.txtFilter.Visible = false;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // cbxFilter
            // 
            this.cbxFilter.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cbxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxFilter.ForeColor = System.Drawing.SystemColors.Desktop;
            this.cbxFilter.FormattingEnabled = true;
            this.cbxFilter.Items.AddRange(new object[] {
            "None",
            "Int License ID",
            "L License ID",
            "Driver ID",
            "Application ID",
            "Is Active"});
            this.cbxFilter.Location = new System.Drawing.Point(123, 337);
            this.cbxFilter.Name = "cbxFilter";
            this.cbxFilter.Size = new System.Drawing.Size(283, 33);
            this.cbxFilter.TabIndex = 30;
            this.cbxFilter.SelectedIndexChanged += new System.EventHandler(this.cbxFilter_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(352, 269);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(631, 40);
            this.label1.TabIndex = 28;
            this.label1.Text = "International Driving License Application";
            // 
            // dgvListIDLApplications
            // 
            this.dgvListIDLApplications.AllowUserToAddRows = false;
            this.dgvListIDLApplications.AllowUserToDeleteRows = false;
            this.dgvListIDLApplications.AllowUserToOrderColumns = true;
            this.dgvListIDLApplications.BackgroundColor = System.Drawing.Color.White;
            this.dgvListIDLApplications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListIDLApplications.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvListIDLApplications.Location = new System.Drawing.Point(3, 382);
            this.dgvListIDLApplications.Name = "dgvListIDLApplications";
            this.dgvListIDLApplications.ReadOnly = true;
            this.dgvListIDLApplications.RowHeadersWidth = 51;
            this.dgvListIDLApplications.RowTemplate.Height = 24;
            this.dgvListIDLApplications.Size = new System.Drawing.Size(1461, 271);
            this.dgvListIDLApplications.TabIndex = 32;
            // 
            // cbxIsActive
            // 
            this.cbxIsActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxIsActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxIsActive.FormattingEnabled = true;
            this.cbxIsActive.Items.AddRange(new object[] {
            "All",
            "Yes",
            "No"});
            this.cbxIsActive.Location = new System.Drawing.Point(546, 337);
            this.cbxIsActive.Name = "cbxIsActive";
            this.cbxIsActive.Size = new System.Drawing.Size(243, 33);
            this.cbxIsActive.TabIndex = 40;
            this.cbxIsActive.Visible = false;
            this.cbxIsActive.SelectedIndexChanged += new System.EventHandler(this.cbxIsActive_SelectedIndexChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::DVLD.Properties.Resources.International_324;
            this.pictureBox2.Location = new System.Drawing.Point(793, 45);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(84, 53);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 39;
            this.pictureBox2.TabStop = false;
            // 
            // btnAddNewInternationalDrivingLicenseApplication
            // 
            this.btnAddNewInternationalDrivingLicenseApplication.BackColor = System.Drawing.Color.White;
            this.btnAddNewInternationalDrivingLicenseApplication.Image = global::DVLD.Properties.Resources.New_Application_64;
            this.btnAddNewInternationalDrivingLicenseApplication.Location = new System.Drawing.Point(1329, 282);
            this.btnAddNewInternationalDrivingLicenseApplication.Name = "btnAddNewInternationalDrivingLicenseApplication";
            this.btnAddNewInternationalDrivingLicenseApplication.Size = new System.Drawing.Size(135, 94);
            this.btnAddNewInternationalDrivingLicenseApplication.TabIndex = 38;
            this.btnAddNewInternationalDrivingLicenseApplication.UseVisualStyleBackColor = false;
            this.btnAddNewInternationalDrivingLicenseApplication.Click += new System.EventHandler(this.btnAddNewInternationalDrivingLicenseApplication_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DVLD.Properties.Resources.Applications1;
            this.pictureBox1.Location = new System.Drawing.Point(404, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(514, 261);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // btnclose1
            // 
            this.btnclose1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnclose1.Image = global::DVLD.Properties.Resources.Close_321;
            this.btnclose1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnclose1.Location = new System.Drawing.Point(1250, 659);
            this.btnclose1.Name = "btnclose1";
            this.btnclose1.Size = new System.Drawing.Size(214, 47);
            this.btnclose1.TabIndex = 36;
            this.btnclose1.Text = "Close";
            this.btnclose1.UseVisualStyleBackColor = true;
            this.btnclose1.Click += new System.EventHandler(this.btnclose1_Click);
            // 
            // InternationalLicensesApplications
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1476, 711);
            this.Controls.Add(this.cbxIsActive);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnAddNewInternationalDrivingLicenseApplication);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnclose1);
            this.Controls.Add(this.lblRecords);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.cbxFilter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvListIDLApplications);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InternationalLicensesApplications";
            this.Text = "InternationalLicensesApplications";
            this.Load += new System.EventHandler(this.InternationalLicensesApplications_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListIDLApplications)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddNewInternationalDrivingLicenseApplication;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem ShowPersonLicenseHistory;
        private System.Windows.Forms.Button btnclose1;
        private System.Windows.Forms.ToolStripMenuItem ShowPersonDetails;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ShowLicenseDetails;
        private System.Windows.Forms.Label lblRecords;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.ComboBox cbxFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvListIDLApplications;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ComboBox cbxIsActive;
    }
}