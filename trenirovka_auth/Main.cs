using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trenirovka_auth
{
    public partial class Main : Form
    {
        private int _userrole;
        public Main(int userrole)
        {
            InitializeComponent();
            _userrole = userrole;
        }
    }
}
