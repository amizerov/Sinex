namespace bot2.Tools;

public static class Utils
{
    static string GetFileName(Form f)
    {
        return Application.StartupPath + $"{f.Name}{f.Tag}.pos";
    }
    public static void SaveFormPosition(Form f)
    {
        string FileFormPosition = GetFileName(f);
        string pos = f.Top + ";" + f.Left + ";" + f.Width + ";" + f.Height;
        File.WriteAllText(FileFormPosition, pos);
    }
    public static void LoadFormPosition(Form f)
    {
        string FileFormPosition = GetFileName(f);

        if (File.Exists(FileFormPosition))
        {
            string[] pos = File.ReadAllText(FileFormPosition).Split(';');
            f.Top = int.Parse(pos[0]);
            f.Left = int.Parse(pos[1]);
            f.Width = int.Parse(pos[2]);
            f.Height = int.Parse(pos[3]); ;
        }
    }
    public static void SaveIndicators(string indics)
    {
        string file = Application.StartupPath + "Indicators.txt";
        File.WriteAllText(file, indics);
    }
    public static string LoadIndicators()
    {
        string file = Application.StartupPath + "Indicators.txt";
        if (File.Exists(file))
            return File.ReadAllText(file);
        else
            return "";
    }
}
