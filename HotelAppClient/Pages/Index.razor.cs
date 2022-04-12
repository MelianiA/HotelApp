using Blazored.LocalStorage;
using Common;
using HotelAppClient.Helper;
using HotelAppClient.Model.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HotelAppClient.Pages
{
    public class IndexBase : ComponentBase
    {
        public HomeVM HomeModel { get; set; } = new HomeVM();
        [Inject] ILocalStorageService localStorage { get; set; }
        [Inject]  IJSRuntime jSRuntime  { get; set; }
        [Inject]  NavigationManager navigationManager  { get; set; }


        protected async Task SaveInitialData()
        {
            try
            {
                HomeModel.EndDate = HomeModel.StartDate.AddDays(HomeModel.NoOfNights);
                await localStorage.SetItemAsync(SD.Local_InitialBooking, HomeModel);
                navigationManager.NavigateTo("hotel/rooms", true);
            }
            catch (Exception ex)
            {

                await jSRuntime.ToastrError(ex.Message);
            }
        }
    }
}
