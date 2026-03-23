namespace CashFlow.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeMenuItems();
        }

        private void InitializeMenuItems()
        {
            cbMenu.Items.Add("Tổng quan");
            cbMenu.Items.Add("Phiếu Thu");
            cbMenu.Items.Add("Phiếu Chi");
            cbMenu.Items.Add("Báo cáo");
            cbMenu.SelectedIndexChanged += CbMenu_SelectedIndexChanged;

            // Set default selected item to "Tổng quan" (index 0)
            cbMenu.SelectedIndex = 0;
        }

        private void CbMenu_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbMenu.SelectedItem != null)
            {
                string selectedItem = cbMenu.SelectedItem.ToString() ?? string.Empty;
                switch (selectedItem)
                {
                    case "Tổng quan":
                        ShowDashboard();
                        break;
                    case "Phiếu Thu":
                        // TODO: Implement Phiếu Thu functionality
                        break;
                    case "Phiếu Chi":
                        // TODO: Implement Phiếu Chi functionality
                        break;
                    case "Báo cáo":
                        // TODO: Implement Báo cáo functionality
                        break;
                }
            }
        }

        private void ShowDashboard()
        {
            panel4.Controls.Clear();
            DashboardControl dashboard = new DashboardControl();
            dashboard.Dock = DockStyle.Fill;
            panel4.Controls.Add(dashboard);
        }
    }
}
