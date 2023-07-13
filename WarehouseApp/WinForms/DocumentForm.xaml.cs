using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Input;
//using Magazynier.DatabaseAccess;
using WarehouseAPI.Models.Dto;
using System.Windows.Controls;
using WarehouseAPI.Models;
using WarehouseApp.ApiAccess;

namespace Magazynier.WinForms
{
    public enum DocumentWindowMode
    {
        VIEW,
        ADD
    }

    public partial class DocumentForm : Window
    {
        //private MagDbContext dbContext;

        private int documentID;
        private int contractID;
        private char docType;

        public DocumentForm()//MagDbContext context)
        {
            InitializeComponent();

            tb_ContName.Clear();
            tb_NIP.Clear();
            tb_Address.Clear();
            tb_signature.Clear();
            //dbContext = context;
            documentID = 0;
            contractID = 0;
            docType = ' ';
        }

        public DocumentForm(/*MagDbContext context, */DocumentWindowMode mode, /*ItemDocument*/DocumentDto doc) : this()//context)
        {
            if (mode == DocumentWindowMode.VIEW)
            {
                documentID = doc.DocID;
                
                /*tb_ContName.IsEnabled = false;
                tb_NIP.IsEnabled = false;
                tb_signature.IsEnabled = false;
                tb_Address.IsEnabled = false;

                rb_in_doc.IsEnabled = false;
                rb_out_doc.IsEnabled = false;

                btn_save.IsEnabled = false;
                btn_choose_article.Visibility = Visibility.Hidden;
                btn_choose_contract.Visibility = Visibility.Hidden;
                btn_delete_article.Visibility = Visibility.Hidden;

                dg_DocArticles.MouseDoubleClick -= lb_dbl_click_articles;*/

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
				/*DbDocument curr_doc = dbContext.Documents.FirstOrDefault(d => d.DocID == documentID);
                if (curr_doc == null) return;
                DbContract curr_contract = dbContext.Contracts.FirstOrDefault(c => c.ContractID == curr_doc.ContractID);
                if (curr_contract == null) return;
                var db_items =
                    from item in dbContext.Items
                    join article in dbContext.Articles
                    on item.ArticleID equals article.ArticleID
                    where item.DocID == documentID
                    select new
                    {
                        ArticleID = article.ArticleID,
                        Name = article.Name,
                        Description = article.Description,
                        Amount = item.Amount
                    };

                tb_signature.Text = curr_doc.Signature;

                if (curr_doc.Operation == 'W')
                {
                    rb_out_doc.IsChecked = true;
                }
                else
                {
                    rb_in_doc.IsChecked = true;
                }

                SelectedContract_getData_CallBack(new ItemContract(curr_contract));

                foreach (var item in db_items)
                {
                    SelectedArticle_getData_CallBack(new ItemArticle(item.ArticleID, item.Name, item.Description, item.Amount));
                }*/
			}
        }

        private void btn_ChooseContract(object sender, RoutedEventArgs e)
        {
            ContractsForm contractsWin = new ContractsForm();// dbContext);
            contractsWin.selected_getData_CallBack += SelectedContract_getData_CallBack;
            contractsWin.ShowDialog();
        }

        private void SelectedContract_getData_CallBack(/*ItemContract */ContractorDto selectedContract)
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

			/*DocumentDataDto

            var transaction = dbContext.Database.BeginTransaction();
            DbDocument doc = new DbDocument();
            doc.Signature = tb_signature.Text;
            doc.Date = DateTime.Now;
            doc.Operation = docType;
            doc.ContractID = contractID;

            dbContext.Add(doc);
            dbContext.SaveChanges();

            int multiplier = docType == 'W' ? -1 : 1; // dla WZ odejmujemy, dla PZ dodajemy

            foreach (ItemArticle item in dg_DocArticles.Items)
            {
                DbItem dbitem = new DbItem { DocID = doc.DocID, ArticleID = item.ArticleID, Amount = item.Amount };
                dbContext.Add(dbitem);
                DbArticle dbarticle = dbContext.Articles.FirstOrDefault(a => a.ArticleID == item.ArticleID);
                dbarticle.Amount += multiplier * item.Amount;
                dbContext.Update(dbarticle);
                dbContext.SaveChanges();
            }
            
            transaction.Commit();*/

			this.Close();
        }

        private void SelectedArticle_getData_CallBack(/*ItemArticle */ArticleDto article)
        {
            dg_DocArticles.Items.Add(article);
        }

        private void btn_ChooseArticle(object sender, RoutedEventArgs e)
        {
            ArticlesForm articlesWin = new ArticlesForm();// dbContext);
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

        private void ArticleUpdate_getData_CallBack(/*ItemArticle */ArticleDto returnedArticle)
        {
			/*ItemArticle */
			ArticleDto selected = (/*ItemArticle */ArticleDto)dg_DocArticles.SelectedItem;
            selected.Amount = returnedArticle.Amount;
            dg_DocArticles.Items.Refresh();
        }

        private void open_Amount_Windows()
        {
			/*ItemArticle */
			ArticleDto selectedItemArticle = (/*ItemArticle */ArticleDto)dg_DocArticles.SelectedItem;
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
