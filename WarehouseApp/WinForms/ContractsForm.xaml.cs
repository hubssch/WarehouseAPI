using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

//using Magazynier.DatabaseAccess;

using WarehouseAPI.Models;
using WarehouseAPI.Models.Dto;
using WarehouseApp.ApiAccess;

namespace Magazynier.WinForms
{
    public enum ContractsWindowMode
    {
        EDIT_LIST,
        SELECT_CONTRACT
    }

    public partial class ContractsForm : Window
    {
        //private MagDbContext dbContext;

        private ContractsWindowMode mode;

        public delegate void selection_callback(/*ItemContract */ContractorDto contract);
        public event selection_callback selected_getData_CallBack;

        private void DefaultCallBack(/*ItemContract */ ContractorDto i) { }

        public ContractsForm(/*MagDbContext context, */ContractsWindowMode mode = ContractsWindowMode.SELECT_CONTRACT)
        {
            InitializeComponent();
            //this.dbContext = context;
            this.mode = mode;
            this.Loaded += Contracts_Loaded;
            this.selected_getData_CallBack += DefaultCallBack;
        }

        private void Contracts_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGridDataSource();
        }

        private async void RefreshGridDataSource()
        {
            this.dg_list.ItemsSource = null;
			this.dg_list.ItemsSource = await ApiWrapper.GetContactors();
		}

        private void lb_dbl_click(object sender, MouseButtonEventArgs e)
        {
            ContractorDto selectedItemContract = (ContractorDto)dg_list.SelectedItem;
			//DbContract selectedContract = (DbContract)dg_list.SelectedItem;
			//ItemContract selectedItemContract = new ItemContract(selectedContract);

			if (mode == ContractsWindowMode.SELECT_CONTRACT)
            {
                this.Close();
                selected_getData_CallBack(selectedItemContract);
            }
            else
            {
                ContractForm contractEditWin = new ContractForm(ContractWindowMode.EDIT, selectedItemContract);
                contractEditWin.getData_CallBack += ContractUpdate_getData_CallBack;
                contractEditWin.ShowDialog();
            }
        }

        private async void ContractAddition_getData_CallBack(/*ItemContract*/ContractorDto contract)
        {
            bool res = await ApiWrapper.CreateContract(contract);
            if (!res)
            {
                MessageBox.Show("Nie udało się dodać kontrahenta. Sprawdź swoją rolę. Tylko Admin może dodawać i modyfikować dane magazynowe.");
            }
            /*DbContract added_contract = new DbContract { Name = contract.Name, NIP = contract.NIP, Code = contract.Code, Post = contract.Post, Street = contract.Post };
            dbContext.Add(added_contract);
            dbContext.SaveChanges();*/
            RefreshGridDataSource();
        }

        private async void ContractUpdate_getData_CallBack(/*ItemContract */ContractorDto contract)
        {
			bool res = await ApiWrapper.UpdateContractor(contract);
			if (!res)
			{
				MessageBox.Show("Nie udało się zmodyfikować danych kontrahenta. Sprawdź swoją rolę. Tylko Admin może dodawać i modyfikować dane magazynowe.");
			}
            /*DbContract updated_contract = dbContext.Contracts.FirstOrDefault(c => c.ContractID == contract.ContractID);
            if (updated_contract == null) return;
            updated_contract.Name = contract.Name;
            updated_contract.NIP = contract.NIP;
            updated_contract.Street = contract.Street;
            updated_contract.Code = contract.Code;
            updated_contract.Post = contract.Post;
            dbContext.Update(updated_contract);
            dbContext.SaveChanges();*/
            RefreshGridDataSource();
        }

        private void btn_AddContract(object sender, RoutedEventArgs e)
        {
            ContractForm contractWin = new ContractForm();
            contractWin.getData_CallBack += ContractAddition_getData_CallBack;
            contractWin.ShowDialog();
        }

        private void btn_CancelContractWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btn_RemoveContract(object sender, RoutedEventArgs e)
        {
            ContractorDto selectedContract = (ContractorDto)dg_list.SelectedItem;
			//DbContract selectedContract = (DbContract) dg_list.SelectedItem;
			if (selectedContract != null)
            {
                bool res = await ApiWrapper.RemoveContractor(selectedContract.ContractID);
				if (!res)
				{
					MessageBox.Show("Nie udało się usunąć kontrahenta. Sprawdź swoją rolę. Tylko Admin może usuwać dane magazynowe.");
				}
				/*dbContext.Contracts.Remove(selectedContract);
                dbContext.SaveChanges();*/
				RefreshGridDataSource();
            }
        }
    }
}
