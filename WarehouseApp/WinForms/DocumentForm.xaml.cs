using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Input;
using WarehouseAPI.Models.Dto;
using System.Windows.Controls;
using WarehouseAPI.Models;
using WarehouseApp.ApiAccess;

namespace WarehouseApp.WinForms
{
    public enum DocumentWindowMode
    {
        VIEW,
        ADD
    }

    public partial class DocumentForm : Window
    {

        private int documentID;
        private int contractID;
        private char docType;

        public DocumentForm()
        {
            InitializeComponent();

            tb_ContName.Clear();
            tb_NIP.Clear();
            tb_Address.Clear();
            tb_signature.Clear();
            documentID = 0;
            contractID = 0;
            docType = ' ';
        }

        public DocumentForm(DocumentWindowMode mode, DocumentDto doc) : this()
        {
            if (mode == DocumentWindowMode.VIEW)
            {
                documentID = doc.DocID;

                this.Loaded += DocumentData_Load;
            }
        }

        private async void DocumentData_Load(object sender, RoutedEventArgs e)
        {
            if (documentID != 0)
            {
                DocumentAllDataDto doc = await ApiWrapper.GetDocument(documentID);

                tb_signature.Text = doc.Signature;

				SelectedContract_getData_CallBack(doc.Contract);

				if (doc.DocType == "WZ")
				{
					rb_out_doc.IsChecked = true;
				}
				else
				{
					rb_in_doc.IsChecked = true;
				}

				foreach (ArticleDto item in doc.Articles)
				{
					SelectedArticle_getData_CallBack(item);
				}
			}
        }

        private void btn_ChooseContract(object sender, RoutedEventArgs e)
        {
            ContractsForm contractsWin = new ContractsForm();
            contractsWin.selected_getData_CallBack += SelectedContract_getData_CallBack;
            contractsWin.ShowDialog();
        }

        private void SelectedContract_getData_CallBack(ContractorDto selectedContract)
        {
            tb_ContName.Text = selectedContract.Name;
            tb_Address.Text = String.Format("{0}, {1} {2}", selectedContract.Street, selectedContract.Code, selectedContract.Post);
            tb_NIP.Text = selectedContract.NIP;
            contractID = selectedContract.ContractID;
        }

        private async void btn_SaveDoc(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_signature.Text))
            {
                MessageBox.Show(this, "Nie wprowadzono sygnatury dla dokumentu", "Brak sygnatury", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (docType == ' ')
            {
                MessageBox.Show(this, "Nie wybrano typu dokumentu: przyjęcie / wydanie", "Typ dokumentu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (contractID == 0)
            {
                MessageBox.Show(this, "Wybierz lub dodaj nowego kontrahenta", "Kontrahent", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dg_DocArticles.Items.Count == 0)
            {
                MessageBox.Show(this, "Dokument zawiera pustą listę towarów", "Dokument", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DocumentAllDataDto savedDoc = new DocumentAllDataDto();

            if (documentID != 0) // means view mode
            {
                savedDoc.DocID = documentID;
            }

            savedDoc.Signature = tb_signature.Text;
            savedDoc.Date = DateTime.Now;
            savedDoc.DocType = docType.ToString();
            savedDoc.Contract = new ContractorDto { ContractID = contractID };

			savedDoc.Articles = new List<ArticleDto>();

			foreach (ArticleDto item in dg_DocArticles.Items)
			{
				savedDoc.Articles.Add(item);
			}

            bool res = false;

            if (documentID != 0) // means view mode
            {
                res = await ApiWrapper.UpdateDocument(savedDoc);
            }
            else
            {
                res = await ApiWrapper.CreateDocument(savedDoc);
            }
			if (!res)
			{
				MessageBox.Show("Nie udało się dodać dokumentu. Sprawdź swoją rolę. Tylko Admin może dodawać i modyfikować dane magazynowe.");
			}

			this.Close();
        }

        private void SelectedArticle_getData_CallBack(ArticleDto article)
        {
            dg_DocArticles.Items.Add(article);
        }

        private void btn_ChooseArticle(object sender, RoutedEventArgs e)
        {
            ArticlesForm articlesWin = new ArticlesForm();
            articlesWin.selected_getData_CallBack += SelectedArticle_getData_CallBack;
            articlesWin.ShowDialog();
        }

        private void btn_DelSelArticle(object sender, RoutedEventArgs e)
        {
            dg_DocArticles.Items.Remove(dg_DocArticles.SelectedItem);
        }

        private void btn_CancelDoc(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lb_dbl_click_articles(object sender, MouseButtonEventArgs e)
        {
            open_Amount_Windows();
        }

        private void keyup_articles(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                open_Amount_Windows();
            }
        }

        private void ArticleUpdate_getData_CallBack(ArticleDto returnedArticle)
        {
			ArticleDto selected = (ArticleDto)dg_DocArticles.SelectedItem;
            selected.Amount = returnedArticle.Amount;
            dg_DocArticles.Items.Refresh();
        }

        private void open_Amount_Windows()
        {
			ArticleDto selectedItemArticle = (ArticleDto)dg_DocArticles.SelectedItem;
            ArticleForm articleEditWin = new ArticleForm(ArticleWindowMode.AMOUNT, selectedItemArticle);
            articleEditWin.getData_CallBack += ArticleUpdate_getData_CallBack;
            articleEditWin.ShowDialog();
        }

        private void rb_in_doc_checked(object sender, RoutedEventArgs e)
        {
            docType = 'P';
        }

        private void rb_out_doc_checked(object sender, RoutedEventArgs e)
        {
            docType = 'W';
        }
    }
}
