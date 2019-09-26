using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace GameCollection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbPlatform.Items.Add("NES");
            cmbPlatform.Items.Add("SNES");
            cmbPlatform.SelectedIndex = 0;
            ListGenres();
            lstGenre.SelectedItems.Clear();
        }

        public void ListGenres()
        {
            lstGenre.DataSource = Enum.GetValues(typeof(Genre)).Cast<Genre>().OrderBy(e => e.ToString()).ToList<Genre>();
        }


        private void AddGame()
        {
            string gameName = txtGameName.Text.ToString();
            string platformName = cmbPlatform.Text.ToString();
            string developerName = txtDeveloper.Text.ToString();
            string publisherName = txtPublisher.Text.ToString();
            string releaseDate = dtpReleaseDate.Value.ToShortDateString();

            string item = "";
            foreach (int i in lstGenre.SelectedIndices)
            {
                item += lstGenre.Items[i] + ", ";
            }
            item = item.Substring(0, item.Length - 2);

            string[] row = {gameName, platformName, developerName, publisherName, releaseDate, item};
            dgvGames.Rows.Add(row);
        }

        private void btnAddGame_Click(object sender, EventArgs e)
        {
            AddGame();
            txtGameName.Clear();
            cmbPlatform.SelectedIndex = 0;
            txtDeveloper.Clear();
            txtPublisher.Clear();
            dtpReleaseDate.Value = DateTime.Now;
            lstGenre.SelectedItems.Clear();
        }

        private void Export()
        {
            int numberOfRows = (dgvGames.RowCount - 1);
            int numberOfColums = (dgvGames.ColumnCount - 1);

            using (StreamWriter file = File.CreateText(@"output.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Collection collection = new Collection();
                for (int row = 0; row < numberOfRows; row++)
                {
                    Game game = new Game(dgvGames.Rows[row].Cells[0].Value.ToString(),
                                dgvGames.Rows[row].Cells[2].Value.ToString(),
                                dgvGames.Rows[row].Cells[3].Value.ToString(),
                                dgvGames.Rows[row].Cells[4].Value.ToString()
                                );

                    string[] genres = dgvGames.Rows[row].Cells[5].Value.ToString().Split(new[] { ", " }, StringSplitOptions.None);
                    foreach (string genre in genres)
                    {
                        Enum.TryParse(genre, out Genre outGenre);
                        game.AddGenre(outGenre);
                    }

                    Platform platform = null;
                    try
                    {
                        platform = collection.Platforms.FirstOrDefault(p => p.Name == dgvGames.Rows[row].Cells[1].Value.ToString());
                        platform.Games.Add(game);
                    }
                    catch (Exception e)
                    {
                        platform = new Platform(dgvGames.Rows[row].Cells[1].Value.ToString());
                        platform.Games.Add(game);
                        collection.Platforms.Add(platform);
                    }
                }
                serializer.Serialize(file, collection);
                MessageBox.Show("Export is done!");
            }
        }


        private void Import()
        {
            dgvGames.Rows.Clear();
            Collection collection = JsonConvert.DeserializeObject<Collection>(File.ReadAllText(@"output.json"));
            foreach (Platform platform in collection.Platforms)
            {
                foreach (Game game in platform.Games)
                {
                    string genreList = "";
                    foreach (Genre genre in game.Genres)
                    {
                        genreList += genre.ToString() + ", ";
                    }
                    genreList = genreList.Substring(0, genreList.Length - 2);
                    dgvGames.Rows.Add(game.Name, platform.Name, game.Developer, game.Publisher, game.ReleaseDate, genreList);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Import();
        }
    }
}
