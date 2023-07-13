using System;
using System.Windows;
using WarehouseAPI.Models.Dto;
using WarehouseApp.ApiAccess;

namespace WarehouseApp.WinForms
{
	public partial class LoginForm : Window
	{
		private string token = String.Empty;
		public string Token { get { return token; } }

		public LoginForm()
		{
			InitializeComponent();
		}

		private async void btn_LogIn(object sender, RoutedEventArgs e)
		{
			LoggedUserRecordDto res = await ApiWrapper.Login(tb_mail.Text, pb_pass.Password.ToString());
			if (!res.IsLogged)
			{
				MessageBox.Show("Login Failed!");
				return;
			}
			MessageBox.Show(String.Format("Użytkownik zalogowany. Twoja rola: {0}", res.UserItem.Role.Name));
			token = res.Token;
			this.DialogResult = true;
			this.Close();
		}
	}
}
