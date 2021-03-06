﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Velacro.UIElements.Basic;
using Velacro.UIElements.Button;
using Velacro.UIElements.TextBlock;
using Velacro.UIElements.TextBox;


namespace guwudang.Invoice
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ListInvoicePage : MyPage
    {
        private BuilderButton buttonBuilder;
        private BuilderTextBox txtBoxBuilder;
        private BuilderTextBlock txtBlockBuilder;
        private IMyButton newInvoiceButton;
        private IMyButton deleteInvoiceButton;
        private IMyTextBox searchInvoiceTxtBox;
        private List<string> selectedItemsID = new List<string>();

        public ListInvoicePage()
        {
            InitializeComponent();
            this.KeepAlive = true;
            //List<Product> items = new List<Product>();
            //items.Add(new Product() { id = "1", product_type_id = "1", user_id = "1", product_name = "Baju Badut", price = "120000", units = "24", description = "Ini Deskripsi product 1", product_picture = "/img/", created_at = "", updated_at = "" });
            //lvProduct.ItemsSource = items;
            setController(new InvoiceController(this));
            InitUIBuilders();
            InitUIElements();
            getInvoice();
        }

        public void onClickDetailInvoice(object sender, System.Windows.RoutedEventArgs e)
        {
            string id = (string)((Button)sender).Tag;
            //DetailInvoice.DetailInvoicePage detail = new DetailInvoice.DetailInvoicePage(id);
            DetailInvoice.DetailInvoice di = new DetailInvoice.DetailInvoice(id);
            Sidebar.secFrame.Navigate(di);
            //Sidebar.secFrame.Navigate(detail);
        }

        private void InitUIBuilders()
        {
            buttonBuilder = new BuilderButton();
            txtBoxBuilder = new BuilderTextBox();
        }

        private void InitUIElements()
        {
            newInvoiceButton = buttonBuilder.activate(this, "newInvoiceBtn").addOnClick(this, "onClick_newInvoice");
            searchInvoiceTxtBox = txtBoxBuilder.activate(this, "searchInvoiceTxt");
        }

        public void onClick_newInvoice()
        {
            //Sidebar.secFrame.Navigate(utils.PageManagement.getPage(utils.EPages.newInvoicePage));
            Sidebar.secFrame.Navigate(utils.PageManagement.getPage(utils.EPages.newInvoicePage));
        }

        private void getInvoice()
        {
            getController().callMethod("listInvoice");
        }

        public void setInvoice(List<Model.Invoice> invoice)
        {
            this.Dispatcher.Invoke(() =>
            {
                lvListInvoice.ItemsSource = invoice;
            });
        }

        public void backToLogin()
        {
            new MainWindow().Show();
            this.KeepAlive = false;
        }

        private void search_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            getController().callMethod("searchInvoice", searchInvoiceTxtBox.getText());
        }

        private void deleteInvoiceBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Console.WriteLine(i);
            string txt = "Konfirmasi";
            String msgtext = "";
            if (selectedItemsID.Count > 0)
            {
                getController().callMethod("deleteInvoice", selectedItemsID);
            }
            else
            {
                msgtext = "Anda belum memilih data untuk dihapus.";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxResult result = MessageBox.Show(msgtext, txt, button);
            }
        }

        private void chkSelected_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            string newVal = chk.Tag.ToString();
            Console.WriteLine("Clicked");
            if (chk.IsChecked.HasValue && chk.IsChecked.Value)
            {
                selectedItemsID.Add(newVal);
            }
            else
            {
                selectedItemsID.Remove(newVal);
            }
        }

        public void createSuccess()
        {
            this.Dispatcher.Invoke(() =>
            {
                utils.PageManagement.initPages();
                Sidebar.secFrame.Navigate(utils.PageManagement.getPage(utils.EPages.listInvoicePage));
            });
        }

    }

    //public class Product
    //{
    //    public string id { get; set; }
    //    public string product_type_id { get; set; }
    //    public string user_id { get; set; }
    //    public string product_name { get; set; }
    //    public string price { get; set; }
    //    public string units { get; set; }
    //    public string description { get; set; }
    //    public string product_picture { get; set; }
    //    public string created_at { get; set; }
    //    public string updated_at { get; set; }

    //}
}