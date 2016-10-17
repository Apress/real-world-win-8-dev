using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ApressDemo.Models
{
    public partial class ApressBook
    {
        #region "Properties"

        public string ApressBookISBN { get; set; }
        public string ApressBookName { get; set; }
        public string ApressBookTechnology { get; set; }
        public string ApressBookAuthor { get; set; }
        public DateTime ApressBookPublishedDate { get; set; }
        public string ApressBookDescription { get; set; }
        public string ApressBookUserLevel { get; set; }
        public string ApressBookImageURI { get; set; }

        public string DisplayablePublishDate
        {
           get
           {
               return ApressBookPublishedDate.ToString("MM/dd/yyyy");
           }           
        }

        #endregion
    }

    public partial class ApressBookForZumo : ApressODataService.ApressBook
    {
        #region "Properties"

        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        #endregion
    }


    public partial class RelationalApressBookModel
    {
        #region "Properties"

        [SQLite.PrimaryKey]
        public string ApressBookISBN { get; set; }
        public string ApressBookName { get; set; }
        public string ApressBookTechnology { get; set; }
        public string ApressBookAuthor { get; set; }
        public DateTime ApressBookPublishedDate { get; set; }
        public string ApressBookDescription { get; set; }
        public string ApressBookUserLevel { get; set; }
        public string ApressBookImageURI { get; set; }

        public string DisplayablePublishDate
        {
            get
            {
                return ApressBookPublishedDate.ToString("MM/dd/yyyy");
            }
        }

        #endregion
    }
}
