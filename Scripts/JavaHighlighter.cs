using Godot;
using Godot.Collections;
using System.IO;

public partial class JavaHighlighter : SyntaxHighlighter
{
    public const string SyntaxSettingsJsonPath = "./JavaSyntaxSettings.json";

    static Color NumberColor, StringColor, SymbolColor, ClassColor, FunctionColor, MemberVariableColor, CommentColor, KeywordColor;
    static Array<string> Keywords = new();

    static JavaHighlighter()
    {
        NumberColor = StringColor = SymbolColor = ClassColor = FunctionColor = MemberVariableColor = CommentColor = KeywordColor = Colors.White;

        if (File.Exists(SyntaxSettingsJsonPath))
        {
            string json = File.ReadAllText(SyntaxSettingsJsonPath);
            Variant? parsedJson = Json.ParseString(json);

            if (parsedJson == null)
            {
                GD.Print("Something went wrong while loading data! Parsed json was null!");
                return;
            }
            try
            {
                Dictionary data = (Dictionary)parsedJson;
                Keywords = data["keywords"].AsGodotArray<string>();
                Dictionary colors = data["colors"].AsGodotDictionary();
                NumberColor = LoadColor(colors, "number");
                StringColor = LoadColor(colors, "string");
                SymbolColor = LoadColor(colors, "symbol");
                ClassColor = LoadColor(colors, "class");
                FunctionColor = LoadColor(colors, "function");
                MemberVariableColor = LoadColor(colors, "memberVariable");
                CommentColor = LoadColor(colors, "comment");
                KeywordColor = LoadColor(colors, "keyword");
            }
            catch
            {
                GD.Print("Something went wrong while loading data! Wrong json format!");
            }
        }
        else
        {
            GD.Print("Could not load json file! File doesn't exist: " + Path.GetFullPath(SyntaxSettingsJsonPath));
        }
    }

    static Color LoadColor(Dictionary colors, string name)
    {
        string hex = colors[name].ToString();

        return new Color(hex);
    }


    public override Dictionary _GetLineSyntaxHighlighting(int lineNum)
    {
        TextEdit textEdit = GetTextEdit();
        string line = textEdit.GetLine(lineNum);
        Color defaultColor = textEdit.GetThemeColor("font_color");

        return GetHighlights(line, defaultColor);
    }

    public Dictionary GetHighlights(string line, Color defaultColor)
    {
        Dictionary<int, Dictionary> highlights = new();

        AddHighlightFromRegEx(ref highlights, "[^\\w\\d]", line, SymbolColor, defaultColor);
        AddHighlightFromRegEx(ref highlights, "(?<=\\.)[\\w\\d]*", line, MemberVariableColor, defaultColor);
        AddHighlightFromRegEx(ref highlights, "[\\w\\d]+(?=\\()", line, FunctionColor, defaultColor);
        AddHighlightFromRegEx(ref highlights, "\\b[-+]?([0-9]*\\.[0-9]+|[0-9]+)(\\w?)\\b", line, NumberColor, defaultColor);
        AddHighlightFromRegEx(ref highlights, "((^|[^\\.])|\\bimport\\b.*)\\K\\b[A-Z]([\\w\\d]*)", line, ClassColor, defaultColor);
        AddHighlightFromKeywords(ref highlights, Keywords, line, KeywordColor, defaultColor);
        AddHighlightFromRegEx(ref highlights, "\"(.*?)\"", line, StringColor, defaultColor);
        AddHighlightFromRegEx(ref highlights, "//.*", line, CommentColor, defaultColor);
        AddHighlightFromRegEx(ref highlights, "/\\*(.*?)(\\*/|$)", line, CommentColor, defaultColor);

        Dictionary sortedHighlights = new();

        Array<int> keys = new();
        keys.AddRange(highlights.Keys);
        keys.Sort();

        foreach (int key in keys)
        {
            sortedHighlights.Add(key, highlights[key]);
        };

        return sortedHighlights;
    }


    public static void AddHighlightFromKeywords(ref Dictionary<int, Dictionary> highlights, Array<string> keywords, string line, Color color, Color defaultColor)
    {
        foreach (string keyword in keywords)
        {
            AddHighlightFromRegEx(ref highlights, "\\b" + keyword + "\\b", line, color, defaultColor);
        }
    }

    public static void AddHighlightFromRegEx(ref Dictionary<int, Dictionary> highlights, string regEx, string line, Color color, Color defaultColor)
    {
        RegEx classRegEx = RegEx.CreateFromString(regEx);
        Array<RegExMatch> matches = classRegEx.SearchAll(line);

        foreach (RegExMatch match in matches)
        {
            int start = match.GetStart();
            int end = match.GetEnd();

            for (int i = start + 1; i < end; i++) highlights.Remove(i);

            highlights[start] = new Dictionary() { { "color", color } };

            if (!highlights.ContainsKey(end))
            {
                highlights[end] = new Dictionary() { { "color", defaultColor } };
            }
        }
    }
}
