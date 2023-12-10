using System.IO;
using System.Text.Json;
using amLogger;

namespace bot4.Toolss;

public static class Utils
{
    static string GetFileName(Form f)
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
        return path + $"\\{f.Name}{f.Tag}.pos";
    }
    public static void SaveFormPosition(Form f)
    {
        string FileFormPosition = GetFileName(f);
        string pos = f.Top + ";" + f.Left + ";" + f.Width + ";" + f.Height;
        File.WriteAllText(FileFormPosition, pos);
    }
    public static void LoadFormPosition(Form f, bool changeSize = true)
    {
        string FileFormPosition = GetFileName(f);

        if (File.Exists(FileFormPosition))
        {
            string[] pos = File.ReadAllText(FileFormPosition).Split(';');
            f.Top = int.Parse(pos[0]);
            f.Left = int.Parse(pos[1]);

            if (changeSize)
            {
                f.Width = int.Parse(pos[2]);
                f.Height = int.Parse(pos[3]);
            }
        }
    }

    /**_indicators*******************
    [
        {
            "Name": "SMA",
            "Settings": ["12;2;-45698","27;2;-654433","99;3;-324466"]
        },
        {
            "Name": "SMMA",
            "Settings": ["12;2;-45698","27;2;-654433","99;3;-324466"]
        }
    ]
    **********************************/
    public static void SaveIndicators(List<JIndica> indiList)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(indiList);
            string file = Application.StartupPath + "Indicators.json";
            File.WriteAllText(file, jsonString);
        }
        catch (Exception ex)
        {
            Log.Error("Utils.SaveIndicators", "Error:" + ex.Message);
        }
    }
    public static List<JIndica> LoadIndicators()
    {
        List<JIndica>? res = null;
        string file = Application.StartupPath + "Indicators.json";
        if (File.Exists(file))
        {
            try
            {
                string jsonString = File.ReadAllText(file);
                res = JsonSerializer.Deserialize<List<JIndica>>(jsonString);
            }
            catch(Exception ex)
            {
                Log.Error("Utils.LoadIndicators", "Error:" + ex.Message);
            }
        }
        if (res == null) res = new();
        return res;
    }
}
