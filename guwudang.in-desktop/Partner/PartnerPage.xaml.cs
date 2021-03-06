﻿using System.Collections.Generic;
using Velacro.UIElements.Basic;
using Velacro.UIElements.Button;
using Velacro.UIElements.TextBlock;
using Velacro.UIElements.TextBox;
using System.Windows;
using System.Windows.Controls;
using System;

namespace guwudang.Partner
{
    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class PartnerPage : MyPage
    {
        private BuilderButton buttonBuilder;
        private BuilderTextBox txtBoxBuilder;
        private BuilderTextBlock txtBlockBuilder;
        private IMyButton newPartnerButton;
        private IMyButton deletePartnerButton;
        private IMyTextBox searchPartnerTxtBox;
        private IMyTextBox passwordTxtBox;
        private IMyTextBlock loginStatusTxtBlock;
        private List<string> listPartnerID = new List<string>();

        public PartnerPage()
        {
            InitializeComponent();
            this.KeepAlive = true;
            //List<Product> items = new List<Product>();
            //items.Add(new Product() { id = "1", product_type_id = "1", user_id = "1", product_name = "Baju Badut", price = "120000", units = "24", description = "Ini Deskripsi product 1", product_picture = "/img/", created_at = "", updated_at = "" });
            //lvProduct.ItemsSource = items;
            setController(new PartnerController(this));
            initUIBuilders();
            initUIElements();
            getPartner();
        }

        private void initUIBuilders()
        {
            buttonBuilder = new BuilderButton();
            txtBoxBuilder = new BuilderTextBox();
        }

        private void initUIElements()
        {
            newPartnerButton = buttonBuilder.activate(this, "newPartnerBtn").addOnClick(this, "onClick_newPartner");
            deletePartnerButton = buttonBuilder.activate(this, "deletePartnerBtn").addOnClick(this, "onClickBtnDelete");
            searchPartnerTxtBox = txtBoxBuilder.activate(this, "searchPartnerTxt");
        }

        private void getPartner()
        {
            getController().callMethod("partner");
        }

        public void setPartner(List<Model.Partner> partners)
        {
            this.Dispatcher.Invoke(() =>
            {
                lvPartner.ItemsSource = partners;
            });
        }

        public void onClick_newPartner()
        {
            Sidebar.secFrame.Navigate(utils.PageManagement.getPage(utils.EPages.newPartnerPage));
        }

        private void search_btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            getController().callMethod("searchPartner", searchPartnerTxtBox.getText());
        }

        private void lvPartner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*foreach (guwudang.Model.Partner item in e.RemovedItems)
            {
                listPartnerID.Remove(item.id);
            }

            foreach (guwudang.Model.Partner item in e.AddedItems)
            {
                listPartnerID.Add(item.id);
            }*/
        }

        public void onClickBtnDelete()
        {
            string txt = "Konfirmasi";
            string msgtext;
            if (listPartnerID.Count > 0)
            {
                msgtext = "Apakah Anda yakin ingin menghapus " + listPartnerID.Count + " data tersebut ? ";
            }
            else
            {
                msgtext = "Anda belum memilih data untuk dihapus.";
            }

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(msgtext, txt, button);

            if(msgtext != "Anda belum memilih data untuk dihapus.")
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        //if (listPartnerID.Count > 0) { delPartner(); }
                        delPartner();
                        break;
                    case MessageBoxResult.No:
                        // No Action
                        break;
                }
        }

        private void delPartner()
        {
            //foreach (string item in listProductID)
            //{
            //    getController().callMethod("deleteProduct", item);
            //}
            getController().callMethod("deletePartner", listPartnerID);
            //getProduct();
        }

        public void onClickDetailPartner(object sender, System.Windows.RoutedEventArgs e)
        {
            string id = (string)((Button)sender).Tag;
            DetailPartner.DetailPartner detail = new DetailPartner.DetailPartner(id);
            Sidebar.secFrame.Navigate(detail);
        }

        public void backToLogin()
        {
            new MainWindow().Show();
            this.KeepAlive = false;
        }

        public void createSuccess()
        {
            this.Dispatcher.Invoke(() =>
            {
                utils.PageManagement.initPages();
                Sidebar.secFrame.Navigate(utils.PageManagement.getPage(utils.EPages.listPartnerPage));
            });
        }

        private void chkSelected_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            string newVal = chk.Tag.ToString();
            Console.WriteLine("Clicked");
            if (chk.IsChecked.HasValue && chk.IsChecked.Value)
            {
                listPartnerID.Add(newVal);
            }
            else
            {
                listPartnerID.Remove(newVal);
            }
        }
    }
}