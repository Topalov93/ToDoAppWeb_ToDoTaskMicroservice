using System.Threading.Tasks;

namespace ToDoApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Task t = MainAsync(args);
            t.Wait();
        }

        static async Task MainAsync(string[] args)
        {
            var controller = new Controller.Controller();

            await controller.Run();
        }
    }
}
