namespace CofferBank
{
    public partial class Form1 : Form
    {
        private DashboardControl? _dashboardControl;
        private TransactionControl? _transactionControl;
        private FontAwesome.Sharp.IconButton? _selectedButton;
        private Color _selectedButtonColor = Color.FromArgb(37, 99, 235);
        private Color _unselectedButtonColor = Color.FromArgb(51, 65, 85);

        public Form1()
        {
            InitializeComponent();
            InitializeUserControls();
            SelectButton(dashboardBtn);
        }

        private void InitializeUserControls()
        {
            _dashboardControl = dashboardControl1;
            _transactionControl = new TransactionControl();
            _transactionControl.Dock = DockStyle.Top;
        }

        private void SelectButton(FontAwesome.Sharp.IconButton button)
        {
            if (_selectedButton != null)
            {
                _selectedButton.BackColor = Color.Transparent;
                _selectedButton.ForeColor = _unselectedButtonColor;
                _selectedButton.IconColor = _unselectedButtonColor;
            }

            _selectedButton = button;
            button.BackColor = Color.FromArgb(226, 232, 240);
            button.ForeColor = _selectedButtonColor;
            button.IconColor = _selectedButtonColor;

            // Switch user controls based on selected button
            panel2.Controls.Clear();
            if (button == dashboardBtn && _dashboardControl != null)
            {
                panel2.Controls.Add(_dashboardControl);
            }
            else if (button == transactionBtn && _transactionControl != null)
            {
                panel2.Controls.Add(_transactionControl);
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            SelectButton(dashboardBtn);
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            SelectButton(transactionBtn);
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            SelectButton(accountBtn);
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            SelectButton(settingBtn);
        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
