using System.Diagnostics;

namespace loc_perm_test;

public partial class MainPage : ContentPage
{
    public string UserName { get; set; }

    public MainPage()
	{
		InitializeComponent();
	}

	async void OnFinemeClicked(object sender, EventArgs e)
    {
		var permissions = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

		if (permissions == PermissionStatus.Granted)
        {
			await SetLocation();
        }
		else
        {
			await App.Current.MainPage.DisplayAlert("Permissions error", "You have not granted the app permission to access your location.", "OK");

			var requested = await MainThread.InvokeOnMainThreadAsync(async () =>
			{
                try
                {
					Debug.WriteLine("GETTING permissions");
                    var req = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
					Debug.WriteLine("GOT permissions");
					return req;
                }
                catch (Exception ex)
                {

                    throw;
                }
			});

			if (requested == PermissionStatus.Granted)
            {
				await SetLocation();
            }
			else
            {
				await App.Current.MainPage.DisplayAlert("Location Required", "Need your location", "OK");
            }
        }
    }

	async Task SetLocation()
    {
		UserName = UsernameEntry.Text;

		var locationRequest = new GeolocationRequest(GeolocationAccuracy.Best);

		var location = await Geolocation.GetLocationAsync(locationRequest);

		lblLocation.Text = $"{UserName} is located at {location.Latitude}~{location.Longitude}";

	}
}

