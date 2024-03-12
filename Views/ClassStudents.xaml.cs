using System;
using System.IO;
using StudentTestPicker.Models;

namespace StudentTestPicker.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class ClassStudents : ContentPage
    {
        public string ItemId
        {
            set { LoadStudents(value); }
        }

        public ClassStudents()
        {
            InitializeComponent();
        }

        public void LoadStudents(string classNumber)
        {
            BindingContext = new Models.AllStudents(classNumber);
        }

        private void Add_Student(object sender, EventArgs e)
        {
            ((Models.AllStudents)BindingContext).AddStudent(((Models.AllStudents)BindingContext).getClassNumber(), StudentName.Text, StudentSurname.Text);
            ((Models.AllStudents)BindingContext).LoadStudents(((Models.AllStudents)BindingContext).getClassNumber());
        }

        private async void ReturnButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        private async void DrawStudent(object sender, EventArgs e)
        {
            Models.AllStudents allStudents = (Models.AllStudents)BindingContext;

            if (allStudents.Students.Count == 0)
            {
                await DisplayAlert("Brak uczniów", "Nie ma dostêpnych uczniów do losowania.", "OK");
            }
            else
            {
                Random random = new Random();
                int randomIndex = random.Next(0, allStudents.Students.Count);

                Models.Student s = allStudents.Students[randomIndex];
                if (string.IsNullOrEmpty(s.Name))
                {
                    await DisplayAlert("Brak ucznia", "Nie ma dostêpnych uczniów do losowania.", "OK");
                }
                else
                {
                    int luckyNumber = random.Next(1, 31);

                    await DisplayAlert("Wylosowany uczeñ", $"Dzisiaj pytany bêdzie {s.Name} {s.Surname}. Szczêœliwy numer: {luckyNumber}", "OK");
                }
            }
        }
        private void RemoveStudentClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var student = (Models.Student)button.CommandParameter;

            ((Models.AllStudents)BindingContext).DeleteStudent(student.ClassNumber, student.Name, student.Surname);
            ((Models.AllStudents)BindingContext).LoadStudents(student.ClassNumber);
        }
    }
}
