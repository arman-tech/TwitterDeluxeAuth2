
namespace TwitterThrice.common {
    public class Tweet {
        public Guid Id { get; set; }
        public string MemberId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; }
    }
}
