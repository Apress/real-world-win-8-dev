using ApressDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApressDemo.Repository
{
    public interface IBookService
    {
        Task<ObservableCollection<ApressBook>> GetFeaturedBooks();
    }
}
