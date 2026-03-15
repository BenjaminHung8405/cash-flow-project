namespace CofferBank
{
    partial class AddTransactionForm
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
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnSave = new FontAwesome.Sharp.IconButton();
            label1 = new Label();
            btnCancel = new FontAwesome.Sharp.IconButton();
            contentPanel = new Panel();
            tableLayoutPanel5 = new TableLayoutPanel();
            panel3 = new Panel();
            txtNote = new TextBox();
            dtpDate = new DateTimePicker();
            tableLayoutPanel6 = new TableLayoutPanel();
            btn0 = new Button();
            btnDot = new Button();
            btn9 = new Button();
            btn8 = new Button();
            btn7 = new Button();
            btn6 = new Button();
            btn5 = new Button();
            btn4 = new Button();
            btn3 = new Button();
            btn2 = new Button();
            btn1 = new Button();
            btnBackspace = new FontAwesome.Sharp.IconButton();
            flpCategories = new FlowLayoutPanel();
            PanelToggle = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            lblAmount = new Panel();
            lbCurrency = new Label();
            tableLayoutPanel4 = new TableLayoutPanel();
            btnExpense = new Button();
            btnIncome = new Button();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            contentPanel.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            PanelToggle.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            lblAmount.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.BackColor = Color.FromArgb(248, 250, 252);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(contentPanel, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 82F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 33F));
            tableLayoutPanel1.Size = new Size(913, 619);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(913, 82);
            panel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.Controls.Add(btnSave, 1, 0);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(btnCancel, 2, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(4, 5, 4, 5);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(913, 82);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(5, 150, 105);
            btnSave.Dock = DockStyle.Fill;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.ForeColor = Color.White;
            btnSave.IconChar = FontAwesome.Sharp.IconChar.Check;
            btnSave.IconColor = Color.White;
            btnSave.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnSave.IconSize = 32;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(490, 16);
            btnSave.Margin = new Padding(34, 16, 16, 16);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(178, 50);
            btnSave.TabIndex = 2;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(4, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(448, 82);
            label1.TabIndex = 0;
            label1.Text = "Thêm giao dịch mới";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(225, 29, 72);
            btnCancel.Dock = DockStyle.Fill;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancel.ForeColor = Color.White;
            btnCancel.IconChar = FontAwesome.Sharp.IconChar.TimesRectangle;
            btnCancel.IconColor = Color.White;
            btnCancel.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnCancel.IconSize = 32;
            btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
            btnCancel.Location = new Point(718, 16);
            btnCancel.Margin = new Padding(34, 16, 16, 16);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(179, 50);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // contentPanel
            // 
            contentPanel.BorderStyle = BorderStyle.Fixed3D;
            contentPanel.Controls.Add(tableLayoutPanel5);
            contentPanel.Controls.Add(flpCategories);
            contentPanel.Controls.Add(PanelToggle);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.ForeColor = Color.White;
            contentPanel.Location = new Point(4, 87);
            contentPanel.Margin = new Padding(4, 5, 4, 5);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(16);
            contentPanel.Size = new Size(905, 527);
            contentPanel.TabIndex = 2;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = Color.FromArgb(248, 250, 252);
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(panel3, 0, 0);
            tableLayoutPanel5.Controls.Add(tableLayoutPanel6, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Top;
            tableLayoutPanel5.Location = new Point(16, 202);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(869, 293);
            tableLayoutPanel5.TabIndex = 3;
            // 
            // panel3
            // 
            panel3.Controls.Add(txtNote);
            panel3.Controls.Add(dtpDate);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(428, 287);
            panel3.TabIndex = 5;
            // 
            // txtNote
            // 
            txtNote.BorderStyle = BorderStyle.None;
            txtNote.Dock = DockStyle.Top;
            txtNote.Location = new Point(0, 31);
            txtNote.Multiline = true;
            txtNote.Name = "txtNote";
            txtNote.PlaceholderText = "Thêm ghi chú...";
            txtNote.Size = new Size(428, 257);
            txtNote.TabIndex = 5;
            // 
            // dtpDate
            // 
            dtpDate.Dock = DockStyle.Top;
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Location = new Point(0, 0);
            dtpDate.Margin = new Padding(8, 4, 8, 4);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(428, 31);
            dtpDate.TabIndex = 4;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 3;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel6.Controls.Add(btn0, 1, 3);
            tableLayoutPanel6.Controls.Add(btnDot, 0, 3);
            tableLayoutPanel6.Controls.Add(btn9, 2, 2);
            tableLayoutPanel6.Controls.Add(btn8, 1, 2);
            tableLayoutPanel6.Controls.Add(btn7, 0, 2);
            tableLayoutPanel6.Controls.Add(btn6, 2, 1);
            tableLayoutPanel6.Controls.Add(btn5, 1, 1);
            tableLayoutPanel6.Controls.Add(btn4, 0, 1);
            tableLayoutPanel6.Controls.Add(btn3, 2, 0);
            tableLayoutPanel6.Controls.Add(btn2, 1, 0);
            tableLayoutPanel6.Controls.Add(btn1, 0, 0);
            tableLayoutPanel6.Controls.Add(btnBackspace, 2, 3);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(437, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 4;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.Size = new Size(429, 287);
            tableLayoutPanel6.TabIndex = 6;
            // 
            // btn0
            // 
            btn0.BackColor = Color.White;
            btn0.Dock = DockStyle.Fill;
            btn0.FlatStyle = FlatStyle.Flat;
            btn0.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn0.ForeColor = Color.FromArgb(51, 51, 51);
            btn0.Location = new Point(145, 216);
            btn0.Name = "btn0";
            btn0.Size = new Size(136, 68);
            btn0.TabIndex = 10;
            btn0.Text = "0";
            btn0.UseVisualStyleBackColor = false;
            // 
            // btnDot
            // 
            btnDot.BackColor = Color.White;
            btnDot.Dock = DockStyle.Fill;
            btnDot.FlatStyle = FlatStyle.Flat;
            btnDot.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDot.ForeColor = Color.FromArgb(51, 51, 51);
            btnDot.Location = new Point(3, 216);
            btnDot.Name = "btnDot";
            btnDot.Size = new Size(136, 68);
            btnDot.TabIndex = 9;
            btnDot.Text = ".";
            btnDot.UseVisualStyleBackColor = false;
            btnDot.Click += BtnDot_Click;
            // 
            // btn9
            // 
            btn9.BackColor = Color.White;
            btn9.Dock = DockStyle.Fill;
            btn9.FlatStyle = FlatStyle.Flat;
            btn9.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn9.ForeColor = Color.FromArgb(51, 51, 51);
            btn9.Location = new Point(287, 145);
            btn9.Name = "btn9";
            btn9.Size = new Size(139, 65);
            btn9.TabIndex = 8;
            btn9.Text = "9";
            btn9.UseVisualStyleBackColor = false;
            // 
            // btn8
            // 
            btn8.BackColor = Color.White;
            btn8.Dock = DockStyle.Fill;
            btn8.FlatStyle = FlatStyle.Flat;
            btn8.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn8.ForeColor = Color.FromArgb(51, 51, 51);
            btn8.Location = new Point(145, 145);
            btn8.Name = "btn8";
            btn8.Size = new Size(136, 65);
            btn8.TabIndex = 7;
            btn8.Text = "8";
            btn8.UseVisualStyleBackColor = false;
            // 
            // btn7
            // 
            btn7.BackColor = Color.White;
            btn7.Dock = DockStyle.Fill;
            btn7.FlatStyle = FlatStyle.Flat;
            btn7.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn7.ForeColor = Color.FromArgb(51, 51, 51);
            btn7.Location = new Point(3, 145);
            btn7.Name = "btn7";
            btn7.Size = new Size(136, 65);
            btn7.TabIndex = 6;
            btn7.Text = "7";
            btn7.UseVisualStyleBackColor = false;
            // 
            // btn6
            // 
            btn6.BackColor = Color.White;
            btn6.Dock = DockStyle.Fill;
            btn6.FlatStyle = FlatStyle.Flat;
            btn6.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn6.ForeColor = Color.FromArgb(51, 51, 51);
            btn6.Location = new Point(287, 74);
            btn6.Name = "btn6";
            btn6.Size = new Size(139, 65);
            btn6.TabIndex = 5;
            btn6.Text = "6";
            btn6.UseVisualStyleBackColor = false;
            // 
            // btn5
            // 
            btn5.BackColor = Color.White;
            btn5.Dock = DockStyle.Fill;
            btn5.FlatStyle = FlatStyle.Flat;
            btn5.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn5.ForeColor = Color.FromArgb(51, 51, 51);
            btn5.Location = new Point(145, 74);
            btn5.Name = "btn5";
            btn5.Size = new Size(136, 65);
            btn5.TabIndex = 4;
            btn5.Text = "5";
            btn5.UseVisualStyleBackColor = false;
            // 
            // btn4
            // 
            btn4.BackColor = Color.White;
            btn4.Dock = DockStyle.Fill;
            btn4.FlatStyle = FlatStyle.Flat;
            btn4.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn4.ForeColor = Color.FromArgb(51, 51, 51);
            btn4.Location = new Point(3, 74);
            btn4.Name = "btn4";
            btn4.Size = new Size(136, 65);
            btn4.TabIndex = 3;
            btn4.Text = "4";
            btn4.UseVisualStyleBackColor = false;
            // 
            // btn3
            // 
            btn3.BackColor = Color.White;
            btn3.Dock = DockStyle.Fill;
            btn3.FlatStyle = FlatStyle.Flat;
            btn3.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn3.ForeColor = Color.FromArgb(51, 51, 51);
            btn3.Location = new Point(287, 3);
            btn3.Name = "btn3";
            btn3.Size = new Size(139, 65);
            btn3.TabIndex = 2;
            btn3.Text = "3";
            btn3.UseVisualStyleBackColor = false;
            // 
            // btn2
            // 
            btn2.BackColor = Color.White;
            btn2.Dock = DockStyle.Fill;
            btn2.FlatStyle = FlatStyle.Flat;
            btn2.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn2.ForeColor = Color.FromArgb(51, 51, 51);
            btn2.Location = new Point(145, 3);
            btn2.Name = "btn2";
            btn2.Size = new Size(136, 65);
            btn2.TabIndex = 1;
            btn2.Text = "2";
            btn2.UseVisualStyleBackColor = false;
            // 
            // btn1
            // 
            btn1.BackColor = Color.White;
            btn1.Dock = DockStyle.Fill;
            btn1.FlatStyle = FlatStyle.Flat;
            btn1.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn1.ForeColor = Color.FromArgb(51, 51, 51);
            btn1.Location = new Point(3, 3);
            btn1.Name = "btn1";
            btn1.Size = new Size(136, 65);
            btn1.TabIndex = 0;
            btn1.Text = "1";
            btn1.UseVisualStyleBackColor = false;
            // 
            // btnBackspace
            // 
            btnBackspace.BackColor = Color.FromArgb(225, 29, 72);
            btnBackspace.Dock = DockStyle.Fill;
            btnBackspace.IconChar = FontAwesome.Sharp.IconChar.ArrowLeft;
            btnBackspace.IconColor = Color.White;
            btnBackspace.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnBackspace.Location = new Point(287, 216);
            btnBackspace.Name = "btnBackspace";
            btnBackspace.Size = new Size(139, 68);
            btnBackspace.TabIndex = 11;
            btnBackspace.UseVisualStyleBackColor = false;
            btnBackspace.Click += BtnBackspace_Click;
            // 
            // flpCategories
            // 
            flpCategories.AutoScroll = true;
            flpCategories.Dock = DockStyle.Top;
            flpCategories.Location = new Point(16, 96);
            flpCategories.Margin = new Padding(3, 16, 3, 3);
            flpCategories.Name = "flpCategories";
            flpCategories.Padding = new Padding(0, 0, 0, 16);
            flpCategories.Size = new Size(869, 106);
            flpCategories.TabIndex = 2;
            flpCategories.WrapContents = false;
            // 
            // PanelToggle
            // 
            PanelToggle.BackColor = Color.White;
            PanelToggle.Controls.Add(tableLayoutPanel3);
            PanelToggle.Dock = DockStyle.Top;
            PanelToggle.Location = new Point(16, 16);
            PanelToggle.Margin = new Padding(0);
            PanelToggle.Name = "PanelToggle";
            PanelToggle.Size = new Size(869, 80);
            PanelToggle.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.FromArgb(248, 250, 252);
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(lblAmount, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Top;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Margin = new Padding(3, 3, 3, 16);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.Padding = new Padding(0, 0, 0, 16);
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(869, 80);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // lblAmount
            // 
            lblAmount.BackColor = Color.White;
            lblAmount.BorderStyle = BorderStyle.Fixed3D;
            lblAmount.Controls.Add(lbCurrency);
            lblAmount.Dock = DockStyle.Fill;
            lblAmount.Location = new Point(3, 3);
            lblAmount.Name = "lblAmount";
            lblAmount.Padding = new Padding(8);
            lblAmount.Size = new Size(428, 58);
            lblAmount.TabIndex = 0;
            // 
            // lbCurrency
            // 
            lbCurrency.AutoSize = true;
            lbCurrency.BackColor = Color.Transparent;
            lbCurrency.Dock = DockStyle.Fill;
            lbCurrency.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbCurrency.ForeColor = Color.FromArgb(51, 51, 51);
            lbCurrency.Location = new Point(8, 8);
            lbCurrency.Margin = new Padding(4, 0, 4, 0);
            lbCurrency.Name = "lbCurrency";
            lbCurrency.Size = new Size(105, 40);
            lbCurrency.TabIndex = 2;
            lbCurrency.Text = "0 VNĐ";
            lbCurrency.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(btnExpense, 1, 0);
            tableLayoutPanel4.Controls.Add(btnIncome, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(437, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(429, 58);
            tableLayoutPanel4.TabIndex = 1;
            // 
            // btnExpense
            // 
            btnExpense.BackColor = Color.FromArgb(225, 29, 72);
            btnExpense.Dock = DockStyle.Fill;
            btnExpense.FlatStyle = FlatStyle.Flat;
            btnExpense.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExpense.ForeColor = Color.White;
            btnExpense.Location = new Point(222, 8);
            btnExpense.Margin = new Padding(8);
            btnExpense.Name = "btnExpense";
            btnExpense.Size = new Size(199, 42);
            btnExpense.TabIndex = 1;
            btnExpense.Text = "Chi";
            btnExpense.UseVisualStyleBackColor = false;
            // 
            // btnIncome
            // 
            btnIncome.BackColor = Color.FromArgb(5, 150, 105);
            btnIncome.Dock = DockStyle.Fill;
            btnIncome.FlatStyle = FlatStyle.Flat;
            btnIncome.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnIncome.ForeColor = Color.White;
            btnIncome.Location = new Point(8, 8);
            btnIncome.Margin = new Padding(8);
            btnIncome.Name = "btnIncome";
            btnIncome.Size = new Size(198, 42);
            btnIncome.TabIndex = 0;
            btnIncome.Text = "Thu";
            btnIncome.UseVisualStyleBackColor = false;
            // 
            // AddTransactionForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(913, 619);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AddTransactionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AddTransactionForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            contentPanel.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            PanelToggle.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            lblAmount.ResumeLayout(false);
            lblAmount.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private FontAwesome.Sharp.IconButton btnCancel;
        private Panel contentPanel;
        private FontAwesome.Sharp.IconButton btnSave;
        private Panel PanelToggle;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel lblAmount;
        private TableLayoutPanel tableLayoutPanel4;
        private Button btnIncome;
        private Button btnExpense;
        private TableLayoutPanel tableLayoutPanel5;
        private FlowLayoutPanel flpCategories;
        private Panel panel3;
        private TextBox txtNote;
        private DateTimePicker dtpDate;
        private TableLayoutPanel tableLayoutPanel6;
        private Button btn0;
        private Button btnDot;
        private Button btn9;
        private Button btn8;
        private Button btn7;
        private Button btn6;
        private Button btn5;
        private Button btn4;
        private Button btn3;
        private Button btn2;
        private Button btn1;
        private FontAwesome.Sharp.IconButton btnBackspace;
        private Label lbCurrency;
    }
}