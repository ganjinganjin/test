﻿using BAR.CommonLib_v1._0;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BAR.Windows
{
    public partial class WarningDoorWnd : Form
    {
        public bool IsShow = false;

        public WarningDoorWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
        }
    }
}
