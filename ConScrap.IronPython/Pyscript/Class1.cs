// using System;

// namespace Lib
// {
//     public class Class1
//     {
//     }
// }
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
// to install dependencies
namespace RunPython
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = Python.CreateEngine();
            var scope = engine.CreateScope();
            scope.SetVariable("test", "2 days ago");
            engine.ExecuteFile(@"parse_date.py", scope);
            Console.WriteLine(scope);
            // var date = scope.getDate("2 days ago");
            // Console.WriteLine(date);
        }
    }
}