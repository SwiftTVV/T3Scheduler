namespace T3Scheduler
{
    partial class Form2
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.airplanedb = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.missliveries = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.airportsearchoption = new System.Windows.Forms.ComboBox();
            this.airportupdateoption = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(31, 53);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(227, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "Select Arrivals File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(31, 99);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(227, 40);
            this.button2.TabIndex = 1;
            this.button2.Text = "Select Departures File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(276, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(276, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(188, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "airplanes database: ";
            // 
            // airplanedb
            // 
            this.airplanedb.FormattingEnabled = true;
            this.airplanedb.Location = new System.Drawing.Point(209, 195);
            this.airplanedb.Name = "airplanedb";
            this.airplanedb.Size = new System.Drawing.Size(236, 32);
            this.airplanedb.TabIndex = 5;
            this.airplanedb.SelectedIndexChanged += new System.EventHandler(this.airplanedb_SelectedIndexChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(31, 289);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(183, 39);
            this.button3.TabIndex = 6;
            this.button3.Text = "Analyze files";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 344);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(276, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "(optional)";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(31, 145);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(227, 40);
            this.button4.TabIndex = 8;
            this.button4.Text = "Select Mappings File";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(272, 289);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(313, 39);
            this.button5.TabIndex = 10;
            this.button5.Text = "Generate mappings file template";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // missliveries
            // 
            this.missliveries.FormattingEnabled = true;
            this.missliveries.Items.AddRange(new object[] {
            "Skip flights with missing liveries",
            "Ignore missing liveries (results in white airplanes)"});
            this.missliveries.Location = new System.Drawing.Point(460, 194);
            this.missliveries.Name = "missliveries";
            this.missliveries.Size = new System.Drawing.Size(469, 32);
            this.missliveries.TabIndex = 11;
            this.missliveries.SelectedIndexChanged += new System.EventHandler(this.missliveries_SelectedIndexChanged);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(633, 290);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(198, 38);
            this.button6.TabIndex = 12;
            this.button6.Text = "Generate Schedule";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(883, 289);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(106, 39);
            this.button7.TabIndex = 13;
            this.button7.Text = "Close";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // airportsearchoption
            // 
            this.airportsearchoption.FormattingEnabled = true;
            this.airportsearchoption.Items.AddRange(new object[] {
            "Ignore flights with unknown airports",
            "Try to find unknown airport info on the Internet"});
            this.airportsearchoption.Location = new System.Drawing.Point(31, 240);
            this.airportsearchoption.Name = "airportsearchoption";
            this.airportsearchoption.Size = new System.Drawing.Size(464, 32);
            this.airportsearchoption.TabIndex = 15;
            // 
            // airportupdateoption
            // 
            this.airportupdateoption.FormattingEnabled = true;
            this.airportupdateoption.Items.AddRange(new object[] {
            "Replace missing airports with closest airport from TS3 database",
            "Update TS3 airport list and add missing airports"});
            this.airportupdateoption.Location = new System.Drawing.Point(535, 240);
            this.airportupdateoption.Name = "airportupdateoption";
            this.airportupdateoption.Size = new System.Drawing.Size(618, 32);
            this.airportupdateoption.TabIndex = 16;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Regular FlightAware schedule files (tab separated)",
            "Premium FlightAware schedule files (tab separated, includes tail number)"});
            this.comboBox1.Location = new System.Drawing.Point(31, 11);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(767, 32);
            this.comboBox1.TabIndex = 17;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1297, 1158);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.airportupdateoption);
            this.Controls.Add(this.airportsearchoption);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.missliveries);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.airplanedb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox airplanedb;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox missliveries;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ComboBox airportsearchoption;
        private System.Windows.Forms.ComboBox airportupdateoption;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}