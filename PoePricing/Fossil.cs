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
        public static readonly Dictionary<string, Fossil> FossilDict = new Dictionary<string, Fossil>
        {
            {"Jagged Fossil", new Fossil(FossilType.Jagged)},
            {"Dense Fossil", new Fossil(FossilType.Dense)},
            {"Frigid Fossil", new Fossil(FossilType.Frigid)},
            {"Aberrant Fossil", new Fossil(FossilType.Aberrant)},
            {"Scorched Fossil", new Fossil(FossilType.Scorched)},
            {"Metallic Fossil", new Fossil(FossilType.Metallic)},
            {"Pristine Fossil", new Fossil(FossilType.Pristine)},
            {"Bound Fossil", new Fossil(FossilType.Bound)},
            {"Corroded Fossil", new Fossil(FossilType.Corroded)},
            {"Perfect Fossil", new Fossil(FossilType.Perfect)},
            {"Prismatic Fossil", new Fossil(FossilType.Prismatic)},
            {"Enchanted Fossil", new Fossil(FossilType.Enchanted)},
            {"Aetheric Fossil", new Fossil(FossilType.Aetheric)},
            {"Lucent Fossil", new Fossil(FossilType.Lucent)},
            {"Serrated Fossil", new Fossil(FossilType.Serrated)},
            {"Shuddering Fossil", new Fossil(FossilType.Shuddering)},
            {"Tangled Fossil", new Fossil(FossilType.Tangled)},
            {"Bloodstained Fossil", new Fossil(FossilType.Bloodstained)},
            {"Gilded Fossil", new Fossil(FossilType.Gilded)},
            {"Encrusted Fossil", new Fossil(FossilType.Encrusted)},
            {"Sanctified Fossil", new Fossil(FossilType.Sanctified)},
            {"Hollow Fossil", new Fossil(FossilType.Hollow)},
            {"Fractured Fossil", new Fossil(FossilType.Fractured)},
            {"Glyphic Fossil", new Fossil(FossilType.Glyphic)},
            {"Faceted Fossil", new Fossil(FossilType.Faceted)}
        };
    }

    public class Fossil
    {
        public FossilType fossilType { get; }
        public Fossil(FossilType _fossilType)
        {
            fossilType = _fossilType;
        }
    }

    public enum FossilType
    {
        Aberrant,
        Aetheric,
        Bloodstained,
        Bound,
        Corroded,
        Dense,
        Enchanted,
        Encrusted,
        Faceted,
        Fractured,
        Frigid,
        Gilded,
        Glyphic,
        Hollow,
        Jagged,
        Lucent,
        Metallic,
        Perfect,
        Prismatic,
        Pristine,
        Sanctified,
        Scorched,
        Serrated,
        Shuddering,
        Tangled
    }

    public partial class PoePricing : Form
    {
        private Dictionary<FossilType, Item> fossils;

        private void GetFossilPrice()
        {
            try
            {
                var url = $"https://poe.ninja/api/data/ItemOverview?league={currentLeague}&type=Fossil&language=en";
                var request = WebRequest.Create(url);
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var reader = new StreamReader(webStream);
                var jsonString = JObject.Parse(reader.ReadToEnd());
                foreach (var itemString in jsonString["lines"])
                {
                    Fossil item;
                    var hasItem = StringToTypeDictionary.FossilDict.TryGetValue(itemString["name"].ToString(), out item);
                    if (!hasItem)
                    {
                        continue;
                    }
                    fossils[item.fossilType].Price = itemString["chaosValue"].ToObject<double>();
                    fossils[item.fossilType].PriceLabel.Text = itemString["chaosValue"].ToString() + "c";
                    fossils[item.fossilType].PricePoorLabel.Text = itemString["chaosValue"].ToString() + "c";
                }
            }
            catch (Exception e)
            {

            }
        }

        private void ResetFossilCount()
        {
            foreach (var fossilType in fossils)
            {
                fossilType.Value.Count = 0;
                fossilType.Value.CountTextBox.Text = "";
                fossilType.Value.CountPoorTextBox.Text = "";
            }
        }

        private void InitializeFossil()
        {
            fossils = new Dictionary<FossilType, Item>();
            foreach (FossilType fossilType in Enum.GetValues(typeof(FossilType)))
            {
                fossils.Add(fossilType, new Item());
            }

            var fossilTypeList = new List<FossilType> {
                FossilType.Jagged,
                FossilType.Dense,
                FossilType.Frigid,
                FossilType.Aberrant,
                FossilType.Scorched,
                FossilType.Metallic,
                FossilType.Pristine,
                FossilType.Bound,
                FossilType.Corroded,
                FossilType.Perfect,
                FossilType.Prismatic,
                FossilType.Enchanted,
                FossilType.Aetheric,
                FossilType.Lucent,
                FossilType.Serrated,
                FossilType.Shuddering,
                FossilType.Tangled,
                FossilType.Bloodstained,
                FossilType.Gilded,
                FossilType.Encrusted,
                FossilType.Sanctified,
                FossilType.Hollow,
                FossilType.Fractured,
                FossilType.Glyphic,
                FossilType.Faceted
            };

            // normal panel
            var currentTabIndex = 1;
            for (var i = 0; i < fossilTypeList.Count; ++i)
            {
                var textBox = new TextBox();

                textBox.BackColor = Color.Black;
                textBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                textBox.TextAlign = HorizontalAlignment.Right;
                textBox.ForeColor = Color.Black;
                var locationX = 327 + i * 66;
                var locationY = 64;
                if (i >= 24)
                {
                    locationX -= 18 * 66;
                    locationY += 3 * 66;
                }
                else if (i >= 23)
                {
                    locationX -= 23 * 66;
                    locationY += 3 * 66;
                }
                else if (i >= 21)
                {
                    locationX -= 15 * 66 + 33;
                    locationY += 2 * 66;
                }
                else if (i >= 18)
                {
                    locationX -= 16 * 66;
                    locationY += 2 * 66;
                }
                else if (i >= 16)
                {
                    locationX -= 16 * 66 + 33;
                    locationY += 2 * 66;
                }
                else if (i >= 7)
                {
                    locationX -= 8 * 66;
                    locationY += 66;
                }
                textBox.Location = new Point(locationX, locationY);
                textBox.MaxLength = 4;
                textBox.Name = Enum.GetName(typeof(FossilType), fossilTypeList[i]) + "Fossil";
                textBox.Size = new Size(57, 26);
                textBox.TabIndex = currentTabIndex;
                textBox.Tag = new Fossil(fossilTypeList[i]);
                textBox.KeyPress += TextBox_KeyPress;
                textBox.TextChanged += FossilTextBox_TextChanged;
                ++currentTabIndex;
                this.FossilPanel.Controls.Add(textBox);
                fossils[fossilTypeList[i]].CountTextBox = textBox;

                Label label = new Label();
                label.AutoSize = true;
                label.ForeColor = Color.Lime;
                label.Location = new Point(locationX, locationY + 29);
                label.Name = Enum.GetName(typeof(FossilType), fossilTypeList[i]) + "FossilPrice";
                label.Size = new Size(19, 13);
                label.TabIndex = 0;
                label.Tag = new Fossil(fossilTypeList[i]);
                fossils[fossilTypeList[i]].PriceLabel = label;
                this.FossilPanel.Controls.Add(label);
            }

            // poor panel
            currentTabIndex = 1;
            foreach (FossilType fossilType in Enum.GetValues(typeof(FossilType)))
            {
                var textBox = new TextBox();
                textBox.BackColor = Color.Black;
                textBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                textBox.TextAlign = HorizontalAlignment.Right;
                textBox.ForeColor = Color.Black;
                var locationX = 440;
                var locationY = 55 + (int)fossilType * 44;
                if ((int)fossilType >= 13)
                {
                    locationX += 256;
                    locationY -= 13 * 44;
                }
                textBox.Location = new Point(locationX, locationY);
                textBox.MaxLength = 4;
                textBox.Name = Enum.GetName(typeof(FossilType), fossilType) + "Fossil";
                textBox.Size = new Size(57, 26);
                textBox.TabIndex = currentTabIndex;
                textBox.Tag = new Fossil(fossilType);
                textBox.KeyPress += TextBox_KeyPress;
                textBox.TextChanged += FossilTextBox_TextChanged;
                ++currentTabIndex;
                this.FossilPoorPanel.Controls.Add(textBox);
                fossils[fossilType].CountPoorTextBox = textBox;

                Label label = new Label();
                label.AutoSize = true;
                label.ForeColor = Color.Lime;
                label.Location = new Point(locationX + 57, locationY);
                label.Name = Enum.GetName(typeof(FossilType), fossilType) + "FossilPrice";
                label.Size = new Size(19, 13);
                label.TabIndex = 0;
                label.Tag = new Fossil(fossilType);
                fossils[fossilType].PricePoorLabel = label;
                this.FossilPoorPanel.Controls.Add(label);
            }

            GetFossilPrice();
        }

        private void FossilTextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var fossil = (Fossil)(textBox.Tag);
            var value = string.IsNullOrEmpty(textBox.Text) ? 0 : int.Parse(textBox.Text);
            fossils[fossil.fossilType].Count = value;
            fossils[fossil.fossilType].CountTextBox.Text = textBox.Text;
            fossils[fossil.fossilType].CountPoorTextBox.Text = textBox.Text;

            if (value == 0)
            {
                fossils[fossil.fossilType].CountTextBox.ForeColor = Color.Black;
                fossils[fossil.fossilType].CountPoorTextBox.ForeColor = Color.Black;

                fossils[fossil.fossilType].CountTextBox.BackColor = Color.Black;
                fossils[fossil.fossilType].CountPoorTextBox.BackColor = Color.Black;
            }
            else
            {
                fossils[fossil.fossilType].CountTextBox.ForeColor = Color.Gold;
                fossils[fossil.fossilType].CountPoorTextBox.ForeColor = Color.Gold;

                fossils[fossil.fossilType].CountTextBox.BackColor = Color.FromArgb(64, 64, 64);
                fossils[fossil.fossilType].CountPoorTextBox.BackColor = Color.FromArgb(64, 64, 64);
            }

            UpdateFossilTotal();
        }

        private void UpdateFossilTotal()
        {
            var fossilTab = tabs[Tabs.Fossil];
            var total = 0.0;

            foreach (var fossilType in fossils)
            {
                var item = fossilType.Value;
                total += item.Price * item.Count;
            }

            fossilTab.normalTotal.Text = total.ToString() + "c";
            fossilTab.poorTotal.Text = total.ToString() + "c";
        }
    }
}
