using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace pt_dfa {
    class IRVisitor : JavaBaseVisitor<Instr[]> {
        int label = 0;

        public override Instr[] VisitAssignment([NotNull] JavaParser.AssignmentContext context) {
            return new[] {
                new AssignInstr {
                    Variable = context.ID().GetText(),
                    Value = int.Parse(context.expr().NUM().GetText())
                }
            };
        }

        public override Instr[] VisitBody([NotNull] JavaParser.BodyContext context) {
            var list = new List<Instr>();
            foreach (var st in context.statement()) {
                list.AddRange(st.Accept(this));
            }
            return list.ToArray();
        }

        public override Instr[] VisitIf_op([NotNull] JavaParser.If_opContext context) {
            var list = new List<Instr>();
            var arrayAccess = context.expr().array_access();
            var branch = new BranchInstr { Label = new LabelInstr { Index = label++ } };
            list.Add(new CompareInstr { Index = int.Parse(arrayAccess.NUM().GetText()) });
            list.Add(branch);
            list.AddRange(context.body().Accept(this));
            list.Add(branch.Label);
            return list.ToArray();
        }

        public override Instr[] VisitMethod_call([NotNull] JavaParser.Method_callContext context) {
            return new[] {
                new MethodCallInstr {
                    Name = string.Join(".", context.ID().Take(context.ID().Count() - 1).Select(x => x.GetText())),
                    Arg = context.ID().Last().GetText()
                }
            };
        }

        public override Instr[] VisitStatement([NotNull] JavaParser.StatementContext context) {
            if (context.assignment() != null)
                return context.assignment().Accept(this);
            if (context.if_op() != null)
                return context.if_op().Accept(this);
            if (context.method_call() != null)
                return context.method_call().Accept(this);
            return new Instr[0];
        }
    }
}
