namespace MasonteVision.Halcon
{
    partial class HaGUI
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HaGUI));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.lbl_X = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.lbl_Y = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.lbl_Gray = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel_ROIid = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.hWindowControl = new HalconDotNet.HWindowControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Zoom = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Move = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Fit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_OpenImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SaveImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_DrawCircle = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_DrawCircleArr = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_DrawRectangle1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_DrawRectangle2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_DrawLine = new System.Windows.Forms.ToolStripButton();
            this.SetCameraSn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteROI = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Orange;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.lbl_X,
            this.toolStripLabel3,
            this.lbl_Y,
            this.toolStripLabel5,
            this.lbl_Gray,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.toolStripLabel_ROIid,
            this.toolStripSeparator3});
            this.toolStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip2.Location = new System.Drawing.Point(0, 450);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(529, 25);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(19, 22);
            this.toolStripLabel1.Text = "X:";
            // 
            // lbl_X
            // 
            this.lbl_X.Name = "lbl_X";
            this.lbl_X.Size = new System.Drawing.Size(32, 22);
            this.lbl_X.Text = "0.00";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(18, 22);
            this.toolStripLabel3.Text = "Y:";
            // 
            // lbl_Y
            // 
            this.lbl_Y.Name = "lbl_Y";
            this.lbl_Y.Size = new System.Drawing.Size(32, 22);
            this.lbl_Y.Text = "0.00";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(35, 22);
            this.toolStripLabel5.Text = "亮度:";
            // 
            // lbl_Gray
            // 
            this.lbl_Gray.Name = "lbl_Gray";
            this.lbl_Gray.Size = new System.Drawing.Size(15, 22);
            this.lbl_Gray.Text = "0";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(67, 22);
            this.toolStripLabel2.Text = "ActiveROI:";
            // 
            // toolStripLabel_ROIid
            // 
            this.toolStripLabel_ROIid.Name = "toolStripLabel_ROIid";
            this.toolStripLabel_ROIid.Size = new System.Drawing.Size(20, 22);
            this.toolStripLabel_ROIid.Text = "-1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // hWindowControl
            // 
            this.hWindowControl.BackColor = System.Drawing.Color.Black;
            this.hWindowControl.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl.Location = new System.Drawing.Point(0, 25);
            this.hWindowControl.Name = "hWindowControl";
            this.hWindowControl.Size = new System.Drawing.Size(529, 425);
            this.hWindowControl.TabIndex = 4;
            this.hWindowControl.WindowSize = new System.Drawing.Size(529, 425);
            this.hWindowControl.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseMove);
            this.hWindowControl.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Orange;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Zoom,
            this.toolStripButton_Move,
            this.toolStripButton_Fit,
            this.toolStripButton_OpenImage,
            this.toolStripButton_SaveImage,
            this.toolStripSeparator1,
            this.toolStripButton_DrawCircle,
            this.toolStripButton_DrawCircleArr,
            this.toolStripButton_DrawRectangle1,
            this.toolStripButton_DrawRectangle2,
            this.toolStripButton_DrawLine,
            this.SetCameraSn,
            this.toolStripSeparator4,
            this.toolStripComboBox1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(529, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_Zoom
            // 
            this.toolStripButton_Zoom.BackColor = System.Drawing.Color.Orange;
            this.toolStripButton_Zoom.Checked = true;
            this.toolStripButton_Zoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_Zoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Zoom.Enabled = false;
            this.toolStripButton_Zoom.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Zoom.Image")));
            this.toolStripButton_Zoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Zoom.Name = "toolStripButton_Zoom";
            this.toolStripButton_Zoom.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Zoom.Text = "缩放";
            // 
            // toolStripButton_Move
            // 
            this.toolStripButton_Move.BackColor = System.Drawing.Color.Orange;
            this.toolStripButton_Move.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Move.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Move.Image")));
            this.toolStripButton_Move.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Move.Name = "toolStripButton_Move";
            this.toolStripButton_Move.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Move.Text = "平移";
            this.toolStripButton_Move.Click += new System.EventHandler(this.toolStripButton_Move_Click);
            // 
            // toolStripButton_Fit
            // 
            this.toolStripButton_Fit.BackColor = System.Drawing.Color.Orange;
            this.toolStripButton_Fit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Fit.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Fit.Image")));
            this.toolStripButton_Fit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Fit.Name = "toolStripButton_Fit";
            this.toolStripButton_Fit.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Fit.Text = "自适应";
            this.toolStripButton_Fit.Click += new System.EventHandler(this.toolStripButton_Fit_Click);
            // 
            // toolStripButton_OpenImage
            // 
            this.toolStripButton_OpenImage.BackColor = System.Drawing.Color.Orange;
            this.toolStripButton_OpenImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_OpenImage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_OpenImage.Image")));
            this.toolStripButton_OpenImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_OpenImage.Name = "toolStripButton_OpenImage";
            this.toolStripButton_OpenImage.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_OpenImage.Text = "打开图片";
            this.toolStripButton_OpenImage.Click += new System.EventHandler(this.toolStripButton_OpenImage_Click);
            // 
            // toolStripButton_SaveImage
            // 
            this.toolStripButton_SaveImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_SaveImage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SaveImage.Image")));
            this.toolStripButton_SaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SaveImage.Name = "toolStripButton_SaveImage";
            this.toolStripButton_SaveImage.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_SaveImage.Text = "保存图片";
            this.toolStripButton_SaveImage.Click += new System.EventHandler(this.toolStripButton_SaveImage_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_DrawCircle
            // 
            this.toolStripButton_DrawCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_DrawCircle.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_DrawCircle.Image")));
            this.toolStripButton_DrawCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_DrawCircle.Name = "toolStripButton_DrawCircle";
            this.toolStripButton_DrawCircle.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_DrawCircle.Text = "画圆";
            this.toolStripButton_DrawCircle.Click += new System.EventHandler(this.toolStripButton_DrawCircle_Click);
            // 
            // toolStripButton_DrawCircleArr
            // 
            this.toolStripButton_DrawCircleArr.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_DrawCircleArr.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_DrawCircleArr.Image")));
            this.toolStripButton_DrawCircleArr.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_DrawCircleArr.Name = "toolStripButton_DrawCircleArr";
            this.toolStripButton_DrawCircleArr.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_DrawCircleArr.Text = "画圆弧";
            this.toolStripButton_DrawCircleArr.Click += new System.EventHandler(this.toolStripButton_DrawCircleArr_Click);
            // 
            // toolStripButton_DrawRectangle1
            // 
            this.toolStripButton_DrawRectangle1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_DrawRectangle1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_DrawRectangle1.Image")));
            this.toolStripButton_DrawRectangle1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_DrawRectangle1.Name = "toolStripButton_DrawRectangle1";
            this.toolStripButton_DrawRectangle1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_DrawRectangle1.Text = "画矩形";
            this.toolStripButton_DrawRectangle1.Click += new System.EventHandler(this.toolStripButton_DrawRectangle1_Click);
            // 
            // toolStripButton_DrawRectangle2
            // 
            this.toolStripButton_DrawRectangle2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_DrawRectangle2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_DrawRectangle2.Image")));
            this.toolStripButton_DrawRectangle2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_DrawRectangle2.Name = "toolStripButton_DrawRectangle2";
            this.toolStripButton_DrawRectangle2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_DrawRectangle2.Text = "画矩形2";
            this.toolStripButton_DrawRectangle2.Click += new System.EventHandler(this.toolStripButton_DrawRectangle2_Click);
            // 
            // toolStripButton_DrawLine
            // 
            this.toolStripButton_DrawLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_DrawLine.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_DrawLine.Image")));
            this.toolStripButton_DrawLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_DrawLine.Name = "toolStripButton_DrawLine";
            this.toolStripButton_DrawLine.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_DrawLine.Text = "画线";
            this.toolStripButton_DrawLine.Click += new System.EventHandler(this.toolStripButton_DrawLine_Click);
            // 
            // SetCameraSn
            // 
            this.SetCameraSn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.SetCameraSn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SetCameraSn.Image = global::MasonteVision.Properties.Resources.选择;
            this.SetCameraSn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SetCameraSn.Name = "SetCameraSn";
            this.SetCameraSn.Size = new System.Drawing.Size(23, 22);
            this.SetCameraSn.Text = "toolStripButton1";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Enabled = false;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteROI,
            this.编辑工具ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 70);
            this.contextMenuStrip1.Text = "右键工具";
            // 
            // DeleteROI
            // 
            this.DeleteROI.Enabled = false;
            this.DeleteROI.Name = "DeleteROI";
            this.DeleteROI.Size = new System.Drawing.Size(180, 22);
            this.DeleteROI.Text = "删除ROI";
            this.DeleteROI.Click += new System.EventHandler(this.删除ROI_Click);
            // 
            // 编辑工具ToolStripMenuItem
            // 
            this.编辑工具ToolStripMenuItem.Enabled = false;
            this.编辑工具ToolStripMenuItem.Name = "编辑工具ToolStripMenuItem";
            this.编辑工具ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.编辑工具ToolStripMenuItem.Text = "编辑工具";
            this.编辑工具ToolStripMenuItem.Click += new System.EventHandler(this.编辑工具ToolStripMenuItem_Click);
            // 
            // HaGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OrangeRed;
            this.Controls.Add(this.hWindowControl);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.toolStrip2);
            this.Name = "HaGUI";
            this.Size = new System.Drawing.Size(529, 475);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel lbl_X;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel lbl_Y;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripLabel lbl_Gray;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private HalconDotNet.HWindowControl hWindowControl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Zoom;
        private System.Windows.Forms.ToolStripButton toolStripButton_Move;
        private System.Windows.Forms.ToolStripButton toolStripButton_Fit;
        private System.Windows.Forms.ToolStripButton toolStripButton_OpenImage;
        private System.Windows.Forms.ToolStripButton toolStripButton_DrawCircle;
        private System.Windows.Forms.ToolStripButton toolStripButton_SaveImage;
        private System.Windows.Forms.ToolStripButton toolStripButton_DrawCircleArr;
        private System.Windows.Forms.ToolStripButton toolStripButton_DrawRectangle1;
        private System.Windows.Forms.ToolStripButton toolStripButton_DrawRectangle2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_ROIid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton_DrawLine;
        private System.Windows.Forms.ToolStripMenuItem DeleteROI;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.ToolStripButton SetCameraSn;
        private System.Windows.Forms.ToolStripMenuItem 编辑工具ToolStripMenuItem;
        public System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
    }
}
