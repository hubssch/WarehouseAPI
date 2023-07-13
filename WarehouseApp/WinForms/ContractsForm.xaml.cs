using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WarehouseAPI.Models;
using WarehouseAPI.Models.Dto;
using WarehouseApp.ApiAccess;

namespace WarehouseApp.WinForms
{
    public enum ContractsWindowMode
    {
        EDIT_LIST,
        SELECT_CONTRACT
    }

    public partial class ContractsForm : Window
    {

        private ContractsWindowMode mode;

        public delegate void selection_callback(ContractorDto contract);
        public event selection_callback selected_getData_CallBack;

        private void DefaultCallBack(ContractorDto i) { }

        public ContractsForm(ContractsWindowMode mode = ContractsWindowMode.SELECT_CONTRACT)
        {
            InitializeComponent();
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

        private async void ContractAddition_getData_CallBack(ContractorDto contract)
        {
            bool res = await ApiWrapper.CreateContract(contract);
            if (!res)
            {
                MessageBox.Show("Nie udało się dodać kontrahenta. Sprawdź swoją rolę. Tylko Admin może dodawać i modyfikować dane magazynowe.");
            }
            RefreshGridDataSource();
        }

        private async void ContractUpdate_getData_CallBack(ContractorDto contract)
        {
			bool res = await ApiWrapper.UpdateContractor(contract);
			if (!res)
			{
				MessageBox.Show("Nie udało się zmodyfikować danych kontrahenta. Sprawdź swoją rolę. Tylko Admin może dodawać i modyfikować dane magazynowe.");
			}
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
			if (selectedContract != null)
            {
                bool res = await ApiWrapper.RemoveContractor(selectedContract.ContractID);
				if (!res)
				{
					MessageBox.Show("Nie udało się usunąć kontrahenta. Sprawdź swoją rolę. Tylko Admin może usuwać dane magazynowe.");
				}
				RefreshGridDataSource();
            }
        }
    }
}
