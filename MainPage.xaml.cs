using StudentTestPicker.Models;
using StudentTestPicker.Views;

namespace StudentTestPicker
{
    public partial class MainPage : ContentPage
    {
        Klasa currentClass = new Klasa();
        int LuckyNumber = -1;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new Models.AllClasses();
        }

        protected override void OnAppearing()
        {
            ((Models.AllClasses)BindingContext).LoadClasses();
        }

        private async void classCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                Klasa klasa = (Models.Klasa)e.CurrentSelection[0];
                currentClass = klasa;
                classCollection.SelectedItem = null;
                await Shell.Current.GoToAsync($"//{nameof(ClassStudents)}?{nameof(ClassStudents.ItemId)}={klasa.ClassNumber}");
            }
        }

        private async void Add_Class(object sender, EventArgs e)
        {
            int i = ((Models.AllClasses)BindingContext).AddClass(ClassNumber.Text);
            if (i != 0)
            {
                if (i == 1)
                    await DisplayAlert("Alert", "Taka  Sama Klasa", "OK");
                else
                    await DisplayAlert("Alert", "Cos poszło nie tak sprobuj jeszcze raz", "OK");
            }
            ((Models.AllClasses)BindingContext).LoadClasses();
        }

        private void Delete_Class_Button_Clicked(object sender, EventArgs e)
        {
            ((Models.AllClasses)BindingContext).DeleteClass(currentClass.ClassNumber);
            ((Models.AllClasses)BindingContext).LoadClasses();
        }
    }
}
    
   