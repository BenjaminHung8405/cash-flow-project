using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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

        public void LoadRecentTransactions()
        {
            // Xóa sạch các control cũ trong danh sách trước khi load mới
            flowLayoutPanel.Controls.Clear();

            // 1. Tạo Mock Data sử dụng C# Tuple
            // Cấu trúc: (Tiêu đề, Loại, Ngày, Số tiền, Là khoản thu?, Icon)
            var mockData = new List<(string Title, string Type, DateTime Date, decimal Amount, bool IsIncome, FontAwesome.Sharp.IconChar Icon)>
            {
                ("Bán 5 máy khoan pin Makita", "Doanh thu bán hàng", DateTime.Now, 7500000, true, FontAwesome.Sharp.IconChar.Store),
                ("Nhập lô máy mài góc", "Nhập hàng hóa", DateTime.Now.AddDays(-1), 12000000, false, FontAwesome.Sharp.IconChar.Boxes),
                ("Quảng cáo Facebook tháng 3", "Marketing / Quảng cáo", DateTime.Now.AddDays(-2), 2000000, false, FontAwesome.Sharp.IconChar.Bullhorn),
                ("Gửi Viettel Post cho khách", "Vận chuyển / Logistics", DateTime.Now.AddDays(-2), 150000, false, FontAwesome.Sharp.IconChar.Truck),
                ("Thu nợ anh khách sỉ", "Thu hồi công nợ", DateTime.Now.AddDays(-3), 5000000, true, FontAwesome.Sharp.IconChar.HandHoldingUsd),
                ("Thanh toán tiền điện kho", "Hóa đơn", DateTime.Now.AddDays(-4), 850000, false, FontAwesome.Sharp.IconChar.FileInvoiceDollar),
                ("Ăn trưa cùng team", "Ăn uống / Tiếp khách", DateTime.Now.AddDays(-5), 350000, false, FontAwesome.Sharp.IconChar.Utensils)
            };

            // 2. Lặp qua danh sách mock data để tạo Card
            foreach (var data in mockData)
            {
                // Khởi tạo 1 Card mới
                TransactionCardItem card = new TransactionCardItem();

                // Xử lý chuỗi tiền tệ và màu sắc dựa vào việc nó là Thu hay Chi
                string formattedCurrency;
                System.Drawing.Color currencyColor;

                if (data.IsIncome)
                {
                    // Nếu là Thu: Thêm dấu +, format kiểu 1,000,000 đ, màu Xanh
                    formattedCurrency = "+ " + data.Amount.ToString("N0") + " đ";
                    currencyColor = System.Drawing.Color.MediumSeaGreen;
                }
                else
                {
                    // Nếu là Chi: Thêm dấu -, format kiểu 1,000,000 đ, màu Đỏ
                    formattedCurrency = "- " + data.Amount.ToString("N0") + " đ";
                    currencyColor = System.Drawing.Color.Crimson;
                }

                // Format ngày tháng (VD: 14/03/2026)
                string formattedDate = data.Date.ToString("dd/MM/yyyy");

                // Truyền dữ liệu vào Card thông qua hàm SetTransactionData bạn đã tạo
                card.SetTransactionData(
                    data.Title,
                    data.Type,
                    formattedDate,
                    formattedCurrency,
                    currencyColor,
                    data.Icon
                );

                // Cân chỉnh chiều rộng Card khớp với FlowLayoutPanel (trừ đi phần thanh cuộn)
                card.Width = flowLayoutPanel.Width - 25;

                // Thêm Card vào danh sách
                flowLayoutPanel.Controls.Add(card);
            }
        }

        public void LoadExpensePieChart()
        {
            // Clear existing controls from panel7
            panel7.Controls.Clear();

            // Create pie chart
            var pieChart = new LiveCharts.WinForms.PieChart
            {
                Dock = DockStyle.Fill
            };

            // Set legend position
            pieChart.LegendLocation = LegendLocation.Bottom;

            // Calculate total for percentage calculation
            decimal total = 1500000 + 2500000 + 3200000 + 800000 + 950000;

            // Create label point function to display currency and percentage
            Func<ChartPoint, string> labelPoint = chartPoint =>
            {
                double value = chartPoint.Y;
                var percentage = (value / (double)total) * 100;
                return $"{value:N0} đ ({percentage:F1}%)";
            };

            // Create series collection with mock expense data
            var seriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Ăn uống",
                    Values = new ChartValues<double> { 1500000 },
                    DataLabels = true,
                    LabelPoint = labelPoint,
                    Fill = new SolidColorBrush(MediaColor.FromRgb(59, 134, 134))  // Teal
                },
                new PieSeries
                {
                    Title = "Hóa đơn",
                    Values = new ChartValues<double> { 2500000 },
                    DataLabels = true,
                    LabelPoint = labelPoint,
                    Fill = new SolidColorBrush(MediaColor.FromRgb(230, 126, 34))  // Orange
                },
                new PieSeries
                {
                    Title = "Mua sắm",
                    Values = new ChartValues<double> { 3200000 },
                    DataLabels = true,
                    LabelPoint = labelPoint,
                    Fill = new SolidColorBrush(MediaColor.FromRgb(155, 89, 182))  // Purple
                },
                new PieSeries
                {
                    Title = "Đi lại",
                    Values = new ChartValues<double> { 800000 },
                    DataLabels = true,
                    LabelPoint = labelPoint,
                    Fill = new SolidColorBrush(MediaColor.FromRgb(52, 152, 219))  // Blue
                },
                new PieSeries
                {
                    Title = "Giải trí",
                    Values = new ChartValues<double> { 950000 },
                    DataLabels = true,
                    LabelPoint = labelPoint,
                    Fill = new SolidColorBrush(MediaColor.FromRgb(46, 204, 113))  // Green
                }
            };

            // Assign series to chart
            pieChart.Series = seriesCollection;

            // Add chart to panel7
            panel7.Controls.Add(pieChart);
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
