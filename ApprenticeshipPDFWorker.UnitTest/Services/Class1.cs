using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ApprenticeshipPDFWorker.UnitTest.Services
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Should()
        {
            foreach (var i in Test())
            {
                Console.WriteLine(i);
            }
        }
        
        [Test]
        public void ShouldEnumerate2()
        {
            var list = Test3();
            foreach (var i in list)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine($"Returned {list.Count()} rows");
        }

        [Test]
        public void ShouldEnumerate()
        {
            var list = Test2().ToList();
            foreach (var i in list)
            {
                Console.WriteLine(i);
            }

            foreach (var i in list)
            {
                Console.WriteLine(i);
            }
        }

        [Test]
        public void T()
        {
            var first = Test2().FirstOrDefault();
            Console.WriteLine(first);
            Console.WriteLine(first);
        }


        public IEnumerable<int> Test()
        {
            var list = new List<int>();
            for (int i = 1; i <= 3; i++)
            {
                list.Add(i);
                Console.WriteLine($"Added {i}");
            }
            return list;
        }

        public IEnumerable<int> Test2()
        {
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"Adding {i}");
                yield return i;
            }
        }

        public ICollection<int> Test3()
        {
            return Test2().ToList();
        }
    }
}