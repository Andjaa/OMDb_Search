using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zadaca;

namespace Zadaca
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int page = 1;
        string param = "";
        int lastPage = 1;

        private void btnSearch_Click(object sender, EventArgs e)
        {
            param = txtTitle.Text.ToString();
            string apiKey = "c58d92ca";

            Cursor = Cursors.WaitCursor;
            RestClient restClient = new RestClient($"http://www.omdbapi.com/?apikey={apiKey}&");
            RestRequest restRequest = new RestRequest(Method.GET);
           

            restRequest.AddQueryParameter("s", param);
            restRequest.AddQueryParameter("page", page.ToString());

            var result = restClient.Execute(restRequest);
            string data = result.Content;
           
            MovieCollection movieCollection = JsonConvert.DeserializeObject<MovieCollection>(data);

            if (movieCollection.Response == "False")
            {
                MessageBox.Show("Movie not found!");
            }
            else
            {

                foreach (Movie movie in movieCollection.Search)
                {
                    listBox1.Items.Add(movie);
                }
            }

            lastPage = calculateLastPage(movieCollection.totalResults);
            buttonVisibility();
            lblPageNumber.Text = page.ToString();
            Cursor = Cursors.Default;  
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (txtTitle.Text.Length > 0)
            {
                btnSearch.Enabled = true;
            }
            else
            {
                btnSearch.Enabled = false;
            }
            page = 1;
            listBox1.Items.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                using (Preview preview = new Preview())
                {
                    preview.movie_preview = (Movie)listBox1.SelectedItem;
                    preview.ShowDialog(this);
                }
            }
        }

        private void txtTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            lblPageNumber.Text = "";
            txtTitle.Text = "";
            btnPreviousPage.Visible = false;
            btnFirstPage.Visible = false;
            btnLastPage.Visible = false;
            btnNextPage.Visible = false;
            listBox1.Items.Clear();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            page++;
            load();
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            page--;
            load();
        }
        private void load()
        {
            listBox1.Items.Clear();
            btnSearch.PerformClick();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            page = lastPage;
            load();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            page = 1;
            load();
        }
        private void buttonVisibility()
        {
            if (page == 1)
            {
                btnPreviousPage.Visible = false;
                btnFirstPage.Visible = false;
                btnNextPage.Visible = true;
                btnLastPage.Visible = true;
            }
            else if (page == 2)
            {
                btnPreviousPage.Visible = true;
                btnFirstPage.Visible = false;
                btnNextPage.Visible = true;
                btnLastPage.Visible = true;
            }
            else if (page == lastPage - 1)
            {
                btnPreviousPage.Visible = true;
                btnFirstPage.Visible = true;
                btnNextPage.Visible = true;
                btnLastPage.Visible = false;
            }
            else if (page == lastPage)
            {
                btnPreviousPage.Visible = true;
                btnFirstPage.Visible = true;
                btnNextPage.Visible = false;
                btnLastPage.Visible = false;
            }
            else
            {
                btnPreviousPage.Visible = true;
                btnFirstPage.Visible = true;
                btnLastPage.Visible = true;
                btnNextPage.Visible = true;
            }
        }

        int calculateLastPage (string totalResults)
        {
            int results = Convert.ToInt16(totalResults);
            if (results % 10 == 0)
            {
                return results / 10;
            }
            else
            {
                return (results / 10) + 1;
            }
        }

      
    }
}
