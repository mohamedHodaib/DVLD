namespace DVLD.Forms
{
    partial class frmManagePeople
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
            this.managePeople1 = new DVLD.MyControls.ManagePeople();
            this.SuspendLayout();
            // 
            // managePeople1
            // 
            this.managePeople1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.managePeople1.Location = new System.Drawing.Point(-3, 0);
            this.managePeople1.Name = "managePeople1";
            this.managePeople1.Size = new System.Drawing.Size(1696, 801);
            this.managePeople1.TabIndex = 0;
            this.managePeople1.OnClose += new System.Action<int>(this.managePeople1_OnClose);
            // 
            // frmManagePeople
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1700, 878);
            this.ControlBox = false;
            this.Controls.Add(this.managePeople1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmManagePeople";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        private MyControls.ManagePeople managePeople1;
    }
}