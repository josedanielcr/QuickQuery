
namespace AutocompleteServiceAPI.DataStructures;

public class Trie
{
    private TrieNode root = new TrieNode();

    public void Insert(string key)
    {
        TrieNode pCrawl = root;
        for (int i = 0; i < key.Length; i++)
        {
            int index = GetCharacterIndex(key[i]);
            if (pCrawl.Children[index] == null)
            {
                pCrawl.Children[index] = new TrieNode();
            }
            pCrawl = pCrawl.Children[index];
        }
        pCrawl.IsEndOfWord = true;
    }

    private static int GetCharacterIndex(char ch)
    {
        if (ch >= 'a' && ch <= 'z')
        {
            return ch - 'a';
        }
        else if (ch == ' ')
        {
            return 26;
        }
        else if (ch == '(')
        {
            return 27;
        }
        else if (ch == ')')
        {
            return 28;
        }
        else
        {
            throw new ArgumentException("Invalid character");
        }
    }

    public List<string> GetWords(string prefix)
    {
        List<string> words = new List<string>();
        TrieNode pCrawl = root;
        for (int i = 0; i < prefix.Length; i++)
        {
            int index = prefix[i] - 'a';
            if (pCrawl.Children[index] == null)
            {
                return words;
            }
            pCrawl = pCrawl.Children[index];
        }
        GetWordsUtil(pCrawl, prefix, words);
        return words;
    }

    private void GetWordsUtil(TrieNode pCrawl, string prefix, List<string> words)
    {
        if (pCrawl == null)
        {
            return;
        }
        if (pCrawl.IsEndOfWord)
        {
            words.Add(prefix);
        }
        for (int i = 0; i < 29; i++)
        {
            if (pCrawl.Children[i] != null)
            {
                GetWordsUtil(pCrawl.Children[i], prefix + (char)('a' + i), words);
            }
        }
    }
}