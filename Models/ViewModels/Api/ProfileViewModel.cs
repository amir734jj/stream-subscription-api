using Models.Models;

namespace Models.ViewModels
{
    public class ProfileViewModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Description { get; set; }

        public User User { get; set; }

        public ProfileViewModel()
        {
        }

        public ProfileViewModel(User user) : this()
        {
            if (user == null) return;

            Name = user.Name;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            User = user;
        }
    }
}