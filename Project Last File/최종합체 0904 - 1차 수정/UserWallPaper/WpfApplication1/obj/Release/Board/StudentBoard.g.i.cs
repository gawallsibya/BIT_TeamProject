﻿#pragma checksum "..\..\..\Board\StudentBoard.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F8D9FF043A9869FACB5353919D539FB7"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.34209
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using RubberBand;
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
using System.Windows.Interactivity;
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
    /// StudentBoard
    /// </summary>
    public partial class StudentBoard : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 97 "..\..\..\Board\StudentBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView boardlist;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\Board\StudentBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox searchname;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\Board\StudentBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label pageNumber;
        
        #line default
        #line hidden
        
        
        #line 118 "..\..\..\Board\StudentBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox readcheck;
        
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
            System.Uri resourceLocater = new System.Uri("/UserWallPaper;component/board/studentboard.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Board\StudentBoard.xaml"
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
            
            #line 55 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Shapes.Rectangle)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_Move);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 70 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_Move);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 73 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ThisMinimize);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 77 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ThisClose);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 91 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.StudentBoard_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.boardlist = ((System.Windows.Controls.ListView)(target));
            
            #line 98 "..\..\..\Board\StudentBoard.xaml"
            this.boardlist.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SelectList_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 108 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Search_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.searchname = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            
            #line 111 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Page_Change);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 112 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Page_Change);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 113 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Page_Change);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 114 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Page_Change);
            
            #line default
            #line hidden
            return;
            case 13:
            this.pageNumber = ((System.Windows.Controls.Label)(target));
            return;
            case 14:
            this.readcheck = ((System.Windows.Controls.CheckBox)(target));
            
            #line 119 "..\..\..\Board\StudentBoard.xaml"
            this.readcheck.Checked += new System.Windows.RoutedEventHandler(this.readcheck_Checked);
            
            #line default
            #line hidden
            
            #line 119 "..\..\..\Board\StudentBoard.xaml"
            this.readcheck.Unchecked += new System.Windows.RoutedEventHandler(this.readcheck_Unchecked);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 120 "..\..\..\Board\StudentBoard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Write_Board);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

