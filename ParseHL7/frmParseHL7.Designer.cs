namespace ParseHL7
{
    partial class frmParseHL7
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbHL7Path = new TextBox();
            lblHL7Path = new Label();
            btnBrowseHL7 = new Button();
            openFileDialog = new OpenFileDialog();
            tabControl1 = new TabControl();
            tabPage3 = new TabPage();
            rtbRaw = new RichTextBox();
            tabPage1 = new TabPage();
            rtbXML = new RichTextBox();
            tabPage5 = new TabPage();
            rtbXml2Json = new RichTextBox();
            tabPage4 = new TabPage();
            rtbFHIR = new RichTextBox();
            lblEIIE = new Label();
            tabControl1.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage4.SuspendLayout();
            SuspendLayout();
            // 
            // tbHL7Path
            // 
            tbHL7Path.Location = new Point(99, 49);
            tbHL7Path.Name = "tbHL7Path";
            tbHL7Path.Size = new Size(860, 27);
            tbHL7Path.TabIndex = 0;
            // 
            // lblHL7Path
            // 
            lblHL7Path.AutoSize = true;
            lblHL7Path.Font = new Font("Lucida Sans", 11.25F, FontStyle.Regular, GraphicsUnit.Point,  0);
            lblHL7Path.Location = new Point(12, 54);
            lblHL7Path.Name = "lblHL7Path";
            lblHL7Path.Size = new Size(77, 17);
            lblHL7Path.TabIndex = 1;
            lblHL7Path.Text = "HL7 Path:";
            // 
            // btnBrowseHL7
            // 
            btnBrowseHL7.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point,  0);
            btnBrowseHL7.Location = new Point(978, 51);
            btnBrowseHL7.Name = "btnBrowseHL7";
            btnBrowseHL7.Size = new Size(75, 23);
            btnBrowseHL7.TabIndex = 2;
            btnBrowseHL7.Text = "Browse";
            btnBrowseHL7.UseVisualStyleBackColor = true;
            btnBrowseHL7.Click += btnBrowseHL7_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Font = new Font("Lucida Sans", 11.25F, FontStyle.Regular, GraphicsUnit.Point,  0);
            tabControl1.Location = new Point(12, 119);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1041, 440);
            tabControl1.TabIndex = 4;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(rtbRaw);
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1033, 410);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Raw";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // rtbRaw
            // 
            rtbRaw.Font = new Font("Lucida Console", 11.25F, FontStyle.Regular, GraphicsUnit.Point,  0);
            rtbRaw.Location = new Point(3, 3);
            rtbRaw.Name = "rtbRaw";
            rtbRaw.Size = new Size(1027, 404);
            rtbRaw.TabIndex = 0;
            rtbRaw.Text = "";
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(rtbXML);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1033, 410);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "XML";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // rtbXML
            // 
            rtbXML.Font = new Font("Lucida Console", 11.25F, FontStyle.Regular, GraphicsUnit.Point,  0);
            rtbXML.Location = new Point(6, 6);
            rtbXML.Name = "rtbXML";
            rtbXML.Size = new Size(1021, 398);
            rtbXML.TabIndex = 5;
            rtbXML.Text = "";
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(rtbXml2Json);
            tabPage5.Location = new Point(4, 26);
            tabPage5.Name = "tabPage5";
            tabPage5.Size = new Size(1033, 410);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "JSON";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // rtbXml2Json
            // 
            rtbXml2Json.Location = new Point(3, 3);
            rtbXml2Json.Name = "rtbXml2Json";
            rtbXml2Json.Size = new Size(1027, 404);
            rtbXml2Json.TabIndex = 2;
            rtbXml2Json.Text = "";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(rtbFHIR);
            tabPage4.Font = new Font("Lucida Console", 11.25F, FontStyle.Regular, GraphicsUnit.Point,  0);
            tabPage4.Location = new Point(4, 26);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(1033, 410);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "FHIR";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // rtbFHIR
            // 
            rtbFHIR.Location = new Point(3, 3);
            rtbFHIR.Name = "rtbFHIR";
            rtbFHIR.Size = new Size(1027, 404);
            rtbFHIR.TabIndex = 0;
            rtbFHIR.Text = "";
            // 
            // lblEIIE
            // 
            lblEIIE.AutoSize = true;
            lblEIIE.Font = new Font("Lucida Sans", 15.75F, FontStyle.Bold, GraphicsUnit.Point,  0);
            lblEIIE.Location = new Point(11, 9);
            lblEIIE.Name = "lblEIIE";
            lblEIIE.Size = new Size(376, 23);
            lblEIIE.TabIndex = 7;
            lblEIIE.Text = "EIIE : : Epi Info Integration Engine";
            // 
            // frmParseHL7
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 627);
            Controls.Add(lblEIIE);
            Controls.Add(tabControl1);
            Controls.Add(btnBrowseHL7);
            Controls.Add(lblHL7Path);
            Controls.Add(tbHL7Path);
            Name = "frmParseHL7";
            Text = "Parse HL7";
            tabControl1.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbHL7Path;
        private Label lblHL7Path;
        private Button btnBrowseHL7;
        private OpenFileDialog openFileDialog;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private RichTextBox rtbXML;
        private TabPage tabPage3;
        private RichTextBox rtbRaw;
        private TabPage tabPage4;
        private Label lblEIIE;
        private RichTextBox rtbFHIR;
        private TabPage tabPage5;
        private RichTextBox rtbXml2Json;
    }
}
