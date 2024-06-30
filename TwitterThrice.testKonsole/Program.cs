using System.Data.SqlClient;
using Dapper;
using Bogus;
using Microsoft.Extensions.Configuration;
using TwitterThrice.common;
using System.ComponentModel.DataAnnotations;


class Program {

    static void CreateRecords(SqlConnection connection, Faker faker, string username, string email, string password) {
        var memberId = Guid.NewGuid();
        var hashedEmail = BCrypt.Net.BCrypt.HashPassword(email);
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var createdDate = faker.Date.Past(2);

        connection.Execute("INSERT INTO Members (Id, Username, Email, Password, CreatedDate) VALUES (@Id, @Username, @Email, @Password, @CreatedDate)",
            new { Id = memberId, Username = username, Email = hashedEmail, Password = hashedPassword, CreatedDate = createdDate });

        // Generate between 0 to 5 Tweets per Member
        var tweetCount = faker.Random.Int(0, 5);

        for (int j = 0; j < tweetCount; j++) {
            var tweetId = Guid.NewGuid();
            var message = faker.Lorem.Sentences();
            var postedDate = faker.Date.Recent();

            // truncate the message to 140 characters
            message = message.TruncateMessage(Constants.MaxTweetMessageLength);

            connection.Execute("INSERT INTO Tweets (Id, MemberId, Message, PostedDate) VALUES (@Id, @MemberId, @Message, @PostedDate)",
                new { Id = tweetId, MemberId = memberId, Message = message, PostedDate = postedDate });
        }
    }

    static void Main(string[] args) {
        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfigurationRoot configuration = builder.Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");


        // max record is 25000
        var numberOfMembers = 25000;

        using (var connection = new SqlConnection(connectionString)) {
            connection.Open();

            var faker = new Faker();

            // create homer simpson
            CreateRecords(connection, faker, "homer", "homer@hotmail.com", "demo");

            // create bart simpson
            CreateRecords(connection, faker, "bart", "bart@hotmail.com", "demo");


            // Generate Members
            for (int i = 0; i < numberOfMembers; i++) {

                CreateRecords(connection, faker, Guid.NewGuid().ToString(), faker.Internet.Email(), faker.Internet.Password());

                // refresh one line in console displaying the percentage completed
                Console.SetCursorPosition(0, Console.CursorTop);
                // Calculate the percentage of completion
                double percentageCompleted = ((double)i / numberOfMembers) * 100;

                // Display progression on console
                Console.Write($"\rGenerated {i} records. Completion: {percentageCompleted:0.00}%");
            }
        }

        Console.WriteLine("Database populated with test data.");
    }
}