namespace OntopDictionary
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
            this.txtWord = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.checkOffline = new System.Windows.Forms.CheckBox();
            this.richTxtDefinition = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trackOpacity = new System.Windows.Forms.TrackBar();
            this.checkDynamicOpacity = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // txtWord
            // 
            this.txtWord.BackColor = System.Drawing.Color.Black;
            this.txtWord.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtWord.ForeColor = System.Drawing.Color.White;
            this.txtWord.HideSelection = false;
            this.txtWord.Location = new System.Drawing.Point(0, 0);
            this.txtWord.Name = "txtWord";
            this.txtWord.Size = new System.Drawing.Size(622, 20);
            this.txtWord.TabIndex = 0;
            this.txtWord.Leave += new System.EventHandler(this.txtWord_Leave);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Black;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(0, 61);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(622, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // checkOffline
            // 
            this.checkOffline.AutoSize = true;
            this.checkOffline.BackColor = System.Drawing.Color.Transparent;
            this.checkOffline.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkOffline.ForeColor = System.Drawing.Color.White;
            this.checkOffline.Location = new System.Drawing.Point(0, 0);
            this.checkOffline.Name = "checkOffline";
            this.checkOffline.Size = new System.Drawing.Size(56, 61);
            this.checkOffline.TabIndex = 3;
            this.checkOffline.TabStop = false;
            this.checkOffline.Text = "Offline";
            this.checkOffline.UseVisualStyleBackColor = false;
            this.checkOffline.CheckedChanged += new System.EventHandler(this.checkOffline_CheckedChanged);
            // 
            // richTxtDefinition
            // 
            this.richTxtDefinition.BackColor = System.Drawing.Color.Black;
            this.richTxtDefinition.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTxtDefinition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTxtDefinition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.richTxtDefinition.ForeColor = System.Drawing.Color.White;
            this.richTxtDefinition.Location = new System.Drawing.Point(0, 20);
            this.richTxtDefinition.Name = "richTxtDefinition";
            this.richTxtDefinition.ReadOnly = true;
            this.richTxtDefinition.Size = new System.Drawing.Size(622, 265);
            this.richTxtDefinition.TabIndex = 4;
            this.richTxtDefinition.TabStop = false;
            this.richTxtDefinition.Text = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.panel1.Controls.Add(this.trackOpacity);
            this.panel1.Controls.Add(this.checkDynamicOpacity);
            this.panel1.Controls.Add(this.checkOffline);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 285);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(622, 84);
            this.panel1.TabIndex = 5;
            // 
            // trackOpacity
            // 
            this.trackOpacity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackOpacity.Location = new System.Drawing.Point(56, 0);
            this.trackOpacity.Minimum = 1;
            this.trackOpacity.Name = "trackOpacity";
            this.trackOpacity.Size = new System.Drawing.Size(481, 61);
            this.trackOpacity.TabIndex = 4;
            this.trackOpacity.Value = 10;
            this.trackOpacity.ValueChanged += new System.EventHandler(this.trackOpacity_ValueChanged);
            // 
            // checkDynamicOpacity
            // 
            this.checkDynamicOpacity.AutoSize = true;
            this.checkDynamicOpacity.BackColor = System.Drawing.Color.Transparent;
            this.checkDynamicOpacity.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkDynamicOpacity.ForeColor = System.Drawing.Color.White;
            this.checkDynamicOpacity.Location = new System.Drawing.Point(537, 0);
            this.checkDynamicOpacity.Name = "checkDynamicOpacity";
            this.checkDynamicOpacity.Size = new System.Drawing.Size(85, 61);
            this.checkDynamicOpacity.TabIndex = 5;
            this.checkDynamicOpacity.TabStop = false;
            this.checkDynamicOpacity.Text = "Auto opacity";
            this.checkDynamicOpacity.UseVisualStyleBackColor = false;
            this.checkDynamicOpacity.CheckedChanged += new System.EventHandler(this.checkDynamicOpacity_CheckedChanged);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 369);
            this.Controls.Add(this.richTxtDefinition);
            this.Controls.Add(this.txtWord);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Opacity = 0.8D;
            this.Text = "On-top Dictionary";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackOpacity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtWord;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckBox checkOffline;
        private System.Windows.Forms.RichTextBox richTxtDefinition;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar trackOpacity;
        private System.Windows.Forms.CheckBox checkDynamicOpacity;
    }
}

