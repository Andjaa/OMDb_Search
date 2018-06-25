using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zadaca
{
    public partial class Preview : Form
    {
        public MovieCollection movieCollection_preview { get; set; }
        public Movie movie_preview { get; set; }
       
        public Preview()
        {
            InitializeComponent();
        }
        bool showInformationsStatus = false;

        private void Preview_Load(object sender, EventArgs e)
        {
            string apiKey = "c58d92ca";

            RestClient restClient = new RestClient($"http://www.omdbapi.com/?apikey={apiKey}&");
            RestRequest restRequest = new RestRequest(Method.GET);
            restRequest.AddQueryParameter("i", movie_preview.imdbID.ToString());
            restRequest.AddQueryParameter("plot", "full");

            var result = restClient.Execute(restRequest);
            string data = result.Content;
            MovieDetails movieDetails = JsonConvert.DeserializeObject<MovieDetails>(data);

            txtDescription.Text = movieDetails.Plot;
            pictureBox1.ImageLocation = movieDetails.Poster;

            //labels
            lblTitle.Text = movieDetails.Title;
            lblGenre.Text = movieDetails.Genre;
            lblRuntime.Text = movieDetails.Runtime;
            lblYear.Text = movieDetails.Year;
            lblDirector.Text = movieDetails.Director;

            lblActors.Text = movieDetails.Actors;
            lblAwards.Text = movieDetails.Awards;
            lblCountry.Text = movieDetails.Country;
            lblLanguage.Text = movieDetails.Language;
            lblProduction.Text = movieDetails.Production;

        }

        private void btnInformations_Click(object sender, EventArgs e)
        {
            if (showInformationsStatus == true)
            {
                btnInformations.Text = "more informations ";
                groupBox3.Visible = false;
            }
            else if(showInformationsStatus== false)
            {
                btnInformations.Text = "less informations ";
                groupBox3.Visible = true;
            }
            showInformationsStatus = !showInformationsStatus;
        }

    
    }
}
