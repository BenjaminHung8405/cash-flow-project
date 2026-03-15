using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using MediaColor = System.Windows.Media.Color;

namespace CofferBank
{
    public partial class DashboardControl : UserControl
    {
        private string _fullAccountNumber = "GB77BARC20038429646868";
        private bool _isAccountNumberHidden = true;

        public DashboardControl()
        {
            InitializeComponent();
            InitializeAccountNumberDisplay();
        }

        private void InitializeAccountNumberDisplay()
        {
            // Set initial hidden state
            lbAccountNumber.Text = MaskAccountNumber(_fullAccountNumber);
            lbAccountNumber.Cursor = Cursors.Hand;
            lbAccountNumber.Click += LbAccountNumber_Click;
        }

        private string MaskAccountNumber(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber) || accountNumber.Length < 10)
                return accountNumber;

            // Hiển thị 4 ký tự đầu và 6 ký tự cuối, ẩn giữa
            string start = accountNumber.Substring(0, 4);
            string end = accountNumber.Substring(accountNumber.Length - 6);
            return $"{start}****{end}";
        }

        private void LbAccountNumber_Click(object? sender, EventArgs e)
        {
            _isAccountNumberHidden = !_isAccountNumberHidden;
            lbAccountNumber.Text = _isAccountNumberHidden
                ? MaskAccountNumber(_fullAccountNumber)
                : _fullAccountNumber;
        }

        public void SetAccountNumber(string accountNumber)
        {
            _fullAccountNumber = accountNumber;
            lbAccountNumber.Text = _isAccountNumberHidden
                ? MaskAccountNumber(_fullAccountNumber)
                : _fullAccountNumber;
        }

        private List<(string Title, string Type, DateTime Date, decimal Amount, bool IsIncome, FontAwesome.Sharp.IconChar Icon)> GetSharedMockData()
        {
            return new List<(string Title, string Type, DateTime Date, decimal Amount, bool IsIncome, FontAwesome.Sharp.IconChar Icon)>
            {
                ("Bán 5 máy khoan pin Makita", "Doanh thu bán hàng", DateTime.Now, 7500000, true, FontAwesome.Sharp.IconChar.Store),
                ("Nhập lô máy mài góc", "Nhập hàng hóa", DateTime.Now.AddDays(-1), 12000000, false, FontAwesome.Sharp.IconChar.Boxes),
                ("Quảng cáo Facebook tháng 3", "Marketing / Quảng cáo", DateTime.Now.AddDays(-2), 2000000, false, FontAwesome.Sharp.IconChar.Bullhorn),
                ("Gửi Viettel Post cho khách", "Vận chuyển / Logistics", DateTime.Now.AddDays(-2), 150000, false, FontAwesome.Sharp.IconChar.Truck),
                ("Thu nợ anh khách sỉ", "Thu hồi công nợ", DateTime.Now.AddDays(-3), 5000000, true, FontAwesome.Sharp.IconChar.HandHoldingUsd),
                ("Thanh toán tiền điện kho", "Hóa đơn", DateTime.Now.AddDays(-4), 850000, false, FontAwesome.Sharp.IconChar.FileInvoiceDollar),
                ("Ăn trưa cùng team", "Ăn uống / Tiếp khách", DateTime.Now.AddDays(-5), 350000, false, FontAwesome.Sharp.IconChar.Utensils),
                ("Mua văn phòng phẩm", "Mua sắm", DateTime.Now.AddDays(-6), 500000, false, FontAwesome.Sharp.IconChar.Pen),
                ("Chi phí xăng xe", "Đi lại", DateTime.Now.AddDays(-7), 300000, false, FontAwesome.Sharp.IconChar.GasPump),
                ("Bảo trì thiết bị", "Dịch vụ", DateTime.Now.AddDays(-8), 1500000, false, FontAwesome.Sharp.IconChar.Wrench)
            };
        }

        public void LoadRecentTransactions()
        {
            flowLayoutPanel.Controls.Clear();
            var mockData = GetSharedMockData();

            foreach (var data in mockData)
            {
                TransactionCardItem card = new TransactionCardItem();

                string formattedCurrency;
                System.Drawing.Color currencyColor;

                if (data.IsIncome)
                {
                    formattedCurrency = "+ " + data.Amount.ToString("N0") + " đ";
                    currencyColor = System.Drawing.Color.MediumSeaGreen;
                }
                else
                {
                    formattedCurrency = "- " + data.Amount.ToString("N0") + " đ";
                    currencyColor = System.Drawing.Color.Crimson;
                }

                string formattedDate = data.Date.ToString("dd/MM/yyyy");

                card.SetTransactionData(
                    data.Title,
                    data.Type,
                    formattedDate,
                    formattedCurrency,
                    currencyColor,
                    data.Icon
                );

                card.Width = flowLayoutPanel.Width - 25;
                flowLayoutPanel.Controls.Add(card);
            }
        }

        public void LoadExpensePieChart()
        {
            panelChart.Controls.Clear();
            var mockData = GetSharedMockData();

            var pieChart = new LiveCharts.WinForms.PieChart
            {
                Dock = DockStyle.Fill,
                LegendLocation = LegendLocation.Bottom
            };

            pieChart.InnerRadius = 50;

            // Filter expenses and group by Type
            var expenseGroups = mockData
                .Where(t => !t.IsIncome)
                .GroupBy(t => t.Type)
                .Select(g => new { Type = g.Key, TotalAmount = g.Sum(t => t.Amount) })
                .ToList();

            decimal totalFilteredExpenses = expenseGroups.Sum(g => g.TotalAmount);
            if (totalFilteredExpenses == 0) return;

            Func<ChartPoint, string> labelPoint = chartPoint =>
            {
                double value = chartPoint.Y;
                var percentage = (value / (double)totalFilteredExpenses) * 100;
                return $"{value:N0} đ ({percentage:F1}%)";
            };

            var modernColors = new[]
            {
                MediaColor.FromRgb(59, 134, 134),   // Teal
                MediaColor.FromRgb(230, 126, 34),   // Orange
                MediaColor.FromRgb(155, 89, 182),   // Purple
                MediaColor.FromRgb(52, 152, 219),   // Blue
                MediaColor.FromRgb(46, 204, 113),   // Green
                MediaColor.FromRgb(241, 196, 15),   // Yellow
                MediaColor.FromRgb(231, 76, 60),    // Red
                MediaColor.FromRgb(52, 73, 94)      // Dark Blue
            };

            var seriesCollection = new SeriesCollection();

            for (int i = 0; i < expenseGroups.Count; i++)
            {
                var group = expenseGroups[i];
                seriesCollection.Add(new PieSeries
                {
                    Title = group.Type,
                    Values = new ChartValues<double> { (double)group.TotalAmount },
                    DataLabels = true,
                    LabelPoint = labelPoint,
                    Fill = new SolidColorBrush(modernColors[i % modernColors.Length])
                });
            }

            pieChart.Series = seriesCollection;
            panelChart.Controls.Add(pieChart);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DashboardControl_Load(object sender, EventArgs e)
        {
            LoadRecentTransactions();
            LoadExpensePieChart();
        }
    }
}
