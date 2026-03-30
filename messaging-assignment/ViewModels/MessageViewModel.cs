using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    [QueryProperty("ToUserId", "ToUserId")]
    [QueryProperty("ToUserFullName", "ToUserFullName")]
    public partial class MessageViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        private readonly MessageService _messageService;

        public MessageViewModel(UserService userService, AuthService authService, MessageService messageService)
        {
            _userService = userService;
            _authService = authService;
            _messageService = messageService;
            if (_authService.LoggedInUser == null)
            {
                throw new InvalidOperationException("User must be logged in to view messages.");
            }
            LoggedInUser = _authService.LoggedInUser;
        }

        [ObservableProperty]
        public partial User LoggedInUser { get; set; }

        [ObservableProperty]
        public partial long ToUserId { get; set; }

        [ObservableProperty]
        public partial string ToUserFullName { get; set; }

        [ObservableProperty]
        public partial string NewMessageContent { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();

        public bool SendActive { get => LoggedInUser.MessagesSentToday < 5 || LoggedInUser.LastMessageSent.GetValueOrDefault().Day != DateTime.Now.Day; } 

        partial void OnToUserIdChanged(long value)
        {
            LoadForUserAsync(value);
        }

        private async Task LoadForUserAsync(long id)
        {
            foreach (Message message in await _messageService.GetMessagesForUsersAsync(LoggedInUser.Id, ToUserId))
            {
                Messages.Add(message);
                if (message.ToUserId == LoggedInUser.Id)
                {
                    message.IsSeen = true;
                    await _messageService.UpdateMessageAsync(message);
                }
            }
        }


        [RelayCommand]
        async Task SendMessage()
        {
            DateTime now = DateTime.Now;
            Message message = new Message()
            {
                FromUserId = LoggedInUser.Id,
                ToUserId = ToUserId,
                Content = NewMessageContent,
                DateSent = now
            };

            if (await _messageService.CreateMessageAsync(message))
            {
                Messages.Add(message);
                LoggedInUser.LastMessageSent = now;

                LoggedInUser.MessagesSentToday = LoggedInUser.MessagesSentToday >= 5 ? (short)1 : (short)(LoggedInUser.MessagesSentToday + 1);
                await _userService.UpdateUserAsync(LoggedInUser);
                OnPropertyChanged(nameof(SendActive));
                NewMessageContent = "";
            }
        }
    }
}
