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
                // Income Transactions - Doanh thu bán hàng
                ("Bán 5 máy khoan pin Makita", "Doanh thu bán hàng", DateTime.Now.AddDays(-25), 7500000, true, FontAwesome.Sharp.IconChar.Store),
                ("Bán máy cắt cỏ chính hãng", "Doanh thu bán hàng", DateTime.Now.AddDays(-20), 15000000, true, FontAwesome.Sharp.IconChar.Store),
                ("Doanh thu bán lẻ tuần trước", "Doanh thu bán hàng", DateTime.Now.AddDays(-15), 5000000, true, FontAwesome.Sharp.IconChar.Store),
                ("Thu nợ khách hàng cũ", "Doanh thu bán hàng", DateTime.Now.AddDays(-12), 10000000, true, FontAwesome.Sharp.IconChar.Store),
                ("Bán buôn linh kiện điện tử", "Doanh thu bán hàng", DateTime.Now.AddDays(-8), 12500000, true, FontAwesome.Sharp.IconChar.Store),
                ("Doanh thu từ dịch vụ sửa chữa", "Doanh thu bán hàng", DateTime.Now.AddDays(-5), 8000000, true, FontAwesome.Sharp.IconChar.Store),
                ("Bán máy khoan hợp đồng công ty", "Doanh thu bán hàng", DateTime.Now.AddDays(-3), 22000000, true, FontAwesome.Sharp.IconChar.Store),
                ("Hoàn tiền lỗi tính khách hàng", "Khác", DateTime.Now.AddDays(-18), 500000, true, FontAwesome.Sharp.IconChar.Briefcase),

                // Expense Transactions - Nhập hàng hóa
                ("Nhập lô máy mài góc 100 chiếc", "Nhập hàng hóa", DateTime.Now.AddDays(-28), 24000000, false, FontAwesome.Sharp.IconChar.Boxes),
                ("Nhập linh kiện thay thế từ nhà cung cấp", "Nhập hàng hóa", DateTime.Now.AddDays(-22), 8000000, false, FontAwesome.Sharp.IconChar.Boxes),
                ("Mua hàng tồn kho máy khoan pin", "Nhập hàng hóa", DateTime.Now.AddDays(-16), 15000000, false, FontAwesome.Sharp.IconChar.Boxes),
                ("Nhập máy cắt cỏ từ NCC", "Nhập hàng hóa", DateTime.Now.AddDays(-10), 18000000, false, FontAwesome.Sharp.IconChar.Boxes),

                // Expense Transactions - Chi phí hoạt động
                ("Chi phí quảng cáo Facebook tháng 3", "Chi phí hoạt động", DateTime.Now.AddDays(-27), 2000000, false, FontAwesome.Sharp.IconChar.Bullhorn),
                ("Chi phí vận chuyển hàng cho khách", "Chi phí hoạt động", DateTime.Now.AddDays(-14), 1500000, false, FontAwesome.Sharp.IconChar.Truck),
                ("Thanh toán tiền điện kho bãi", "Chi phí hoạt động", DateTime.Now.AddDays(-9), 850000, false, FontAwesome.Sharp.IconChar.Lightbulb),
                ("Sửa chữa thiết bị máy khoan", "Chi phí hoạt động", DateTime.Now.AddDays(-6), 3000000, false, FontAwesome.Sharp.IconChar.Wrench),
                ("Chi phí phí bảo hiểm kinh doanh", "Chi phí hoạt động", DateTime.Now.AddDays(-2), 5000000, false, FontAwesome.Sharp.IconChar.FileInvoiceDollar),
                ("Chi phí vệ sinh và bảo trì văn phòng", "Chi phí hoạt động", DateTime.Now.AddDays(-1), 1200000, false, FontAwesome.Sharp.IconChar.Broom)
            };
        }

        private void PopulateTypeComboBox()
        {
            cboType.Items.Clear();
            cboType.Items.Add("Tất cả");
            cboType.Items.Add("Doanh thu bán hàng");
            cboType.Items.Add("Nhập hàng hóa");
            cboType.Items.Add("Chi phí hoạt động");
            cboType.Items.Add("Khác");
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
                    if (selectedType != "Tất cả" && t.Type != selectedType)
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
                        Title: form.Category,
                        Type: form.Category,
                        Date: form.Date,
                        Amount: form.Amount,
                        IsIncome: form.IsIncome,
                        Icon: form.Icon
                    );

                    _allTransactions.Add(newTransaction);
                    FilterAndLoadTransactions();
                }
            }
        }
    }
}
