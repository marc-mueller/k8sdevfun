﻿namespace DevFun.Api.System.Tests.Entities
{
    public class DevJoke
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string Tags { get; set; }
        public int LikeCount { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}