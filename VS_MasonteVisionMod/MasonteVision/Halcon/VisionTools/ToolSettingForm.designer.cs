namespace HalconVision.VisionTool
{
    partial class ToolSettingForm
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btRuntest = new System.Windows.Forms.Button();
            this.viewDisplayCtrl1 = new NBCommon.Controls.ViewDisplay.ViewDisplayCtrl();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(294, 411);
            this.propertyGrid1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.propertyGrid1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.viewDisplayCtrl1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(847, 465);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btRuntest);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 420);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 42);
            this.panel1.TabIndex = 2;
            // 
            // btRuntest
            // 
            this.btRuntest.Location = new System.Drawing.Point(214, 10);
            this.btRuntest.Name = "btRuntest";
            this.btRuntest.Size = new System.Drawing.Size(75, 23);
            this.btRuntest.TabIndex = 0;
            this.btRuntest.Text = "测试运行";
            this.btRuntest.UseVisualStyleBackColor = true;
            this.btRuntest.Click += new System.EventHandler(this.btRuntest_Click);
            // 
            // viewDisplayCtrl1
            // 
            this.viewDisplayCtrl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.viewDisplayCtrl1.CurImage = null;
            this.viewDisplayCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewDisplayCtrl1.Location = new System.Drawing.Point(303, 3);
            this.viewDisplayCtrl1.Name = "viewDisplayCtrl1";
            this.viewDisplayCtrl1.Size = new System.Drawing.Size(541, 411);
            this.viewDisplayCtrl1.TabIndex = 3;
            // 
            // ToolSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 465);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ToolSettingForm";
            this.Text = "ToolSettingForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btRuntest;
        private NBCommon.Controls.ViewDisplay.ViewDisplayCtrl viewDisplayCtrl1;
        public System.Windows.Forms.Panel panel1;
    }
}