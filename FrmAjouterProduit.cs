using api.Utils;
using ApiProject.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace api
{
    public partial class FrmAjouterProduit : Form
    {

        ApiService apiService = new ApiService();
        public FrmAjouterProduit()
        {
            InitializeComponent();
            btnEffacer.Enabled = false;
            btnModifier.Enabled = false;
            btnSupprimer.Enabled = false;
        }

        private async Task LoadPhotosAsync()
        {
            var apiService = new ApiService();
            var photos = await apiService.GetProduitsAsync();
            dgProduit.DataSource = photos;
            
        }

        private async void FrmAjouterProduit_Load(object sender, EventArgs e)
        {
            await LoadPhotosAsync();
           
        }

        private void Effacer()
        {
            txtCode.Clear();
            txtLibelle.Clear();
            txtPrix.Clear();
            txtLibelle.Focus();
            txtId.Clear();
        }

        private void Viderlbl()
        {
            lblCode.Visible = false;
            lblCode.Text = "";
            lblPrix.Visible = false;
            lblPrix.Text = "";
            lblLibelle.Visible = false;
            lblLibelle.Text = "";
        }

        private bool ValiderTextBox()
        {
            bool valid = true;
            Viderlbl();

            if (!CheckEnter.IsAlphaNumeric(txtCode.Text) || string.IsNullOrWhiteSpace(txtCode.Text))
            {
                lblCode.Visible = true;
                if (string.IsNullOrWhiteSpace(txtCode.Text))
                {
                    lblCode.Text = "le code ne peut etre une chaine vide !";
                }
                else
                {
                    lblCode.Text = "Le code ne peux contenir que des chiffres ou des letrres !";
                }

                //MessageBox.Show("Erreur : le code ne peux pas contenir de caracteres speciaux!.");
                valid = false;

            }
            if (!CheckEnter.IsAlphaNumericSpace(txtLibelle.Text) || string.IsNullOrWhiteSpace(txtLibelle.Text))
            {
                lblLibelle.Visible = true;
                if (string.IsNullOrWhiteSpace(txtLibelle.Text))
                {
                    lblLibelle.Text = "La designation ne peux etre une chaine vide !";
                }
                else
                {
                    lblLibelle.Text = "La designation ne peux contenir que des lettres !";
                }

                //MessageBox.Show("");
                valid = false;

            }
            if (!CheckEnter.checkIsNumber(txtPrix.Text)) // Vérifier la troisième TextBox
            {

                lblPrix.Visible = true;
                lblPrix.Text = "le prix unitaire doit etre un entier !";
                //MessageBox.Show("Erreur : le prix unitaire doit etre un nombre entier");
                valid = false;

            }

            return valid;

        }

        private async void btnValider_Click(object sender, EventArgs e)
        {

            if (ValiderTextBox())
            {

                Produit produit = new Produit();
                produit.Code = txtCode.Text;
                produit.Libelle = txtLibelle.Text;
                produit.Prix = int.Parse(txtPrix.Text);

                //AddProduit(produit);
                await apiService.StoreProduitAsync(produit);

                await LoadPhotosAsync();
                Effacer();
                Viderlbl();




            }

        }


        private void btnSelectionner_Click(object sender, EventArgs e)
        {

            Viderlbl();
            txtId.Text = dgProduit.CurrentRow.Cells[0].Value.ToString();
            txtCode.Text = dgProduit.CurrentRow.Cells[1].Value.ToString();
            txtLibelle.Text = dgProduit.CurrentRow.Cells[2].Value.ToString();
            txtPrix.Text = dgProduit.CurrentRow.Cells[3].Value.ToString();
            btnValider.Enabled = false;
            btnEffacer.Enabled = true;
            btnModifier.Enabled = true;
            btnSupprimer.Enabled = true;

        }

        private async void btnModifier_Click(object sender, EventArgs e)
        {
            if (ValiderTextBox())
            {
                Produit produit = new Produit();
                produit.Code = txtCode.Text;
                produit.Libelle = txtLibelle.Text;
                produit.Prix = int.Parse(txtPrix.Text);
                int id = int.Parse(txtId.Text);
                await apiService.UpdateProduitAsync(id, produit);

                await LoadPhotosAsync();
                Effacer();
                Viderlbl();
            }

        }

        private void btnEffacer_Click(object sender, EventArgs e)
        {
            Viderlbl();
            Effacer();
            btnValider.Enabled = true;
            btnEffacer.Enabled = false;
            btnModifier.Enabled = false;
            btnSupprimer.Enabled = false;

        }


        public bool AddProduit(Produit emp)
        {
            bool rep = false;
            string Id = emp.Id > 0 ? emp.Id.ToString() : "0";
            var values = new Dictionary<string, string>
                    {
                       { "id", Id },
                       { "libelle", emp.Libelle },
                       { "code", emp.Code },
                       { "prix", emp.Prix.ToString() },
                    };
            var content = new FormUrlEncodedContent(values);
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://127.0.0.1:8000/api/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsync("produits", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        rep = true;
                    }
                    else
                    {
                        var responseBody = response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Erreur : {responseBody}", "Erreur lors de l'ajout du produit");

                    }

                }
            }
            catch (Exception ex)
            {

            }
            return rep;
        }

        private async void btnSupprimer_Click(object sender, EventArgs e)
        {

            int id = int.Parse(txtId.Text);
            await apiService.DeleteProduitAsync(id);

            await LoadPhotosAsync();
            Effacer();
            Viderlbl();

        }
    }
}
