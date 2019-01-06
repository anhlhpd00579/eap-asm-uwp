﻿using Client.Entities;
using Client.Service;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Client.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListSubject : Page
    {
        private ObservableCollection<Subject> listAllSubjects;
        internal ObservableCollection<Subject> ListAllSubjects { get => listAllSubjects; set => listAllSubjects = value; }
        public ListSubject()
        {
            this.InitializeComponent();
            Get_List_Subject();
        }

        private async void Get_List_Subject()
        {
            this.listAllSubjects = new ObservableCollection<Subject>();
            
            var response = await APIHandle.Get_Subjects();
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var array = JArray.Parse(responseContent);
                foreach (var obj in array)
                {
                    Subject subject = obj.ToObject<Subject>();
                    this.listAllSubjects.Add(subject);
                    Debug.WriteLine(subject.name);
                }
            }
            else
            {
                var dialog = new ContentDialog()
                {
                    Title = "Error!",
                    MaxWidth = this.ActualWidth,
                    Content = "There's an error! Please try later!",
                    CloseButtonText = "OK!"
                };
                ErrorResponse errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                if (errorObject != null)
                {
                    Debug.WriteLine(errorObject.message);
                }
            }
        }

        private void StackPanel_Tap(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Mark));
        }
    }
}
