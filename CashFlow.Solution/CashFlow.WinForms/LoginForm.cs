using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CashFlow.WinForms.Services;
using CashFlow.WinForms.Models;
using CashFlow.WinForms.Utils;

namespace CashFlow.WinForms
{
    public partial class LoginForm : Form
    {
        private readonly ApiClient _apiClient;

        public LoginForm()
        {
            InitializeComponent();
            _apiClient = new ApiClient(); // Sử dụng cấu hình từ appsettings.json
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text;
            string pass = txtPassword.Text;

            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Hiển thị loading state
                btnLogin.Enabled = false;
                btnLogin.Text = "Đang đăng nhập...";

                // Gọi API login endpoint
                var loginRequest = new { username = user, password = pass };
                var response = await _apiClient.PostAsync<LoginResponseModel>("auth/login", loginRequest);

                // Nếu đăng nhập thành công
                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    // Lưu token vào ApiClient
                    _apiClient.SetAuthToken(response.Token);

                    // Lưu user info vào session/config (có thể dùng static class hoặc settings)
                    CurrentUser.Token = response.Token;
                    CurrentUser.UserId = response.UserId;
                    CurrentUser.FullName = response.FullName;
                    CurrentUser.Role = response.Role;
                    CurrentUser.TenantId = response.TenantId;

                    MessageBox.Show($"Đăng nhập thành công! Chào mừng {response.FullName}", "Chúc mừng",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    MainForm main = new MainForm();
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}\n\nVui lòng kiểm tra kết nối API.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            AcceptButton = btnLogin;
        }
    }
}
