using messaging_assignment.ViewModels;

namespace messaging_assignment.Pages;

public partial class UsersPage : ContentPage
{
	public UsersPage(UsersViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UsersViewModel vm)
        {
            Task.Run(() => vm.ResortUserCategories());
        }
    }
}