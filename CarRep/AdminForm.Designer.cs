namespace CarRep
{
    partial class AdminForm
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
            this.dgvAdminAppointments = new System.Windows.Forms.DataGridView();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnComplete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAdminAppointments
            // 
            this.dgvAdminAppointments.AllowUserToAddRows = false;
            this.dgvAdminAppointments.AllowUserToDeleteRows = false;
            this.dgvAdminAppointments.AllowUserToOrderColumns = true;
            this.dgvAdminAppointments.AllowUserToResizeRows = false;
            this.dgvAdminAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdminAppointments.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvAdminAppointments.Location = new System.Drawing.Point(40, 95);
            this.dgvAdminAppointments.Name = "dgvAdminAppointments";
            this.dgvAdminAppointments.Size = new System.Drawing.Size(531, 150);
            this.dgvAdminAppointments.TabIndex = 0;
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(40, 271);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(93, 38);
            this.btnApprove.TabIndex = 1;
            this.btnApprove.Text = "Принять";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnReject
            // 
            this.btnReject.Location = new System.Drawing.Point(261, 271);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(93, 38);
            this.btnReject.TabIndex = 2;
            this.btnReject.Text = "Отклонить";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.Location = new System.Drawing.Point(478, 271);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(93, 38);
            this.btnComplete.TabIndex = 3;
            this.btnComplete.Text = "Выполнено";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CarRep.Properties.Resources.fon1;
            this.ClientSize = new System.Drawing.Size(620, 425);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.dgvAdminAppointments);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminAppointments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAdminAppointments;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnComplete;
    }
}