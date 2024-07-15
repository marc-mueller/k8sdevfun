namespace DevFun.Common.Entities
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "ok for sample")]
    public class DevJoke
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string Tags { get; set; }
        public int LikeCount { get; set; }

        public virtual int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}