using System.IO;

namespace bot4;

public static class Tools
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
}