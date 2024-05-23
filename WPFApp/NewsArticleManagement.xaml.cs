using BusinessObjects;
using Services.Implement;
using Services.Interface;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for NewsArticleManagement.xaml
    /// </summary>
    public partial class NewsArticleManagement : Window
    {
        ITagSvc _tagSvc;
        ICategorySvc _categorySvc;
        ISystemAccountSvc _accountSvc;
        INewsArticleSvc _newsArticleSvc;

        public NewsArticleManagement()
        {
            _tagSvc = new TagSvc();
            _categorySvc = new CategorySvc();
            _accountSvc = new SystemAccountSvc();
            _newsArticleSvc = new NewsArticleSvc();
            InitializeComponent();
        }
        public bool IsHistory { get; set; }
        public short AccountId { get; set; }

        private void LoadNewsArticles()
        {
            List<NewsArticle> newsArticles;
            if (IsHistory)
            {
                newsArticles = _newsArticleSvc.GetNewsArticlesByAccountId(AccountId);
            }
            else
            {
                newsArticles = _newsArticleSvc.GetNewsArticles();
            }
            if (newsArticles.Count > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
            lvNewsArticles.ItemsSource = newsArticles;
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

        private void lvNewsArticles_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NewsArticle item = (sender as ListView).SelectedItem as NewsArticle;
            if (item != null)
            {
                txtArticleId.Text = item.NewsArticleId.ToString();
                txtContent.Text = item.NewsContent;
                txtTitle.Text = item.NewsTitle;

                //cbCategory.SelectedItem = item.Category; //Why this not work for me ? :(
                // Find the index of the CategoryName in the ComboBox items
                int selectedIndex = -1;
                for (int i = 0; i < cbCategory.Items.Count; i++)
                {
                    Category category = cbCategory.Items[i] as Category;
                    if (category != null && category.CategoryId == item.CategoryId)
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


                //cbCreateBy.SelectedItem = item.CreatedBy;//Why this not work for me ? :(
                int selectedIndexAccount = -1;
                for (int i = 0; i < cbCreateBy.Items.Count; i++)
                {
                    SystemAccount account = cbCreateBy.Items[i] as SystemAccount;
                    if (account != null && account.AccountId == item.CreatedById)
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



                rbStatusTrue.IsChecked = item.NewsStatus;
                rbStatusFlase.IsChecked = !item.NewsStatus;
                dpCreateDate.SelectedDate = item.CreatedDate;
                dpModifiedDate.SelectedDate = item.ModifiedDate;

                lbNewsTags.SelectedItems.Clear();
                foreach (var tag in item.Tags)
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



        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (txtArticleId.Text.Trim().Length > 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this news?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _newsArticleSvc.DeleteNewsArticle(txtArticleId.Text.Trim());
                    LoadNewsArticles();
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewsArticleDetails newsArticleDetails = new NewsArticleDetails
            {
                Title = "Add System Account",
                InsertOrUpdate = false,
                _accountSvc = _accountSvc,
                _categorySvc = _categorySvc,
                _newsArticleSvc = _newsArticleSvc,
                _tagSvc = _tagSvc,
                NewsArticleDetail = GetNewsArticleDetails()
            };

            newsArticleDetails.Show();
            newsArticleDetails.Closed += (s, args) => LoadNewsArticles();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void lvNewsArticles_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NewsArticleDetails newsArticleDetails = new NewsArticleDetails
            {
                Title = "Update System Account",
                InsertOrUpdate = true,
                _accountSvc = _accountSvc,
                _categorySvc = _categorySvc,
                _newsArticleSvc = _newsArticleSvc,
                _tagSvc = _tagSvc,
                NewsArticleDetail = GetNewsArticleDetails()
            };

            newsArticleDetails.Show();
            newsArticleDetails.Closed += (s, args) => LoadNewsArticles();
        }

        private NewsArticle GetNewsArticleDetails()
        {
            NewsArticle newsArticleDetail = null;
            try
            {
                List<Tag> tags = new List<Tag>();
                foreach (Tag tag in lbNewsTags.SelectedItems)
                {
                    tags.Add(tag);
                }

                newsArticleDetail = new NewsArticle
                {
                    NewsArticleId = txtArticleId.Text,
                    NewsContent = txtContent.Text,
                    NewsTitle = txtTitle.Text,
                    NewsStatus = rbStatusTrue.IsChecked == true,
                    CreatedById = (cbCreateBy.SelectedItem as SystemAccount).AccountId,
                    CategoryId = (cbCategory.SelectedItem as Category).CategoryId,
                    CreatedDate = Convert.ToDateTime(dpCreateDate.Text),
                    ModifiedDate = Convert.ToDateTime(dpModifiedDate.Text),
                    Tags = tags
                };

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get News Article");

            }
            return newsArticleDetail;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
            LoadNewsTags();
            LoadAccounts();
            LoadNewsArticles();
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            LoadNewsArticles();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<NewsArticle> newsArticles;
            if (IsHistory)
            {
                newsArticles = _newsArticleSvc.GetNewsArticlesByAccountId(AccountId);
            }
            else
            {
                newsArticles = _newsArticleSvc.SearchNewsArticlesByTitle(txtSearch.Text);
            }
            if (newsArticles.Count > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
            lvNewsArticles.ItemsSource = newsArticles;
        }
    }
}
