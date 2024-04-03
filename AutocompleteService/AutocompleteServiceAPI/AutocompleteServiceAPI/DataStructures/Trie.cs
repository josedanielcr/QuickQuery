
using System.Runtime.Versioning;

namespace AutocompleteServiceAPI.DataStructures;

public static class Trie
{
    private static TrieNode root = new TrieNode();

    public static void Insert(string key)
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
    private static char IndexToChar(int index)
    {
        if (index >= 0 && index <= 25) // 'a' to 'z'
        {
            return (char)('a' + index);
        }
        else if (index == 26)
        {
            return ' ';
        }
        else if (index == 27)
        {
            return '(';
        }
        else if (index == 28)
        {
            return ')';
        }
        else
        {
            throw new ArgumentException("Invalid index");
        }
    }

    public static List<string> GetWords(string prefix)
    {
        prefix = prefix.ToLower();
        List<string> words = new List<string>();
        TrieNode pCrawl = root;
        for (int i = 0; i < prefix.Length; i++)
        {
            int index = GetCharacterIndex(prefix[i]);
            if (pCrawl.Children[index] == null)
            {
                return words;
            }
            pCrawl = pCrawl.Children[index];
        }
        GetWordsUtil(pCrawl, prefix, words);
        CapitalizeCountry(words);
        return words;
    }

    private static void CapitalizeCountry(List<string> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
            string? item = words[i];
            item = string.Join(" ", item.Split(' ').Select(word =>
            word.Length > 0 ? char.ToUpper(word[0]) + word.Substring(1).ToLower() : word));
            words.RemoveAt(i);
            words.Insert(i, item);
        }
    }

    private static void GetWordsUtil(TrieNode pCrawl, string prefix, List<string> words)
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
                char nextChar = IndexToChar(i);
                GetWordsUtil(pCrawl.Children[i], prefix + nextChar, words);
            }
        }
    }
}