using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation;
//using CefSharp.WinForms;
using EO.WebBrowser;
using EO.WinForm; 

namespace Ischool.discipline_competition
{
    public partial class DisciplineCompetitionPanel : BlankPanel
    {
        //  public CefSharp.WinForms.ChromiumWebBrowser browser;

        public WebControl wCtrl1 = new WebControl(); 

        public DisciplineCompetitionPanel()
        {
            InitializeComponent();

            Group = "秩序競賽";

            EO.WebBrowser.Runtime.AddLicense("mZvLn1mXwAAd457pzf8R7lnb5QUQvFuotcDcr2iptMPboVnq+fPw96ng9vYe" + "wK20psLcrmqns8PbsWqZpAcQ8azg8//ooWunprHavUaBpLHLn3Xq7fgZ4K3s" + "9vbpx23h9cXewI+/uNrsrnzm1QH++ajm+87ou2jq7fgZ4K3s9vbpjEOzs/0U" + "4p7l9/bpjEN14+30EO2s3MKetZ9Zl6TNF+ic3PIEEMidtbjD3rNtqrvJ47Vs" + "s7P9FOKe5ff29ON3hI6xy59Zs/D6DuSn6un26bto4+30EO2s3OnPuIlZl6Sx" + "5+Cl4/MI6YxDl6Sxy59Zl6TNDOOdl/gKG+R2mcng2cKh6fP+EKFZ7ekDHuio" + "5cGz3a9np6ax2r1GgaSxy591puX9F+6wtZE=");

            //browser = new ChromiumWebBrowser("https://sites.google.com/ischool.com.tw/discipline-competition/%E9%A6%96%E9%A0%81");
            //browser.Dock = DockStyle.Fill;
            //ContentPanePanel.Controls.Add(browser);
            WebView wv1 = new WebView();
            wv1.Url = "https://sites.google.com/ischool.com.tw/discipline-competition/%E9%A6%96%E9%A0%81";
            wCtrl1.WebView = wv1;
            wCtrl1.Dock = DockStyle.Fill;
            ContentPanePanel.Controls.Add(wCtrl1);


        }
        private static DisciplineCompetitionPanel _DisciplineCompetitionPanel;

        public static DisciplineCompetitionPanel Instance
        {
            get
            {
                if (_DisciplineCompetitionPanel == null)
                {
                    _DisciplineCompetitionPanel = new DisciplineCompetitionPanel();
                }
                return _DisciplineCompetitionPanel;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ContentPanePanel
            // 
            this.ContentPanePanel.Location = new System.Drawing.Point(0, 163);
            this.ContentPanePanel.Size = new System.Drawing.Size(870, 421);
            // 
            // DisciplineCompetitionPanel
            // 
            this.Name = "DisciplineCompetitionPanel";
            this.ResumeLayout(false);

        }
    }
}
