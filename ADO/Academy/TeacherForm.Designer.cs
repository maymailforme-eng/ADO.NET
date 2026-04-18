namespace Academy
{
    partial class TeacherForm
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
            this.dtpWorkSince = new System.Windows.Forms.DateTimePicker();
            this.rbRate = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // dtpWorkSince
            // 
            this.dtpWorkSince.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dtpWorkSince.Location = new System.Drawing.Point(262, 444);
            this.dtpWorkSince.Name = "dtpWorkSince";
            this.dtpWorkSince.Size = new System.Drawing.Size(536, 44);
            this.dtpWorkSince.TabIndex = 18;
            // 
            // rbRate
            // 
            this.rbRate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.rbRate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            this.rbRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbRate.Location = new System.Drawing.Point(262, 543);
            this.rbRate.Name = "rbRate";
            this.rbRate.Size = new System.Drawing.Size(100, 44);
            this.rbRate.TabIndex = 19;
            // 
            // TeacherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 679);
            this.Controls.Add(this.rbRate);
            this.Controls.Add(this.dtpWorkSince);
            this.Name = "TeacherForm";
            this.Text = "Teacher";
            this.Controls.SetChildIndex(this.dtpWorkSince, 0);
            this.Controls.SetChildIndex(this.rbRate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpWorkSince;
        private System.Windows.Forms.TextBox rbRate;
    }
}