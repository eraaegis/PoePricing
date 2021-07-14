using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoePricing
{
    public class SaveFile
    {
        public int SelectedIndex { get; set; }
        public List<string> Leagues { get; set; }
        public string TabIndex { get; set; }
        public string AccountName { get; set; }
        public SaveFile()
        {
            SelectedIndex = 0;
            Leagues = new List<string>();
            TabIndex = "0";
            AccountName = "";
        }
    }

    public enum Tabs
    {
        Scarab,
        Essence,
        Fossil
    }

    public class Item
    {
        public int Count { get; set; }
        public double Price { get; set; }
        public Label PriceLabel { get; set; }
        public Label PricePoorLabel { get; set; }
        public TextBox CountTextBox { get; set; }
        public TextBox CountPoorTextBox { get; set; }
        public Item()
        {
            Count = 0;
            Price = 0;
        }
    }

    public class Tab
    {
        public Dictionary<DisplayMode, Panel> panels { get; }
        public Panel basePanel { get; }
        public Label normalTotal { get; }
        public Label poorTotal { get; }
        public Tab(Panel _basePanel, Panel _normalPanel, Panel _poorPanel, Label _normalTotal, Label _poorTotal)
        {
            basePanel = _basePanel;
            panels = new Dictionary<DisplayMode, Panel>();
            panels.Add(DisplayMode.NormalMode, _normalPanel);
            panels.Add(DisplayMode.PoorMode, _poorPanel);

            normalTotal = _normalTotal;
            poorTotal = _poorTotal;
        }
    }

    public enum DisplayMode
    {
        NormalMode,
        PoorMode
    }
}
