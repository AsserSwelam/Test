﻿#pragma checksum "C:\Users\asse7505\Documents\Test\Geo\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "50E0B1536F43BA9EB56DCAA872EEAFAD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ESRI.ArcGIS.Client;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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


namespace Geo {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.Animation.Storyboard beamImgW_SB;
        
        internal System.Windows.Media.Animation.Storyboard beamImgN_SB;
        
        internal System.Windows.Media.Animation.Storyboard beamImgS_SB;
        
        internal System.Windows.Media.Animation.Storyboard beamImgE_SB;
        
        internal System.Windows.Media.Animation.Storyboard beamImgNE_SB;
        
        internal System.Windows.Media.Animation.Storyboard beamImgSE_SB;
        
        internal System.Windows.Media.Animation.Storyboard beamImgSW_SB;
        
        internal System.Windows.Media.Animation.Storyboard beamImgNW_SB;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Pivot functionsPivot;
        
        internal System.Windows.Controls.Image btnFullExtent_MapNav;
        
        internal System.Windows.Controls.Image btnPrevExtent_MapNav;
        
        internal System.Windows.Controls.Image btnNextExtent_MapNav;
        
        internal System.Windows.Controls.Image btnAutoNav_MapNav;
        
        internal System.Windows.Controls.Image btnRedDrawPoint;
        
        internal System.Windows.Controls.Image btnRedDrawLine;
        
        internal System.Windows.Controls.Image btnRedDrawPolygon;
        
        internal System.Windows.Controls.Image btnRedDrawText;
        
        internal System.Windows.Controls.Image btnRedSettings;
        
        internal System.Windows.Controls.Button btnMeasurSet;
        
        internal System.Windows.Controls.Grid mapContent;
        
        internal ESRI.ArcGIS.Client.Map myMap;
        
        internal System.Windows.Controls.Button btnMapFullScrn;
        
        internal System.Windows.Controls.Button btnAutoNavBase_MapNav;
        
        internal System.Windows.Controls.Image imgN;
        
        internal System.Windows.Controls.Image imgW;
        
        internal System.Windows.Controls.Image imgE;
        
        internal System.Windows.Controls.Image imgS;
        
        internal System.Windows.Controls.Image imgNW;
        
        internal System.Windows.Controls.Image imgSE;
        
        internal System.Windows.Controls.Image imgNE;
        
        internal System.Windows.Controls.Image imgSW;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem btnClearMapGraphics;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Geo;component/MainPage.xaml", System.UriKind.Relative));
            this.beamImgW_SB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("beamImgW_SB")));
            this.beamImgN_SB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("beamImgN_SB")));
            this.beamImgS_SB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("beamImgS_SB")));
            this.beamImgE_SB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("beamImgE_SB")));
            this.beamImgNE_SB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("beamImgNE_SB")));
            this.beamImgSE_SB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("beamImgSE_SB")));
            this.beamImgSW_SB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("beamImgSW_SB")));
            this.beamImgNW_SB = ((System.Windows.Media.Animation.Storyboard)(this.FindName("beamImgNW_SB")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.functionsPivot = ((Microsoft.Phone.Controls.Pivot)(this.FindName("functionsPivot")));
            this.btnFullExtent_MapNav = ((System.Windows.Controls.Image)(this.FindName("btnFullExtent_MapNav")));
            this.btnPrevExtent_MapNav = ((System.Windows.Controls.Image)(this.FindName("btnPrevExtent_MapNav")));
            this.btnNextExtent_MapNav = ((System.Windows.Controls.Image)(this.FindName("btnNextExtent_MapNav")));
            this.btnAutoNav_MapNav = ((System.Windows.Controls.Image)(this.FindName("btnAutoNav_MapNav")));
            this.btnRedDrawPoint = ((System.Windows.Controls.Image)(this.FindName("btnRedDrawPoint")));
            this.btnRedDrawLine = ((System.Windows.Controls.Image)(this.FindName("btnRedDrawLine")));
            this.btnRedDrawPolygon = ((System.Windows.Controls.Image)(this.FindName("btnRedDrawPolygon")));
            this.btnRedDrawText = ((System.Windows.Controls.Image)(this.FindName("btnRedDrawText")));
            this.btnRedSettings = ((System.Windows.Controls.Image)(this.FindName("btnRedSettings")));
            this.btnMeasurSet = ((System.Windows.Controls.Button)(this.FindName("btnMeasurSet")));
            this.mapContent = ((System.Windows.Controls.Grid)(this.FindName("mapContent")));
            this.myMap = ((ESRI.ArcGIS.Client.Map)(this.FindName("myMap")));
            this.btnMapFullScrn = ((System.Windows.Controls.Button)(this.FindName("btnMapFullScrn")));
            this.btnAutoNavBase_MapNav = ((System.Windows.Controls.Button)(this.FindName("btnAutoNavBase_MapNav")));
            this.imgN = ((System.Windows.Controls.Image)(this.FindName("imgN")));
            this.imgW = ((System.Windows.Controls.Image)(this.FindName("imgW")));
            this.imgE = ((System.Windows.Controls.Image)(this.FindName("imgE")));
            this.imgS = ((System.Windows.Controls.Image)(this.FindName("imgS")));
            this.imgNW = ((System.Windows.Controls.Image)(this.FindName("imgNW")));
            this.imgSE = ((System.Windows.Controls.Image)(this.FindName("imgSE")));
            this.imgNE = ((System.Windows.Controls.Image)(this.FindName("imgNE")));
            this.imgSW = ((System.Windows.Controls.Image)(this.FindName("imgSW")));
            this.btnClearMapGraphics = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("btnClearMapGraphics")));
        }
    }
}

