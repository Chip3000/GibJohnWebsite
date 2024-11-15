namespace GibJohnWebsite.Models
{
    public class TutorsClass
    {
        private static int _idCounter = 0;

        public string Id { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Subject { get; set; }

        public TutorsClass()
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
