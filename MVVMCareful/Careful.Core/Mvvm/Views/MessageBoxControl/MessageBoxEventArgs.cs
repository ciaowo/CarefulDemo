﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core.Mvvm.Views.MessageBoxControl
{
    public class MessageBoxEventArgs : EventArgs
    {
        public string Caption { get; set; }
    }
}
