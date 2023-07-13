using System.Windows;
using WarehouseAPI.Models.Dto;

namespace WarehouseApp.WinForms
{
    public enum ContractWindowMode
    {
        EDIT,
        ADD
    }

    public partial class ContractForm : Window
    {
        public delegate void return_contract_callback(ContractorDto contract);
        public event return_contract_callback getData_CallBack;

        private ContractWindowMode EditMode;

        private ContractorDto stored_contract;

        public ContractForm()
        {
            InitializeComponent();

            this.EditMode = ContractWindowMode.ADD;

            this.tb_ContractName.Clear();
            this.tb_NIP.Clear();
            this.tb_Post.Clear();
            this.tb_PostalCode.Clear();
            this.tb_Street.Clear();

            this.stored_contract = new ContractorDto();

            this.getData_CallBack += DefaultCallBack;
        }

        public ContractForm(ContractWindowMode mode, ContractorDto contract) : this()
        {
            this.EditMode = mode;

            this.stored_contract = contract;

            this.tb_ContractName.Text = contract.Name;
            this.tb_NIP.Text = contract.NIP;
            this.tb_Post.Text = contract.Post;
            this.tb_PostalCode.Text = contract.Code;
            this.tb_Street.Text = contract.Street;
        }

        private void btn_SaveContract(object sender, RoutedEventArgs e)
        {
            this.stored_contract.Name = tb_ContractName.Text;
            this.stored_contract.NIP = tb_NIP.Text;
            this.stored_contract.Code = tb_PostalCode.Text;
            this.stored_contract.Post = tb_Post.Text;
            this.stored_contract.Street = tb_Street.Text;

            this.Close();

            getData_CallBack(this.stored_contract);
        }

        private void DefaultCallBack(ContractorDto i) { }

        private void btn_CancelContractWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
