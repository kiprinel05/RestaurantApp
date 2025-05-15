using Restaurant.Models;
using System;

namespace Restaurant.Services
{
    public interface IUserService
    {
        User CurrentUser { get; }
        event EventHandler UserChanged;
        void SetCurrentUser(User user);
        void ClearCurrentUser();
        bool IsAuthenticated { get; }
    }

    public class UserService : IUserService
    {
        private User _currentUser;

        public User CurrentUser => _currentUser;

        public bool IsAuthenticated => _currentUser != null;

        public event EventHandler UserChanged;

        public void SetCurrentUser(User user)
        {
            _currentUser = user;
            OnUserChanged();
        }

        public void ClearCurrentUser()
        {
            _currentUser = null;
            OnUserChanged();
        }

        private void OnUserChanged()
        {
            UserChanged?.Invoke(this, EventArgs.Empty);
        }
    }
} 