﻿#pragma checksum "..\..\..\UserControl\BookmarkControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AB2071C5160E01F00E94B9DAE670F9CF"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.34209
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace UserWallPaper {
    
    
    /// <summary>
    /// BookmarkControl
    /// </summary>
    public partial class BookmarkControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 64 "..\..\..\UserControl\BookmarkControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image image;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\UserControl\BookmarkControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton myToggle;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\UserControl\BookmarkControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Popup Popup;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\UserControl\BookmarkControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\UserControl\BookmarkControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton addtogle;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\UserControl\BookmarkControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Popup addpopup;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\UserControl\BookmarkControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox memo;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/UserWallPaper;component/usercontrol/bookmarkcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControl\BookmarkControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 6 "..\..\..\UserControl\BookmarkControl.xaml"
            ((UserWallPaper.BookmarkControl)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.image = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.myToggle = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 65 "..\..\..\UserControl\BookmarkControl.xaml"
            this.myToggle.Checked += new System.Windows.RoutedEventHandler(this.myToggle_Checked);
            
            #line default
            #line hidden
            
            #line 65 "..\..\..\UserControl\BookmarkControl.xaml"
            this.myToggle.Unchecked += new System.Windows.RoutedEventHandler(this.myToggle_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Popup = ((System.Windows.Controls.Primitives.Popup)(target));
            
            #line 74 "..\..\..\UserControl\BookmarkControl.xaml"
            this.Popup.Closed += new System.EventHandler(this.Popup_Closed);
            
            #line default
            #line hidden
            return;
            case 5:
            this.listview = ((System.Windows.Controls.ListView)(target));
            
            #line 76 "..\..\..\UserControl\BookmarkControl.xaml"
            this.listview.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.listview_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 86 "..\..\..\UserControl\BookmarkControl.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.BookMarkDelete_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 87 "..\..\..\UserControl\BookmarkControl.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.BookMarkDeleteAll_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.addtogle = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 93 "..\..\..\UserControl\BookmarkControl.xaml"
            this.addtogle.Checked += new System.Windows.RoutedEventHandler(this.addToggle_Checked);
            
            #line default
            #line hidden
            
            #line 93 "..\..\..\UserControl\BookmarkControl.xaml"
            this.addtogle.Unchecked += new System.Windows.RoutedEventHandler(this.addToggle_Checked);
            
            #line default
            #line hidden
            return;
            case 9:
            this.addpopup = ((System.Windows.Controls.Primitives.Popup)(target));
            return;
            case 10:
            this.memo = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            
            #line 103 "..\..\..\UserControl\BookmarkControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 104 "..\..\..\UserControl\BookmarkControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

