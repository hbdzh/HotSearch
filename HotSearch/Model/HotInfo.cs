using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace HotSearch.Model
{
    internal class HotInfo : ObservableObject
    {
        private string number;
        public string Number
        {
            get => number;
            set => SetProperty(ref number, value);
        }
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private string describe;
        public string Describe
        {
            get => describe;
            set => SetProperty(ref describe, value);
        }
        private string link;
        public string Link
        {
            get => link;
            set => SetProperty(ref link, value);
        }
    }
}
