namespace CofferBank
{
    partial class TransactionControl
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
            panel1 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new Label();
            addTransactionBtn = new FontAwesome.Sharp.IconButton();
            contentPanel = new Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            PanelFilter = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            cboType = new ComboBox();
            txtSearch = new TextBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            dtpToDate = new DateTimePicker();
            dtpFromDate = new DateTimePicker();
            flpTransactions = new FlowLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            contentPanel.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            PanelFilter.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
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
            tableLayoutPanel1.TabIndex = 1;
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
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48.7931023F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 51.2068977F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 109F));
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(addTransactionBtn, 2, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(4, 5, 4, 5);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(913, 82);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(4, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(384, 82);
            label1.TabIndex = 0;
            label1.Text = "Giao dịch";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // addTransactionBtn
            // 
            addTransactionBtn.BackColor = Color.FromArgb(37, 99, 235);
            addTransactionBtn.Dock = DockStyle.Fill;
            addTransactionBtn.FlatAppearance.BorderSize = 0;
            addTransactionBtn.FlatStyle = FlatStyle.Flat;
            addTransactionBtn.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            addTransactionBtn.IconColor = Color.White;
            addTransactionBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            addTransactionBtn.IconSize = 32;
            addTransactionBtn.Location = new Point(837, 16);
            addTransactionBtn.Margin = new Padding(34, 16, 16, 16);
            addTransactionBtn.Name = "addTransactionBtn";
            addTransactionBtn.Size = new Size(60, 50);
            addTransactionBtn.TabIndex = 1;
            addTransactionBtn.UseVisualStyleBackColor = false;
            // 
            // contentPanel
            // 
            contentPanel.Controls.Add(tableLayoutPanel4);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(4, 87);
            contentPanel.Margin = new Padding(4, 5, 4, 5);
            contentPanel.Name = "contentPanel";
            contentPanel.Size = new Size(905, 527);
            contentPanel.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(PanelFilter, 0, 0);
            tableLayoutPanel4.Controls.Add(flpTransactions, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(905, 527);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // PanelFilter
            // 
            PanelFilter.BackColor = Color.White;
            PanelFilter.Controls.Add(tableLayoutPanel3);
            PanelFilter.Dock = DockStyle.Fill;
            PanelFilter.Location = new Point(4, 4);
            PanelFilter.Margin = new Padding(4);
            PanelFilter.Name = "PanelFilter";
            PanelFilter.Size = new Size(897, 42);
            PanelFilter.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 388F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 285F));
            tableLayoutPanel3.Controls.Add(cboType, 0, 0);
            tableLayoutPanel3.Controls.Add(txtSearch, 2, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel5, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.Padding = new Padding(4, 0, 4, 0);
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(897, 42);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // cboType
            // 
            cboType.Dock = DockStyle.Fill;
            cboType.FormattingEnabled = true;
            cboType.Location = new Point(12, 4);
            cboType.Margin = new Padding(8, 4, 8, 4);
            cboType.Name = "cboType";
            cboType.Size = new Size(200, 33);
            cboType.TabIndex = 0;
            // 
            // txtSearch
            // 
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.Location = new Point(616, 4);
            txtSearch.Margin = new Padding(8, 4, 8, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(269, 31);
            txtSearch.TabIndex = 2;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(dtpToDate, 1, 0);
            tableLayoutPanel5.Controls.Add(dtpFromDate, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(223, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(382, 36);
            tableLayoutPanel5.TabIndex = 3;
            // 
            // dtpToDate
            // 
            dtpToDate.Dock = DockStyle.Fill;
            dtpToDate.Format = DateTimePickerFormat.Short;
            dtpToDate.Location = new Point(199, 4);
            dtpToDate.Margin = new Padding(8, 4, 8, 4);
            dtpToDate.Name = "dtpToDate";
            dtpToDate.Size = new Size(175, 31);
            dtpToDate.TabIndex = 3;
            // 
            // dtpFromDate
            // 
            dtpFromDate.Dock = DockStyle.Fill;
            dtpFromDate.Format = DateTimePickerFormat.Short;
            dtpFromDate.Location = new Point(8, 4);
            dtpFromDate.Margin = new Padding(8, 4, 8, 4);
            dtpFromDate.Name = "dtpFromDate";
            dtpFromDate.Size = new Size(175, 31);
            dtpFromDate.TabIndex = 2;
            // 
            // flpTransactions
            // 
            flpTransactions.AutoScroll = true;
            flpTransactions.Dock = DockStyle.Fill;
            flpTransactions.FlowDirection = FlowDirection.TopDown;
            flpTransactions.Location = new Point(3, 53);
            flpTransactions.Name = "flpTransactions";
            flpTransactions.Size = new Size(899, 471);
            flpTransactions.TabIndex = 1;
            flpTransactions.WrapContents = false;
            // 
            // TransactionControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "TransactionControl";
            Size = new Size(913, 619);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            contentPanel.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            PanelFilter.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private FontAwesome.Sharp.IconButton addTransactionBtn;
        private Panel contentPanel;
        private TableLayoutPanel tableLayoutPanel4;
        private Panel PanelFilter;
        private TableLayoutPanel tableLayoutPanel3;
        private ComboBox cboType;
        private TextBox txtSearch;
        private TableLayoutPanel tableLayoutPanel5;
        private DateTimePicker dtpToDate;
        private DateTimePicker dtpFromDate;
        private FlowLayoutPanel flpTransactions;
    }
}
