using BusinessObjects;
using Services.Implement;
using Services.Interface;
using System.Diagnostics;
using System.Windows;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for NewsArticleDetails.xaml
    /// </summary>
    public partial class NewsArticleDetails : Window
    {
        public ITagSvc _tagSvc;
        public ICategorySvc _categorySvc;
        public ISystemAccountSvc _accountSvc;
        public INewsArticleSvc _newsArticleSvc { get; set; }
        public bool InsertOrUpdate { get; set; }

        public NewsArticle NewsArticleDetail { get; set; }

        public NewsArticleDetails()
        {
            _tagSvc = new TagSvc();
            _categorySvc = new CategorySvc();
            _accountSvc = new SystemAccountSvc();
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewsArticle newsArticle;
                if (InsertOrUpdate)
                {//update
                    newsArticle = new NewsArticle
                    {
                        NewsArticleId = txtArticleId.Text,
                        NewsContent = txtContent.Text,
                        NewsTitle = txtTitle.Text,
                        NewsStatus = rbStatusTrue.IsChecked == true,
                        CreatedById = (cbCreateBy.SelectedItem as SystemAccount).AccountId,
                        CategoryId = (cbCategory.SelectedItem as Category).CategoryId,
                        CreatedDate = Convert.ToDateTime(dpCreateDate.Text),
                        ModifiedDate = DateTime.Now
                    };
                }
                else
                {//add
                    newsArticle = new NewsArticle
                    {
                        NewsArticleId = txtArticleId.Text,
                        NewsContent = txtContent.Text,
                        NewsTitle = txtTitle.Text,
                        NewsStatus = rbStatusTrue.IsChecked == true,
                        CreatedById = (cbCreateBy.SelectedItem as SystemAccount).AccountId,
                        CategoryId = (cbCategory.SelectedItem as Category).CategoryId,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };
                }

                List<Tag> tags = new List<Tag>();
                foreach (Tag tag in lbNewsTags.SelectedItems)
                {
                    tags.Add(tag);
                }

                Debug.WriteLine(newsArticle.NewsStatus.ToString());

                if (InsertOrUpdate)
                {
                    _newsArticleSvc.UpdateNewsArticle(newsArticle, tags);
                    MessageBox.Show("NewsArticle " + newsArticle.NewsTitle + " updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _newsArticleSvc.AddNewsArticle(newsArticle, tags);
                    MessageBox.Show("NewsArticle " + newsArticle.NewsTitle + " added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, InsertOrUpdate == false ? "Add New NewsArticle" : "Update A NewsArticle");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => Close();

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            txtArticleId.IsEnabled = !InsertOrUpdate;
            LoadAccounts();
            LoadCategories();
            LoadNewsTags();

            if (InsertOrUpdate)
            {
                txtArticleId.Text = NewsArticleDetail.NewsArticleId.ToString();
                txtContent.Text = NewsArticleDetail.NewsContent;
                txtTitle.Text = NewsArticleDetail.NewsTitle;

                //cbCategory.SelectedItem = _categorySvc.GetCategoryById(NewsArticleDetail.CategoryId.Value);
                //cbCreateBy.SelectedItem = _accountSvc.GetSystemAccountById(NewsArticleDetail.CreatedById.Value);

                // Find the index of the CategoryName in the ComboBox items
                int selectedIndex = -1;
                for (int i = 0; i < cbCategory.Items.Count; i++)
                {
                    Category category = cbCategory.Items[i] as Category;
                    if (category != null && category.CategoryId == NewsArticleDetail.CategoryId)
                    {
                        selectedIndex = i;
                        break;
                    }
                }

                // Set the selected index if found
                if (selectedIndex != -1)
                {
                    cbCategory.SelectedIndex = selectedIndex;
                }


                int selectedIndexAccount = -1;
                for (int i = 0; i < cbCreateBy.Items.Count; i++)
                {
                    SystemAccount account = cbCreateBy.Items[i] as SystemAccount;
                    if (account != null && account.AccountId == NewsArticleDetail.CreatedById)
                    {
                        selectedIndexAccount = i;
                        break;
                    }
                }

                // Set the selected index if found
                if (selectedIndexAccount != -1)
                {
                    cbCreateBy.SelectedIndex = selectedIndexAccount;
                }


                dpCreateDate.SelectedDate = NewsArticleDetail.CreatedDate;
                dpModifiedDate.SelectedDate = NewsArticleDetail.ModifiedDate;
                rbStatusTrue.IsChecked = true;
                rbStatusFlase.IsChecked = NewsArticleDetail.NewsStatus == false;

                foreach (var tag in NewsArticleDetail.Tags)
                {
                    foreach (Tag checkBoxItem in lbNewsTags.Items)
                    {
                        if (tag.TagName == checkBoxItem.TagName)
                        {

                            lbNewsTags.SelectedItems.Add(checkBoxItem);

                            break;

                        }
                    }
                }
            }


        }

        private void LoadNewsTags()
        {

            lbNewsTags.ItemsSource = _tagSvc.GetTags();
        }

        private void LoadCategories()
        {

            cbCategory.ItemsSource = _categorySvc.GetCategories();
            cbCategory.SelectedIndex = 0;
        }

        private void LoadAccounts()
        {
            cbCreateBy.ItemsSource = _accountSvc.GetAccounts();
            cbCreateBy.SelectedIndex = 0;
        }
    }
}
