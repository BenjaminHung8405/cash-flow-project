using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CofferBank
{
    public partial class TransactionControl : UserControl
    {
        private List<(string Title, string Type, DateTime Date, decimal Amount, bool IsIncome, FontAwesome.Sharp.IconChar Icon)> _allTransactions;

        public TransactionControl()
        {
            InitializeComponent();
            this.Load += TransactionControl_Load;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            cboType.SelectedIndexChanged += CboType_SelectedIndexChanged;
            dtpFromDate.ValueChanged += DtpFromDate_ValueChanged;
            dtpToDate.ValueChanged += DtpToDate_ValueChanged;
            addTransactionBtn.Click += AddTransactionBtn_Click;
        }

        private void TransactionControl_Load(object sender, EventArgs e)
        {
            InitializeMockData();
            PopulateTypeComboBox();
            SetDefaultDateRange();
            FilterAndLoadTransactions();
        }

        private void InitializeMockData()
        {
            _allTransactions = new List<(string, string, DateTime, decimal, bool, FontAwesome.Sharp.IconChar)>
            {
                ("Tiền lương tháng 1", "Thu nhập", DateTime.Now.AddDays(-5), 50000000, true, FontAwesome.Sharp.IconChar.MoneyBill),
                ("Hóa đơn điện nước", "Chi phí", DateTime.Now.AddDays(-10), 2500000, false, FontAwesome.Sharp.IconChar.Lightbulb),
                ("Bán hàng online", "Thu nhập", DateTime.Now.AddDays(-8), 15000000, true, FontAwesome.Sharp.IconChar.ShoppingCart),
                ("Thuê văn phòng", "Chi phí", DateTime.Now.AddDays(-15), 8000000, false, FontAwesome.Sharp.IconChar.Building),
                ("Chiết khấu khách hàng", "Thu nhập", DateTime.Now.AddDays(-3), 5000000, true, FontAwesome.Sharp.IconChar.Percent),
                ("Mua hàng tồn kho", "Chi phí", DateTime.Now.AddDays(-20), 25000000, false, FontAwesome.Sharp.IconChar.Boxes),
                ("Lãi suất ngân hàng", "Thu nhập", DateTime.Now.AddDays(-12), 500000, true, FontAwesome.Sharp.IconChar.PiggyBank),
                ("Lương nhân viên", "Chi phí", DateTime.Now.AddDays(-1), 35000000, false, FontAwesome.Sharp.IconChar.CreditCard),
                ("Doanh thu dịch vụ", "Thu nhập", DateTime.Now.AddDays(-7), 10000000, true, FontAwesome.Sharp.IconChar.Briefcase),
                ("Bảo trì và sửa chữa", "Chi phí", DateTime.Now.AddDays(-25), 3000000, false, FontAwesome.Sharp.IconChar.Wrench)
            };
        }

        private void PopulateTypeComboBox()
        {
            cboType.Items.Clear();
            cboType.Items.Add("Tất cả");
            cboType.Items.Add("Khoản Thu");
            cboType.Items.Add("Khoản Chi");
            cboType.SelectedIndex = 0;
        }

        private void SetDefaultDateRange()
        {
            dtpToDate.Value = DateTime.Now;
            dtpFromDate.Value = DateTime.Now.AddDays(-30);
        }

        private void FilterAndLoadTransactions()
        {
            flpTransactions.Controls.Clear();

            var searchText = txtSearch.Text.ToLower();
            var selectedType = cboType.SelectedItem?.ToString() ?? "Tất cả";
            var fromDate = dtpFromDate.Value.Date;
            var toDate = dtpToDate.Value.Date;

            var filteredTransactions = _allTransactions
                .Where(t =>
                {
                    // Search filter
                    if (!string.IsNullOrWhiteSpace(searchText) && !t.Title.ToLower().Contains(searchText))
                        return false;

                    // Type filter
                    if (selectedType == "Khoản Thu" && !t.IsIncome)
                        return false;
                    if (selectedType == "Khoản Chi" && t.IsIncome)
                        return false;

                    // Date range filter
                    if (t.Date.Date < fromDate || t.Date.Date > toDate)
                        return false;

                    return true;
                })
                .ToList();

            foreach (var transaction in filteredTransactions)
            {
                var card = new TransactionCardItem();

                var amountDisplay = transaction.IsIncome 
                    ? $"+{transaction.Amount:N0}" 
                    : $"-{transaction.Amount:N0}";

                var currencyColor = transaction.IsIncome 
                    ? Color.MediumSeaGreen 
                    : Color.Crimson;

                card.SetTransactionData(
                    transaction.Title,
                    transaction.Type,
                    transaction.Date.ToString("dd/MM/yyyy"),
                    amountDisplay,
                    currencyColor,
                    transaction.Icon
                );

                card.Width = flpTransactions.Width - 25;
                flpTransactions.Controls.Add(card);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterAndLoadTransactions();
        }

        private void CboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterAndLoadTransactions();
        }

        private void DtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            FilterAndLoadTransactions();
        }

        private void DtpToDate_ValueChanged(object sender, EventArgs e)
        {
            FilterAndLoadTransactions();
        }

        private void AddTransactionBtn_Click(object? sender, EventArgs e)
        {
            using (AddTransactionForm form = new AddTransactionForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var newTransaction = (
                        Title: form.CategoryName,
                        Type: form.CategoryName,
                        Date: form.TransactionDate,
                        Amount: form.Amount,
                        IsIncome: form.IsIncome,
                        Icon: form.CategoryIcon
                    );

                    _allTransactions.Add(newTransaction);
                    FilterAndLoadTransactions();
                }
            }
        }
    }
}
