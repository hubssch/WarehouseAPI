using System.Windows;

// using Magazynier.DatabaseAccess;

using WarehouseAPI.Models.Dto;

namespace Magazynier.WinForms
{
    public enum ArticleWindowMode
    {
        AMOUNT, // do zmiany ilosci sztuk artykulu na dokumencie
        EDIT,   // edytujemy artykul w lisie w bazie (wlacznie z iloscia)
        ADD     // dodajemy artykul do listy artykulow w bazie
    }

    public partial class ArticleForm : Window
    {
        public delegate void return_article_callback(/*ItemArticle */ArticleDto article);
        public event return_article_callback getData_CallBack;

		private /*ItemArticle */ArticleDto stored_article;

        private ArticleWindowMode EditMode;

        // default is for new
        public ArticleForm()
        {
            InitializeComponent();

            this.tb_Amount.Clear();
            this.tb_Amount.Text = "0";
            this.tb_Desc.Clear();
            this.tb_Name.Clear();

            this.stored_article = new /*ItemArticle */ArticleDto();

            this.EditMode = ArticleWindowMode.ADD;

            this.getData_CallBack += this.DefaultCallBack;
        }

        public ArticleForm(ArticleWindowMode mode, /*ItemArticle */ArticleDto article) : this()
        {
            this.EditMode = mode;
            this.stored_article = article;

            tb_Name.Text = article.Name;
            tb_Desc.Text = article.Description;
            tb_Amount.Text = article.Amount.ToString();

            if (mode == ArticleWindowMode.AMOUNT)
            {
                this.tb_Name.IsEnabled = false;
                this.tb_Desc.IsEnabled = false;
            }
        }

        private void btn_SaveArticle(object sender, RoutedEventArgs e)
        {
            this.stored_article.Name = tb_Name.Text;
            this.stored_article.Amount = int.Parse(tb_Amount.Text); // TODO: funkcja do konwersji na liczby + try catch
            this.stored_article.Description = tb_Desc.Text;

            this.Close();

            getData_CallBack(this.stored_article);
        }

        private void btn_CancelArticle(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DefaultCallBack(/*ItemArticle */ArticleDto i) { }
    }
}
