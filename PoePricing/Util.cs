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
        public SaveFile()
        {
            Leagues = new List<string>();
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

    public static class StringToTypeDictionary
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
}
