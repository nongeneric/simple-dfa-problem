using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pt_dfa {
    class Instr { }

    class AssignInstr : Instr {
        public string Variable;
        public int Value;

        public override string ToString() {
            return $"{Variable} = {Value}";
        }
    }

    class CompareInstr : Instr {
        public int Index;

        public override string ToString() {
            return $"Compare {Index}";
        }
    }

    class BranchInstr : Instr {
        public LabelInstr Label;

        public override string ToString() {
            return $"Branch L{Label.Index}";
        }
    }

    class LabelInstr : Instr {
        public int Index;

        public override string ToString() {
            return $"L{Index}:";
        }
    }

    class MethodCallInstr : Instr {
        public string Name;
        public string Arg;

        public override string ToString() {
            return $"Call {Name}({Arg})";
        }
    }
}
