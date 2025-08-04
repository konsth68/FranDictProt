namespace FranDictProt;

using KTrie;
using Nestor;

public interface IControl
{
    public List<WordKey> Words { get; set; }
    public TrieDictionary<WordKey> Trie { get; set; }

    
    public List<string> FindWord(string partialWord); 
    
    public List<FullParagraph> FindParagraphsFromKey(string partialWord);
    
    public List<FullParagraph> FindParagraphsFromMorph(string partialWord);
    
    public List<FullParagraph> FindParagraphs(string partialWord);
    
}

public class Control : IControl
{
    private readonly IWordKeyRepository? _wordRep;
    private readonly IParagraphRepository? _parRep;
    private readonly IMorphology? _morph;
    
    
    public List<WordKey> Words { get; set; }
    public TrieDictionary<WordKey> Trie { get; set; }
    
    public Control(IWordKeyRepository? wordRep, IParagraphRepository? parRep, IMorphology? morph)
    {
        _wordRep = wordRep; 
        _parRep = parRep;
        _morph = morph;
        
        Words = GetAllWord();
        Trie = FillTrie();
        
    }
    
    private List<WordKey> GetAllWord()
    {
        var res = _wordRep?.GetAll() ?? [];
        return res;
    }

    private TrieDictionary<WordKey> FillTrie()
    {
        if (Words.Count == 0) return new TrieDictionary<WordKey>();
        
        var trieDict = new TrieDictionary<WordKey>();
        foreach (var w in Words)
        {
            if (w.Word != null)
            {
                trieDict.Add(w.Word, w);
            }
        }
        return trieDict;
    }
    
    public List<string> FindWord(string partialWord)
    {
        if(partialWord.Length < 2) return [];
        
        var res =
            Trie.EnumerateByPrefix(partialWord)
                .Where(a => a.Value.Word != null)
                .Select(a => a.Value.Word!)
                .ToList();
        return res;
    }

    private FullParagraph GetFullParagraph(List<Paragraph> pars)
    {
        var fp = new FullParagraph();
        fp.Paragraphs = new List<Paragraph>();
        
        fp.WordId = pars[0].WordKeyId;
        fp.Word = pars[0].Word;
        
        foreach (var p in pars)
        {
            var pr = FillPlaceholders(p);
            fp.Paragraphs.Add(pr);
        }

        return fp;
    }

    private Paragraph FillPlaceholders(Paragraph par)
    {
        if (par.ParagraphStr!= null && par.ParagraphStr.Contains("&Tilde;"))
        {
            par.ParagraphStr = par.ParagraphStr.Replace("&Tilde;", par.ReplaceStr);
        }
        
        return par;
    }
    
    public List<FullParagraph> FindParagraphsFromKey(string partialWord)
    {
        var wr = Trie.EnumerateByPrefix(partialWord)
            .Where(a => a.Value.Word != null)
            .Select(a => a.Value.WordId)
            .ToList();
        
        var pars = _parRep?.GetWordId(wr.ToArray()) ?? [];

        return wr.Select(id => pars.Where(a => a.WordKeyId == id).ToList())
                 .Select(par => GetFullParagraph(par)).ToList();
    }

    public List<FullParagraph> FindParagraphsFromMorph(string partilalWord)
    {
        var res = new List<FullParagraph>();
        
        var words = _morph?.GetLemmaMorph(partilalWord);
        if (words == null) return [];
        
        foreach (var w in  words)
        {
            var pr = FindParagraphsFromKey(w);
            res.AddRange(pr);
        }
        
        return res;
    }

    public List<FullParagraph> FindParagraphs(string partialWord)
    {
        var r = FindParagraphsFromKey(partialWord);

        if (r.Count == 0)
        {
            return  FindParagraphsFromMorph(partialWord).OrderBy(a => a.Word).ToList();
        }
        else
        {
            return r.OrderBy(a => a.Word).ToList();       
        }
    }
    
}