using System;
using System.Collections.Generic;
using System.Text;

namespace Is.Services.Interface
{
    public interface IBackgroundEmailSender
    {
        Task DoWork();
    }
}
