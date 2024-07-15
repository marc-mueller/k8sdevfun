using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevFun.Clients.V1_0;
using DevFun.Common.Model.Dtos.V1_0;

namespace DevFun.DataInitializer
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "ok for sample")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "ok for sample")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "ok for sample")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "ok for sample")]
    public class DataInitializer
    {
        private readonly IDevFunServiceClient client;

        public DataInitializer(IDevFunServiceClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task InitializeData()
        {
            Console.WriteLine("Start initializing test data...");

            await InitializeCategories().ConfigureAwait(false);

            await InitializeJokes().ConfigureAwait(false);

            Console.WriteLine("Initializing test data finished.");
        }

        private async Task InitializeCategories()
        {
            Console.WriteLine("Check for category data");

            ICollection<CategoryDto> categories = null;

            for (int i = 0; i < 3; i++) {
                try
                {
                    categories = (await client.GetCategorysAsync().ConfigureAwait(false)).Result;
                    break;
                }
                catch (Exception) {
                    await Task.Delay(5000).ConfigureAwait(false);
                }
            }   
            
            if(categories == null)
            {
                Console.WriteLine("Error reading categories!");
                return;
            }
            
            if (!categories.Any())
            {
                Console.WriteLine("No data found, initialize category data");

                try
                {
                    await client.CreateCategoryAsync(
                        new CategoryDto()
                        {
                            //Id = 0,
                            Name = "General"
                        }).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating category: {ex.ToString()}");
                }

                try
                {
                    await client.CreateCategoryAsync(
                        new CategoryDto()
                        {
                            //Id = 1,
                            Name = ".NET"
                        }).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating category: {ex.ToString()}");
                }

                try
                {
                    await client.CreateCategoryAsync(
                        new CategoryDto()
                        {
                            //Id = 2,
                            Name = "Java"
                        }).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating category: {ex.ToString()}");
                }
            }

            Console.WriteLine("category data initialized");
        }

        private async Task InitializeJokes()
        {
            var categories = (await client.GetCategorysAsync().ConfigureAwait(false)).Result;
            var generalId = categories.First(c => c.Name.Equals("General", StringComparison.OrdinalIgnoreCase)).Id;
            var netId = categories.First(c => c.Name.Equals(".NET", StringComparison.OrdinalIgnoreCase)).Id;
            var javaId = categories.First(c => c.Name.Equals("Java", StringComparison.OrdinalIgnoreCase)).Id;

            Console.WriteLine("Check for jokes data");


            ICollection<DevJokeDto> existingJokes = null;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    existingJokes = (await client.GetJokesAsync().ConfigureAwait(false)).Result;
                    break;
                }
                catch (Exception) {
                    await Task.Delay(5000).ConfigureAwait(false);
                }
            }

            if (existingJokes == null)
            {
                Console.WriteLine("Error reading jokes!");
                return;
            }

            if (!existingJokes.Any())
            {
                Console.WriteLine("No data found, initialize jokes data");
                var jokes = new List<DevJokeDto>()
                {
                    new DevJokeDto() { Text = @"Programmer\r\nA machine that turns coffee into code.", CategoryId=generalId },
                    new DevJokeDto() { Text = @"Programmer\r\nA person who fixed a problem that you don't know your have, in a way you don't understand.", CategoryId=generalId },
                    new DevJokeDto() { Text = @"Algorithm\r\nWord used by programmers when... they do not want to explain what they did.", CategoryId=generalId },
                    new DevJokeDto() { Text = @"Q: What's the object-oriented way to become wealthy?\r\nA: Inheritance", CategoryId=generalId },
                    new DevJokeDto() { Text = @"Q: What's the programmer's favourite hangout place?\r\nA: Foo Bar", CategoryId=generalId },
                    new DevJokeDto() { Text = @"Q: How to you tell an introverted computer scientist from an extroverted computer scientist?\r\nA: An extroverted computer scientist looks at your shoes when he talks to you.", CategoryId=generalId },
                    new DevJokeDto() { Text = @"Q: Why do Java programmers wear glasses?\r\nA: Because they don't C#", CategoryId=netId },
                    new DevJokeDto() { Text = @"Have you heard about the new Cray super computer?\r\nIt’s so fast, it executes an infinite loop in 6 seconds.", CategoryId=generalId },
                    new DevJokeDto() { Text = @"There are three kinds of lies: Lies, damned lies, and benchmarks.", CategoryId=generalId },
                    new DevJokeDto() { Text = @"“Knock, knock“.\r\n“Who’s there ?“\r\nvery long pause….\r\n“Java.“" , CategoryId=javaId}
                };

                foreach (var joke in jokes)
                {
                    try
                    {
                        await client.CreateJokeAsync(joke).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating category: {ex.ToString()}");
                    }
                }
            }

            Console.WriteLine("jokes data initialized");
        }
    }
}