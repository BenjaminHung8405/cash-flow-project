namespace CofferBank
{
    public partial class Form1 : Form
    {
        private DashboardControl? _dashboardControl;
        private FontAwesome.Sharp.IconButton? _selectedButton;
        private Color _selectedButtonColor = Color.FromArgb(37, 99, 235);
        private Color _unselectedButtonColor = Color.FromArgb(51, 65, 85);

        public Form1()
        {
            InitializeComponent();
            SelectButton(iconButton1);
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
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            SelectButton(iconButton1);
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            SelectButton(iconButton2);
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            SelectButton(iconButton3);
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            SelectButton(iconButton4);
        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
