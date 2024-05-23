using BusinessObjects;
using Services.Interface;
using System.Windows;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for CategoryDetails.xaml
    /// </summary>
    public partial class CategoryDetails : Window
    {
        public CategoryDetails()
        {
            InitializeComponent();
        }

        public ICategorySvc _categorySvc { get; set; }
        public bool InsertOrUpdate { get; set; }
        public Category CategoryInfo { get; set; }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            txtCategoryId.IsEnabled = !InsertOrUpdate;

            if (InsertOrUpdate)
            {
                txtCategoryId.Text = CategoryInfo.CategoryId.ToString();
                txtCategoryName.Text = CategoryInfo.CategoryName;
                txtCategoryDescription.Text = CategoryInfo.CategoryDesciption;
            }

            if (!InsertOrUpdate) txtCategoryId.IsEnabled = false;

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Category category;
                if (InsertOrUpdate)
                {
                    category = new Category
                    {
                        CategoryId = short.Parse(txtCategoryId.Text),
                        CategoryName = txtCategoryName.Text,
                        CategoryDesciption = txtCategoryDescription.Text
                    };
                }
                else
                {
                    category = new Category
                    {
                        CategoryName = txtCategoryName.Text,
                        CategoryDesciption = txtCategoryDescription.Text
                    };
                }

                if (InsertOrUpdate)
                {
                    _categorySvc.UpdateCategory(category);
                    MessageBox.Show("Category " + category.CategoryName + " updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _categorySvc.AddCategory(category);
                    MessageBox.Show("Category " + category.CategoryName + " added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate == false ? "Add New Category" : "Update A Category");
            }
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e) => Close();


    }
}
