using System;
using System.Windows;
using WarehouseApp.ApiAccess;
using WarehouseApp.WinForms;

namespace WarehouseApp
{
    public partial class MainWindow : Window
    {
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
        }

        private void btn_NewDoc(object sender, RoutedEventArgs e)
        {
            DocumentForm documentWin = new DocumentForm();
            documentWin.ShowDialog();
        }

        private void btn_ListDoc(object sender, RoutedEventArgs e)
        {
            DocumentsForm documentsWin = new DocumentsForm();
            documentsWin.ShowDialog();
        }

        private void btn_StoreData(object sender, RoutedEventArgs e)
        {
            ArticlesForm articlesWin = new ArticlesForm(ArticlesWindowMode.EDIT_LIST);
            articlesWin.ShowDialog();
        }

        private void btn_ListContract(object sender, RoutedEventArgs e)
        {
            ContractsForm contractsWin = new ContractsForm(ContractsWindowMode.EDIT_LIST);
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
			if (res ?? false)
			{
				ApiWrapper.Token = Token = form.Token;
				InitialButtonSettings(true);
			}
		}
	}
}
