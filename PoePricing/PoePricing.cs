using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoePricing
{
    public partial class PoePricing : Form
    {
        private readonly Dictionary<Tabs, Tab> tabs;
        private string currentLeague = "";

        private Dictionary<ScarabType, Dictionary<ScarabQuality, Item>> scarabs;
        private Dictionary<EssenceType, Item> essences;
        private Dictionary<FossilType, Item> fossils;

        private DisplayMode currentDisplayMode;
        public PoePricing()
        {
            InitializeComponent();

            foreach (var tab in Enum.GetNames(typeof(Tabs)))
            {
                this.StashTabSelector.Items.Add(tab);
            }
            
            tabs = new Dictionary<Tabs, Tab>();
            tabs.Add(Tabs.Scarab, new Tab(this.ScarabBasePanel, this.ScarabPanel, this.ScarabPoorPanel, this.ScarabTotal, this.ScarabTotalPoor));
            tabs.Add(Tabs.Essence, new Tab(this.EssenceBasePanel, this.EssencePanel, this.EssencePoorPanel, this.EssenceTotal, this.EssenceTotalPoor));
            tabs.Add(Tabs.Fossil, new Tab(this.FossilBasePanel, this.FossilPanel, this.FossilPoorPanel, this.FossilTotal, this.FossilTotalPoor));

            this.LeagueSelector.SelectedIndexChanged += LeagueSelector_SelectedIndexChanged;

            InitializeScarab();
            InitializeEssence();
            InitializeFossil();

            this.RefreshPriceButton.Click += RefreshPriceButton_Click;
            this.DisplayModeButton.Click += DisplayModeButton_Click;
            currentDisplayMode = DisplayMode.NormalMode;
            foreach (var tab in tabs)
            {
                tab.Value.panels[currentDisplayMode].Visible = true;
                tab.Value.panels[(DisplayMode)(1 - (int)currentDisplayMode)].Visible = false;
            }
            this.ScreencapButton.Click += ScreencapButton_Click;
            this.ResetButton.Click += ResetButton_Click;
            this.StashTabSelector.SelectedIndexChanged += StashTabSelector_SelectedIndexChanged;
            this.StashTabSelector.SelectedIndex = 0;

            // first time settings
            // if there exists a file that already stores the leagues, then dont do these
            try
            {
                var leagueFile = File.ReadAllText(@"leagues.txt");
                var saveFile = (JObject.Parse(leagueFile)).ToObject<SaveFile>();
                foreach (var league in saveFile.Leagues)
                {
                    this.LeagueSelector.Items.Add(league);
                }
                this.LeagueSelector.Items.Add("");
                this.LeagueSelector.Items.Add("");
                this.LeagueSelector.Items.Add("");
                this.LeagueSelector.Items.Add("Reset Leagues");
                this.LeagueSelector.SelectedIndex = saveFile.SelectedIndex;

                this.StartPanel.Visible = false;
            } catch (Exception e)
            {

            }
            this.StartPastebin.GotFocus += StartPastebin_GotFocus;
            this.StartPastebin.TextChanged += StartPastebin_TextChanged;
        }

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
            } catch (Exception e)
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
                    scarabQuality.Value.CountTextBox.Text = "0";
                    scarabQuality.Value.CountPoorTextBox.Text = "0";
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
                essenceType.Value.CountTextBox.Text = "0";
                essenceType.Value.CountPoorTextBox.Text = "0";
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
                fossilType.Value.CountTextBox.Text = "0";
                fossilType.Value.CountPoorTextBox.Text = "0";
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
                FossilType.Aberrant,
                FossilType.Aetheric,
                FossilType.Bloodstained,
                FossilType.Bound,
                FossilType.Corroded,
                FossilType.Dense,
                FossilType.Enchanted,
                FossilType.Encrusted,
                FossilType.Faceted,
                FossilType.Fractured,
                FossilType.Frigid,
                FossilType.Gilded,
                FossilType.Glyphic,
                FossilType.Hollow,
                FossilType.Jagged,
                FossilType.Lucent,
                FossilType.Metallic,
                FossilType.Perfect,
                FossilType.Prismatic,
                FossilType.Pristine,
                FossilType.Sanctified,
                FossilType.Scorched,
                FossilType.Serrated,
                FossilType.Shuddering,
                FossilType.Tangled
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
                } else if (i >= 23)
                {
                    locationX -= 23 * 66;
                    locationY += 3 * 66;
                } else if (i >= 21)
                {
                    locationX -= 15 * 66 + 33;
                    locationY += 2 * 66;
                } else if (i >= 18)
                {
                    locationX -= 16 * 66;
                    locationY += 2 * 66;
                } else if (i >= 16)
                {
                    locationX -= 16 * 66 + 33;
                    locationY += 2 * 66;
                } else if (i >= 7)
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

        private void LeagueSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            var text = ((ComboBox)sender).Text;
            if (text == "")
            {
                return;
            } else if (text == "Reset Leagues")
            {
                this.StartPastebin.Text = "Paste things here";
                this.StartPanel.Visible = true;
                this.LeagueSelector.Items.Clear();
                return;
            }

            // save selected index
            try
            {
                var leagueFile = File.ReadAllText(@"leagues.txt");
                var saveFile = (JObject.Parse(leagueFile)).ToObject<SaveFile>();
                saveFile.SelectedIndex = ((ComboBox)sender).SelectedIndex;

                var fs = File.Create(@"leagues.txt");
                var info = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(saveFile));
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
            catch (Exception ex)
            {

            }

            currentLeague = text;

            GetScarabPrice();
            GetEssencePrice();
            GetFossilPrice();

            UpdateScarabTotal();
            UpdateEssenceTotal();
            UpdateFossilTotal();
        }

        private void StashTabSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (var i = 0; i < tabs.Count; ++i)
            {
                if (i == ((ToolStripComboBox)sender).SelectedIndex)
                {
                    tabs.ElementAt(i).Value.basePanel.Visible = true;
                } else
                {
                    tabs.ElementAt(i).Value.basePanel.Visible = false;
                }
            }
        }

        private void RefreshPriceButton_Click(object sender, EventArgs e)
        {
            GetScarabPrice();
            GetEssencePrice();
            GetFossilPrice();

            UpdateScarabTotal();
            UpdateEssenceTotal();
            UpdateFossilTotal();
        }

        private void DisplayModeButton_Click(object sender, EventArgs e)
        {
            currentDisplayMode = (DisplayMode)(1 - (int)currentDisplayMode);

            foreach (var tab in tabs)
            {
                tab.Value.panels[currentDisplayMode].Visible = true;
                tab.Value.panels[(DisplayMode)(1 - (int)currentDisplayMode)].Visible = false;
            }
        }

        private void ScreencapButton_Click(object sender, EventArgs e)
        {
            var bounds = this.Bounds;
            var bitmap = new Bitmap(bounds.Width - 14, bounds.Height - 7, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(new Point(bounds.Left + 7, bounds.Top), Point.Empty, bounds.Size);
            Clipboard.SetImage(bitmap);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetScarabCount();
            ResetEssenceCount();
            ResetFossilCount();
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (char)Keys.Back != e.KeyChar)
            {
                e.Handled = true;
            }
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
            } else
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
            }
            else
            {
                var color = Color.Tan;
                if (essence.essenceType == EssenceType.Delirium ||
                    essence.essenceType == EssenceType.Horror ||
                    essence.essenceType == EssenceType.Hysteria ||
                    essence.essenceType == EssenceType.Insanity)
                {
                    color = Color.Crimson;
                }
                essences[essence.essenceType].CountTextBox.ForeColor = color;
                essences[essence.essenceType].CountPoorTextBox.ForeColor = color;
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
            }
            else
            {
                fossils[fossil.fossilType].CountTextBox.ForeColor = Color.Gold;
                fossils[fossil.fossilType].CountPoorTextBox.ForeColor = Color.Gold;
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

        private void StartPastebin_GotFocus(object sender, EventArgs e)
        {
            ((RichTextBox)sender).Text = "";
        }

        private void StartPastebin_TextChanged(object sender, EventArgs e)
        {
            var text = ((RichTextBox)sender).Text;
            try
            {
                var jsonObj = JObject.Parse(text);
                if (jsonObj["error"] != null)
                {
                    return;
                }

                var leagues = jsonObj["leagues"];
                if (leagues == null)
                {
                    return;
                }

                var saveFile = new SaveFile();
                this.LeagueSelector.Items.Clear();
                var hasLeague = false;
                foreach (var leagueString in leagues)
                {
                    if (leagueString["id"].ToString().IndexOf("SSF ") == -1)
                    {
                        this.LeagueSelector.Items.Add(leagueString["id"].ToString());
                        saveFile.Leagues.Add(leagueString["id"].ToString());
                        if (!hasLeague)
                        {
                            hasLeague = true;
                            currentLeague = leagueString["id"].ToString();
                            this.LeagueSelector.SelectedIndex = 0;
                        }
                    }
                }

                this.LeagueSelector.Items.Add("");
                this.LeagueSelector.Items.Add("");
                this.LeagueSelector.Items.Add("");
                this.LeagueSelector.Items.Add("Reset Leagues");

                this.StartPanel.Visible = false;

                // save to file
                saveFile.SelectedIndex = 0;
                var fs = File.Create(@"leagues.txt");
                var info = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(saveFile));
                fs.Write(info, 0, info.Length);
                fs.Close();
            } catch (Exception ex)
            {

            }
        }

        private void PoePricing_Load(object sender, EventArgs e)
        {

        }
    }
}
