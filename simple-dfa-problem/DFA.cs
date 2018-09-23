using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pt_dfa {
    static class DFA {
        static IEnumerable<int> GetDefinitions(BasicBlock block) {
            var last = block.Body.OfType<AssignInstr>().LastOrDefault();
            if (last != null)
                yield return last.Value;
        }

        public static int[] GetReachingDefinitions(BasicBlock rootBlock) {
            var definitions = new Dictionary<BasicBlock, int[]>();
            Func<BasicBlock, int[]> recurse = null;
            recurse = block => {
                int[] values;
                if (definitions.TryGetValue(block, out values))
                    return values;
                var blockDefinitions = GetDefinitions(block);
                if (blockDefinitions.Any())
                    return blockDefinitions.ToArray();
                foreach (var reference in block.References) {
                    blockDefinitions = blockDefinitions.Union(recurse(reference));
                }
                var result = blockDefinitions.ToArray();
                definitions[block] = result;
                return result;
            };
            return recurse(rootBlock);
        }
    }
}
