﻿

#pragma checksum "C:\Users\aapelpro\documents\visual studio 2012\Projects\ApressDemo\ApressDemo\Views\FeaturedBookList.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6EC1AF1D66EC8190300D4F164DCE30C3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApressDemo.Views
{
    partial class FeaturedBookList : global::ApressDemo.Common.LayoutAwarePage, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 123 "..\..\..\Views\FeaturedBookList.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.bookItem_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 132 "..\..\..\Views\FeaturedBookList.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GroupHeader_Clicked;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 66 "..\..\..\Views\FeaturedBookList.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.bookItem_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 79 "..\..\..\Views\FeaturedBookList.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GroupHeader_Clicked;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 36 "..\..\..\Views\FeaturedBookList.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.GoBack;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 180 "..\..\..\Views\FeaturedBookList.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.searchButton_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 181 "..\..\..\Views\FeaturedBookList.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.loadButton_Click;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 182 "..\..\..\Views\FeaturedBookList.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.accountButton_Click;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 183 "..\..\..\Views\FeaturedBookList.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.mediaButton_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

