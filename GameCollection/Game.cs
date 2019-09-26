using System.Collections.Generic;

namespace GameCollection
{
    public class Game
    {
        public string Name { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public string ReleaseDate { get; set; }
        public List<Genre> Genres { get; set; }

        public Game(string name, string developer, string publisher, string releaseDate)
        {
            Name = name;
            Developer = developer;
            Publisher = publisher;
            ReleaseDate = releaseDate;
            Genres = new List<Genre>();
        }

        public void AddGenre(Genre genre)
        {
            Genres.Add(genre);
        }
    }
}
