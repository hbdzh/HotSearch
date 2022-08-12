using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace HotSearch.Model
{
    internal class HotModel : ObservableObject
    {
        private ObservableCollection<HotInfo> hotList;
        public ObservableCollection<HotInfo> HotList
        {
            get => hotList;
            set => SetProperty(ref hotList, value);
        }
        private HotInfo selectHotContent;
        public HotInfo SelectHotContent
        {
            get => selectHotContent;
            set => SetProperty(ref selectHotContent, value);
        }
    }
}
