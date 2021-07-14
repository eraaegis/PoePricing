using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoePricing
{
    public static partial class StringToTypeDictionary
    {
        public static readonly Dictionary<string, Scarab> ScarabDict = new Dictionary<string, Scarab>
        {
            {"Rusted Bestiary Scarab", new Scarab(ScarabType.Bestiary, ScarabQuality.Rusted)},
            {"Polished Bestiary Scarab", new Scarab(ScarabType.Bestiary, ScarabQuality.Polished)},
            {"Gilded Bestiary Scarab", new Scarab(ScarabType.Bestiary, ScarabQuality.Gilded)},
            {"Winged Bestiary Scarab", new Scarab(ScarabType.Bestiary, ScarabQuality.Winged)},
            {"Rusted Reliquary Scarab", new Scarab(ScarabType.Reliquary, ScarabQuality.Rusted)},
            {"Polished Reliquary Scarab", new Scarab(ScarabType.Reliquary, ScarabQuality.Polished)},
            {"Gilded Reliquary Scarab", new Scarab(ScarabType.Reliquary, ScarabQuality.Gilded)},
            {"Winged Reliquary Scarab", new Scarab(ScarabType.Reliquary, ScarabQuality.Winged)},
            {"Rusted Torment Scarab", new Scarab(ScarabType.Torment, ScarabQuality.Rusted)},
            {"Polished Torment Scarab", new Scarab(ScarabType.Torment, ScarabQuality.Polished)},
            {"Gilded Torment Scarab", new Scarab(ScarabType.Torment, ScarabQuality.Gilded)},
            {"Winged Torment Scarab", new Scarab(ScarabType.Torment, ScarabQuality.Winged)},
            {"Rusted Sulphite Scarab", new Scarab(ScarabType.Sulphite, ScarabQuality.Rusted)},
            {"Polished Sulphite Scarab", new Scarab(ScarabType.Sulphite, ScarabQuality.Polished)},
            {"Gilded Sulphite Scarab", new Scarab(ScarabType.Sulphite, ScarabQuality.Gilded)},
            {"Winged Sulphite Scarab", new Scarab(ScarabType.Sulphite, ScarabQuality.Winged)},
            {"Rusted Metamorph Scarab", new Scarab(ScarabType.Metamorph, ScarabQuality.Rusted)},
            {"Polished Metamorph Scarab", new Scarab(ScarabType.Metamorph, ScarabQuality.Polished)},
            {"Gilded Metamorph Scarab", new Scarab(ScarabType.Metamorph, ScarabQuality.Gilded)},
            {"Winged Metamorph Scarab", new Scarab(ScarabType.Metamorph, ScarabQuality.Winged)},
            {"Rusted Legion Scarab", new Scarab(ScarabType.Legion, ScarabQuality.Rusted)},
            {"Polished Legion Scarab", new Scarab(ScarabType.Legion, ScarabQuality.Polished)},
            {"Gilded Legion Scarab", new Scarab(ScarabType.Legion, ScarabQuality.Gilded)},
            {"Winged Legion Scarab", new Scarab(ScarabType.Legion, ScarabQuality.Winged)},
            {"Rusted Ambush Scarab", new Scarab(ScarabType.Ambush, ScarabQuality.Rusted)},
            {"Polished Ambush Scarab", new Scarab(ScarabType.Ambush, ScarabQuality.Polished)},
            {"Gilded Ambush Scarab", new Scarab(ScarabType.Ambush, ScarabQuality.Gilded)},
            {"Winged Ambush Scarab", new Scarab(ScarabType.Ambush, ScarabQuality.Winged)},
            {"Rusted Blight Scarab", new Scarab(ScarabType.Blight, ScarabQuality.Rusted)},
            {"Polished Blight Scarab", new Scarab(ScarabType.Blight, ScarabQuality.Polished)},
            {"Gilded Blight Scarab", new Scarab(ScarabType.Blight, ScarabQuality.Gilded)},
            {"Winged Blight Scarab", new Scarab(ScarabType.Blight, ScarabQuality.Winged)},
            {"Rusted Shaper Scarab", new Scarab(ScarabType.Shaper, ScarabQuality.Rusted)},
            {"Polished Shaper Scarab", new Scarab(ScarabType.Shaper, ScarabQuality.Polished)},
            {"Gilded Shaper Scarab", new Scarab(ScarabType.Shaper, ScarabQuality.Gilded)},
            {"Winged Shaper Scarab", new Scarab(ScarabType.Shaper, ScarabQuality.Winged)},
            {"Rusted Perandus Scarab", new Scarab(ScarabType.Perandus, ScarabQuality.Rusted)},
            {"Polished Perandus Scarab", new Scarab(ScarabType.Perandus, ScarabQuality.Polished)},
            {"Gilded Perandus Scarab", new Scarab(ScarabType.Perandus, ScarabQuality.Gilded)},
            {"Winged Perandus Scarab", new Scarab(ScarabType.Perandus, ScarabQuality.Winged)},
            {"Rusted Cartography Scarab", new Scarab(ScarabType.Cartography, ScarabQuality.Rusted)},
            {"Polished Cartography Scarab", new Scarab(ScarabType.Cartography, ScarabQuality.Polished)},
            {"Gilded Cartography Scarab", new Scarab(ScarabType.Cartography, ScarabQuality.Gilded)},
            {"Winged Cartography Scarab", new Scarab(ScarabType.Cartography, ScarabQuality.Winged)},
            {"Rusted Harbinger Scarab", new Scarab(ScarabType.Harbinger, ScarabQuality.Rusted)},
            {"Polished Harbinger Scarab", new Scarab(ScarabType.Harbinger, ScarabQuality.Polished)},
            {"Gilded Harbinger Scarab", new Scarab(ScarabType.Harbinger, ScarabQuality.Gilded)},
            {"Winged Harbinger Scarab", new Scarab(ScarabType.Harbinger, ScarabQuality.Winged)},
            {"Rusted Elder Scarab", new Scarab(ScarabType.Elder, ScarabQuality.Rusted)},
            {"Polished Elder Scarab", new Scarab(ScarabType.Elder, ScarabQuality.Polished)},
            {"Gilded Elder Scarab", new Scarab(ScarabType.Elder, ScarabQuality.Gilded)},
            {"Winged Elder Scarab", new Scarab(ScarabType.Elder, ScarabQuality.Winged)},
            {"Rusted Divination Scarab", new Scarab(ScarabType.Divination, ScarabQuality.Rusted)},
            {"Polished Divination Scarab", new Scarab(ScarabType.Divination, ScarabQuality.Polished)},
            {"Gilded Divination Scarab", new Scarab(ScarabType.Divination, ScarabQuality.Gilded)},
            {"Winged Divination Scarab", new Scarab(ScarabType.Divination, ScarabQuality.Winged)},
            {"Rusted Breach Scarab", new Scarab(ScarabType.Breach, ScarabQuality.Rusted)},
            {"Polished Breach Scarab", new Scarab(ScarabType.Breach, ScarabQuality.Polished)},
            {"Gilded Breach Scarab", new Scarab(ScarabType.Breach, ScarabQuality.Gilded)},
            {"Winged Breach Scarab", new Scarab(ScarabType.Breach, ScarabQuality.Winged)},
            {"Rusted Abyss Scarab", new Scarab(ScarabType.Abyss, ScarabQuality.Rusted)},
            {"Polished Abyss Scarab", new Scarab(ScarabType.Abyss, ScarabQuality.Polished)},
            {"Gilded Abyss Scarab", new Scarab(ScarabType.Abyss, ScarabQuality.Gilded)},
            {"Winged Abyss Scarab", new Scarab(ScarabType.Abyss, ScarabQuality.Winged)}
        };
    }

    public class Scarab
    {
        public ScarabType scarabType { get; }
        public ScarabQuality scarabQuality { get; }
        public Scarab(ScarabType _scarabType, ScarabQuality _scarabQuality)
        {
            scarabType = _scarabType;
            scarabQuality = _scarabQuality;
        }
    }

    public enum ScarabType
    {
        Abyss,
        Ambush,
        Bestiary,
        Blight,
        Breach,
        Cartography,
        Divination,
        Elder,
        Harbinger,
        Legion,
        Metamorph,
        Perandus,
        Reliquary,
        Shaper,
        Sulphite,
        Torment
    }

    public enum ScarabQuality
    {
        Rusted,
        Polished,
        Gilded,
        Winged
    }

    public partial class PoePricing : Form
    {
        private Dictionary<ScarabType, Dictionary<ScarabQuality, Item>> scarabs;

        private void GetScarabPrice()
        {
            try
            {
                var url = $"https://poe.ninja/api/data/ItemOverview?league={currentLeague}&type=Scarab&language=en";
                var request = WebRequest.Create(url);
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var reader = new StreamReader(webStream);
                var jsonString = JObject.Parse(reader.ReadToEnd());
                foreach (var itemString in jsonString["lines"])
                {
                    Scarab item;
                    var hasItem = StringToTypeDictionary.ScarabDict.TryGetValue(itemString["name"].ToString(), out item);
                    if (!hasItem)
                    {
                        continue;
                    }
                    scarabs[item.scarabType][item.scarabQuality].Price = itemString["chaosValue"].ToObject<double>();
                    scarabs[item.scarabType][item.scarabQuality].PriceLabel.Text = itemString["chaosValue"].ToString() + "c";
                    scarabs[item.scarabType][item.scarabQuality].PricePoorLabel.Text = itemString["chaosValue"].ToString() + "c";
                }
            }
            catch (Exception e)
            {

            }
        }

        private void ResetScarabCount()
        {
            foreach (var scarabType in scarabs)
            {
                foreach (var scarabQuality in scarabType.Value)
                {
                    scarabQuality.Value.Count = 0;
                    scarabQuality.Value.CountTextBox.Text = "";
                    scarabQuality.Value.CountPoorTextBox.Text = "";
                }
            }
        }

        private void InitializeScarab()
        {
            scarabs = new Dictionary<ScarabType, Dictionary<ScarabQuality, Item>>();
            foreach (ScarabType scarabType in Enum.GetValues(typeof(ScarabType)))
            {
                var scarabTypeDict = new Dictionary<ScarabQuality, Item>();
                foreach (ScarabQuality scarabQuality in Enum.GetValues(typeof(ScarabQuality)))
                {
                    scarabTypeDict.Add(scarabQuality, new Item());
                }
                scarabs.Add(scarabType, scarabTypeDict);
            }

            var scarabTypeList = new List<ScarabType> {
                ScarabType.Bestiary,
                ScarabType.Reliquary,
                ScarabType.Torment,
                ScarabType.Sulphite,
                ScarabType.Metamorph,
                ScarabType.Legion,
                ScarabType.Ambush,
                ScarabType.Blight,
                ScarabType.Shaper,
                ScarabType.Perandus,
                ScarabType.Cartography,
                ScarabType.Harbinger,
                ScarabType.Elder,
                ScarabType.Divination,
                ScarabType.Breach,
                ScarabType.Abyss
            };

            // normal panel
            var currentTabIndex = 1;
            for (var i = 0; i < scarabTypeList.Count; ++i)
            {
                foreach (ScarabQuality scarabQuality in Enum.GetValues(typeof(ScarabQuality)))
                {
                    var textBox = new TextBox();

                    textBox.BackColor = Color.Black;
                    textBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    textBox.TextAlign = HorizontalAlignment.Right;
                    textBox.ForeColor = Color.Black;
                    var locationX = 281 + (int)scarabQuality * 68;
                    var locationY = 73 + i * 66;
                    if (i >= scarabTypeList.Count / 2)
                    {
                        locationX += 284;
                        locationY -= 8 * 66;
                    }
                    textBox.Location = new Point(locationX, locationY);
                    textBox.MaxLength = 4;
                    textBox.Name = Enum.GetName(typeof(ScarabQuality), scarabQuality) + Enum.GetName(typeof(ScarabType), scarabTypeList[i]);
                    textBox.Size = new Size(57, 26);
                    textBox.TabIndex = currentTabIndex;
                    textBox.Tag = new Scarab(scarabTypeList[i], scarabQuality);
                    textBox.KeyPress += TextBox_KeyPress;
                    textBox.TextChanged += ScarabTextBox_TextChanged;
                    ++currentTabIndex;
                    this.ScarabPanel.Controls.Add(textBox);
                    scarabs[scarabTypeList[i]][scarabQuality].CountTextBox = textBox;

                    Label label = new Label();
                    label.AutoSize = true;
                    label.ForeColor = Color.Lime;
                    label.Location = new Point(locationX, locationY + 29);
                    label.Name = Enum.GetName(typeof(ScarabQuality), scarabQuality) + Enum.GetName(typeof(ScarabType), scarabTypeList[i]) + "Price";
                    label.Size = new Size(19, 13);
                    label.TabIndex = 0;
                    label.Tag = new Scarab(scarabTypeList[i], scarabQuality);
                    scarabs[scarabTypeList[i]][scarabQuality].PriceLabel = label;
                    this.ScarabPanel.Controls.Add(label);
                }
            }

            // poor panel
            currentTabIndex = 1;
            foreach (ScarabType scarabType in Enum.GetValues(typeof(ScarabType)))
            {
                foreach (ScarabQuality scarabQuality in Enum.GetValues(typeof(ScarabQuality)))
                {
                    var textBox = new TextBox();
                    textBox.BackColor = Color.Black;
                    textBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    textBox.TextAlign = HorizontalAlignment.Right;
                    textBox.ForeColor = Color.Black;
                    var locationX = 281 + (int)scarabQuality * 68;
                    var locationY = 73 + (int)scarabType * 66;
                    if ((int)scarabType >= scarabTypeList.Count / 2)
                    {
                        locationX += 395;
                        locationY -= 8 * 66;
                    }
                    textBox.Location = new Point(locationX, locationY);
                    textBox.MaxLength = 4;
                    textBox.Name = Enum.GetName(typeof(ScarabQuality), scarabQuality) + Enum.GetName(typeof(ScarabType), scarabType);
                    textBox.Size = new Size(57, 26);
                    textBox.TabIndex = currentTabIndex;
                    textBox.Tag = new Scarab(scarabType, scarabQuality);
                    textBox.KeyPress += TextBox_KeyPress;
                    textBox.TextChanged += ScarabTextBox_TextChanged;
                    ++currentTabIndex;
                    this.ScarabPoorPanel.Controls.Add(textBox);
                    scarabs[scarabType][scarabQuality].CountPoorTextBox = textBox;

                    Label label = new Label();
                    label.AutoSize = true;
                    label.ForeColor = Color.Lime;
                    label.Location = new Point(locationX, locationY + 29);
                    label.Name = Enum.GetName(typeof(ScarabQuality), scarabQuality) + Enum.GetName(typeof(ScarabType), scarabType) + "Price";
                    label.Size = new Size(19, 13);
                    label.TabIndex = 0;
                    label.Tag = new Scarab(scarabType, scarabQuality);
                    scarabs[scarabType][scarabQuality].PricePoorLabel = label;
                    this.ScarabPoorPanel.Controls.Add(label);
                }
            }

            GetScarabPrice();
        }

        private void ScarabTextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var scarab = (Scarab)(textBox.Tag);
            var value = string.IsNullOrEmpty(textBox.Text) ? 0 : int.Parse(textBox.Text);
            scarabs[scarab.scarabType][scarab.scarabQuality].Count = value;
            scarabs[scarab.scarabType][scarab.scarabQuality].CountTextBox.Text = textBox.Text;
            scarabs[scarab.scarabType][scarab.scarabQuality].CountPoorTextBox.Text = textBox.Text;

            if (value == 0)
            {
                scarabs[scarab.scarabType][scarab.scarabQuality].CountTextBox.ForeColor = Color.Black;
                scarabs[scarab.scarabType][scarab.scarabQuality].CountPoorTextBox.ForeColor = Color.Black;

                scarabs[scarab.scarabType][scarab.scarabQuality].CountTextBox.BackColor = Color.Black;
                scarabs[scarab.scarabType][scarab.scarabQuality].CountPoorTextBox.BackColor = Color.Black;
            }
            else
            {
                var color = Color.Lime;
                var scarabQuality = scarab.scarabQuality;
                if (scarabQuality == ScarabQuality.Polished)
                {
                    color = Color.DeepSkyBlue;
                }
                else if (scarabQuality == ScarabQuality.Gilded)
                {
                    color = Color.Violet;
                }
                else if (scarabQuality == ScarabQuality.Winged)
                {
                    color = Color.Gold;
                }
                scarabs[scarab.scarabType][scarab.scarabQuality].CountTextBox.ForeColor = color;
                scarabs[scarab.scarabType][scarab.scarabQuality].CountPoorTextBox.ForeColor = color;

                scarabs[scarab.scarabType][scarab.scarabQuality].CountTextBox.BackColor = Color.FromArgb(64, 64, 64);
                scarabs[scarab.scarabType][scarab.scarabQuality].CountPoorTextBox.BackColor = Color.FromArgb(64, 64, 64);
            }

            UpdateScarabTotal();
        }

        private void UpdateScarabTotal()
        {
            var scarabTab = tabs[Tabs.Scarab];
            var total = 0.0;

            foreach (var scarabType in scarabs)
            {
                foreach (var scarabQuality in scarabType.Value)
                {
                    var item = scarabQuality.Value;
                    total += item.Price * item.Count;
                }
            }

            scarabTab.normalTotal.Text = total.ToString() + "c";
            scarabTab.poorTotal.Text = total.ToString() + "c";
        }
    }
}
