using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PowerShellTools.Explorer
{
    internal class ExceptionHandler : IExceptionHandler
    {
        public ExceptionHandler()
        {
        }

        public void HandleException(Exception exception)
        {
            // TODO: implement logging to poshtools logger
            if (exception == null)
            {
                return;
            }

            var message = string.IsNullOrWhiteSpace(exception.Message) ? "Unknown exception" : exception.Message;
            MessageBox.Show(message, "PowerShell Command Explorer", MessageBoxButton.OK);
        }
    }
}
