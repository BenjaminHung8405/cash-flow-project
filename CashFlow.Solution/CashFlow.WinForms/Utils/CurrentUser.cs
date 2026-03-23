namespace CashFlow.WinForms.Utils
{
    /// <summary>
    /// Lớp static để lưu thông tin user hiện tại trong session
    /// </summary>
    public static class CurrentUser
    {
        public static string? Token { get; set; }
        public static int UserId { get; set; }
        public static int TenantId { get; set; }
        public static string? FullName { get; set; }
        public static string? Role { get; set; }

        public static bool IsLoggedIn => !string.IsNullOrEmpty(Token);

        /// <summary>
        /// Xóa toàn bộ thông tin user khi đăng xuất
        /// </summary>
        public static void Clear()
        {
            Token = null;
            UserId = 0;
            TenantId = 0;
            FullName = null;
            Role = null;
        }
    }
}
