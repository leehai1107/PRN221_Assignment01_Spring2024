using BusinessObjects;
using Services.Implement;
using Services.Interface;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for CategoryManagement.xaml
    /// </summary>
    public partial class CategoryManagement : Window
    {
        private ICategorySvc _categorySvc;
        public string thamchieu;
        public CategoryManagement()
        {
            InitializeComponent();
            _categorySvc = new CategorySvc();
        }

        public void LoadCategories()
        {
            List<Category> categories = _categorySvc.GetCategories();
            if (categories.Count > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }

            lvCategories.ItemsSource = categories;
        }


        private void lvCategoriesMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Category item = (sender as ListView).SelectedItem as Category;

            if (item != null)
            {
                txtCategoryId.Text = item.CategoryId.ToString();
                txtCategoryName.Text = item.CategoryName;
                txtCategoryDescription.Text = item.CategoryDesciption;
            }

        }

        private Category GetCategoryInfo()
        {
            Category category = null;
            try
            {
                category = new Category
                {
                    CategoryId = Convert.ToInt16(txtCategoryId.Text),
                    CategoryName = txtCategoryName.Text,
                    CategoryDesciption = txtCategoryDescription.Text
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Category");
            }
            return category;
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var data = txtCategoryId.Text;
            if (data.Length > 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this category?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _categorySvc.RemoveCategory(Convert.ToInt16(data));
                    LoadCategories();
                }
            }
            else
            {
                MessageBox.Show("Please insert data!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            CategoryDetails categoryDetails = new CategoryDetails
            {
                Title = "Add New Category",
                InsertOrUpdate = false,
                _categorySvc = _categorySvc,
            };
            categoryDetails.Show();
            categoryDetails.Closed += (s, args) => LoadCategories();
        }

        private void lvCategories_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CategoryDetails categoryDetails = new CategoryDetails
            {
                Title = "Update Category",
                InsertOrUpdate = true,
                CategoryInfo = GetCategoryInfo(),
                _categorySvc = _categorySvc,
            };
            categoryDetails.Show();
            categoryDetails.Closed += (s, args) => LoadCategories();
        }

        private void lvCategories_Loaded(object sender, RoutedEventArgs e)
        {
            btnDelete.IsEnabled = false;
            LoadCategories();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<Category> categories = _categorySvc.SearchCategoriesByName(txtSearch.Text);
            if (categories.Count > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }

            lvCategories.ItemsSource = categories;
        }
    }
}
