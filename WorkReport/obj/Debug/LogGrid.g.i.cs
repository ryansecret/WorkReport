﻿#pragma checksum "c:\users\dell\documents\visual studio 2012\Projects\WorkReport\WorkReport\LogGrid.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0774683ACD5A2EAC389261BEB83AAFC1"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18052
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WorkReport {
    
    
    public partial class LogGrid : System.Windows.Controls.Grid {
        
        internal System.Windows.Controls.ContentControl headerContent;
        
        internal System.Windows.Controls.ContentControl bodyContent;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/WorkReport;component/LogGrid.xaml", System.UriKind.Relative));
            this.headerContent = ((System.Windows.Controls.ContentControl)(this.FindName("headerContent")));
            this.bodyContent = ((System.Windows.Controls.ContentControl)(this.FindName("bodyContent")));
        }
    }
}
