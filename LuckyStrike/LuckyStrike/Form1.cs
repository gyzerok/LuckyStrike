using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Forms;

using Input;

namespace LuckyStrike
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScreenInterpreter asd = new ScreenInterpreter();
            asd.Interprete();
        }
    }
}
