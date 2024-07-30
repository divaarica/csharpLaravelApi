using api.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApiProject.Model
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        //pour envoyer des requêtes HTTP

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://127.0.0.1:8000/api/")
            };

            //A revoir cette partie
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<Produit> GetProduitAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"produits/{id}");
            response.EnsureSuccessStatusCode();

            String responseBody = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ProduitResponse>(responseBody);

            var produits = apiResponse.Data;

            return JsonConvert.DeserializeObject<Produit>(responseBody);
        }

        public async Task<Produit[]> GetProduitsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("produits");
            response.EnsureSuccessStatusCode();

            String responseBody = await response.Content.ReadAsStringAsync();

            var apiResponse =  JsonConvert.DeserializeObject<ProduitsResponse>(responseBody);

            var produits = apiResponse.Data;

            return produits;
        }

        public async Task<bool> StoreProduitAsync(Produit produit)
        {
            bool rep = false;
            var values = new Dictionary<string, string>
            {
                { "code", produit.Code},
                { "libelle", produit.Libelle },
                { "prix", produit.Prix.ToString() },
            };
            var content = new FormUrlEncodedContent(values);

            try
            {
                var response = await _httpClient.PostAsync("produits", content);
                if (response.IsSuccessStatusCode)
                {
                    rep = true;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erreur : {responseBody}", "Erreur lors de l'ajout du produit");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}", "Erreur lors de l'ajout du produit");
            }

            return rep;

        }

        public async Task<bool> UpdateProduitAsync(int id, Produit produit)
        {
            bool rep = false;
            var values = new Dictionary<string, string>
            {
                { "code", produit.Code},
                { "libelle", produit.Libelle },
                { "prix", produit.Prix.ToString() },
            };
            var content = new FormUrlEncodedContent(values);

            try
            {
                var response = await _httpClient.PutAsync($"produits/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    rep = true;
                }
                else
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erreur : {responseBody}", "Erreur lors de l'ajout du produit");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception : {ex.Message}", "Erreur lors de l'ajout du produit");
            }

            return rep;

        }

        public async Task DeleteProduitAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"produits/{id}");
            response.EnsureSuccessStatusCode();

        }

        public bool deleteProduct()
        {
            //Produit produit = new Produit();
            var rep = false;

            var idProduit = int.Parse(dgProduit.CurrentRow.Cells[0].Value.ToString());

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(System.Configuration.ConfigurationSettings.AppSettings["ServeurApiURL"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Laravel Rest Api
                    var response = client.DeleteAsync($"api/produit/{idProduit}").Result;

                    // Wcf Soap Api
                    //var response = client.DeleteAsync($"api/Produits/DeleteProduit/{idProduit}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        rep = true;
                        var responseData = response.Content.ReadAsStringAsync().Result;
                        Produit produit = JsonConvert.DeserializeObject<Produit>(responseData);

                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {

            }
            return rep;
        }

        // var response = (Id.Equals("0")) ? client.PostAsync("api/produit", content).Result : client.PutAsync($"api/produit/{int.Parse(Id)}", content).Result;
        private void btnModifier_Click(object sender, EventArgs e)
        {
            Produit p = new Produit();
            p.idProduit = int.Parse(txtIdProduit.Text);
            p.CodeProduit = txtCode.Text;
            p.DesignationProduit = txtDesignation.Text;
            p.PrixAchat = double.Parse(txtPrixAchat.Text);
            p.PrixUnitaire = double.Parse(txtPrixVente.Text);
            p.QuantiteMinimale = int.Parse(txtQteMin.Text);
            p.QuantiteMaximale = int.Parse(txtQteMax.Text);
            p.CodeCategorie = "C001";
            AddProduct(p);
            effacer();
        }

    }
}
