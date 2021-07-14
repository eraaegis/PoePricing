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
        public static readonly Dictionary<string, Essence> EssenceDict = new Dictionary<string, Essence>
        {
            {"Deafening Essence of Greed", new Essence(EssenceType.Greed)},
            {"Deafening Essence of Contempt", new Essence(EssenceType.Contempt)},
            {"Deafening Essence of Hatred", new Essence(EssenceType.Hatred)},
            {"Deafening Essence of Woe", new Essence(EssenceType.Woe)},
            {"Deafening Essence of Fear", new Essence(EssenceType.Fear)},
            {"Deafening Essence of Anger", new Essence(EssenceType.Anger)},
            {"Deafening Essence of Torment", new Essence(EssenceType.Torment)},
            {"Deafening Essence of Sorrow", new Essence(EssenceType.Sorrow)},
            {"Deafening Essence of Rage", new Essence(EssenceType.Rage)},
            {"Deafening Essence of Suffering", new Essence(EssenceType.Suffering)},
            {"Deafening Essence of Wrath", new Essence(EssenceType.Wrath)},
            {"Deafening Essence of Doubt", new Essence(EssenceType.Doubt)},
            {"Deafening Essence of Loathing", new Essence(EssenceType.Loathing)},
            {"Deafening Essence of Zeal", new Essence(EssenceType.Zeal)},
            {"Deafening Essence of Anguish", new Essence(EssenceType.Anguish)},
            {"Deafening Essence of Spite", new Essence(EssenceType.Spite)},
            {"Deafening Essence of Scorn", new Essence(EssenceType.Scorn)},
            {"Deafening Essence of Envy", new Essence(EssenceType.Envy)},
            {"Deafening Essence of Misery", new Essence(EssenceType.Misery)},
            {"Deafening Essence of Dread", new Essence(EssenceType.Dread)},
            {"Essence of Insanity", new Essence(EssenceType.Insanity)},
            {"Essence of Horror", new Essence(EssenceType.Horror)},
            {"Essence of Delirium", new Essence(EssenceType.Delirium)},
            {"Essence of Hysteria", new Essence(EssenceType.Hysteria)}
        };
    }

    public class Essence
    {
        public EssenceType essenceType { get; }
        public Essence(EssenceType _essenceType)
        {
            essenceType = _essenceType;
        }
    }

    public enum EssenceType
    {
        Anger,
        Anguish,
        Contempt,
        Doubt,
        Dread,
        Envy,
        Fear,
        Greed,
        Hatred,
        Loathing,
        Misery,
        Rage,
        Scorn,
        Sorrow,
        Spite,
        Suffering,
        Torment,
        Woe,
        Wrath,
        Zeal,
        Delirium,
        Horror,
        Hysteria,
        Insanity
    }

    public partial class PoePricing : Form
    {
        private Dictionary<EssenceType, Item> essences;

        private void GetEssencePrice()
        {
            try
            {
                var url = $"https://poe.ninja/api/data/ItemOverview?league={currentLeague}&type=Essence&language=en";
                var request = WebRequest.Create(url);
                var webResponse = request.GetResponse();
                var webStream = webResponse.GetResponseStream();
                var reader = new StreamReader(webStream);
                var jsonString = JObject.Parse(reader.ReadToEnd());
                foreach (var itemString in jsonString["lines"])
                {
                    Essence item;
                    var hasItem = StringToTypeDictionary.EssenceDict.TryGetValue(itemString["name"].ToString(), out item);
                    if (!hasItem)
                    {
                        continue;
                    }
                    essences[item.essenceType].Price = itemString["chaosValue"].ToObject<double>();
                    essences[item.essenceType].PriceLabel.Text = itemString["chaosValue"].ToString() + "c";
                    essences[item.essenceType].PricePoorLabel.Text = itemString["chaosValue"].ToString() + "c";
                }
            }
            catch (Exception e)
            {

            }
        }

        private void ResetEssenceCount()
        {
            foreach (var essenceType in essences)
            {
                essenceType.Value.Count = 0;
                essenceType.Value.CountTextBox.Text = "";
                essenceType.Value.CountPoorTextBox.Text = "";
            }
        }

        private void InitializeEssence()
        {
            essences = new Dictionary<EssenceType, Item>();
            foreach (EssenceType essenceType in Enum.GetValues(typeof(EssenceType)))
            {
                essences.Add(essenceType, new Item());
            }

            var essenceTypeList = new List<EssenceType> {
                EssenceType.Greed,
                EssenceType.Contempt,
                EssenceType.Hatred,
                EssenceType.Woe,
                EssenceType.Fear,
                EssenceType.Anger,
                EssenceType.Torment,
                EssenceType.Sorrow,
                EssenceType.Rage,
                EssenceType.Suffering,
                EssenceType.Wrath,
                EssenceType.Doubt,
                EssenceType.Loathing,
                EssenceType.Zeal,
                EssenceType.Anguish,
                EssenceType.Spite,
                EssenceType.Scorn,
                EssenceType.Envy,
                EssenceType.Misery,
                EssenceType.Dread,
                EssenceType.Insanity,
                EssenceType.Horror,
                EssenceType.Delirium,
                EssenceType.Hysteria
            };

            // normal panel
            var currentTabIndex = 1;
            for (var i = 0; i < essenceTypeList.Count; ++i)
            {
                var textBox = new TextBox();

                textBox.BackColor = Color.Black;
                textBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                textBox.TextAlign = HorizontalAlignment.Right;
                textBox.ForeColor = Color.Black;
                var locationX = 260;
                var locationY = 36 + i * 49;
                if (i >= essenceTypeList.Count / 2)
                {
                    locationX += 533;
                    locationY -= 12 * 49;
                }
                textBox.Location = new Point(locationX, locationY);
                textBox.MaxLength = 4;
                textBox.Name = Enum.GetName(typeof(EssenceType), essenceTypeList[i]) + "Essence";
                textBox.Size = new Size(57, 26);
                textBox.TabIndex = currentTabIndex;
                textBox.Tag = new Essence(essenceTypeList[i]);
                textBox.KeyPress += TextBox_KeyPress;
                textBox.TextChanged += EssenceTextBox_TextChanged;
                ++currentTabIndex;
                this.EssencePanel.Controls.Add(textBox);
                essences[essenceTypeList[i]].CountTextBox = textBox;

                Label label = new Label();
                label.AutoSize = true;
                label.ForeColor = Color.Lime;
                label.Location = new Point(locationX, locationY + 29);
                label.Name = Enum.GetName(typeof(EssenceType), essenceTypeList[i]) + "EssencePrice";
                label.Size = new Size(19, 13);
                label.TabIndex = 0;
                label.Tag = new Essence(essenceTypeList[i]);
                essences[essenceTypeList[i]].PriceLabel = label;
                this.EssencePanel.Controls.Add(label);
            }

            // poor panel
            currentTabIndex = 1;
            foreach (EssenceType essenceType in Enum.GetValues(typeof(EssenceType)))
            {
                var textBox = new TextBox();
                textBox.BackColor = Color.Black;
                textBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                textBox.TextAlign = HorizontalAlignment.Right;
                textBox.ForeColor = Color.Black;
                var locationX = 440;
                var locationY = 55 + (int)essenceType * 44;
                if ((int)essenceType >= essenceTypeList.Count / 2)
                {
                    locationX += 240;
                    locationY -= 12 * 44;
                }
                textBox.Location = new Point(locationX, locationY);
                textBox.MaxLength = 4;
                textBox.Name = Enum.GetName(typeof(EssenceType), essenceType) + "Essence";
                textBox.Size = new Size(57, 26);
                textBox.TabIndex = currentTabIndex;
                textBox.Tag = new Essence(essenceType);
                textBox.KeyPress += TextBox_KeyPress;
                textBox.TextChanged += EssenceTextBox_TextChanged;
                ++currentTabIndex;
                this.EssencePoorPanel.Controls.Add(textBox);
                essences[essenceType].CountPoorTextBox = textBox;

                Label label = new Label();
                label.AutoSize = true;
                label.ForeColor = Color.Lime;
                label.Location = new Point(locationX + 57, locationY);
                label.Name = Enum.GetName(typeof(EssenceType), essenceType) + "EssencePrice";
                label.Size = new Size(19, 13);
                label.TabIndex = 0;
                label.Tag = new Essence(essenceType);
                essences[essenceType].PricePoorLabel = label;
                this.EssencePoorPanel.Controls.Add(label);
            }

            GetEssencePrice();
        }

        private void EssenceTextBox_TextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var essence = (Essence)(textBox.Tag);
            var value = string.IsNullOrEmpty(textBox.Text) ? 0 : int.Parse(textBox.Text);
            essences[essence.essenceType].Count = value;
            essences[essence.essenceType].CountTextBox.Text = textBox.Text;
            essences[essence.essenceType].CountPoorTextBox.Text = textBox.Text;

            if (value == 0)
            {
                essences[essence.essenceType].CountTextBox.ForeColor = Color.Black;
                essences[essence.essenceType].CountPoorTextBox.ForeColor = Color.Black;

                essences[essence.essenceType].CountTextBox.BackColor = Color.Black;
                essences[essence.essenceType].CountPoorTextBox.BackColor = Color.Black;
            }
            else
            {
                essences[essence.essenceType].CountTextBox.ForeColor = Color.Tan;
                essences[essence.essenceType].CountPoorTextBox.ForeColor = Color.Tan;

                essences[essence.essenceType].CountTextBox.BackColor = Color.FromArgb(64, 64, 64);
                essences[essence.essenceType].CountPoorTextBox.BackColor = Color.FromArgb(64, 64, 64);
            }

            UpdateEssenceTotal();
        }

        private void UpdateEssenceTotal()
        {
            var essenceTab = tabs[Tabs.Essence];
            var total = 0.0;

            foreach (var essenceType in essences)
            {
                var item = essenceType.Value;
                total += item.Price * item.Count;
            }

            essenceTab.normalTotal.Text = total.ToString() + "c";
            essenceTab.poorTotal.Text = total.ToString() + "c";
        }
    }
}
