using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Models;
using DAL.Services;
using messaging_assignment.Pages;
using messaging_assignment.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace messaging_assignment.ViewModels
{
    public partial class RegisterViewModel : ObservableValidator
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;

        public RegisterViewModel(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [ObservableProperty]
        [Required(ErrorMessage = "Username is required")]
        [CustomValidation(typeof(RegisterViewModel), nameof(ValidateUsername))]
        public partial string Username { get; set; }

        [ObservableProperty]
        [Required(ErrorMessage = "Full Name is required")]
        public partial string FullName { get; set; }

        [ObservableProperty]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
        ErrorMessage = "Password must be at least 6 characters with at least one lowercase letter, one uppercase letter, and one number")]
        public partial string Password { get; set; }

        public string? UsernameError => GetErrors(nameof(Username))?.Cast<ValidationResult>().FirstOrDefault()?.ErrorMessage;

        public bool HasUsernameError => GetErrors(nameof(Username))?.Cast<object>().Any() == true;

        public string? PasswordError => GetErrors(nameof(Password))?.Cast<ValidationResult>().FirstOrDefault()?.ErrorMessage;
        public bool HasPasswordError => GetErrors(nameof(Password))?.Cast<object>().Any() == true;

        public string? FullNameError => GetErrors(nameof(FullName))?.Cast<ValidationResult>().FirstOrDefault()?.ErrorMessage;
        public bool HasFullNameError => GetErrors(nameof(FullName))?.Cast<object>().Any() == true;

        [RelayCommand]
        async Task Register()
        {
            ValidateAllProperties();

            OnPropertyChanged(nameof(UsernameError));
            OnPropertyChanged(nameof(HasUsernameError));

            OnPropertyChanged(nameof(PasswordError));
            OnPropertyChanged(nameof(HasPasswordError));

            OnPropertyChanged(nameof(FullNameError));
            OnPropertyChanged(nameof(HasFullNameError));

            if (!HasErrors)
            {
                User newUser = new User
                {
                    Username = Username,
                    FullName = FullName,
                    PasswordHash = PasswordHasher.Hash(Password)
                };

                await _userService.CreateUserAsync(newUser);

                _authService.LoginUser(newUser);

                await Shell.Current.GoToAsync(nameof(UsersPage));
            }
        }

        public static ValidationResult ValidateUsername(string username, ValidationContext context)
        {
            RegisterViewModel instance = (RegisterViewModel)context.ObjectInstance;

            // Validation pipeline doesn't support async, so we have to do this synchronously.
            if (!instance._userService.IsUsernameUnique(username))
            {
                return new("Username already exists");
            }

            return ValidationResult.Success;
        }
    }
}
