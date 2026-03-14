namespace CofferBank
{
    partial class TransactionCardItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            lbType = new Label();
            lbTitle = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            lbDate = new Label();
            lbCurrency = new Label();
            iconPictureBox = new FontAwesome.Sharp.IconPictureBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.2258072F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 71.77419F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 2, 0);
            tableLayoutPanel1.Controls.Add(iconPictureBox, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(343, 80);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(lbType, 0, 1);
            tableLayoutPanel2.Controls.Add(lbTitle, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(68, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(159, 74);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // lbType
            // 
            lbType.AutoSize = true;
            lbType.Dock = DockStyle.Fill;
            lbType.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lbType.ForeColor = Color.FromArgb(100, 116, 139);
            lbType.Location = new Point(3, 37);
            lbType.Name = "lbType";
            lbType.Size = new Size(153, 37);
            lbType.TabIndex = 1;
            lbType.Text = "Ăn uống";
            // 
            // lbTitle
            // 
            lbTitle.AutoSize = true;
            lbTitle.Dock = DockStyle.Fill;
            lbTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTitle.Location = new Point(3, 0);
            lbTitle.Name = "lbTitle";
            lbTitle.Size = new Size(153, 37);
            lbTitle.TabIndex = 0;
            lbTitle.Text = "Ăn bún đậu";
            lbTitle.TextAlign = ContentAlignment.BottomLeft;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(lbDate, 0, 0);
            tableLayoutPanel3.Controls.Add(lbCurrency, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(233, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(107, 74);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // lbDate
            // 
            lbDate.AutoSize = true;
            lbDate.Dock = DockStyle.Fill;
            lbDate.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbDate.ForeColor = Color.FromArgb(100, 116, 139);
            lbDate.Location = new Point(3, 0);
            lbDate.Name = "lbDate";
            lbDate.Size = new Size(101, 37);
            lbDate.TabIndex = 2;
            lbDate.Text = "14/03/2026";
            lbDate.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lbCurrency
            // 
            lbCurrency.AutoSize = true;
            lbCurrency.Dock = DockStyle.Fill;
            lbCurrency.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbCurrency.ForeColor = Color.FromArgb(225, 29, 72);
            lbCurrency.Location = new Point(3, 37);
            lbCurrency.Name = "lbCurrency";
            lbCurrency.Size = new Size(101, 37);
            lbCurrency.TabIndex = 1;
            lbCurrency.Text = "- 150,000 đ";
            lbCurrency.TextAlign = ContentAlignment.MiddleRight;
            // 
            // iconPictureBox
            // 
            iconPictureBox.Anchor = AnchorStyles.None;
            iconPictureBox.BackColor = Color.White;
            iconPictureBox.ForeColor = SystemColors.ControlText;
            iconPictureBox.IconChar = FontAwesome.Sharp.IconChar.None;
            iconPictureBox.IconColor = SystemColors.ControlText;
            iconPictureBox.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconPictureBox.Location = new Point(16, 24);
            iconPictureBox.Name = "iconPictureBox";
            iconPictureBox.Size = new Size(32, 32);
            iconPictureBox.TabIndex = 3;
            iconPictureBox.TabStop = false;
            // 
            // TransactionCardItem
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(tableLayoutPanel1);
            Name = "TransactionCardItem";
            Size = new Size(343, 80);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lbTitle;
        private Label lbType;
        private TableLayoutPanel tableLayoutPanel3;
        private Label lbDate;
        private Label lbCurrency;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox;
    }
}
