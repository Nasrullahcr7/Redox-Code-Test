using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Redox_Code_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("scheduler", StringComparison.OrdinalIgnoreCase))
            {
                RunSchedulerUi();
            }
            else
            {
                RunExercise1();
            }
        }

        private static void RunExercise1()
        {
            var numbers = Enumerable.Range(1, 100).ToList();
            var evens = numbers.Where(n => n % 2 == 0).ToList();

            Console.WriteLine("Even numbers (1..100):");
            Console.WriteLine(string.Join(", ", evens));
            Console.WriteLine();

            var onlyOne = new List<int>();
            foreach (var n in Enumerable.Range(1, 99))
            {
                bool by3 = (n % 3 == 0);
                bool by5 = (n % 5 == 0);
                if (by3 ^ by5) onlyOne.Add(n);
            }

            var pretty = onlyOne.Select(n => n.ToString().PadLeft(2));
            Console.WriteLine("Divisible by 3 or 5, but not both:");
            Console.WriteLine(string.Join(", ", pretty));
        }

        private static void RunSchedulerUi()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "events.json");
            var scheduler = new EventScheduler(new FileEventStore(filePath));
            scheduler.Load();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Event Scheduler");
                Console.WriteLine("1) List upcoming");
                Console.WriteLine("2) Schedule");
                Console.WriteLine("3) Cancel");
                Console.WriteLine("4) Save & Exit");
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var upcoming = scheduler.GetUpcomingEvents(DateTime.Now);
                        if (upcoming.Count == 0)
                        {
                            Console.WriteLine("No upcoming events.");
                        }
                        else
                        {
                            foreach (var e in upcoming)
                                Console.WriteLine($"{e.DateTime:yyyy-MM-dd HH:mm} | {e.Name} @ {e.Location}");
                        }
                        break;

                    case "2":
                        Console.Write("Name: ");
                        var name = Console.ReadLine() ?? string.Empty;

                        Console.Write("Location: ");
                        var location = Console.ReadLine() ?? string.Empty;

                        Console.Write("DateTime (yyyy-MM-dd HH:mm): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out var when))
                        {
                            Console.WriteLine("Invalid DateTime.");
                            break;
                        }

                        try
                        {
                            scheduler.ScheduleEvent(new Event(name, location, when));
                            Console.WriteLine("Scheduled.");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case "3":
                        Console.Write("DateTime to cancel (yyyy-MM-dd HH:mm): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out var at))
                        {
                            Console.WriteLine("Invalid DateTime.");
                            break;
                        }
                        Console.WriteLine(scheduler.CancelEvent(at) ? "Cancelled." : "No event at that time.");
                        break;

                    case "4":
                        scheduler.Save();
                        Console.WriteLine("Saved.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
