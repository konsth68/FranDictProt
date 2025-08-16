namespace FranDictProt;

using Nestor;

public class DisplayMorphlogy
{
    public string? Word { get; set; }
    public string? Info { get; set; }
}

public interface IMorphology
{
    public List<string> GetLemmaMorph(string str);
    
    public List<DisplayMorphlogy> GetAllMorph(string str);
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

    private string PrintPos(Pos pos)
    {
        string str = string.Empty;

        switch (pos)
        {
            case Pos.None:
                str = "";
                break;
            case Pos.Noun:
                str = "Существительное";
                break;
            case Pos.Adjective:
                str = "Прилагательное";
                break;
            case Pos.Verb:
                str = "Глагол";
                break;
            case Pos.Adverb:
                str = "Наречие";
                break;
            case Pos.Numeral:
                str = "Числительное";
                break;
            case Pos.Participle:
                str = "Причастие";
                break;
            case Pos.Transgressive:
                str = "Деепричастие";
                break;
            case Pos.Pronoun:
                str = "Местоимение";
                break;
            case Pos.Preposition:
                str = "Предлог";
                break;
            case Pos.Conjunction:
                str = "Союз";
                break;
            case Pos.Particle:
                str = "Частица";
                break;
            case Pos.Interjection:
                str = "Междометие";
                break;
            case Pos.Predicative:
                str = "Предикатив";
                break;
            case Pos.Parenthesis:
                str = "Вводное слово";
                break;
        }

        return str;
    }

    private string PrintGender(Gender gen)
    {
        string str = string.Empty;

        switch (gen)
        {
            case Gender.None:
                str = "";
                break;
            case Gender.Masculine:
                str = "Мужской";
                break;
            case Gender.Feminine:
                str = "Женский";
                break;
            case Gender.Neuter:
                str = "Средний";
                break;
            case Gender.Common:
                str = "Общий";
                break;
        }

        return str;
    }

    private string PrintNumber(Number num)
    {
        string str = string.Empty;

        switch (num)
        {
            case Number.None:
                str = "";
                break;
            case Number.Singular:
                str = "Единственное";
                break;
            case Number.Plural:
                str = "Множественное";
                break;
        }

        return str;
    }

    private string PrintCase(Case cas)
    {
        string str = string.Empty;

        switch (cas)
        {
            case Case.None:
                str = "";
                break;
            case Case.Nominative:
                str = "Именительный";
                break;
            case Case.Genitive:
                str = "Родительный";
                break;
            case Case.Dative:
                str = "Дательный";
                break;
            case Case.Accusative:
                str = "Винительный";
                break;
            case Case.Instrumental:
                str = "Творительный";
                break;
            case Case.Prepositional:
                str = "Предложный";
                break;
            case Case.Locative:
                str = "Местный";
                break;
            case Case.Partitive:
                str = "Частичный";
                break;
            case Case.Vocative:
                str = "Звательный";
                break;
        }

        return str;
    }

    private string PrintTense(Tense ten)
    {
        string str = string.Empty;

        switch (ten)
        {
            case Tense.None:
                str = "";
                break;
            case Tense.Past:
                str = "Прошедшее";
                break;
            case Tense.Present:
                str = "Настоящее";
                break;
            case Tense.Future:
                str = "Будущее";
                break;
            case Tense.Infinitive:
                str = "Инфинитив";
                break;
        }

        return str;
    }

    private string PrintPerson(Person per)
    {
        string str = string.Empty;

        switch (per)
        {
            case Person.None:
                str = "";
                break;
            case Person.First:
                str = "Первое";
                break;
            case Person.Second:
                str = "Второе";
                break;
            case Person.Third:
                str = "Третье";
                break;
        }

        return str;
    }

    private string FormatMorphTag(Nestor.Models.Tag tag)
    {
        
        string strTag = $" {PrintPos(tag.Pos)} {PrintGender(tag.Gender)} {PrintNumber(tag.Number)} {PrintCase(tag.Case)}" +
                     $" {PrintTense(tag.Tense)} {PrintPerson(tag.Person)}";                        
        return strTag;
    }

    public List<DisplayMorphlogy> GetAllMorph(string str)
    {
        Nestor.Models.Word[] words = _nMorf.WordInfo(str);

        if(words == null && words!.Length == 0) return [];

        List<DisplayMorphlogy> dml = new List<DisplayMorphlogy>();

        foreach(var w in words)
        {
            foreach(var wf in w.Forms)
            {
                DisplayMorphlogy dm = new DisplayMorphlogy();
                dm.Word = wf.Word;
                dm.Info = FormatMorphTag(wf.Tag);
                dml.Add(dm);
            }
            
        }

        return dml;
    }
}