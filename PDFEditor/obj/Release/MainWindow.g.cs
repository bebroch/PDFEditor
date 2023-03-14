﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "578916B8388571BF4099368764CAD4B4D0D9B54A2A0F24694254B6C705C91427"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using PDFEditor;
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


namespace PDFEditor {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 106 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainPanel;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image PictureBox;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas canvas;
        
        #line default
        #line hidden
        
        
        #line 153 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label nowPage;
        
        #line default
        #line hidden
        
        
        #line 154 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label allPages;
        
        #line default
        #line hidden
        
        
        #line 158 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton saveButton;
        
        #line default
        #line hidden
        
        
        #line 164 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton addLineButton;
        
        #line default
        #line hidden
        
        
        #line 170 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton addImageButton;
        
        #line default
        #line hidden
        
        
        #line 176 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton paintButt;
        
        #line default
        #line hidden
        
        
        #line 187 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem saveMenuItem;
        
        #line default
        #line hidden
        
        
        #line 188 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem saveHowMenuItem;
        
        #line default
        #line hidden
        
        
        #line 189 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem saveAllMenuItem;
        
        #line default
        #line hidden
        
        
        #line 194 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem cancelMenuItem;
        
        #line default
        #line hidden
        
        
        #line 196 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem deleteMenuItem;
        
        #line default
        #line hidden
        
        
        #line 198 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem convertMenuItem;
        
        #line default
        #line hidden
        
        
        #line 199 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem convertJpglMenuItem;
        
        #line default
        #line hidden
        
        
        #line 200 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem convertPnglMenuItem;
        
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
            System.Uri resourceLocater = new System.Uri("/PDFEditor;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
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
            
            #line 11 "..\..\MainWindow.xaml"
            ((PDFEditor.MainWindow)(target)).Drop += new System.Windows.DragEventHandler(this.MainPanel_Drop);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MainPanel = ((System.Windows.Controls.Grid)(target));
            
            #line 107 "..\..\MainWindow.xaml"
            this.MainPanel.KeyDown += new System.Windows.Input.KeyEventHandler(this.MainWindow_KeyDown);
            
            #line default
            #line hidden
            
            #line 108 "..\..\MainWindow.xaml"
            this.MainPanel.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.CanvasMouseDown);
            
            #line default
            #line hidden
            
            #line 109 "..\..\MainWindow.xaml"
            this.MainPanel.MouseMove += new System.Windows.Input.MouseEventHandler(this.CanvasMouseMove);
            
            #line default
            #line hidden
            
            #line 110 "..\..\MainWindow.xaml"
            this.MainPanel.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.CanvasMouseUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PictureBox = ((System.Windows.Controls.Image)(target));
            
            #line 130 "..\..\MainWindow.xaml"
            this.PictureBox.SourceUpdated += new System.EventHandler<System.Windows.Data.DataTransferEventArgs>(this.ImageSourceUpdatedLL);
            
            #line default
            #line hidden
            return;
            case 4:
            this.canvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 5:
            
            #line 151 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_Next);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 152 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_Back);
            
            #line default
            #line hidden
            return;
            case 7:
            this.nowPage = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.allPages = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.saveButton = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 158 "..\..\MainWindow.xaml"
            this.saveButton.Click += new System.Windows.RoutedEventHandler(this.SaveButt);
            
            #line default
            #line hidden
            return;
            case 10:
            this.addLineButton = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 164 "..\..\MainWindow.xaml"
            this.addLineButton.Click += new System.Windows.RoutedEventHandler(this.AddLineButt);
            
            #line default
            #line hidden
            return;
            case 11:
            this.addImageButton = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 170 "..\..\MainWindow.xaml"
            this.addImageButton.Click += new System.Windows.RoutedEventHandler(this.AddImageButt);
            
            #line default
            #line hidden
            return;
            case 12:
            this.paintButt = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 176 "..\..\MainWindow.xaml"
            this.paintButt.Click += new System.Windows.RoutedEventHandler(this.PaintButt);
            
            #line default
            #line hidden
            return;
            case 13:
            this.saveMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 187 "..\..\MainWindow.xaml"
            this.saveMenuItem.Click += new System.Windows.RoutedEventHandler(this.MenuSave);
            
            #line default
            #line hidden
            return;
            case 14:
            this.saveHowMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 188 "..\..\MainWindow.xaml"
            this.saveHowMenuItem.Click += new System.Windows.RoutedEventHandler(this.MenuSaveHow);
            
            #line default
            #line hidden
            return;
            case 15:
            this.saveAllMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 189 "..\..\MainWindow.xaml"
            this.saveAllMenuItem.Click += new System.Windows.RoutedEventHandler(this.MenuSaveAll);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 190 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuOpen);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 191 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuAddFile);
            
            #line default
            #line hidden
            return;
            case 18:
            this.cancelMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 194 "..\..\MainWindow.xaml"
            this.cancelMenuItem.Click += new System.Windows.RoutedEventHandler(this.MenuCancel);
            
            #line default
            #line hidden
            return;
            case 19:
            
            #line 195 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuAddPage);
            
            #line default
            #line hidden
            return;
            case 20:
            this.deleteMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 196 "..\..\MainWindow.xaml"
            this.deleteMenuItem.Click += new System.Windows.RoutedEventHandler(this.MenuDeletePage);
            
            #line default
            #line hidden
            return;
            case 21:
            this.convertMenuItem = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 22:
            this.convertJpglMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 199 "..\..\MainWindow.xaml"
            this.convertJpglMenuItem.Click += new System.Windows.RoutedEventHandler(this.MenuConvertToJPG);
            
            #line default
            #line hidden
            return;
            case 23:
            this.convertPnglMenuItem = ((System.Windows.Controls.MenuItem)(target));
            
            #line 200 "..\..\MainWindow.xaml"
            this.convertPnglMenuItem.Click += new System.Windows.RoutedEventHandler(this.MenuConvertToPNG);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
