namespace FranDictProt;

using Nestor;

public interface IMorphology
{
    public List<string> GetLemmaMorph(string str);
    
    public List<string> GetAllMorph(string str);
}

public class Morphology : IMorphology
{
    private readonly NestorMorph _nMorf = new();

    public List<string> GetLemmaMorph(string str)
    {
        Nestor.Models.Word[] words = _nMorf.WordInfo(str);
        
        if(words == null && words!.Length == 0) return [];
        
        var res = words.Select(a => a.Lemma).Select(b => b.Word).ToList();
        
        return res;
    }

    public List<string> GetAllMorph(string str)
    {
        Nestor.Models.Word[] words = _nMorf.WordInfo(str);

        if(words == null && words!.Length == 0) return [];

        return (from w in words from wf in w.Forms select wf.Word).ToList();
    }
}