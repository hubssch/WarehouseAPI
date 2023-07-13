using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Input;

// using Magazynier.DatabaseAccess;

using WarehouseAPI.Models.Dto;
using WarehouseApp.ApiAccess;

namespace Magazynier.WinForms
{
    public enum ArticlesWindowMode
    {
        EDIT_LIST,
        SELECT_ARTICLE
    }

    public partial class ArticlesForm : Window
    {
        //private MagDbContext dbContext;

        private ArticlesWindowMode mode;

        public delegate void selection_callback(/*ItemArticle*/ArticleDto article);
        public event selection_callback selected_getData_CallBack;

        public ArticlesForm(/*MagDbContext db, */ArticlesWindowMode mode = ArticlesWindowMode.SELECT_ARTICLE)
        {
            InitializeComponent();
            //this.dbContext = db;
            this.mode = mode;
            this.Loaded += Articles_Loaded;
            this.selected_getData_CallBack += DefaultCallBack;
        }

        private void DefaultCallBack(/*ItemArticle */ArticleDto i) { }

        private void Articles_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGridDataSource();
        }

        private void lb_dbl_click(object sender, MouseButtonEventArgs e)
        {
            //DbArticle selectedArticle = (DbArticle) dg_list.SelectedItem;
            //ItemArticle selectedItemArticle = new ItemArticle(selectedArticle);
            ArticleDto selectedItemArticle = (ArticleDto) dg_list.SelectedItem;


			if (mode == ArticlesWindowMode.SELECT_ARTICLE)
            {
                this.Close();
                selectedItemArticle.Amount = 0; /* wybrany towar zawsze zwracam bez ilosci (stan magazynowy mozna zmienic z widoku listy, badz zmieni sie on po zapisaniu dokumentu */
                selected_getData_CallBack(selectedItemArticle);
            }
            else
            {
                ArticleForm articleEditWin = new ArticleForm(ArticleWindowMode.EDIT, selectedItemArticle);
                articleEditWin.getData_CallBack += ArticleUpdate_getData_CallBack;
                articleEditWin.ShowDialog();
            }
        }

        private async void RefreshGridDataSource()
        {
            this.dg_list.ItemsSource = null;
            this.dg_list.ItemsSource = await ApiWrapper.GetArticles();
			//this.dg_list.ItemsSource = dbContext.Articles.OrderBy(a => a.Name).ToList();
		}

        private async void ArticleAddition_getData_CallBack(/*ItemArticle */ArticleDto article)
        {
            bool res = await ApiWrapper.CreateArticle(article);
			if (!res)
			{
				MessageBox.Show("Nie udało się dodać artykułu. Sprawdź swoją rolę. Tylko Admin może dodawać i modyfikować dane magazynowe.");
			}
			/*DbArticle added_article = new DbArticle { Name = article.Name, Description = article.Description, Amount = article.Amount };
            dbContext.Add(added_article);
            dbContext.SaveChanges();*/
			RefreshGridDataSource();
        }

        private async void ArticleUpdate_getData_CallBack(/*ItemArticle */ArticleDto article)
        {
			bool res = await ApiWrapper.UpdateArticle(article);
			if (!res)
			{
				MessageBox.Show("Nie udało się zmodyfikować danych artykułu. Sprawdź swoją rolę. Tylko Admin może dodawać i modyfikować dane magazynowe.");
			}
			/*DbArticle updated_article = dbContext.Articles.FirstOrDefault(a => a.ArticleID == article.ArticleID);
            if (updated_article == null) return;
            updated_article.Name = article.Name;
            updated_article.Description = article.Description;
            updated_article.Amount = article.Amount;
            dbContext.Update(updated_article);
            dbContext.SaveChanges();*/
			RefreshGridDataSource();
        }

        private void btn_CancelArticleWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_AddArticle(object sender, RoutedEventArgs e)
        {
            ArticleForm articleWin = new ArticleForm();
            articleWin.getData_CallBack += ArticleAddition_getData_CallBack;
            articleWin.ShowDialog();
        }

        private async void btn_RemoveArticle(object sender, RoutedEventArgs e)
        {
            ArticleDto selectedArticle = (ArticleDto) dg_list.SelectedItem;
			//DbArticle selectedArticle = (DbArticle) dg_list.SelectedItem;
			if (selectedArticle != null)
            {
				bool res = await ApiWrapper.RemoveArticle(selectedArticle.ArticleID);
				if (!res)
				{
					MessageBox.Show("Nie udało się usunąć artykułu. Sprawdź swoją rolę. Tylko Admin może usuwać dane magazynowe.");
				}
				/*dbContext.Articles.Remove(selectedArticle);
                dbContext.SaveChanges();*/
				RefreshGridDataSource();
            }
		}
    }
}
