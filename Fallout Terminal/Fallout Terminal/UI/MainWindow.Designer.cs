namespace Fallout_Terminal
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.TextDisplay = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // TextDisplay
            // 
            this.TextDisplay.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.TextDisplay.BackColor = System.Drawing.Color.Black;
            this.TextDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextDisplay.Cursor = System.Windows.Forms.Cursors.Default;
            this.TextDisplay.DetectUrls = false;
            this.TextDisplay.Font = new System.Drawing.Font("Lucida Console", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextDisplay.ForeColor = System.Drawing.Color.LimeGreen;
            this.TextDisplay.Location = new System.Drawing.Point(42, 30);
            this.TextDisplay.MaxLength = 1100;
            this.TextDisplay.Name = "TextDisplay";
            this.TextDisplay.ReadOnly = true;
            this.TextDisplay.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.TextDisplay.ShortcutsEnabled = false;
            this.TextDisplay.Size = new System.Drawing.Size(552, 420);
            this.TextDisplay.TabIndex = 0;
            this.TextDisplay.Text = resources.GetString("TextDisplay.Text");
            this.TextDisplay.WordWrap = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(642, 496);
            this.Controls.Add(this.TextDisplay);
            this.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fallout 4 Terminal";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox TextDisplay;
    }
}

