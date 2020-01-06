namespace Models.ViewModels.Identities
{
    /// <summary>
    ///     Register view model
    /// </summary>
    public class RegisterViewModel
    {
        public string PhoneNumber { get; set; }

        public string Fullname { get; set; }

        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string Email { get; set; }
    }
}