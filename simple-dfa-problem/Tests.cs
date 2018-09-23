using Antlr4.Runtime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pt_dfa {
    [TestFixture]
    public class Tests {
        static void TestDefinitions(string resourceName, IEnumerable<int> expected) {
            var resourceStream = typeof(Tests).Assembly.GetManifestResourceStream($"pt_dfa.tests.{resourceName}");
            var stream = new AntlrInputStream(resourceStream);
            var lexer = new JavaLexer(stream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new JavaParser(tokenStream);
            var classContext = parser.class_definition();
            var methodContext = classContext.method();
            var ir = methodContext.Accept(new IRVisitor());
            var blocks = BasicBlockCreator.Create(ir);
            var definitions = DFA.GetReachingDefinitions(blocks.Last());
            Assert.IsTrue(definitions.SequenceEqual(expected));
        }

        [Test]
        public void BasicTest() {
            TestDefinitions("basic.java", new[] { 1, 4, 5, 6 });
        }

        [Test]
        public void EmptyTest() {
            TestDefinitions("empty.java", new int[0]);
        }

        [Test]
        public void ComplexTest() {
            TestDefinitions("complex.java", new[] { 1, 3, 5, 6, 7 });
        }

        [Test]
        public void NestedTest() {
            TestDefinitions("nested.java", new[] { 1, 2 });
        }
    }
}
