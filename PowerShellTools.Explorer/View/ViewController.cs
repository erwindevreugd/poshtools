using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerShellTools.Explorer
{
    internal sealed class ViewController : IViewController
    {
        private readonly IViewHost _viewHost;
        private readonly IViewFactory _viewFactory;
        private List<IView> _views = new List<IView>();

        public ViewController(IViewHost viewHost, IViewFactory viewFactory)
        {
            _viewHost = viewHost;
            _viewFactory = viewFactory;
        }

        public IViewHost ViewHost
        {
            get
            {
                return _viewHost;
            }
        }

        public void Show(IView view)
        {
            _viewHost.ContentHost.Content = view;
        }
    }
}
