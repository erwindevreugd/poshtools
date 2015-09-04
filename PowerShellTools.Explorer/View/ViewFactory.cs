using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerShellTools.Explorer
{
    internal sealed class ViewFactory : IViewFactory
    {
        private readonly IViewHost _viewHost;
        private readonly IDataProvider _dataProvider;
        private readonly IExceptionHandler _exceptionHandler;

        private ViewFactory(IViewHost viewHost, IDataProvider dataProvider, IExceptionHandler exceptionHandler)
        {
            _viewHost = viewHost;
            _dataProvider = dataProvider;
            _exceptionHandler = exceptionHandler;
        }

        internal static IViewFactory GetInstance(IViewHost viewHost, IDataProvider dataProvider, IExceptionHandler exceptionHandler)
        {
            return new ViewFactory(viewHost, dataProvider, exceptionHandler);
        }

        internal PSCommandExplorer CreatePSCommandExplorer()
        {
            return new PSCommandExplorer(_viewHost, _dataProvider, _exceptionHandler);
        }

        internal PSParameterEditor CreatePSParameterEditor()
        {
            return new PSParameterEditor(_viewHost, _dataProvider, _exceptionHandler);
        }
    }
}
