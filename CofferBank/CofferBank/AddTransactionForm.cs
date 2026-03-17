using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace CofferBank
{
    public partial class AddTransactionForm : Form
    {
        private string _currentInput = "0";
        private bool _isIncome = false;
        private string _selectedCategory = "";
        private IconChar _selectedIcon = IconChar.None;
        private FontAwesome.Sharp.IconButton? _selectedCategoryButton;

        public decimal Amount { get; private set; }
        public string Category { get; private set; }
        public bool IsIncome { get; private set; }
        public bool IsDebt { get; private set; }
        public string CustomerName { get; private set; }
        public DateTime Date { get; private set; }
        public string Note { get; private set; }
        public string Status { get; private set; }
        public IconChar Icon { get; private set; }

        public AddTransactionForm()
        {
            InitializeComponent();
            InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
            // Numpad buttons
            btn0.Click += NumpadButton_Click;
            btn1.Click += NumpadButton_Click;
            btn2.Click += NumpadButton_Click;
            btn3.Click += NumpadButton_Click;
            btn4.Click += NumpadButton_Click;
            btn5.Click += NumpadButton_Click;
            btn6.Click += NumpadButton_Click;
            btn7.Click += NumpadButton_Click;
            btn8.Click += NumpadButton_Click;
            btn9.Click += NumpadButton_Click;
            btnDot.Click += BtnDot_Click;
            btnBackspace.Click += BtnBackspace_Click;

            // Toggle buttons
            btnIncome.Click += BtnIncome_Click;
            btnExpense.Click += BtnExpense_Click;

            // Category buttons - Income
            btnCatRevenue.Click += (s, e) => CategoryButton_Click(btnCatRevenue, "Doanh thu bán hàng", IconChar.Store);
            btnCatOtherIncome.Click += (s, e) => CategoryButton_Click(btnCatOtherIncome, "Khác", IconChar.Briefcase);

            // Category buttons - Expense
            btnCatCOGS.Click += (s, e) => CategoryButton_Click(btnCatCOGS, "Nhập hàng hóa", IconChar.Boxes);
            btnCatOpEx.Click += (s, e) => CategoryButton_Click(btnCatOpEx, "Chi phí hoạt động", IconChar.Coins);
            btnCatOtherExpense.Click += (s, e) => CategoryButton_Click(btnCatOtherExpense, "Khác", IconChar.EllipsisH);

            // Action buttons
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;

            // Form load
            Load += AddTransactionForm_Load;
        }

        private void AddTransactionForm_Load(object? sender, EventArgs e)
        {
            _isIncome = false;
            dtpDate.Value = DateTime.Now;
            dtpDate.Format = DateTimePickerFormat.Short;
            UpdateAmountDisplay();
            ToggleTransactionType(false);
        }

        #region Numpad Logic

        private void NumpadButton_Click(object? sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null) return;

            string digit = btn.Text;

            if (_currentInput == "0" && digit != ".")
            {
                _currentInput = digit;
            }
            else
            {
                _currentInput += digit;
            }

            UpdateAmountDisplay();
        }

        private void BtnDot_Click(object? sender, EventArgs e)
        {
            if (!_currentInput.Contains("."))
            {
                _currentInput += ".";
                UpdateAmountDisplay();
            }
        }

        private void BtnBackspace_Click(object? sender, EventArgs e)
        {
            if (_currentInput.Length > 1)
            {
                _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            }
            else
            {
                _currentInput = "0";
            }

            UpdateAmountDisplay();
        }

        private void UpdateAmountDisplay()
        {
            if (decimal.TryParse(_currentInput, out decimal amount))
            {
                string formatted = amount % 1 == 0 ? amount.ToString("N0") : amount.ToString("N2");
                lbCurrency.Text = formatted + " đ";
            }
            else
            {
                lbCurrency.Text = "0 đ";
            }
        }

        #endregion

        #region Income/Expense Toggle

        private void BtnIncome_Click(object? sender, EventArgs e)
        {
            _isIncome = true;
            ToggleTransactionType(true);
        }

        private void BtnExpense_Click(object? sender, EventArgs e)
        {
            _isIncome = false;
            ToggleTransactionType(false);
        }

        private void ToggleTransactionType(bool isIncome)
        {
            _selectedCategory = "";
            _selectedIcon = IconChar.None;
            _selectedCategoryButton = null;

            if (isIncome)
            {
                btnIncome.BackColor = Color.FromArgb(5, 150, 105);
                btnIncome.Font = new Font(btnIncome.Font, FontStyle.Bold);
                btnExpense.BackColor = Color.FromArgb(200, 200, 200);
                btnExpense.Font = new Font(btnExpense.Font, FontStyle.Regular);

                btnCatRevenue.Visible = true;
                btnCatOtherIncome.Visible = true;
                btnCatCOGS.Visible = false;
                btnCatOpEx.Visible = false;
                btnCatOtherExpense.Visible = false;
            }
            else
            {
                btnExpense.BackColor = Color.FromArgb(225, 29, 72);
                btnExpense.Font = new Font(btnExpense.Font, FontStyle.Bold);
                btnIncome.BackColor = Color.FromArgb(200, 200, 200);
                btnIncome.Font = new Font(btnIncome.Font, FontStyle.Regular);

                btnCatRevenue.Visible = false;
                btnCatOtherIncome.Visible = false;
                btnCatCOGS.Visible = true;
                btnCatOpEx.Visible = true;
                btnCatOtherExpense.Visible = true;
            }

            ResetCategoryButtonStyles();
        }

        #endregion

        #region Category Selection

        private void CategoryButton_Click(IconButton btn, string categoryName, IconChar icon)
        {
            // Deselect previous category
            if (_selectedCategoryButton != null)
            {
                _selectedCategoryButton.BackColor = Color.White;
                _selectedCategoryButton.IconColor = Color.FromArgb(100, 100, 100);
            }

            // Select new category
            _selectedCategoryButton = btn;
            _selectedCategory = categoryName;
            _selectedIcon = icon;

            btn.BackColor = Color.FromArgb(226, 232, 240);
            btn.FlatAppearance.BorderColor = Color.FromArgb(37, 99, 235);
            btn.IconColor = Color.FromArgb(37, 99, 235);
        }

        private void ResetCategoryButtonStyles()
        {
            var allCategoryButtons = new[] { btnCatRevenue, btnCatOtherIncome, btnCatCOGS, btnCatOpEx, btnCatOtherExpense };

            foreach (var btn in allCategoryButtons)
            {
                btn.BackColor = Color.White;
                btn.IconColor = Color.FromArgb(100, 100, 100);
                btn.FlatAppearance.BorderSize = 0;
            }
        }

        #endregion

        #region Save & Cancel

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            Amount = decimal.Parse(_currentInput);
            Category = _selectedCategory;
            IsIncome = _isIncome;
            IsDebt = chkIsDebt.Checked;
            CustomerName = txtCustomerName.Text;
            Date = dtpDate.Value;
            Note = txtNote.Text;
            Status = chkIsDebt.Checked ? "Ghi nợ" : "Hoàn thành";
            Icon = _selectedIcon;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateInput()
        {
            if (!decimal.TryParse(_currentInput, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(_selectedCategory))
            {
                MessageBox.Show("Vui lòng chọn danh mục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (chkIsDebt.Checked && string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng khi ghi nợ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        #endregion
    }
}
