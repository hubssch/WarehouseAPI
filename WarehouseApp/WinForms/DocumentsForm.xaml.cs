using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
//sing Magazynier.DatabaseAccess;

//using Magazynier.DatabaseAccess;

using WarehouseAPI.Models.Dto;
using WarehouseApp.ApiAccess;

namespace Magazynier.WinForms
{

    public partial class DocumentsForm : Window
    {
        //private MagDbContext dbContext;

        public DocumentsForm()//MagDbContext db)
        {
            InitializeComponent();

            //this.dbContext = db;

            this.Loaded += DocumentsForm_Loaded;
        }

        private void DocumentsForm_Loaded(object sender, RoutedEventArgs e)// => dg_docs_list.ItemsSource = LoadDocumentsData();
        {
            RefreshGridDataSource();
        }

        /*private List<ItemDocument> LoadDocumentsData()
        {
            var docs_list =
                from doc in dbContext.Documents
                join contract in dbContext.Contracts
                on doc.ContractID equals contract.ContractID
                select new
                {
                    DocID = doc.DocID,
                    Signature = doc.Signature,
                    DocType = doc.Operation == 'W' ? "WZ" : "PZ",
                    ContractData = String.Format("{0}, {1}, {2} {3}, NIP: {4}", contract.Name, contract.Street, contract.Code, contract.Post, contract.NIP),
                    Date = doc.Date
                };
            List<ItemDocument> list = new List<ItemDocument>();
            foreach (var doc_item in docs_list)
            {
                list.Add(new ItemDocument(doc_item.DocID, doc_item.Signature, doc_item.DocType, doc_item.ContractData, doc_item.Date));
            }
            return list;
        }*/

        private async void RefreshGridDataSource()
        {
            dg_docs_list.ItemsSource = null;
            dg_docs_list.ItemsSource = await ApiWrapper.GetDocuments();
			//dg_docs_list.ItemsSource = LoadDocumentsData();
		}

        private void btn_CancelDocumentsWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btn_RemoveDocument(object sender, RoutedEventArgs e)
        {
            /*ItemDocument item = (ItemDocument) dg_docs_list.SelectedItem;
            DbDocument doc_item = dbContext.Documents.FirstOrDefault(d => d.DocID == item.DocID);
            if (doc_item == null) return;
            dbContext.Remove(doc_item);
            dbContext.SaveChanges();*/
            DocumentDto doc_item = (DocumentDto) dg_docs_list.SelectedItem;
            if (doc_item != null)
            {
				bool res = await ApiWrapper.RemoveDocument(doc_item.DocID);
				if (!res)
				{
					MessageBox.Show("Nie udało się usunąć artykułu. Sprawdź swoją rolę. Tylko Admin może usuwać dane magazynowe.");
				}
				RefreshGridDataSource();
			}
		}

        private void lb_dbl_click_DocList(object sender, MouseButtonEventArgs e)
        {
            /*ItemDocument*/DocumentDto item = (/*ItemDocument*/DocumentDto)dg_docs_list.SelectedItem;
            DocumentForm documentWin = new DocumentForm(/*dbContext, */DocumentWindowMode.VIEW, item);
            documentWin.ShowDialog();
        }
    }
}
