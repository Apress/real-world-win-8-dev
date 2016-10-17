using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ApressDemo.Repository;


namespace ApressDemo.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MVVMFeaturedBookListViewModel>();
            SimpleIoc.Default.Register<IBookService, ApressBooksRepository>();
        }

        public MVVMFeaturedBookListViewModel MVVMFeaturedBookListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MVVMFeaturedBookListViewModel>();
            }
        }
    }
}
