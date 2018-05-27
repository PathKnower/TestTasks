namespace TestTask1
{
    partial class Form1
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
            this.UpdateButton = new System.Windows.Forms.Button();
            this.ShowFileVersionButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(46, 44);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(134, 78);
            this.UpdateButton.TabIndex = 0;
            this.UpdateButton.Text = "Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // ShowFileVersionButton
            // 
            this.ShowFileVersionButton.Location = new System.Drawing.Point(301, 44);
            this.ShowFileVersionButton.Name = "ShowFileVersionButton";
            this.ShowFileVersionButton.Size = new System.Drawing.Size(140, 78);
            this.ShowFileVersionButton.TabIndex = 1;
            this.ShowFileVersionButton.Text = "Show Program Version";
            this.ShowFileVersionButton.UseVisualStyleBackColor = true;
            this.ShowFileVersionButton.Click += new System.EventHandler(this.ShowFileVersionButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 180);
            this.Controls.Add(this.ShowFileVersionButton);
            this.Controls.Add(this.UpdateButton);
            this.Name = "Form1";
            this.Text = "TestTask1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button ShowFileVersionButton;
    }
}

