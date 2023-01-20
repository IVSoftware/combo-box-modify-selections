using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms.ComponentModel.Com2Interop;

namespace combo_box_modify_selections
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            comboBoxCom.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCom.DataSource = MockDataSource;
            refreshAvailableComPorts(init: true);
            comboBoxCom.DropDown += onComboBoxComDropDown;
            comboBoxCom.DropDownClosed += (sender, e) => ActiveControl = null;
        }

        BindingList<string> MockDataSource = new BindingList<string>();

        /// <summary>
        /// This is a simulation that will choose a different
        /// set of "available" ports on each drop down.
        /// </summary>
        private void refreshAvailableComPorts(bool init)
        {
            if(init) 
            {
                MockDataSource.Clear();
                MockDataSource.Add("COM2");
                BeginInvoke(() => ActiveControl = null);
            }
            else
            {
                string 
                    selB4 = string.Join(",", MockDataSource),
                    selFtr;
                do
                {
                    var flags = _rando.Next(1, 0x10);
                    if ((flags & 0x1) == 0) MockDataSource.Remove("COM1");
                    else if(!MockDataSource.Contains("COM1")) MockDataSource.Add("COM1");
                    if ((flags & 0x2) == 0) MockDataSource.Remove("COM2");
                    else if (!MockDataSource.Contains("COM2")) MockDataSource.Add("COM2");
                    if ((flags & 0x4) == 0) MockDataSource.Remove("COM3");
                    else if (!MockDataSource.Contains("COM3")) MockDataSource.Add("COM3");
                    if ((flags & 0x8) == 0) MockDataSource.Remove("COM4");
                    else if (!MockDataSource.Contains("COM4")) MockDataSource.Add("COM4");
                    selFtr = string.Join(",", MockDataSource);
                    if (!selFtr.Equals(selB4))
                    {
                        break;
                    }
                } while (true); 
            }
        }
        private Random _rando = new Random(5);

        private void onComboBoxComDropDown(object? sender, EventArgs e)
        {
            refreshAvailableComPorts(init: false);
        }
    }
}