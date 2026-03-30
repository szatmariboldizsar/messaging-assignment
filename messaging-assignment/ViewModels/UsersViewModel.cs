using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.DTOs;
using DAL.Models;
using DAL.Services;
using messaging_assignment.Pages;
using messaging_assignment.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace messaging_assignment.ViewModels
{
    public partial class UsersViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        private readonly UserConnectionService _userConnectionService;
        private readonly MessageService _messageService;

        public UsersViewModel(UserService userService, AuthService authService, UserConnectionService userConnectionService, MessageService messageService)
        {
            _userService = userService;
            _authService = authService;
            _userConnectionService = userConnectionService;
            _messageService = messageService;

            if (_authService.LoggedInUser == null)
            {
                throw new InvalidOperationException("User must be logged in to view Users.");
            }
            LoggedInUser = _authService.LoggedInUser;
            ResortUserCategories();
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsHidden))]
        public partial User LoggedInUser { get; set; }

        public bool IsHidden
        {
            get => LoggedInUser.IsHidden;
            set
            {
                LoggedInUser.IsHidden = value;
                Task.Run(async () => await _userService.UpdateUserAsync(LoggedInUser)).GetAwaiter().GetResult();
            }
        }

        public bool NoUsersLabelVisible { get => UsersWithLastMessage.Count == 0; }

        public bool FavoritesVisible { get => FavoritedUsersWithLastMessage.Count > 0; }

        public bool BlockedVisible { get => BlockedUsers.Count > 0; }

        [ObservableProperty]
        public partial ObservableCollection<UserWithMessage> UsersWithLastMessage { get; set; } = new ObservableCollection<UserWithMessage>();

        [ObservableProperty]
        public partial ObservableCollection<UserWithMessage> FavoritedUsersWithLastMessage { get; set; } = new ObservableCollection<UserWithMessage>();

        [ObservableProperty]
        public partial ObservableCollection<User> BlockedUsers { get; set; } = new ObservableCollection<User>();

        [ObservableProperty]
        public partial User? SelectedUser { get; set; }

        [RelayCommand]
        async Task MessageUser(User user)
        {
            await Shell.Current.GoToAsync($"{nameof(MessagePage)}?ToUserId={user.Id}&ToUserFullName={user.FullName}");
        }

        [RelayCommand]
        async Task FavoriteUser(User user)
        {
            await _userConnectionService.FavoriteUserForUser(LoggedInUser.Id, user.Id);
            ResortUserCategories();
        }

        [RelayCommand]
        async Task UnfavoriteUser(User user)
        {
            await _userConnectionService.UnfavoriteUserForUser(LoggedInUser.Id, user.Id);
            ResortUserCategories();
        }

        [RelayCommand]
        async Task BlockUser(User user)
        {
            await _userConnectionService.BlockUserForUser(LoggedInUser.Id, user.Id);
            ResortUserCategories();
        }

        [RelayCommand]
        async Task UnblockUser(User user)
        {
            await _userConnectionService.UnblockUserForUser(LoggedInUser.Id, user.Id);
            ResortUserCategories();
        }

        [RelayCommand]
        async Task LogOut()
        {
            _authService.LogoutUser();
            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
        }

        [RelayCommand]
        async Task ToggleRead(Message message)
        {
            message.IsSeen = !message.IsSeen;
            await _messageService.UpdateMessageAsync(message);
            ResortUserCategories();
        }

        {
            UsersWithLastMessage.Clear();
            FavoritedUsersWithLastMessage.Clear();
            BlockedUsers.Clear();
            List<UserWithMessage> usersWithLastMessage = Task.Run(async () => await _messageService.GetLastMessagesForUser(LoggedInUser.Id)).GetAwaiter().GetResult();
            List<UserConnection> userConnections = Task.Run(async () => await _userConnectionService.GetUserConnectionsForUser(LoggedInUser.Id)).GetAwaiter().GetResult();
            List<long> connectedUserIds = userConnections.Where(c => c.ConnectedUserId != LoggedInUser.Id).Select(c => c.ConnectedUserId).ToList();
            List<long> hiddenFromUserIds = userConnections.Where(c => c.ConnectedUserId == LoggedInUser.Id && c.IsBlocked).Select(c => c.UserId).Union(usersWithLastMessage.Where(u => u.User.IsHidden).Select(u => u.User.Id)).ToList();

            foreach (UserWithMessage userWithMessage in usersWithLastMessage)
            {
                if (hiddenFromUserIds.Contains(userWithMessage.User.Id))
                {
                    continue;
                }

                if (connectedUserIds.Contains(userWithMessage.User.Id))
                {
                    UserConnection userConnection = userConnections.First(uc => uc.ConnectedUserId == userWithMessage.User.Id);
                    if (userConnection.IsBlocked)
                    {
                        BlockedUsers.Add(userWithMessage.User);
                    }
                    else if (userConnection.IsFavorited)
                    {
                        FavoritedUsersWithLastMessage.Add(userWithMessage);
                    }
                    else
                    {
                        UsersWithLastMessage.Add(userWithMessage);
                    }
                }
                else
                {
                    UsersWithLastMessage.Add(userWithMessage);
                }
            }
            OnPropertyChanged(nameof(NoUsersLabelVisible));
            OnPropertyChanged(nameof(FavoritesVisible));
            OnPropertyChanged(nameof(BlockedVisible));
        }
    }
}
