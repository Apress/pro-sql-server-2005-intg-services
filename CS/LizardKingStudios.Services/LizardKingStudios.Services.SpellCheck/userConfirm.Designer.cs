namespace LizardKingStudios.Services.SpellCheck
{
    partial class userConfirm
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
            this.Suggestions = new System.Windows.Forms.ListBox();
            this.selectSuggestion = new System.Windows.Forms.Button();
            this.cancelForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Suggestions
            // 
            this.Suggestions.FormattingEnabled = true;
            this.Suggestions.Location = new System.Drawing.Point(12, 12);
            this.Suggestions.Name = "Suggestions";
            this.Suggestions.Size = new System.Drawing.Size(279, 173);
            this.Suggestions.TabIndex = 0;
            // 
            // selectSuggestion
            // 
            this.selectSuggestion.Location = new System.Drawing.Point(295, 128);
            this.selectSuggestion.Name = "selectSuggestion";
            this.selectSuggestion.Size = new System.Drawing.Size(75, 23);
            this.selectSuggestion.TabIndex = 1;
            this.selectSuggestion.Text = "&Select";
            this.selectSuggestion.UseVisualStyleBackColor = true;
            this.selectSuggestion.Click += new System.EventHandler(this.selectSuggestion_Click);
            // 
            // cancelForm
            // 
            this.cancelForm.Location = new System.Drawing.Point(295, 162);
            this.cancelForm.Name = "cancelForm";
            this.cancelForm.Size = new System.Drawing.Size(75, 23);
            this.cancelForm.TabIndex = 2;
            this.cancelForm.Text = "&Cancel";
            this.cancelForm.UseVisualStyleBackColor = true;
            this.cancelForm.Click += new System.EventHandler(this.cancelForm_Click);
            // 
            // userConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 205);
            this.Controls.Add(this.cancelForm);
            this.Controls.Add(this.selectSuggestion);
            this.Controls.Add(this.Suggestions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "userConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Confirm Suggestion";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Suggestions;
        private System.Windows.Forms.Button selectSuggestion;
        private System.Windows.Forms.Button cancelForm;
    }
}