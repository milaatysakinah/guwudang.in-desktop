﻿using guwudang.Product;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Velacro.Basic;
using Velacro.LocalFile;
using Velacro.UIElements.Basic;
using Velacro.UIElements.Button;
using Velacro.UIElements.TextBlock;
using Velacro.UIElements.TextBox;

namespace guwudang.CreateProduct
{
    /// <summary>
    /// Interaction logic for CreateProductPage.xaml
    /// </summary>
    public partial class CreateProductPage : MyPage
    {
        private BuilderButton buttonBuilder;
        private BuilderTextBox txtBoxBuilder;
        private BuilderTextBlock txtBlockBuilder;
        private IMyButton createButton;
        private IMyButton uploadButton;
        private IMyTextBlock statusTextBlock;
        private IMyTextBox productNameTxtBox;
        private IMyTextBox stockTxtBox;
        private IMyTextBox priceTxtBox;
        private IMyTextBox descriptionTxtBox;

        private MyList<MyFile> uploadImage = new MyList<MyFile>();
        public CreateProductPage()
        {
            InitializeComponent();
            setController(new CreateProductController(this));
            initUIBuilders();
            initUIElements();
        }

        private void initUIBuilders()
        {
            buttonBuilder = new BuilderButton();
            txtBoxBuilder = new BuilderTextBox();
            txtBlockBuilder = new BuilderTextBlock();
        }

        private void initUIElements()
        {
            uploadButton = buttonBuilder
                .activate(this, "upload_btn")
                .addOnClick(this, "onPictureButtonClick");
            createButton = buttonBuilder
                .activate(this, "create_btn")
                .addOnClick(this, "onCreateButtonClick");
            productNameTxtBox = txtBoxBuilder.activate(this, "productName_tb");
            stockTxtBox = txtBoxBuilder.activate(this, "stock_tb");
            priceTxtBox = txtBoxBuilder.activate(this, "price_tb");
            descriptionTxtBox = txtBoxBuilder.activate(this, "description_tb");
            statusTextBlock = txtBlockBuilder.activate(this, "status_txt");

        }

        public void onPictureButtonClick()
        {
            uploadImage.Clear();
            OpenFile openFile = new OpenFile();
            uploadImage.Add(openFile.openFile(false)[0]);
            if (uploadImage[0] != null)
            {
                /*if (uploadImage[0].extension.Equals(".png", StringComparison.InvariantCultureIgnoreCase) ||
                    uploadImage[0].extension.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase) ||
                    uploadImage[0].extension.Equals(".jpeg", StringComparison.InvariantCultureIgnoreCase))
                    picture.Source = new BitmapImage(new Uri(uploadImage[0].fullPath));
                else
                {
                    MessageBoxResult result = MessageBox.Show("File format not supported !", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    uploadImage.Clear();
                }*/
            }
        }

        public void onCreateButtonClick()
        {
            getController().callMethod("createProduct", productNameTxtBox.getText(), stockTxtBox.getText(), priceTxtBox.getText(),
                descriptionTxtBox.getText(), uploadImage);
        }

        public void setStatusError(String error)
        {
            this.Dispatcher.Invoke(() =>
            {
                statusTextBlock.setText(error);
            });
        }

        public void createSuccess()
        {
            this.Dispatcher.Invoke(() =>
            {
                productNameTxtBox.setText("");
                stockTxtBox.setText("");
                priceTxtBox.setText("");
                descriptionTxtBox.setText("");
                statusTextBlock.setText("");

                utils.PageManagement.initPages();
                Sidebar.secFrame.Navigate(utils.PageManagement.getPage(utils.EPages.listProductPage));
            });
        }
    }
}
