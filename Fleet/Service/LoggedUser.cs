using Fleet.Interfaces.Service;

namespace Fleet.Service
{
    public class LoggedUser : ILoggedUser
    {
        public int UserId { get; set; }
    }
}
