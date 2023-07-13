using System.Windows;
//using Magazynier.DatabaseAccess;
using Magazynier.WinForms;
using System;
using WarehouseApp.ApiAccess;
using WarehouseApp.WinForms;

namespace Magazynier
{
    public partial class MainWindow : Window
    {
        //private MagDbContext db;
        private string Token = String.Empty;

        private void InitialButtonSettings(bool ifEnable)
        {
            btnNewDoc.IsEnabled = ifEnable;
            btnListDoc.IsEnabled = ifEnable;
            btnListCont.IsEnabled = ifEnable;
			btnStoreData.IsEnabled = ifEnable;
        }

        public MainWindow()
        {
            InitializeComponent();

            InitialButtonSettings(false);

            //db = new MagDbContext();
        }

        private void btn_NewDoc(object sender, RoutedEventArgs e)
        {
            DocumentForm documentWin = new DocumentForm();//db);
            documentWin.ShowDialog();
        }

        private void btn_ListDoc(object sender, RoutedEventArgs e)
        {
            DocumentsForm documentsWin = new DocumentsForm();//db);
            documentsWin.ShowDialog();
        }

        private void btn_StoreData(object sender, RoutedEventArgs e)
        {
            ArticlesForm articlesWin = new ArticlesForm(/*db, */ArticlesWindowMode.EDIT_LIST);
            articlesWin.ShowDialog();
        }

        private void btn_ListContract(object sender, RoutedEventArgs e)
        {
            ContractsForm contractsWin = new ContractsForm(/*db, */ContractsWindowMode.EDIT_LIST);
            contractsWin.ShowDialog();
        }

        private void btn_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

		private async void btn_Login(object sender, RoutedEventArgs e)
		{
            InitialButtonSettings(false);
            ApiWrapper.Token = String.Empty;
			LoginForm form = new LoginForm();
            bool? res = form.ShowDialog();
			if (res ?? false)            {
				ApiWrapper.Token = Token = form.Token;
				InitialButtonSettings(true);
			}
		}
	}
}
