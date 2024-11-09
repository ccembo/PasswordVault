using PasswordVault.Mobile.ViewModel;

namespace PasswordVault.Mobile;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}