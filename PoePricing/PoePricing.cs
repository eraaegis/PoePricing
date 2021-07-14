using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

                // also put in stash dump saved details
                if (!string.IsNullOrEmpty(saveFile.TabIndex))
                {
                    this.StashDumpTabIndex.Text = saveFile.TabIndex;
                }
                if (!string.IsNullOrEmpty(saveFile.AccountName))
                {
                    this.StashDumpAccountName.Text = saveFile.AccountName;
                }
            } catch (Exception e)
            {

            }
            this.StartPastebin.GotFocus += StartPastebin_GotFocus;
            this.StartPastebin.TextChanged += StartPastebin_TextChanged;

            // stash dumping
            this.StashDumpPanel.Visible = false;
            this.StashDumpButton.Click += StashDumpButton_Click;
            this.StashDumpExitButton.Click += StashDumpExitButton_Click;
            this.StashDumpGoButton.Click += StashDumpGoButton_Click;
            this.StashDumpTabIndex.KeyPress += TextBox_KeyPress;
            this.StashDumpPastebin.GotFocus += StashDumpPastebin_GotFocus;
            this.StashDumpPastebin.TextChanged += StashDumpPastebin_TextChanged;

            this.StashDumpToolTip.SetToolTip(this.StashDumpTabIndexLabel, "Tab indices start from 0 from the top");
            this.StashDumpToolTip.SetToolTip(this.StashDumpAccountNameLabel, "Account name can be found when you logged in to pathofexile.com at the top left corner");
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
                var fs = File.Create(@"leagues.txt");
                var info = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(saveFile));
                fs.Write(info, 0, info.Length);
                fs.Close();
            } catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(text) && text != "Paste things here")
                {
                    this.StartPastebin.Text = "ERROR PARSING DATA";
                }
            }
        }

        private void StashDumpButton_Click(object sender, EventArgs e)
        {
            this.StashDumpPanel.Visible = true;
            this.StashDumpPastebin.Text = "Paste things here";
        }

        private void StashDumpExitButton_Click(object sender, EventArgs e)
        {
            this.StashDumpPanel.Visible = false;
        }

        private void StashDumpGoButton_Click(object sender, EventArgs e)
        {
            var tabIndex = this.StashDumpTabIndex.Text;
            var accountName = this.StashDumpAccountName.Text;
            Process.Start($"https://www.pathofexile.com/character-window/get-stash-items?league={currentLeague}&tabs=1&tabIndex={tabIndex}&accountName={accountName}");
            try
            {
                var leagueFile = File.ReadAllText(@"leagues.txt");
                var saveFile = (JObject.Parse(leagueFile)).ToObject<SaveFile>();
                saveFile.TabIndex = tabIndex;
                saveFile.AccountName = accountName;

                var fs = File.Create(@"leagues.txt");
                var info = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(saveFile));
                fs.Write(info, 0, info.Length);
                fs.Close();
            } catch (Exception ex)
            {

            }
        }

        private void StashDumpPastebin_GotFocus(object sender, EventArgs e)
        {
            ((RichTextBox)sender).Text = "";
        }

        private void StashDumpPastebin_TextChanged(object sender, EventArgs e)
        {
            var text = ((RichTextBox)sender).Text;
            try
            {
                var jsonObj = JObject.Parse(text);
                if (jsonObj["error"] != null)
                {
                    return;
                }

                var items = jsonObj["items"];
                if (items == null)
                {
                    return;
                }

                foreach (var item in items)
                {
                    var name = item["baseType"].ToString();
                    var count = item["stackSize"];
                    Scarab scarab;
                    var hasItem = StringToTypeDictionary.ScarabDict.TryGetValue(name, out scarab);
                    if (scarab != null)
                    {
                        if (count == null)
                        {
                            scarabs[scarab.scarabType][scarab.scarabQuality].Count += 1;
                        } else
                        {
                            scarabs[scarab.scarabType][scarab.scarabQuality].Count += count.ToObject<int>();
                        }
                        scarabs[scarab.scarabType][scarab.scarabQuality].CountTextBox.Text = scarabs[scarab.scarabType][scarab.scarabQuality].Count.ToString();
                        scarabs[scarab.scarabType][scarab.scarabQuality].CountPoorTextBox.Text = scarabs[scarab.scarabType][scarab.scarabQuality].Count.ToString();
                        continue;
                    }
                    Essence essence;
                    hasItem = StringToTypeDictionary.EssenceDict.TryGetValue(name, out essence);
                    if (essence != null)
                    {
                        if (count == null)
                        {
                            essences[essence.essenceType].Count += 1;
                        }
                        else
                        {
                            essences[essence.essenceType].Count += count.ToObject<int>();
                        }
                        essences[essence.essenceType].CountTextBox.Text = essences[essence.essenceType].Count.ToString();
                        essences[essence.essenceType].CountPoorTextBox.Text = essences[essence.essenceType].Count.ToString();
                        continue;
                    }
                    Fossil fossil;
                    hasItem = StringToTypeDictionary.FossilDict.TryGetValue(name, out fossil);
                    if (fossil != null)
                    {
                        if (count == null)
                        {
                            fossils[fossil.fossilType].Count += 1;
                        }
                        else
                        {
                            fossils[fossil.fossilType].Count += count.ToObject<int>();
                        }
                        fossils[fossil.fossilType].CountTextBox.Text = fossils[fossil.fossilType].Count.ToString();
                        fossils[fossil.fossilType].CountPoorTextBox.Text = fossils[fossil.fossilType].Count.ToString();
                        continue;
                    }
                }

                this.StashDumpPanel.Visible = false;
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(text) && text != "Paste things here")
                {
                    this.StashDumpPastebin.Text = "ERROR PARSING DATA";
                }
            }
        }

        private void PoePricing_Load(object sender, EventArgs e)
        {

        }
    }
}
