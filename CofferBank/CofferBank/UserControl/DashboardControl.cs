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
using LiveCharts.WinForms;
using System.Windows.Media;
using MediaColor = System.Windows.Media.Color;
using DrawingColor = System.Drawing.Color;
using FontAwesome.Sharp;

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
            // Account number display is already initialized in Designer
        }

        private string MaskAccountNumber(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber) || accountNumber.Length < 10)
                return accountNumber;

            string start = accountNumber.Substring(0, 4);
            string end = accountNumber.Substring(accountNumber.Length - 6);
            return $"{start}****{end}";
        }

        private void LbAccountNumber_Click(object? sender, EventArgs e)
        {
            // Account number toggle logic
        }

        public void SetAccountNumber(string accountNumber)
        {
            _fullAccountNumber = accountNumber;
        }

        // ============ BUSINESS LOGIC ============

        private class BusinessTransaction
        {
            public string Title { get; set; }
            public string Category { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public bool IsIncome { get; set; }
            public bool IsDebt { get; set; }
            public string CustomerName { get; set; }
            public string Status { get; set; }
            public IconChar Icon { get; set; }
        }

        private List<BusinessTransaction> GetBusinessMockData()
        {
            return new List<BusinessTransaction>
            {
                new BusinessTransaction
                {
                    Title = "Bán 5 máy khoan pin Makita",
                    Category = "Doanh thu bán hàng",
                    Date = DateTime.Now,
                    Amount = 7500000,
                    IsIncome = true,
                    IsDebt = false,
                    CustomerName = "Cửa hàng điện nước Minh",
                    Status = "Hoàn thành",
                    Icon = IconChar.Store
                },
                new BusinessTransaction
                {
                    Title = "Nhập lô máy mài góc",
                    Category = "Nhập hàng hóa",
                    Date = DateTime.Now.AddDays(-1),
                    Amount = 12000000,
                    IsIncome = false,
                    IsDebt = true,
                    CustomerName = "Công ty NK Tân Phát",
                    Status = "Ghi nợ",
                    Icon = IconChar.Boxes
                },
                new BusinessTransaction
                {
                    Title = "Quảng cáo Facebook tháng 3",
                    Category = "Marketing / Quảng cáo",
                    Date = DateTime.Now.AddDays(-2),
                    Amount = 2000000,
                    IsIncome = false,
                    IsDebt = false,
                    CustomerName = "Facebook Ads",
                    Status = "Hoàn thành",
                    Icon = IconChar.Bullhorn
                },
                new BusinessTransaction
                {
                    Title = "Gửi Viettel Post cho khách",
                    Category = "Vận chuyển / Logistics",
                    Date = DateTime.Now.AddDays(-2),
                    Amount = 150000,
                    IsIncome = false,
                    IsDebt = false,
                    CustomerName = "Viettel Post",
                    Status = "Hoàn thành",
                    Icon = IconChar.Truck
                },
                new BusinessTransaction
                {
                    Title = "Thu nợ anh khách sỉ",
                    Category = "Thu hồi công nợ",
                    Date = DateTime.Now.AddDays(-3),
                    Amount = 5000000,
                    IsIncome = true,
                    IsDebt = false,
                    CustomerName = "Anh Khách Sỉ",
                    Status = "Hoàn thành",
                    Icon = IconChar.HandHoldingUsd
                },
                new BusinessTransaction
                {
                    Title = "Thanh toán tiền điện kho",
                    Category = "Hóa đơn",
                    Date = DateTime.Now.AddDays(-4),
                    Amount = 850000,
                    IsIncome = false,
                    IsDebt = false,
                    CustomerName = "EVN",
                    Status = "Hoàn thành",
                    Icon = IconChar.FileInvoiceDollar
                },
                new BusinessTransaction
                {
                    Title = "Ăn trưa cùng team",
                    Category = "Ăn uống / Tiếp khách",
                    Date = DateTime.Now.AddDays(-5),
                    Amount = 350000,
                    IsIncome = false,
                    IsDebt = false,
                    CustomerName = "Nhà hàng Kim Long",
                    Status = "Hoàn thành",
                    Icon = IconChar.Utensils
                },
                new BusinessTransaction
                {
                    Title = "Mua văn phòng phẩm",
                    Category = "Mua sắm",
                    Date = DateTime.Now.AddDays(-6),
                    Amount = 500000,
                    IsIncome = false,
                    IsDebt = true,
                    CustomerName = "Công ty Tổng hợp Hà Nội",
                    Status = "Chờ duyệt",
                    Icon = IconChar.Pen
                },
                new BusinessTransaction
                {
                    Title = "Chi phí xăng xe",
                    Category = "Đi lại",
                    Date = DateTime.Now.AddDays(-7),
                    Amount = 300000,
                    IsIncome = false,
                    IsDebt = false,
                    CustomerName = "Cửa hàng xăng dầu Hà Nội",
                    Status = "Hoàn thành",
                    Icon = IconChar.GasPump
                },
                new BusinessTransaction
                {
                    Title = "Bảo trì thiết bị",
                    Category = "Dịch vụ",
                    Date = DateTime.Now.AddDays(-8),
                    Amount = 1500000,
                    IsIncome = false,
                    IsDebt = true,
                    CustomerName = "Công ty Bảo trì Techno",
                    Status = "Ghi nợ",
                    Icon = IconChar.Wrench
                }
            };
        }

        private void CalculateAndDisplayKPIs()
        {
            var mockData = GetBusinessMockData();

            var totalRevenue = mockData
                .Where(t => t.IsIncome)
                .Sum(t => t.Amount);

            var totalExpense = mockData
                .Where(t => !t.IsIncome)
                .Sum(t => t.Amount);

            var netProfit = totalRevenue - totalExpense;

            var totalDebt = mockData
                .Where(t => t.IsDebt)
                .Sum(t => t.Amount);

            lblTotalRevenue.Text = $"{totalRevenue:N0} đ";
            lblTotalExpense.Text = $"{totalExpense:N0} đ";
            lblTotalDebt.Text = $"{totalDebt:N0} đ";

            lblNetProfit.Text = $"{netProfit:N0} đ";
            lblNetProfit.ForeColor = netProfit < 0 ? DrawingColor.Crimson : DrawingColor.MediumSeaGreen;
        }

        private void LoadCashflowChart()
        {
            pnlChartContainer.Controls.Clear();
            var mockData = GetBusinessMockData();

            var cartesianChart = new LiveCharts.WinForms.CartesianChart
            {
                Dock = DockStyle.Fill
            };

            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Now.AddDays(-6 + i).Date)
                .ToList();

            var incomeByDay = new ChartValues<double>();
            var expenseByDay = new ChartValues<double>();

            foreach (var day in last7Days)
            {
                var dayIncome = mockData
                    .Where(t => t.IsIncome && t.Date.Date == day)
                    .Sum(t => t.Amount);
                var dayExpense = mockData
                    .Where(t => !t.IsIncome && t.Date.Date == day)
                    .Sum(t => t.Amount);

                incomeByDay.Add((double)dayIncome);
                expenseByDay.Add((double)dayExpense);
            }

            var labels = last7Days.Select(d => d.ToString("dd/MM")).ToList();

            var seriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Doanh thu",
                    Values = incomeByDay,
                    Fill = new SolidColorBrush(MediaColor.FromRgb(60, 180, 113))
                },
                new ColumnSeries
                {
                    Title = "Chi phí",
                    Values = expenseByDay,
                    Fill = new SolidColorBrush(MediaColor.FromRgb(220, 53, 69))
                }
            };

            cartesianChart.Series = seriesCollection;
            cartesianChart.AxisX.Add(new Axis
            {
                Labels = labels
            });

            pnlChartContainer.Controls.Add(cartesianChart);
        }

        private void LoadDebtAlerts()
        {
            flpDebtAlerts.Controls.Clear();
            var mockData = GetBusinessMockData();

            var debtTransactions = mockData
                .Where(t => t.IsDebt)
                .OrderByDescending(t => t.Date)
                .ToList();

            foreach (var transaction in debtTransactions)
            {
                var alertPanel = new Panel
                {
                    Height = 115,
                    Width = flpDebtAlerts.Width - 25,
                    BackColor = DrawingColor.WhiteSmoke,
                    Padding = new Padding(10, 8, 10, 8)
                };

                var customerLabel = new Label
                {
                    Text = transaction.CustomerName,
                    Font = new Font(alertPanel.Font, FontStyle.Bold),
                    Location = new Point(10, 8),
                    AutoSize = true,
                    ForeColor = DrawingColor.Black
                };

                var amountLabel = new Label
                {
                    Text = $"{transaction.Amount:N0} đ",
                    Font = new Font(alertPanel.Font.FontFamily, 11, FontStyle.Bold),
                    Location = new Point(10, 32),
                    AutoSize = true,
                    ForeColor = DrawingColor.Crimson
                };

                var dateLabel = new Label
                {
                    Text = $"Ngày: {transaction.Date:dd/MM/yyyy}",
                    Location = new Point(10, 56),
                    AutoSize = true,
                    ForeColor = DrawingColor.Gray,
                    Font = new Font(alertPanel.Font.FontFamily, 9)
                };

                var statusLabel = new Label
                {
                    Text = $"Trạng thái: {transaction.Status}",
                    Location = new Point(10, 82),
                    AutoSize = true,
                    ForeColor = DrawingColor.Gray,
                    Font = new Font(alertPanel.Font.FontFamily, 9)
                };

                alertPanel.Controls.Add(customerLabel);
                alertPanel.Controls.Add(amountLabel);
                alertPanel.Controls.Add(dateLabel);
                alertPanel.Controls.Add(statusLabel);

                flpDebtAlerts.Controls.Add(alertPanel);
            }
        }

        public void LoadRecentTransactions()
        {
            flpRecentTransactions.Controls.Clear();
            var mockData = GetBusinessMockData();

            var recentTransactions = mockData
                .OrderByDescending(t => t.Date)
                .Take(5)
                .ToList();

            foreach (var transaction in recentTransactions)
            {
                var card = new TransactionCardItem();

                var formattedCurrency = transaction.IsIncome
                    ? $"+ {transaction.Amount:N0} đ"
                    : $"- {transaction.Amount:N0} đ";

                var currencyColor = transaction.IsIncome
                    ? DrawingColor.MediumSeaGreen
                    : DrawingColor.Crimson;

                card.SetTransactionData(
                    transaction.Title,
                    transaction.Category,
                    transaction.Date.ToString("dd/MM/yyyy"),
                    formattedCurrency,
                    currencyColor,
                    transaction.Icon
                );

                card.Width = flpRecentTransactions.Width - 25;
                flpRecentTransactions.Controls.Add(card);
            }
        }

        private void DashboardControl_Load(object sender, EventArgs e)
        {
            CalculateAndDisplayKPIs();
            LoadCashflowChart();
            LoadDebtAlerts();
            LoadRecentTransactions();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }
    }
}
