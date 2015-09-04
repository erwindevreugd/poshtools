using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerShellTools.Explorer
{
    internal interface IViewController
    {
        IViewHost ViewHost { get; }
        void Show(IView view);
    }
}
