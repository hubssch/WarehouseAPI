using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WarehouseAPI.Models.Dto;
using WarehouseApp.ApiAccess;

namespace WarehouseApp.WinForms
{

    public partial class DocumentsForm : Window
    {

        public DocumentsForm()
        {
            InitializeComponent();

            this.Loaded += DocumentsForm_Loaded;
        }

        private void DocumentsForm_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGridDataSource();
        }

        private async void RefreshGridDataSource()
        {
            dg_docs_list.ItemsSource = null;
            dg_docs_list.ItemsSource = await ApiWrapper.GetDocuments();
		}

        private void btn_CancelDocumentsWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void btn_RemoveDocument(object sender, RoutedEventArgs e)
        {
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
            DocumentDto item = (DocumentDto)dg_docs_list.SelectedItem;
            DocumentForm documentWin = new DocumentForm(DocumentWindowMode.VIEW, item);
            documentWin.ShowDialog();
        }
    }
}
