namespace CofferBank
{
    partial class Form1
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
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            accountBtn = new FontAwesome.Sharp.IconButton();
            transactionBtn = new FontAwesome.Sharp.IconButton();
            panel1 = new Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            label1 = new Label();
            dashboardBtn = new FontAwesome.Sharp.IconButton();
            tableLayoutPanel5 = new TableLayoutPanel();
            settingBtn = new FontAwesome.Sharp.IconButton();
            label2 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            iconPictureBox2 = new FontAwesome.Sharp.IconPictureBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            label5 = new Label();
            label4 = new Label();
            panel2 = new Panel();
            dashboardControl1 = new DashboardControl();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).BeginInit();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox2).BeginInit();
            tableLayoutPanel6.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(16, 17);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1227, 629);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.White;
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(accountBtn, 0, 3);
            tableLayoutPanel3.Controls.Add(transactionBtn, 0, 2);
            tableLayoutPanel3.Controls.Add(panel1, 0, 0);
            tableLayoutPanel3.Controls.Add(dashboardBtn, 0, 1);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel5, 0, 5);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel2, 0, 7);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 8;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 198F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 95F));
            tableLayoutPanel3.Size = new Size(300, 623);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // accountBtn
            // 
            accountBtn.Anchor = AnchorStyles.Left;
            accountBtn.BackColor = Color.Transparent;
            accountBtn.FlatStyle = FlatStyle.Flat;
            accountBtn.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            accountBtn.ForeColor = Color.FromArgb(51, 65, 85);
            accountBtn.IconChar = FontAwesome.Sharp.IconChar.UserLarge;
            accountBtn.IconColor = Color.FromArgb(51, 65, 85);
            accountBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            accountBtn.IconSize = 32;
            accountBtn.ImageAlign = ContentAlignment.MiddleLeft;
            accountBtn.Location = new Point(3, 215);
            accountBtn.Name = "accountBtn";
            accountBtn.Padding = new Padding(16, 0, 0, 0);
            accountBtn.Size = new Size(294, 58);
            accountBtn.TabIndex = 3;
            accountBtn.Text = "Tài khoản";
            accountBtn.UseVisualStyleBackColor = false;
            accountBtn.Click += iconButton3_Click;
            // 
            // transactionBtn
            // 
            transactionBtn.Anchor = AnchorStyles.Left;
            transactionBtn.BackColor = Color.Transparent;
            transactionBtn.FlatStyle = FlatStyle.Flat;
            transactionBtn.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            transactionBtn.ForeColor = Color.FromArgb(51, 65, 85);
            transactionBtn.IconChar = FontAwesome.Sharp.IconChar.MoneyBillTransfer;
            transactionBtn.IconColor = Color.FromArgb(51, 65, 85);
            transactionBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            transactionBtn.IconSize = 32;
            transactionBtn.ImageAlign = ContentAlignment.MiddleLeft;
            transactionBtn.Location = new Point(3, 150);
            transactionBtn.Name = "transactionBtn";
            transactionBtn.Padding = new Padding(16, 0, 0, 0);
            transactionBtn.Size = new Size(294, 58);
            transactionBtn.TabIndex = 2;
            transactionBtn.Text = "Giao dịch";
            transactionBtn.UseVisualStyleBackColor = false;
            transactionBtn.Click += iconButton2_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel4);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Margin = new Padding(3, 3, 3, 17);
            panel1.Name = "panel1";
            panel1.Size = new Size(294, 62);
            panel1.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel4.Controls.Add(iconPictureBox1, 0, 0);
            tableLayoutPanel4.Controls.Add(label1, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(294, 62);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // iconPictureBox1
            // 
            iconPictureBox1.BackColor = Color.White;
            iconPictureBox1.Dock = DockStyle.Fill;
            iconPictureBox1.ForeColor = Color.FromArgb(37, 99, 235);
            iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Wallet;
            iconPictureBox1.IconColor = Color.FromArgb(37, 99, 235);
            iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox1.IconSize = 52;
            iconPictureBox1.Location = new Point(3, 3);
            iconPictureBox1.Name = "iconPictureBox1";
            iconPictureBox1.Size = new Size(52, 56);
            iconPictureBox1.TabIndex = 0;
            iconPictureBox1.TabStop = false;
            iconPictureBox1.Click += iconPictureBox1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.White;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(37, 99, 235);
            label1.Location = new Point(61, 0);
            label1.Name = "label1";
            label1.Size = new Size(230, 62);
            label1.TabIndex = 1;
            label1.Text = "CofferBank";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dashboardBtn
            // 
            dashboardBtn.Anchor = AnchorStyles.Left;
            dashboardBtn.BackColor = Color.Transparent;
            dashboardBtn.FlatStyle = FlatStyle.Flat;
            dashboardBtn.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dashboardBtn.ForeColor = Color.FromArgb(51, 65, 85);
            dashboardBtn.IconChar = FontAwesome.Sharp.IconChar.ChartSimple;
            dashboardBtn.IconColor = Color.FromArgb(51, 65, 85);
            dashboardBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            dashboardBtn.IconSize = 32;
            dashboardBtn.ImageAlign = ContentAlignment.MiddleLeft;
            dashboardBtn.Location = new Point(3, 85);
            dashboardBtn.Name = "dashboardBtn";
            dashboardBtn.Padding = new Padding(16, 0, 0, 0);
            dashboardBtn.Size = new Size(294, 58);
            dashboardBtn.TabIndex = 1;
            dashboardBtn.Text = "Tổng quan";
            dashboardBtn.UseVisualStyleBackColor = false;
            dashboardBtn.Click += iconButton1_Click;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(settingBtn, 0, 1);
            tableLayoutPanel5.Controls.Add(label2, 0, 0);
            tableLayoutPanel5.Location = new Point(3, 345);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Size = new Size(294, 123);
            tableLayoutPanel5.TabIndex = 4;
            // 
            // settingBtn
            // 
            settingBtn.Anchor = AnchorStyles.Left;
            settingBtn.BackColor = Color.Transparent;
            settingBtn.FlatStyle = FlatStyle.Flat;
            settingBtn.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            settingBtn.ForeColor = Color.FromArgb(51, 65, 85);
            settingBtn.IconChar = FontAwesome.Sharp.IconChar.Cog;
            settingBtn.IconColor = Color.FromArgb(51, 65, 85);
            settingBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            settingBtn.IconSize = 32;
            settingBtn.ImageAlign = ContentAlignment.MiddleLeft;
            settingBtn.Location = new Point(3, 64);
            settingBtn.Name = "settingBtn";
            settingBtn.Padding = new Padding(16, 0, 0, 0);
            settingBtn.Size = new Size(288, 55);
            settingBtn.TabIndex = 4;
            settingBtn.Text = "Cài đặt";
            settingBtn.UseVisualStyleBackColor = false;
            settingBtn.Click += iconButton4_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(51, 65, 85);
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(288, 61);
            label2.TabIndex = 0;
            label2.Text = "Hệ thống";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel2.Controls.Add(iconPictureBox2, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel6, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 543);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(294, 89);
            tableLayoutPanel2.TabIndex = 5;
            // 
            // iconPictureBox2
            // 
            iconPictureBox2.BackColor = Color.White;
            iconPictureBox2.ForeColor = SystemColors.ControlText;
            iconPictureBox2.IconChar = FontAwesome.Sharp.IconChar.None;
            iconPictureBox2.IconColor = SystemColors.ControlText;
            iconPictureBox2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox2.IconSize = 48;
            iconPictureBox2.Location = new Point(3, 3);
            iconPictureBox2.Name = "iconPictureBox2";
            iconPictureBox2.Size = new Size(49, 48);
            iconPictureBox2.TabIndex = 0;
            iconPictureBox2.TabStop = false;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Controls.Add(label5, 0, 1);
            tableLayoutPanel6.Controls.Add(label4, 0, 0);
            tableLayoutPanel6.Location = new Point(61, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Size = new Size(230, 63);
            tableLayoutPanel6.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(3, 31);
            label5.Name = "label5";
            label5.Size = new Size(224, 32);
            label5.TabIndex = 1;
            label5.Text = "Premium Member";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(3, 0);
            label4.Name = "label4";
            label4.Size = new Size(224, 31);
            label4.TabIndex = 0;
            label4.Text = "Nguyễn Tuấn Hải";
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(dashboardControl1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(310, 5);
            panel2.Margin = new Padding(4, 5, 4, 5);
            panel2.Name = "panel2";
            panel2.Size = new Size(913, 619);
            panel2.TabIndex = 1;
            // 
            // dashboardControl1
            // 
            dashboardControl1.Dock = DockStyle.Top;
            dashboardControl1.Location = new Point(0, 0);
            dashboardControl1.Margin = new Padding(6, 8, 6, 8);
            dashboardControl1.Name = "dashboardControl1";
            dashboardControl1.Size = new Size(887, 1000);
            dashboardControl1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1259, 663);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form1";
            Padding = new Padding(16, 17, 16, 17);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox1).EndInit();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconPictureBox2).EndInit();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel4;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private Label label1;
        private FontAwesome.Sharp.IconButton dashboardBtn;
        private FontAwesome.Sharp.IconButton accountBtn;
        private FontAwesome.Sharp.IconButton transactionBtn;
        private TableLayoutPanel tableLayoutPanel5;
        private FontAwesome.Sharp.IconButton settingBtn;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel2;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox2;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label5;
        private Label label4;
        private Panel panel2;
        private DashboardControl dashboardControl1;
    }
}
