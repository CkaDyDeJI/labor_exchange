namespace labor_exchange
{
    static class Information
    {
        public static string connString { get; set; } = Properties.Settings.Default["labor_exchangeConnectionString"].ToString();

        public static string currentUser { get; set; } = "admin";
    }
}
