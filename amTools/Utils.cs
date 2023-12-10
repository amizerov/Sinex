namespace amTools;

public static class Utils
{
    public static string GetFileName(Form f, string ext = "pos")
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
        path = path + $"\\{f.Name}{f.Tag}.{ext}";
        //if(!File.Exists(path))
        //    File.Create(path);
        return path;
    }
    public static void SaveFormPosition(Form f)
    {
        string FileFormPosition = GetFileName(f);
        string pos = f.Top + ";" + f.Left + ";" + f.Width + ";" + f.Height;
        File.WriteAllText(FileFormPosition, pos);
    }
    public static void RestoreFormPosition(Form f, bool changeSize = true)
    {
        string FileFormPosition = GetFileName(f);

        if (File.Exists(FileFormPosition))
        {
            string str = File.ReadAllText(FileFormPosition);
            if (str.Length == 0) return;

            string[] pos = str.Split(';');
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
