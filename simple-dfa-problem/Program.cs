using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace pt_dfa {
    class Program {
        static void Main(string[] args) {
            var fileName = args[0];
            var stream = new AntlrInputStream(File.ReadAllText(fileName));
            var lexer = new JavaLexer(stream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new JavaParser(tokenStream);
            var classContext = parser.class_definition();
            var methodContext = classContext.method();

            var program = new IRVisitor().Visit(methodContext.body());
            Console.WriteLine("IR:");
            Console.WriteLine(string.Join("\n", program.Select(x => "  " + x.ToString())));
            Console.WriteLine();

            var blocks = BasicBlockCreator.Create(program);
            Console.WriteLine("Basic Blocks:");
            var graphViz = BasicBlockCreator.DumpGraphViz(blocks);
            Console.WriteLine(graphViz);

            var definitions = DFA.GetReachingDefinitions(blocks.Last());
            Console.WriteLine("Reaching definitions: " + string.Join(", ", definitions));
        }
    }
}
