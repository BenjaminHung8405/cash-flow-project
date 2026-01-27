using CashFlow.WinForms.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CashFlow.WinForms.Forms
{
    /// <summary>
    /// Ví dụ Form Quản Lý Quỹ sử dụng API
    /// </summary>
    public partial class FundsManagementForm : Form
    {
        private ApiClient _apiClient;
        private List<ApiClient.FundDto> _fundsList;

        public FundsManagementForm()
        {
            InitializeComponent();
            _apiClient = new ApiClient("https://localhost:5001");
        }

        /// <summary>
        /// Form Load - Khởi tạo và load dữ liệu
        /// </summary>
        private async void FundsManagementForm_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Lấy token từ login form (hoặc settings)
                var token = GetStoredJwtToken();  // Lấy từ settings
                
                if (string.IsNullOrEmpty(token))
                {
                    MessageBox.Show("Please login first");
                    return;
                }

                // 2. Set token vào ApiClient
                _apiClient.SetJwtToken(token);

                // 3. Load dữ liệu
                await LoadFundsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading funds: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Lấy dữ liệu từ API
        /// </summary>
        private async System.Threading.Tasks.Task LoadFundsAsync()
        {
            try
            {
                // Hiển thị loading indicator
                StatusLabel.Text = "Loading...";
                this.Cursor = Cursors.WaitCursor;

                // Gọi API GET /api/funds
                _fundsList = await _apiClient.GetFundsAsync();

                // Bind dữ liệu vào DataGridView
                FundsDataGridView.DataSource = _fundsList;

                // Format columns
                if (FundsDataGridView.Columns.Count > 0)
                {
                    FundsDataGridView.Columns["Id"].Width = 80;
                    FundsDataGridView.Columns["FundName"].Width = 150;
                    FundsDataGridView.Columns["FundType"].Width = 80;
                    FundsDataGridView.Columns["CurrentBalance"].Width = 120;
                    FundsDataGridView.Columns["CreatedAt"].Width = 150;
                    FundsDataGridView.Columns["TenantId"].Visible = false;  // Ẩn
                }

                StatusLabel.Text = $"Loaded {_fundsList.Count} funds";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load funds: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error loading funds";
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Click nút "Add Fund" - Tạo quỹ mới
        /// </summary>
        private async void AddFundButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validate input
                if (string.IsNullOrWhiteSpace(FundNameTextBox.Text))
                {
                    MessageBox.Show("Please enter fund name", "Validation Error");
                    return;
                }

                if (!decimal.TryParse(InitialBalanceTextBox.Text, out var balance))
                {
                    MessageBox.Show("Invalid initial balance", "Validation Error");
                    return;
                }

                // 2. Tạo request object
                var request = new ApiClient.CreateFundRequest
                {
                    FundName = FundNameTextBox.Text,
                    FundType = FundTypeComboBox.SelectedValue?.ToString() ?? "CASH",
                    AccountNumber = AccountNumberTextBox.Text,
                    InitialBalance = balance
                };

                // 3. Gọi API POST /api/funds
                StatusLabel.Text = "Creating fund...";
                this.Cursor = Cursors.WaitCursor;

                var newFund = await _apiClient.CreateFundAsync(request);

                MessageBox.Show($"Fund created successfully: {newFund.FundName}", "Success");

                // 4. Clear input fields
                ClearInputFields();

                // 5. Reload danh sách
                await LoadFundsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating fund: {ex.Message}", "Error");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Click nút "Edit Fund" - Cập nhật quỹ
        /// </summary>
        private async void EditFundButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra có chọn dòng nào không
                if (FundsDataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a fund to edit", "Info");
                    return;
                }

                // 2. Lấy fund được chọn
                var selectedRow = FundsDataGridView.SelectedRows[0];
                var fundId = selectedRow.Cells["Id"].Value.ToString();
                var fund = selectedRow.DataBoundItem as ApiClient.FundDto;

                // 3. Load data vào text boxes
                FundNameTextBox.Text = fund.FundName;
                FundTypeComboBox.SelectedValue = fund.FundType;
                AccountNumberTextBox.Text = fund.AccountNumber;
                InitialBalanceTextBox.Text = fund.CurrentBalance.ToString();

                // 4. Tạo request cập nhật
                var request = new ApiClient.UpdateFundRequest
                {
                    FundName = FundNameTextBox.Text,
                    FundType = FundTypeComboBox.SelectedValue?.ToString(),
                    AccountNumber = AccountNumberTextBox.Text
                };

                // 5. Gọi API PUT /api/funds/{id}
                StatusLabel.Text = "Updating fund...";
                this.Cursor = Cursors.WaitCursor;

                var updatedFund = await _apiClient.UpdateFundAsync(fundId, request);

                MessageBox.Show($"Fund updated successfully", "Success");

                // 6. Clear input fields
                ClearInputFields();

                // 7. Reload danh sách
                await LoadFundsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating fund: {ex.Message}", "Error");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Click nút "Delete Fund" - Xóa quỹ
        /// </summary>
        private async void DeleteFundButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra có chọn dòng nào không
                if (FundsDataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a fund to delete", "Info");
                    return;
                }

                // 2. Confirm xóa
                var result = MessageBox.Show(
                    "Are you sure you want to delete this fund?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes)
                    return;

                // 3. Lấy fund ID
                var selectedRow = FundsDataGridView.SelectedRows[0];
                var fundId = selectedRow.Cells["Id"].Value.ToString();

                // 4. Gọi API DELETE /api/funds/{id}
                StatusLabel.Text = "Deleting fund...";
                this.Cursor = Cursors.WaitCursor;

                await _apiClient.DeleteFundAsync(fundId);

                MessageBox.Show("Fund deleted successfully", "Success");

                // 5. Reload danh sách
                await LoadFundsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting fund: {ex.Message}", "Error");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Click nút "View Audit Trail" - Xem lịch sử thay đổi
        /// </summary>
        private async void ViewAuditTrailButton_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra có chọn dòng nào không
                if (FundsDataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a fund", "Info");
                    return;
                }

                // 2. Lấy fund ID
                var selectedRow = FundsDataGridView.SelectedRows[0];
                var fundId = selectedRow.Cells["Id"].Value.ToString();

                // 3. Gọi API GET /api/funds/{id}/audit-trail
                StatusLabel.Text = "Loading audit trail...";
                this.Cursor = Cursors.WaitCursor;

                var auditLogs = await _apiClient.GetFundAuditTrailAsync(fundId);

                // 4. Hiển thị trong MessageBox hoặc form mới
                var auditText = "Audit Trail:\n\n";
                foreach (var log in auditLogs)
                {
                    auditText += $"Action: {log.Action}\n";
                    auditText += $"User: {log.UserId}\n";
                    auditText += $"Time: {log.Timestamp}\n";
                    auditText += $"Old Values: {log.OldValues}\n";
                    auditText += $"New Values: {log.NewValues}\n";
                    auditText += "---\n\n";
                }

                MessageBox.Show(auditText, "Audit Trail", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading audit trail: {ex.Message}", "Error");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Clear input fields
        /// </summary>
        private void ClearInputFields()
        {
            FundNameTextBox.Clear();
            FundTypeComboBox.SelectedIndex = 0;
            AccountNumberTextBox.Clear();
            InitialBalanceTextBox.Clear();
        }

        /// <summary>
        /// Lấy JWT Token được lưu từ login
        /// </summary>
        private string GetStoredJwtToken()
        {
            // TODO: Lấy từ:
            // 1. Properties.Settings.Default["JwtToken"]
            // 2. File cấu hình
            // 3. Secure storage
            
            return Properties.Settings.Default["JwtToken"]?.ToString() ?? "";
        }

        // Designer-generated code sẽ thêm các controls:
        // - FundsDataGridView (hiển thị danh sách)
        // - FundNameTextBox (tên quỹ)
        // - FundTypeComboBox (loại quỹ)
        // - AccountNumberTextBox (số tài khoản)
        // - InitialBalanceTextBox (số dư ban đầu)
        // - AddFundButton, EditFundButton, DeleteFundButton
        // - ViewAuditTrailButton
        // - StatusLabel
    }
}
