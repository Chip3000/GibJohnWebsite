﻿namespace GibJohnWebsite.Models
{
    public class AddLessonClass
    {
        private static int _idCounter = 0;

        public string Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public string Tutor { get; set; }

        public AddLessonClass()
        {
            Id = GenerateId();
        }

        private string GenerateId()
        {
            _idCounter++;
            return _idCounter.ToString();
        }
    }
}