using aniliberty.Api;
using aniliberty.Api.Exceptions;
using aniliberty.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace aniliberty.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SignInPage : Page
{
    public SignInPage()
    {
        InitializeComponent();
    }

    private async void SignInButton_ClickAsync(object sender, RoutedEventArgs e)
    {
        Client api = new();

        try
        {
            var response = await api.SignIn(UsernameBox.Text, PasswordBox.Password);

            AppSettings.SetSession(response.Token);

            App.MainWindow.Navigate(typeof(AccountPage));
        }
        catch (ApiException ex)
        {
            ErrorMessage.Text = ex.Message;
        }
    }
}