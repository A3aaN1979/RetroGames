using System.Collections.Generic;


namespace GameCollection
{
    public class Platform
    {
        public string Name { get; set; }
        public List<Game> Games { get; set; }

        public Platform(string name)
        {
            Name = name;
            Games = new List<Game>();
        }
    }
}
