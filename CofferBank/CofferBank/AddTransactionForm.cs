using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CofferBank
{
    public partial class AddTransactionForm : Form
    {
        private string _currentInput = "0";
        private bool _isIncome = false;
        private string _selectedCategoryName = string.Empty;
        private FontAwesome.Sharp.IconChar _selectedCategoryIcon;
        private FontAwesome.Sharp.IconButton? _selectedCategoryButton;

        public decimal Amount { get; private set; }
        public bool IsIncome { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string Note { get; private set; }
        public string CategoryName { get; private set; }
        public FontAwesome.Sharp.IconChar CategoryIcon { get; private set; }

        public AddTransactionForm()
        {
            InitializeComponent();
            InitializeEventHandlers();
            InitializeForm();
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

            // Action buttons
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void InitializeForm()
        {
            SetupModernUI();
            _isIncome = false;
            UpdateToggleButtonAppearance();
            LoadCategories(false);
            dtpDate.Value = DateTime.Now;
            dtpDate.Format = DateTimePickerFormat.Short;
            UpdateAmountLabel();
        }

        private void SetupModernUI()
        {
            // Style numpad buttons (0-9, dot)
            var numpadButtons = new[] { btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9, btnDot };

            foreach (var btn in numpadButtons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.White;
                btn.Cursor = Cursors.Hand;
                btn.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(220, 220, 220);
            }

            // Style note input textbox
            txtNote.BorderStyle = BorderStyle.None;
            txtNote.Font = new Font("Segoe UI", 11F);
        }

        #region Numpad Logic

        private void NumpadButton_Click(object? sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn == null) return;

            string digit = btn.Text;

            // Handle leading zeros
            if (_currentInput == "0" && digit != ".")
            {
                _currentInput = digit;
            }
            else
            {
                _currentInput += digit;
            }

            UpdateAmountLabel();
        }

        private void BtnDot_Click(object? sender, EventArgs e)
        {
            if (!_currentInput.Contains("."))
            {
                _currentInput += ".";
                UpdateAmountLabel();
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

            UpdateAmountLabel();
        }

        private void UpdateAmountLabel()
        {
            if (decimal.TryParse(_currentInput, out decimal amount))
            {
                string formatted = amount % 1 == 0 ? amount.ToString("N0") : amount.ToString("N2");
                lbCurrency.Text = formatted + " VNĐ";
            }
            else
            {
                lbCurrency.Text = "0 VNĐ";
            }
        }

        #endregion

        #region Income/Expense Toggle

        private void BtnIncome_Click(object? sender, EventArgs e)
        {
            _isIncome = true;
            UpdateToggleButtonAppearance();
            LoadCategories(true);
        }

        private void BtnExpense_Click(object? sender, EventArgs e)
        {
            _isIncome = false;
            UpdateToggleButtonAppearance();
            LoadCategories(false);
        }

        private void UpdateToggleButtonAppearance()
        {
            if (_isIncome)
            {
                btnIncome.BackColor = Color.FromArgb(5, 150, 105);
                btnIncome.Font = new Font(btnIncome.Font, FontStyle.Bold);
                btnExpense.BackColor = Color.FromArgb(200, 200, 200);
                btnExpense.Font = new Font(btnExpense.Font, FontStyle.Regular);
            }
            else
            {
                btnExpense.BackColor = Color.FromArgb(225, 29, 72);
                btnExpense.Font = new Font(btnExpense.Font, FontStyle.Bold);
                btnIncome.BackColor = Color.FromArgb(200, 200, 200);
                btnIncome.Font = new Font(btnIncome.Font, FontStyle.Regular);
            }
        }

        #endregion

        #region Categories

        private void LoadCategories(bool isIncome)
        {
            flpCategories.Controls.Clear();
            _selectedCategoryButton = null;
            _selectedCategoryName = string.Empty;

            List<(string Name, FontAwesome.Sharp.IconChar Icon)> categories;

            if (isIncome)
            {
                categories = new List<(string, FontAwesome.Sharp.IconChar)>
                {
                    ("Tiền lương", FontAwesome.Sharp.IconChar.MoneyBill),
                    ("Bán hàng", FontAwesome.Sharp.IconChar.Store),
                    ("Tiền gửi", FontAwesome.Sharp.IconChar.PiggyBank),
                    ("Hoàn tiền", FontAwesome.Sharp.IconChar.Percent),
                    ("Khác", FontAwesome.Sharp.IconChar.Briefcase)
                };
            }
            else
            {
                categories = new List<(string, FontAwesome.Sharp.IconChar)>
                {
                    ("Ăn uống", FontAwesome.Sharp.IconChar.Utensils),
                    ("Xăng xe", FontAwesome.Sharp.IconChar.GasPump),
                    ("Mua sắm", FontAwesome.Sharp.IconChar.ShoppingBag),
                    ("Giải trí", FontAwesome.Sharp.IconChar.Gamepad),
                    ("Nhà ở", FontAwesome.Sharp.IconChar.Home),
                    ("Y tế", FontAwesome.Sharp.IconChar.HeartPulse),
                    ("Giáo dục", FontAwesome.Sharp.IconChar.BookOpen),
                    ("Khác", FontAwesome.Sharp.IconChar.EllipsisH)
                };
            }

            foreach (var category in categories)
            {
                var btn = new FontAwesome.Sharp.IconButton
                {
                    IconChar = category.Icon,
                    IconColor = Color.FromArgb(100, 100, 100),
                    IconFont = FontAwesome.Sharp.IconFont.Auto,
                    IconSize = 32,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    Width = 140,
                    Height = 60,
                    Tag = category.Name,
                    Text = category.Name,
                    TextAlign = ContentAlignment.BottomCenter,
                    ImageAlign = ContentAlignment.TopCenter,
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = Color.FromArgb(51, 51, 51),
                    Margin = new Padding(4)
                };

                btn.FlatAppearance.BorderSize = 0;

                btn.Click += (s, e) => CategoryButton_Click(btn, category.Name, category.Icon);

                flpCategories.Controls.Add(btn);
            }
        }

        private void CategoryButton_Click(FontAwesome.Sharp.IconButton btn, string categoryName, FontAwesome.Sharp.IconChar icon)
        {
            // Deselect previous category
            if (_selectedCategoryButton != null)
            {
                _selectedCategoryButton.BackColor = Color.White;
                _selectedCategoryButton.IconColor = Color.FromArgb(100, 100, 100);
            }

            // Select new category
            _selectedCategoryButton = btn;
            _selectedCategoryName = categoryName;
            _selectedCategoryIcon = icon;
            btn.BackColor = Color.FromArgb(226, 232, 240);
            btn.FlatAppearance.BorderColor = Color.FromArgb(37, 99, 235);
            btn.IconColor = Color.FromArgb(37, 99, 235);
        }

        #endregion

        #region Save & Cancel

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            Amount = decimal.Parse(_currentInput);
            IsIncome = _isIncome;
            TransactionDate = dtpDate.Value;
            Note = txtNote.Text;
            CategoryName = _selectedCategoryName;
            CategoryIcon = _selectedCategoryIcon;

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

            if (string.IsNullOrWhiteSpace(_selectedCategoryName))
            {
                MessageBox.Show("Vui lòng chọn danh mục.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        #endregion
    }
}
