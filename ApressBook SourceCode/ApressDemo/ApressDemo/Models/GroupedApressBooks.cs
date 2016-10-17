using ApressDemo.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApressDemo.Models
{
    public class GroupedApressBooks : BindableBase
    {
        #region "Members"

        private string _ApressBookGroupName;
        private int _ApressBookGroupCount;

        private ObservableCollection<ApressBook> _bookCollection;

        #endregion

        #region "Properties"

        public string ApressBookGroupName
        {
            get { return _ApressBookGroupName; }
            set { SetProperty(ref _ApressBookGroupName, value); }
        }
        public int ApressBookGroupCount
        {
            get { return _ApressBookGroupCount; }
            set { SetProperty(ref _ApressBookGroupCount, value); }
        }

        public ObservableCollection<ApressBook> BookCollection
        {
            get { return _bookCollection; }
            set { SetProperty(ref _bookCollection, value); }
        }

        public string ImageURI
        {
            get
            {
                string imageUri = string.Empty;
                switch (ApressBookGroupName)
                {
                    case "Windows Phone":
                        imageUri = "/Assets/WindowsPhoneLogo.png";
                        break;
                    case "Windows Azure":
                        imageUri = "/Assets/WindowsAzureLogo.png";
                        break;
                    case "ASP.NET":
                        imageUri = "/Assets/ASPDotNetLogo.png";
                        break;
                }

                return imageUri;
            }
        }
    
       #endregion

        #region "Constructor"

        public GroupedApressBooks()
        {
            BookCollection = new ObservableCollection<ApressBook>();
        }

        #endregion
   }
}
