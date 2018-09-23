using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pt_dfa {
    class BasicBlock {
        public Instr[] Body;
        public int Id;
        public List<BasicBlock> References = new List<BasicBlock>();
        public BasicBlock BranchTarget;
        public BasicBlock Next;
    }

    static class BasicBlockCreator {
        public static BasicBlock[] Create(Instr[] program) {
            var blocks = GenerateBlocks(program).ToArray();
            SetBranchTargets(blocks);
            SetNextBlock(blocks);
            SetReferences(blocks);
            return blocks;
        }

        private static IEnumerable<BasicBlock> GenerateBlocks(Instr[] program) {
            var body = new List<Instr>();
            int id = 0;
            Func<BasicBlock> makeBlock = () => {
                var block = new BasicBlock { Id = id++, Body = body.ToArray() };
                body.Clear();
                return block;
            };
            foreach (var instr in program) {
                var label = instr as LabelInstr;
                var branch = instr as BranchInstr;
                if (label != null) {
                    yield return makeBlock();
                    body.Add(instr);
                } else if (branch != null) {
                    body.Add(instr);
                    yield return makeBlock();
                } else {
                    body.Add(instr);
                }
            }
            if (body.Any()) {
                yield return makeBlock();
            }
        }

        static void SetReferences(BasicBlock[] blocks) {
            foreach (var block in blocks) {
                if (block.Next != null) {
                    block.Next.References.Add(block);
                }
                if (block.BranchTarget != null) {
                    block.BranchTarget.References.Add(block);
                }
            }
        }

        static void SetNextBlock(BasicBlock[] blocks) {
            for (int i = 0; i < blocks.Length - 1; i++) {
                blocks[i].Next = blocks[i + 1];
            }
        }

        static void SetBranchTargets(BasicBlock[] blocks) {
            var labelDict = new Dictionary<LabelInstr, BasicBlock>();
            foreach (var block in blocks) {
                var label = block.Body.FirstOrDefault() as LabelInstr;
                if (label != null) {
                    labelDict[label] = block;
                }
            }
            foreach (var block in blocks) {
                var branch = block.Body.LastOrDefault() as BranchInstr;
                if (branch != null) {
                    block.BranchTarget = labelDict[branch.Label];
                }
            }
        }

        public static string DumpGraphViz(BasicBlock[] program) {
            var sb = new StringBuilder();
            sb.AppendLine("digraph {");
            foreach(var block in program) {
                var body = string.Join("\\l", block.Body.Select(x => x.ToString()));
                sb.AppendLine($"  {block.Id} [label=\"{body}\"]");
            }
            foreach (var block in program) {
                var targets = new[] { block.Next, block.BranchTarget }.Where(x => x != null);
                foreach (var target in targets) {
                    sb.AppendLine($"  {block.Id} -> {target.Id}");
                }
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
