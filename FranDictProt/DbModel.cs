using System.Collections.Generic;

namespace FranDictProt;

public class WordKey
{
    public long WordId { get; set; }
    public string? Word { get; set; }
}

public class Paragraph
{
    public long ParagraphId { get; set; }
    public long WordKeyId { get; set; }
    public string? Word { get; set; }
    public int IndexPar { get; set; }
    public string? ParagraphStr { get; set; }
    public string? ReplaceStr { get; set; }
}

public class FullParagraph
{
    public long WordId { get; set; }
    public string? Word { get; set; }

    public List<Paragraph>? Paragraphs { get; set; }
}

