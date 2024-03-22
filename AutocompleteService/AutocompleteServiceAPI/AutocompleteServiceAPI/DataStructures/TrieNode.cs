namespace AutocompleteServiceAPI.DataStructures;

public class TrieNode
    {
        public List<TrieNode> Children = null!;
        public bool IsEndOfWord = false;

        public TrieNode()
        {
            Children = new List<TrieNode>();
            for (int i = 0; i < 29; i++)
            {
                Children.Add(null!);
            }
        }
    }
