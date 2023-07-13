using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using WarehouseAPI.Models.Dto;
using WarehouseApp.ApiAccess;

namespace WarehouseApp.WinForms
{
    public enum ArticlesWindowMode
    {
        EDIT_LIST,
        SELECT_ARTICLE
    }

    public partial class ArticlesForm : Window
    {

        private ArticlesWindowMode mode;

        public delegate void selection_callback(ArticleDto article);
        public event selection_callback selected_getData_CallBack;

        public ArticlesForm(ArticlesWindowMode mode = ArticlesWindowMode.SELECT_ARTICLE)
        {
            InitializeComponent();

            this.mode = mode;
            this.Loaded += Articles_Loaded;
            this.selected_getData_CallBack += DefaultCallBack;
        }

        private void DefaultCallBack(ArticleDto i) { }

        private void Articles_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGridDataSource();
        }

        private void lb_dbl_click(object sender, MouseButtonEventArgs e)
        {
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
		}

        private async void ArticleAddition_getData_CallBack(/*ItemArticle */ArticleDto article)
        {
            bool res = await ApiWrapper.CreateArticle(article);
			if (!res)
			{
				MessageBox.Show("Nie udało się dodać artykułu. Sprawdź swoją rolę. Tylko Admin może dodawać i modyfikować dane magazynowe.");
			}
			RefreshGridDataSource();
        }

        private async void ArticleUpdate_getData_CallBack(/*ItemArticle */ArticleDto article)
        {
			bool res = await ApiWrapper.UpdateArticle(article);
			if (!res)
			{
				MessageBox.Show("Nie udało się zmodyfikować danych artykułu. Sprawdź swoją rolę. Tylko Admin może dodawać i modyfikować dane magazynowe.");
			}
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
			if (selectedArticle != null)
            {
				bool res = await ApiWrapper.RemoveArticle(selectedArticle.ArticleID);
				if (!res)
				{
					MessageBox.Show("Nie udało się usunąć artykułu. Sprawdź swoją rolę. Tylko Admin może usuwać dane magazynowe.");
				}
				RefreshGridDataSource();
            }
		}
    }
}
