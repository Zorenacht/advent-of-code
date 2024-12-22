namespace AoC._2024;

[PuzzleType(PuzzleType.Ordering)]
public sealed class Day09 : Day
{
    [Puzzle(answer: 6_279_058_075_753)]
    public long Part1() => new DiskFragmenter(InputAsText).Compact().Checksum();
    
    [Puzzle(answer: 6_301_361_958_738)]
    public long Part2() => new DiskFragmenter(InputAsText).CompactFiles().Checksum();
    
    private class DiskFragmenter
    {
        private readonly List<int?> _disk = [];
        private readonly List<Space> _empty = [];
        private readonly List<Space> _files = [];
        
        public DiskFragmenter(string input)
        {
            var digits = input
                .Where(x => x >= '0' && x <= '9')
                .Select(x => x - '0').ToArray();
            
            int index = 0;
            for (int i = 0; i < digits.Length; ++i)
            {
                for (int j = 0; j < digits[i]; ++j)
                {
                    _disk.Add(i % 2 == 0 ? i / 2 : null);
                }
                
                if (i % 2 == 0) _files.Add(new Space(i / 2, digits[i], index));
                else _empty.Add(new Space(0, digits[i], index));
                index += digits[i];
            }
        }
        
        public DiskFragmenter Compact()
        {
            int left = 0;
            int right = _disk.Count - 1;
            while (left < right)
            {
                if (_disk[left] != null)
                {
                    ++left;
                    continue;
                }
                if (_disk[right] == null)
                {
                    --right;
                    continue;
                }
                _disk[left] = _disk[right];
                _disk[right] = null;
            }
            return this;
        }
        
        public DiskFragmenter CompactFiles()
        {
            for (int i = _files.Count - 1; i >= 0; --i)
            {
                var firstEmpty = _empty.FirstOrDefault(x => _files[i].Length <= x.Length && x.Index < _files[i].Index);
                if (firstEmpty == default) continue;
                for (int j = 0; j < _files[i].Length; ++j)
                {
                    _disk[firstEmpty.Index + j] = _files[i].Value;
                    _disk[_files[i].Index + j] = null;
                }
                if (firstEmpty.Length == _files[i].Length) _empty.Remove(firstEmpty);
                else
                {
                    firstEmpty.Index += _files[i].Length;
                    firstEmpty.Length -= _files[i].Length;
                }
            }
            return this;
        }
        
        public long Checksum() =>
            _disk
                .Select((d, i) => (long)((d ?? 0) * i))
                .Sum();
        
        private class Space(int value, int length, int index)
        {
            public int Value { get; set; } = value;
            public int Length { get; set; } = length;
            public int Index { get; set; } = index;
            public override string ToString() => $"Value={Value}, Length={Length}, Index={Index}";
        }
    }
}